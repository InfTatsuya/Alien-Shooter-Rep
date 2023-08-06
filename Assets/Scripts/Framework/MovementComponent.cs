using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] float turnSpeed = 30f;

    public float RotateToward(Vector3 aimDir)
    {
        float currentTurnSpeed = 0f;

        if (aimDir.magnitude > 0f)
        {
            Quaternion previousRot = transform.rotation;

            transform.rotation =
                Quaternion.Slerp(transform.rotation,
                                 Quaternion.LookRotation(aimDir, Vector3.up),
                                 turnSpeed * Time.deltaTime);

            Quaternion currentRot = transform.rotation;
            float dir = Vector3.Dot(aimDir, transform.right) > 0f ? 1 : -1;
            float rotationDelta = Quaternion.Angle(previousRot, currentRot) * dir;
            currentTurnSpeed = rotationDelta / Time.deltaTime;
        }

        return currentTurnSpeed;
    }
}
