using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class MilkyWay_phase2 : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] GameObject bulletA;
    [SerializeField] GameObject bulletB;

    [Header("Pattern")]
    [SerializeField] int dc = 9;
    [SerializeField] float shootInterval = 0.12f;
    [SerializeField] float bulletSpeed = 3.5f;
    [SerializeField] float rotateSpeed = 40f;

    float baseAngle;
    bool shootA = true;
    Coroutine shootCoroutine;

    void OnEnable()
    {
        StartShoot();
    }

    void OnDisable()
    {
        StopShoot();
    }
    public void StartShoot()
    {
        baseAngle = 0f;
        shootA = true;
        if (shootCoroutine != null)
            StopCoroutine(shootCoroutine);

        shootCoroutine = StartCoroutine(Shoot());
    }

    public void StopShoot()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            FireOnce();
            baseAngle += rotateSpeed * Time.deltaTime;
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void FireOnce()
    {
        Vector3 origin = transform.position;
        for (int i = 0; i < dc; i++)
        {
            GameObject prefab = shootA ? bulletA : bulletB;
            float angle = baseAngle + i * (360f / dc);
            float rad = angle * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject obj = BulletManager.Instance.GetBullet(
                prefab,
                origin,
                Quaternion.identity
            );

            Bullet b = obj.GetComponent<Bullet>();
            b.ResetState();
            b.SetSpeed(bulletSpeed);
            b.Fire(dir);
            
            shootA = !shootA;
        }
    }
}
