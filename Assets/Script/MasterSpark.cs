using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSpark : MonoBehaviour
{
    [SerializeField] Transform laser;
    [SerializeField] float maxLength = 20f;
    [SerializeField] float expandSpeed = 35f;
    [SerializeField] float duration = 2.5f;
    [SerializeField] float width = 1.5f;

    public void Fire()
    {
        StartCoroutine(LaserRoutine());
    }

    IEnumerator LaserRoutine()
    {
        float length = 0f;

        // 初期化
        laser.localPosition = Vector3.zero;

        Vector3 scale = laser.localScale;
        scale.x = width;
        scale.y = 0f;
        laser.localScale = scale;

        // 位置補正は一切しない！
        while (length < maxLength)
        {
            length += expandSpeed * Time.deltaTime;
            scale.y = length;
            laser.localScale = scale;
            yield return null;
        }

        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}