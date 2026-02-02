using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_GO : MonoBehaviour
{
    [SerializeField] GameObject marisaPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform[] phase1_MovePoints;
    [SerializeField] Transform[] phase3_MovePoints;
    [SerializeField] float startDelay = 2.0f;
    [SerializeField] BossHP bossHPUI;

    private bool started = false;

    void Start()
    {
        
    }

    public void StartBoss()
    {
        if (started) return;
        started = true;
        StartCoroutine(SpawnMarisa());
    }

    IEnumerator SpawnMarisa()
    {
        GameObject obj = Instantiate(
            marisaPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        Marisa marisa = obj.GetComponent<Marisa>();
        marisa.SetPhase1_MP(phase1_MovePoints);
        marisa.SetPhase3_MP(phase3_MovePoints);
        bossHPUI.Show();
        marisa.SetBossHPUI(bossHPUI);
        yield return new WaitForSeconds(startDelay);

    }

    // if (EnemyManager.Instance.AllEnemiesDefeated())
    // {
    //     bossSpawner.SpawnBoss();
    // }
}
