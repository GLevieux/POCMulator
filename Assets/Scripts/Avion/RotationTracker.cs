using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTracker : MonoBehaviour
{
    public Transform trackItsRotation;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = trackItsRotation.rotation;
    }
}
