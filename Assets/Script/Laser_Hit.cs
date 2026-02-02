using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Hit : MonoBehaviour
{
    [SerializeField] int damage = 2;

    void OnTriggerStay2D(Collider2D other)
    {
        // if (!other.CompareLayer("Player")) return;
        // または
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        player players = other.GetComponent<player>();
        if (players == null) return;

        players._Damage(damage);
    }
}
