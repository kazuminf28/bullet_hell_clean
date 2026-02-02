using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_bullet : MonoBehaviour
{
    [SerializeField, Header("弾の速度")]
    private float _speed;

    [SerializeField, Header("弾の威力")]
    private int _power;

    private Rigidbody2D _rigid;
     
    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move(); 
    }

    private void _Move()
    {
        _rigid.velocity = transform.up * _speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        IDamageable enemy = other.GetComponent<IDamageable>();
        if (enemy != null)
        {
            enemy.Damage(_power);
        }

        Destroy(gameObject); // or プール回収
        // ReturnBullet(gameObject);
    }
}
