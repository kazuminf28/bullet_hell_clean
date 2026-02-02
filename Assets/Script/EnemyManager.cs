using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] enemySpawnPoint;

    [Header("Boss")]
    [SerializeField] private Boss_GO bossGO;

    int aliveEnemyCount = 0;

    void Start()
    {
        StartCoroutine(SpawnFlow());
    }

    IEnumerator SpawnFlow()
    {
        yield return Spawn_1();
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => aliveEnemyCount <= 0);
        yield return Spawn_2();
        yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => aliveEnemyCount <= 0);
        yield return Spawn_3();
        yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => aliveEnemyCount <= 0);
        yield return new WaitForSeconds(2f);
        // ★ ボス開始
        bossGO.StartBoss();
    }

    IEnumerator Spawn_1()
    {
        int index = 0;
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemyPrefabs[index], enemySpawnPoint[0].position, Quaternion.identity);
            aliveEnemyCount++;
            yield return new WaitForSeconds(4f);
        }
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemyPrefabs[index + 1], enemySpawnPoint[1].position, Quaternion.identity);
            aliveEnemyCount++;
            yield return new WaitForSeconds(4f);
        }
    }

    IEnumerator Spawn_2()
    {
        int index = 2;
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemyPrefabs[index], enemySpawnPoint[3].position, Quaternion.identity);
            aliveEnemyCount++;
            yield return new WaitForSeconds(4f);
        }
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemyPrefabs[index + 1], enemySpawnPoint[2].position, Quaternion.identity);
            aliveEnemyCount++;
            yield return new WaitForSeconds(4f);
        }
    }

    IEnumerator Spawn_3()
    {
        int index = 0;
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemyPrefabs[index], enemySpawnPoint[0].position, Quaternion.identity);
            Instantiate(enemyPrefabs[index + 1], enemySpawnPoint[1].position, Quaternion.identity);
            aliveEnemyCount += 2;
            yield return new WaitForSeconds(3f);
        }
    }

    public void EnemyDie()
    {
        aliveEnemyCount--;
    }
}
