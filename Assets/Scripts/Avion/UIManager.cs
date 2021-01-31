using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider leftEngineThruster;
    [SerializeField] private Slider rightThruster;
    [SerializeField] private Slider verticalThruster;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateLeftEngineThruster(float speed)
    {
        leftEngineThruster.value = speed;
    }

    public void UpdateRightEngineThruster(float speed)
    {
        rightThruster.value = speed;
    }

    public void UpdateVerticalThruster(float speed)
    {
        verticalThruster.value = speed;
    }
}
