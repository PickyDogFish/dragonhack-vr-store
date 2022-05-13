using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDirection : MonoBehaviour
{
    [SerializeField]
    private Transform handTransform;
    public Vector3 handUp { get; private set;}
    public Vector3 handForward { get; private set;}

    // Update is called once per frame
    void Update()
    {
        handUp = handTransform.TransformDirection(Vector3.up);
        handForward = handTransform.TransformDirection(Vector3.forward);
    }
}
