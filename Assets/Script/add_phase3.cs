using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class add_phase3 : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] GameObject[] bullets;

    [Header("Pattern")]
    [SerializeField] int dc = 9;
    [SerializeField] float shootInterval = 1f;
    [SerializeField] float bulletSpeed = 3.5f;
    [SerializeField] float rotateSpeed = 950f;

    float baseAngle;
    int count = 0;
    Coroutine shootCoroutine;

    void Awake()
    {
        StopShoot();   // 最初は撃たない
    }
    public void StartShoot()
    {
        baseAngle = 0f;
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
            StartCoroutine(Rotate());
            yield return new WaitForSeconds(shootInterval);
        }
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            baseAngle += rotateSpeed * Time.deltaTime;
            yield return null;
        }
    }

    void FireOnce()
    {
        if (count >= bullets.Length) count = 0;
        Vector3 origin = transform.position;
        for (int i = 0; i < dc; i++)
        {
            GameObject prefab = bullets[count];
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
        }
        count++;
    }
}