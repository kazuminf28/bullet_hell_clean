using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance;

    [System.Serializable]
    public class BulletPool
    {
        public GameObject prefab;
        public int initialSize = 50;
        [HideInInspector] public Queue<GameObject> pool = new Queue<GameObject>();
    }

    public List<BulletPool> bulletPools = new List<BulletPool>();
    private Dictionary<GameObject, BulletPool> prefabToPool = new Dictionary<GameObject, BulletPool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 全弾種のプールを作る
        foreach (var bp in bulletPools)
        {
            prefabToPool[bp.prefab] = bp;

            for (int i = 0; i < bp.initialSize; i++)
            {
                GameObject obj = Instantiate(bp.prefab);
                obj.SetActive(false);

                // どのプレハブか覚えさせる
                obj.GetComponent<Bullet>().originalPrefab = bp.prefab;

                bp.pool.Enqueue(obj);
            }
        }
    }

    // 弾を借りる
    public GameObject GetBullet(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        BulletPool pool = prefabToPool[prefab];
        GameObject obj;

        if (pool.pool.Count > 0)
        {
            obj = pool.pool.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
            obj.GetComponent<Bullet>().originalPrefab = prefab;
        }

        obj.transform.position = pos;
        obj.transform.rotation = rot;
        Bullet b = obj.GetComponent<Bullet>();
        b.ResetState();//初期速度に!!
        obj.SetActive(true);

        return obj;
    }

    // 弾を返す
    public void ReturnBullet(GameObject obj)
    {
        obj.SetActive(false);
        Bullet bullet = obj.GetComponent<Bullet>();
        prefabToPool[bullet.originalPrefab].pool.Enqueue(obj);
    }

    // 弾幕の全削除
    public void ClearAllBullets()
    {
        Bullet[] bullets = FindObjectsOfType<Bullet>();

        foreach (var b in bullets)
        {
            if (b.gameObject.activeSelf)
            {
                ReturnBullet(b.gameObject);
            }
        }
    }
}