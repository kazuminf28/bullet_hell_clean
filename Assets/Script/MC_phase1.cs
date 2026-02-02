using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MC_phase1 : MonoBehaviour
{
    [Header("bulletPrefab")]
    public GameObject bulletPrefab;

    public float shootInterval = 0.1f;

    public float traceTime = 0.8f;
    public float traceSpeed = 1.5f;
    public float accelSpeed = 8f;

    Coroutine shootCoroutine;

    public void StartShoot()
    {
        shootCoroutine = StartCoroutine(Shoot());
    }

    public void StopShoot()
    {
        if (shootCoroutine != null)
            StopCoroutine(shootCoroutine);
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            Vector2 radial =
                (transform.position - transform.parent.position).normalized;

            Vector2 tangent = new Vector2(-radial.y, radial.x);
            // ① なぞり弾
            GameObject b1 = BulletManager.Instance.GetBullet(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

            Bullet bullet1 = b1.GetComponent<Bullet>();
            bullet1.ResetState();
            bullet1.SetSpeed(traceSpeed);
            bullet1.Fire(tangent);
            StartCoroutine(TraceBullet(bullet1));

            GameObject b2 = BulletManager.Instance.GetBullet(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

            Bullet bullet2 = b2.GetComponent<Bullet>();
            bullet2.ResetState();
            bullet2.SetSpeed(accelSpeed);
            bullet2.Fire(radial); 

            yield return new WaitForSeconds(shootInterval);
        }
    }

    IEnumerator TraceBullet(Bullet bullet)
    {
        float timer = 0f;

        while (timer < traceTime && bullet != null)
        {
            // Vector2 dir =
            //     ((Vector2)transform.position -
            //      (Vector2)bullet.transform.position).normalized;

            // bullet.SetDirection(dir);
            bullet.SetSpeed(traceSpeed);

            timer += Time.deltaTime;
            yield return null;
        }

        // なぞり終了 → 加速
        
        Vector2 finalDir =
            ((Vector2)transform.position -
            (Vector2)transform.parent.position).normalized;
        bullet.SetDirection(finalDir);
        bullet.SetSpeed(accelSpeed);
        bullet.changeSpeed = false;
    }
}