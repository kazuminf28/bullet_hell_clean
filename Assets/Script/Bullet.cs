using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region 以前のもの
    // [Header("弾の速度")]
    // [SerializeField] private float speed;

    // [Header("弾の威力")]
    // [SerializeField] private float power;


    // private Rigidbody2D rb;
    // // Start is called before the first frame update

    // public bool changeSpeed = false;
    // public float accel = 0;


    // void Start()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    //     rb.velocity = transform.up * speed;
        
    // }

    // void Update()
    // {
    //     if (!changeSpeed) return;
    //     speed += accel * Time.deltaTime;
    //     speed = Mathf.Max(speed, 0.5f);
    //     rb.velocity = transform.up * speed;
    // }
    #endregion

    #region Managerを使用する形
    [Header("基本ステータス")]
    public float speed = 5f;     // ← Serialized でなく public にする
    public float power = 1;

    [Header("速度変化用")]
    public bool changeSpeed = false;
    public float accel = 0f;

    [Header("遅延加速")]
    public bool delayedAccel = false;
    public float accelStartTime = 1.5f;

    float timer;
    bool accelerating;

    [HideInInspector] 
    public GameObject originalPrefab;   // ← BulletManager が設定する

    private Vector2 direction;

    private float initial_speed;
    private float initial_accel;
    private bool initial_changeSpeed;


    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void SetSpeed(float spd)
    {
        speed = spd;
    }

    private void Awake()
    {
        // rb = GetComponent<Rigidbody2D>();
        initial_speed = speed;
        initial_accel = accel;
        initial_changeSpeed = changeSpeed;
    }

    private void OnEnable()
    {
        // 弾が再利用された時にも必ず初速度セット
        // rb = GetComponent<Rigidbody2D>();
        // rb.velocity = transform.up * speed;
        ResetState();
    }

    public void Fire(Vector2 dir)
    {
        // speed = initial_speed;            // 今回の弾速はこれ
        direction = dir.normalized;
        // accel = initial_accel;
        // changeSpeed = initial_changeSpeed;
        // ★ 角度を移動方向に合わせる
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (delayedAccel && !accelerating && timer >= accelStartTime)
        {
            accelerating = true;
            changeSpeed = true;
        }

        if (changeSpeed) //速度変化
        {
            speed += accel * Time.deltaTime;
            if (speed < 1f) speed = 1f;
        }

        // rb.velocity = transform.up * speed;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (!IsInsideScreen()) //弾幕を返す判定後の処理
        {
            BulletManager.Instance.ReturnBullet(gameObject);
        }
        //  ★ 方向を回転
        // direction = Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime) * direction;

        // ★ 見た目も追従
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }


    #region カメラを使った画面外判定だが色んなカメラに反応するため削除
    // private void OnBecameInvisible()
    // {
    //     // 画面外で自動的にプールへ返却
    //     BulletManager.Instance.ReturnBullet(gameObject);
    // }
    #endregion

    // ===== 画面内判定 =====
    bool IsInsideScreen()
    {
        Camera cam = Camera.main;
        if (!cam) return true;

        // Vector3 vp = cam.WorldToViewportPoint(transform.position);
        Vector3 pos = transform.position;

        return pos.x > -8.3f && pos.x < 0.8f &&
               pos.y > -4.5f && pos.y < 4.4f;
    }

    public void ResetState()
    {
        speed = initial_speed;
        accel = initial_accel;
        changeSpeed = initial_changeSpeed;

        timer = 0f;
        accelerating = false;
    }

    #endregion
}