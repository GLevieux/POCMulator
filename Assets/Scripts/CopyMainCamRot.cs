using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMainCamRot : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        GetComponent<Camera>().fieldOfView = Camera.main.fieldOfView;
    }
}
