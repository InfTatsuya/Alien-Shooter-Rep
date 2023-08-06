using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTransform;
    [SerializeField] float turnSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.position = followTransform.position;
    }

    public void AddYawInput(float yawAmt)
    {
        transform.Rotate(Vector3.up, yawAmt * Time.deltaTime * turnSpeed);
    }
}
