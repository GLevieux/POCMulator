using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SimpleSingle<CameraManager>
{
    public Camera walkCamera;
    public Camera flyCamera;
    public Camera skyCamera;
    public Camera UICockpitCamera;
    
    public enum CamMode
    {
        WALKING,
        FLYING
    };
    private CamMode mode;

    public void SetCamMode(CamMode mode)
    {  
        switch (mode)
        {
            case CamMode.WALKING:
                walkCamera.enabled = true;
                UICockpitCamera.enabled = false;
                flyCamera.enabled = false;
                break;
            case CamMode.FLYING:
                walkCamera.enabled = false;
                walkCamera.transform.position = flyCamera.transform.position;
                UICockpitCamera.enabled = true;
                flyCamera.enabled = true;
                break;
        }
        this.mode = mode;
        
    }
}
