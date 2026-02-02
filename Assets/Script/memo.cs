using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class memo : MonoBehaviour
{
        // ① 渦巻き弾
    IEnumerator SpiralShot()
    {
        #region rigibody_true
        // float angle = 0f;

        // for (int i = 0; i < 120; i++)
        // {
        //     angle += 10f;
        //     Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        //     // Instantiate(bulletPrefab2, firePoint.position, rot);
        //     GameObject obj = BulletManager.Instance.GetBullet(bulletPrefab2, firePoint.position, rot);
        //     Bullet b = obj.GetComponent<Bullet>();
        //     b.changeSpeed = false;
        //     yield return new WaitForSeconds(0.05f);
        // }
        #endregion
        #region rigibody_false
        // float angle = 0f;

        // for (int i = 0; i < 120; i++)
        // {
        //     angle += 10f;
        //     float rad = angle * Mathf.Deg2Rad;

        //     Vector2 dir = new Vector2(
        //         Mathf.Cos(rad),
        //         Mathf.Sin(rad)
        //     );

        //     GameObject obj = BulletManager.Instance.GetBullet(
        //         bulletPrefab2,
        //         firePoint.position,
        //         Quaternion.identity
        //     );

        //     obj.GetComponent<Bullet>().Fire(dir);

            yield return new WaitForSeconds(0.05f);
        // }
        #endregion
    }

    // ② 全方位20方向弾
    IEnumerator RadialBurst()
    {
        #region rigibody_true
        // for (int b = 0; b < 5; b++)
        // {
        //     int bulletDirCount = 20;
        //     for (int i = 0; i < bulletDirCount; i++)
        //     {
        //         float angle = i * (360f / bulletDirCount);
        //         Quaternion rot = Quaternion.Euler(0, 0, angle);
        //         // Instantiate(bulletPrefab1, firePoint.position, rot);
        //         GameObject obj = BulletManager.Instance.GetBullet(bulletPrefab1, firePoint.position, rot);
        //         Bullet bl = obj.GetComponent<Bullet>();
        //         bl.changeSpeed = false;
        //     }
        //     yield return new WaitForSeconds(1f);
        // }
        #endregion

        #region rigibody_false
        // int count = 20;          // ★ ここで宣言（外に出す）
        // for (int b = 0; b < 5; b++)
        // {
        //     for (int i = 0; i < count; i++)
        //     {
        //         float angle = i * (360f / count);
        //         float rad = angle * Mathf.Deg2Rad;

        //         Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        //         GameObject obj = BulletManager.Instance.GetBullet(
        //             bulletPrefab1,
        //             firePoint.position,
        //             Quaternion.identity
        //         );
        //         obj.GetComponent<Bullet>().Fire(dir);
        //     }
            yield return new WaitForSeconds(1f);
        // }
        #endregion
    }

    // ③ 乱数方向にバラ撒き
    IEnumerator RandomSpread()
    {
        #region rigibody_true
        // for (int i = 0; i < 100; i++)
        // {
        //     float angle = Random.Range(0f, 360f);
        //     Quaternion rot = Quaternion.Euler(0, 0, angle);
        //     // Instantiate(bulletPrefab1, firePoint.position, rot);
        //     GameObject obj = BulletManager.Instance.GetBullet(bulletPrefab1, firePoint.position, rot);
        //     Bullet b = obj.GetComponent<Bullet>();
        //     b.changeSpeed = false;
        //     yield return new WaitForSeconds(0.03f);
        // }
        #endregion
        #region rigibody_false

        // for (int i = 0; i < 100; i++)
        // {
        //     float angle = Random.Range(0f, 360f);
        //     float rad = angle * Mathf.Deg2Rad;

        //     Vector2 dir = new Vector2(
        //         Mathf.Cos(rad),
        //         Mathf.Sin(rad)
        //     );

        //     GameObject obj = BulletManager.Instance.GetBullet(
        //         bulletPrefab1,
        //         firePoint.position,
        //         Quaternion.identity
        //     );

        //     obj.GetComponent<Bullet>().Fire(dir);

            yield return new WaitForSeconds(0.03f);
        // }
        #endregion
    }


}
