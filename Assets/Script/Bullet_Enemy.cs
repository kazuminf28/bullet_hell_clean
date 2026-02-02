using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Enemy : MonoBehaviour
{
    public int damage;

    void OnEnable()
    {
        // Bullet コンポーネントから威力を取得
        Bullet b = GetComponent<Bullet>();
        damage = (int)b.power;
    }

    void OnTriggerEnter2D(Collider2D other) { 
        player player = other.GetComponent<player>();
        if (player == null) return;
        player._Damage(damage);
        Destroy(gameObject);
    }
}
