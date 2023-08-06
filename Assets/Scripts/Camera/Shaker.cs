using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField] float shakeMagtitude = 0.1f;
    [SerializeField] float shakeDuration = 0.1f;
    [SerializeField] float shakeRecoverySpeed = 10f;
    [SerializeField] Transform shakeTrans;

    private Vector3 originalPosition;

    private Coroutine shakeCoroutine;
    private bool isShaking;

    private void Start()
    {
        originalPosition = shakeTrans.localPosition;
        isShaking = false;
    }

    public void StartShaking()
    {
        if (!isShaking)
        {
            isShaking = true;
            shakeCoroutine = StartCoroutine(ShakeRoutine());
        }
    }

    private IEnumerator ShakeRoutine()
    {
        yield return new WaitForSeconds(shakeDuration);

        isShaking = false;
        shakeCoroutine = null;
    }

    private void LateUpdate()
    {
        DoShaking();
    }

    private void DoShaking()
    {
        if (isShaking)
        {
            Vector3 shakeAmt = new Vector3(Random.value, Random.value, Random.value) * 
                                        shakeMagtitude * (Random.value > 0.5f ? 1 : -1);
            shakeTrans.localPosition += shakeAmt;
        }
        else
        {
            shakeTrans.localPosition = originalPosition;              
        }
    }
}
