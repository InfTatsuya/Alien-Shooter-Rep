using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraArm : MonoBehaviour
{
    [SerializeField] float armLength = 5f;
    [SerializeField] Transform child;


    void Update()
    {
        child.position = transform.position - child.forward * armLength;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, child.position);
    }
}
