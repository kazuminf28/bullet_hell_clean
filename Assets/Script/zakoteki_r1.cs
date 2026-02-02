using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zakoteki_r1 : MonoBehaviour, IDamageable
{
    [Header("HP")]
    [SerializeField] private int HP = 10;

    [Header("弾の加速度")]
    [SerializeField] private float accel;

    [Header("弾プレハブ1")]
    [SerializeField] private GameObject bulletPrefab1;

    [Header("弾プレハブ2")]
    [SerializeField] private GameObject bulletPrefab2;

    [Header("発射位置")]
    [SerializeField] private Transform firePoint;

    // [Header("Player")]
    // [SerializeField] private Transform player;

    [Header("移動設定")]
    public float speed = 3f;

    private float startX;             // 出現時のX座標

    bool isShooting = false;

    private Transform player;

    [SerializeField] private int scoreValue = 100;

    void Awake()
    {
        startX = transform.position.x;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(ShootLoop());
    }

    void Update()
    {
        if (IsInsideScreen()){
            // 左から右
            transform.position += Vector3.right * speed * Time.deltaTime;
        } else
        {
            FindObjectOfType<EnemyManager>().EnemyDie();
            Destroy(gameObject);
        }
    }

    public void Damage(int value)
    {
        HP -= value;
        Debug.Log($"Enemy HP: {HP}");

        if (HP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        FindObjectOfType<EnemyManager>().EnemyDie();
        ScoreManager.ScoreManagers.AddScore(scoreValue);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    // ====== 弾幕パターン ======
    IEnumerator zakoPattern()
    {
        #region 変更前
        // float angle = 40f;

        // for (int i = 0; i < 50; i++)
        // {
        //     angle += 10f;
        //     if (angle >= 150f)
        //         break;

        //     Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        //     // ▼ Instantiate → BulletManager に変更
        //     GameObject obj = BulletManager.Instance.GetBullet(bulletPrefab2, firePoint.position, rot);
        //     Bullet b = obj.GetComponent<Bullet>();
        //     b.accel = accel;
        //     b.changeSpeed = true;

        //     yield return new WaitForSeconds(0.05f);
        // }
        #endregion 

        #region 変更後

        int burstCount = 5;   // 1セットの弾数
        int burstTimes = 3;   // 何セット撃つか
        for (int t = 0; t < burstTimes; t++)
            {
                // ★ ここでプレイヤー位置を更新（5発ごと）
                Vector3 dir = (player.position - firePoint.position).normalized;
                // Debug.Log("player pos = " + player.position);
                for (int i = 0; i < burstCount; i++)
                {
                    GameObject obj = BulletManager.Instance.GetBullet(
                        bulletPrefab2,
                        firePoint.position,
                        Quaternion.identity
                    );

                    Bullet b = obj.GetComponent<Bullet>();
                    b.Fire(dir);

                    yield return new WaitForSeconds(0.05f);
                }

                // ★ 次の5発までの間
                yield return new WaitForSeconds(0.3f);
            }
        #endregion
    }

    bool IsInsideScreen()
    {
        Camera cam = Camera.main;
        if (!cam) return true;

        // Vector3 vp = cam.WorldToViewportPoint(transform.position);
        Vector3 pos = transform.position;

        return pos.x > -8.3f && pos.x < 0.8f &&
               pos.y > -4.5f && pos.y < 4.4f;
    }

    // ====== 発射ループ ======
    IEnumerator ShootLoop()
    {
        if (isShooting) yield break;
        isShooting = true;

        while (HP > 0)
        {
            yield return StartCoroutine(zakoPattern());
            yield return new WaitForSeconds(3f);
        }

        isShooting = false;
    }
}
