using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Marisa : MonoBehaviour, IDamageable
{
    [Header("移動系の処理")]
    [SerializeField] float moveInterval = 5f;
    Transform[] phase1_MP;
    Transform[] phase3_MP;
    [SerializeField] float moveSpeed = 5f;
    [Header("HP")]
    [SerializeField] private int HP;
    BossHP bossHPUI;

    [Header("弾プレハブ1")]
    [SerializeField] private GameObject bulletPrefab1;

    [Header("弾プレハブ2")]
    [SerializeField] private GameObject bulletPrefab2;

    [Header("phase1")]
    [SerializeField] GameObject magicCircleRootPrefab;
    [SerializeField] GameObject[] phase1_MagicCirclePrefabs;
    Transform magicCircleRoot;
    [SerializeField] float circleRadius = 2.5f;
    [SerializeField] float rotateSpeed = 30f;

    Coroutine phase1_MoveC;

    [Header("phase2")]
    [SerializeField] GameObject milkyWayPrefab;
    MilkyWay_phase2 milkyWay;

    [Header("phase3")]
    [SerializeField] GameObject masterSparkPrefab;
    [SerializeField] GameObject routedPrefab;
    [SerializeField] Hakkero hakkero;

    Coroutine phase3_MoveC;

    private int phase = 1;

    private int HPP = 0;


    List<MC_phase1> circles = new List<MC_phase1>();

    private Vector3 initialPosition;

    [SerializeField] private int scoreValue = 100;

    private void Start()
    {
        StartCoroutine(BossPattern());
    }

    public void SetPhase1_MP(Transform[] points)
    {
        phase1_MP = points;
    }

    public void SetPhase3_MP(Transform[] points)
    {
        phase3_MP = points;
    }

    public void SetBossHPUI(BossHP ui)
    {
        bossHPUI = ui;
        HPP = HP;
        bossHPUI.Show();
        bossHPUI.SetHP(HP, HPP);
    } 

    private void Awake()
    {
        HPP = HP;
        initialPosition = transform.position;
    }

    public void Damage(int value)
    {
        HP -= value;
        bossHPUI.SetHP(HP, HPP);
        Debug.Log($"Marisa HP: {HP}, Phase: {phase}");

        if (HP > 0) return;

        phase++;
        if (phase == 2) phase1_MoveC = null;

        if (phase >= 4)
        {
            Die();   // 最終撃破
            SceneManager.LoadScene("ClearScene");
            return;
        }

        // 次フェーズへ
        HP = HPP;
        bossHPUI.SetHP(HP, HPP);
        // Phase1専用移動停止
        if (phase1_MoveC != null)
        {
            StopCoroutine(phase1_MoveC);
            phase1_MoveC = null;
        }

        // 初期位置に戻す
        transform.position = initialPosition;
        
        CleanupMagicCircles();
        // 弾幕リセット
        BulletManager.Instance.ClearAllBullets();
    }

    void CleanupMagicCircles()
    {
        RemoveMagicCircles();

        if (magicCircleRoot != null)
        {
            Destroy(magicCircleRoot.gameObject);
            magicCircleRoot = null;
        }
    }


    void Die()
    {
        bossHPUI.Hide();
        ScoreManager.ScoreManagers.AddScore(scoreValue);
        ScoreManager.ScoreManagers.SaveHighScore();
        StopAllCoroutines();
        Destroy(gameObject);
    }

    #region  魔理沙簡易版のパターン管理
    IEnumerator BossPattern()
    {
        while(phase < 5)
        {
            yield return new WaitForSeconds(2f);
            if (phase == 1)
            {
                yield return StartCoroutine(Phase1_MC());      // 魔法陣
            } 
            if (phase == 2)
            {
                yield return StartCoroutine(Phase2_MW());     // ミルキーウェイ
                yield return new WaitForSeconds(1f);
            }
            if (phase == 3)
            {
                yield return StartCoroutine(Phase3_MasterSpark());
            }
        }
    }
    #endregion

    IEnumerator Phase1_MC()
    {
        phase1_MoveC = StartCoroutine(Phase1Move());
        //Rootを生成
        magicCircleRoot = Instantiate(magicCircleRootPrefab).transform;
        magicCircleRoot.SetParent(transform);

        magicCircleRoot.localPosition = Vector3.zero;
        magicCircleRoot.localRotation = Quaternion.identity;
        magicCircleRoot.localScale = Vector3.one;

        CreateMagicCircles();

        float phaseTime = 60f;
        float timer = 0f;

        while (timer < phaseTime && phase == 1)
        {
            magicCircleRoot.Rotate(0, 0, 30f * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // 魔法陣をしまう
        RemoveMagicCircles();

        if (magicCircleRoot != null) Destroy(magicCircleRoot.gameObject);
    }

    IEnumerator Phase2_MW()
    {
        //Rootを生成
        magicCircleRoot = Instantiate(magicCircleRootPrefab).transform;
        magicCircleRoot.SetParent(transform);
        magicCircleRoot.localPosition = Vector3.zero;
        magicCircleRoot.localRotation = Quaternion.identity;
        magicCircleRoot.localScale = Vector3.one;
        CreateMagicCircles();

        GameObject mwObj = Instantiate(milkyWayPrefab, magicCircleRoot);
        mwObj.transform.localPosition = Vector3.zero;

        milkyWay = mwObj.GetComponent<MilkyWay_phase2>();
        milkyWay.StartShoot();

        float phaseTime = 60f;
        float timer = 0f;

        while (timer < phaseTime && phase == 2)
        {
            magicCircleRoot.Rotate(0, 0, 30f * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        milkyWay.StopShoot();

        // 魔法陣をしまう
        RemoveMagicCircles();

        if (magicCircleRoot != null) Destroy(magicCircleRoot.gameObject);
    }

    IEnumerator Phase3_MasterSpark()
    {
        int index = 0;
        while (phase == 3)
        {
            // 移動
            yield return StartCoroutine(Phase3Move(index));

            // マスタースパーク
            yield return StartCoroutine(FireMasterSparkOnce());

            // 次の移動先
            index = (index + 1) % phase3_MP.Length;

            // 少し間
            yield return new WaitForSeconds(1f);
        }
        if (magicCircleRoot != null)
        {
            Destroy(magicCircleRoot.gameObject);
            magicCircleRoot = null;
        }
    }


    // phase1_Move
    IEnumerator Phase1Move()
    {
        int index = 0;
        while (phase == 1)
        {
            Vector3 target = phase1_MP[index].position;
            while (Vector3.Distance(transform.position, target) > 0.05f && phase == 1)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }
            yield return new WaitForSeconds(moveInterval);
            index = (index + 1) % phase1_MP.Length;
        }
    }

    IEnumerator Phase3Move(int index)
    {
        Vector3 target = phase3_MP[index].position;
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = target;
    }

    IEnumerator FireMasterSparkOnce()
    {
        // 魔法陣
        magicCircleRoot = Instantiate(magicCircleRootPrefab).transform;
        magicCircleRoot.SetParent(transform);
        magicCircleRoot.localPosition = Vector3.zero;
        magicCircleRoot.localRotation = Quaternion.identity;
        magicCircleRoot.localScale = Vector3.one;
        CreateMagicCircles();

        // チャージ
        yield return StartCoroutine(hakkero.Charge());

        // ===== マスパ開始 =====
        GameObject ms = Instantiate(masterSparkPrefab, transform);
        ms.transform.localPosition = new Vector3(-0.5f, 0f, 0f);

        MasterSpark spark = ms.GetComponent<MasterSpark>();
        spark.Fire();

        // 追加：回転弾幕ON
        add_phase3 phase3Bullet = GetComponentInChildren<add_phase3>(true);
        if (phase3Bullet != null)
            phase3Bullet.gameObject.SetActive(true);
        
        phase3Bullet.StartShoot();

        // マスパ継続時間
        yield return new WaitForSeconds(4f);

        // ===== マスパ終了 =====
        if (phase3Bullet != null)
        {
            phase3Bullet.StopShoot();
            phase3Bullet.gameObject.SetActive(false);
        }

        Destroy(ms);
        RemoveMagicCircles();
        if (magicCircleRoot != null) Destroy(magicCircleRoot.gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        if (phase == 1) RotateMagicCircles();
        if (phase == 2) RotateMagicCircles();
        if (phase == 3) RotateMagicCircles();
    }

    void CreateMagicCircles()
    {
        circles.Clear();

        int count = phase1_MagicCirclePrefabs.Length;

        for (int i = 0; i < count; i++)
        {
            float angle = 360f / count * i;
            Vector3 pos = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0f
            ) * circleRadius;

            // GameObject obj = Instantiate(
            //     phase1_MagicCirclePrefabs[i],
            //     magicCircleRoot
            // );

            GameObject obj = Instantiate(phase1_MagicCirclePrefabs[i]);
            obj.transform.SetParent(magicCircleRoot);
            obj.transform.localPosition = pos;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;

            MC_phase1 mc = obj.GetComponent<MC_phase1>();
            circles.Add(mc);

            if (phase == 1) mc.StartShoot();
        }
    }

    void RotateMagicCircles()
    {
        if (magicCircleRoot == null) return;
        magicCircleRoot.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }

    void RemoveMagicCircles()
    {
        foreach (var mc in circles)
        {
            if (mc != null)
            {
                mc.StopShoot();
                Destroy(mc.gameObject);
            }
        }
        circles.Clear();
    }
}