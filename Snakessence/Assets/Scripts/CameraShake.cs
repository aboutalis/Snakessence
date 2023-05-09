using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 50f;

    private Vector3 originalPosition;

    public IEnumerator Shake(float duration)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    void Start()
    {
        originalPosition = transform.localPosition;
    }
}
