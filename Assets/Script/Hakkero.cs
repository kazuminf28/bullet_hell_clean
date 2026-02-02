using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hakkero : MonoBehaviour
{
    [SerializeField] float chargeScale = 1.3f;
    [SerializeField] float chargeTime = 1.2f;

    Vector3 baseScale;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    public IEnumerator Charge()
    {
        gameObject.SetActive(true);

        float t = 0f;
        while (t < chargeTime)
        {
            t += Time.deltaTime;
            float p = t / chargeTime;
            // 脈打つ拡大
            float s = Mathf.Lerp(1f, chargeScale, Mathf.Sin(p * Mathf.PI));
            transform.localScale = baseScale * s;

            yield return null;
        }

        transform.localScale = baseScale;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
