using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float duration = 1.0f;
    private bool start = false;
    public AnimationCurve animCurve;

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            start = false;
            StartCoroutine(ShakeCamera());
        }
    }

    public void Shake()
    {
        start = true;
    }


    IEnumerator ShakeCamera()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float magnitude = animCurve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * magnitude;
            yield return null;
        }
        transform.position = startPosition;
    }
}
