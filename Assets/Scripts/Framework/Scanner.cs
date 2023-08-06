using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public delegate void OnScanDetectionUpdated(GameObject newDetection);
    public event OnScanDetectionUpdated onScanDetectionUpdated;

    [SerializeField] Transform scannerPivot;

    private float scanRange;
    private float scanDuration;

    public void SetScanRange(float scanRange)
    {
        this.scanRange = scanRange;
    }

    public void SetScanDuration(float scanDuration)
    {
        this.scanDuration = scanDuration;
    }

    public void AddChildAttached(Transform newChild)
    {
        newChild.SetParent(scannerPivot);
        newChild.localPosition = Vector3.zero;
    }

    public void StartScan()
    {
        scannerPivot.localScale = Vector3.zero;
        StartCoroutine(StartScanCoroutine());
    }

    private IEnumerator StartScanCoroutine()
    {
        float scanGrowRate = scanRange / scanDuration;
        float timer = 0f;

        while(timer < scanDuration)
        {
            timer += Time.deltaTime;

            scannerPivot.localScale += Vector3.one * scanGrowRate * Time.deltaTime;

            yield return null;
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        onScanDetectionUpdated?.Invoke(other.gameObject);
    }
}
