using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    [SerializeField, Header("残機")] //追加したもの
    private int _HP;
    [SerializeField, Header("無敵時間")] // 追加したもの
    private float _invincibleTime;

    [SerializeField, Header("移動速度")]
    private float _speed;
    [SerializeField, Header("弾オブジェクト")]
    private GameObject _player_bullet;
    [SerializeField, Header("弾を発射する時間")]
    private float _shootTime;
    [SerializeField, Header("弾の発射位置(左)")]
    private Transform _firePointL; 
    [SerializeField, Header("弾の発射位置(右)")]
    private Transform _firePointR;
    [SerializeField, Header("低速移動倍率")]
    private float _slowRate = 0.4f;


    private bool _isInvincible = false; // 無敵時間の有無
    private SpriteRend _spriteRend; //点滅に必要なもの
    private Collider2D hitBox; //追加したもの
    private Vector2 _inputVelocity;
    private Rigidbody2D _rigid;
    private float _shootCount;
    private bool _isSlow = false;
    private bool _isFocusShot = false;


    // Start is called before the first frame update
    void Start()
    {
        _spriteRend = GetComponentInChildren<SpriteRend>();
        hitBox = GetComponent<CircleCollider2D>();
        _inputVelocity = Vector2.zero;
        _rigid = GetComponent<Rigidbody2D>();
        _shootCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _Shooting();
    }

    public void _Damage(int value) //ダメージ処理
    {
        if (_isInvincible) return;
         
        _HP -= value;
        Debug.Log($"霊夢: {_HP}, ダメージ: {value}");


        if (_HP <= 0)
        {
            //ゲームオーバーの処理をここ以降に!!
            Debug.Log("Game Over");
            ScoreManager.ScoreManagers.SaveHighScore();
            SceneManager.LoadScene("RetryScene");
            return;
        }

        //無敵時間
        StartCoroutine(InvincibleCoroutine());
    }

    IEnumerator InvincibleCoroutine() //無敵時間のコルーチン
    {
        _isInvincible = true;

        hitBox.enabled = false;

        if (_spriteRend != null) _spriteRend.StartBlink(_invincibleTime);

        yield return new WaitForSeconds(_invincibleTime); //動作の待機時間

        hitBox.enabled = true;

        _isInvincible = false;
    }

    private void _Move()
    {
        float speed = _isSlow ? _speed * _slowRate : _speed;

        Vector2 nextPos = _rigid.position + _inputVelocity * speed * Time.fixedDeltaTime;

        nextPos.x = Mathf.Clamp(nextPos.x, -8.3f, 0.8f);
        nextPos.y = Mathf.Clamp(nextPos.y, -4.5f, 4.4f);

        _rigid.MovePosition(nextPos);
    }

    private void _Shooting()
    {
        _shootCount += Time.deltaTime;
        if (_shootCount < _shootTime) return;

        if (_isFocusShot)
        {
            float offsetY = 0.8f;
            float offsetX = 0.05f;

            Vector3 basePos = transform.position + Vector3.up * offsetY;

            Instantiate(
                _player_bullet,
                basePos + Vector3.right * offsetX,
                Quaternion.identity
            );

            Instantiate(
                _player_bullet,
                basePos + Vector3.left * offsetX,
                Quaternion.identity
            );
        }
        else
        {
            // 通常ショット（左右）
            Instantiate(_player_bullet, _firePointL.position, _firePointL.rotation);
            Instantiate(_player_bullet, _firePointR.position, _firePointR.rotation);
        }

        _shootCount = 0f;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        _inputVelocity = context.ReadValue<Vector2>();
    }

    public void OnSlow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isSlow = true;
        }
        else if (context.canceled)
        {
            _isSlow = false;
        }
    }


    public int GetHP()
    {
        return _HP;
    }

    public void OnFocusShot(InputAction.CallbackContext context)
    {
        if (context.performed)
            _isFocusShot = true;
        else if (context.canceled)
            _isFocusShot = false;
    }

}
