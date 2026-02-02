using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRend : MonoBehaviour
{
    SpriteRenderer _sr;
    Coroutine _blink;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void StartBlink(float time)
    {
        if (_blink != null) StopCoroutine(_blink);
        _blink = StartCoroutine(Blink(time));
    }

    IEnumerator Blink(float time)
    {
        float t = 0f;

        while (t < time)
        {
            _sr.enabled = !_sr.enabled;
            yield return new WaitForSeconds(0.1f);
            t += 0.1f;
        }

        _sr.enabled = true;
    }
}
