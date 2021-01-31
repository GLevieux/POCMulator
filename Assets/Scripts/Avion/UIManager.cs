using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider leftEngineThrusterIncrease;
    [SerializeField] private Slider leftEngineThrusterDecrease;
    [SerializeField] private Slider rightThrusterIncrease;
    [SerializeField] private Slider rightThrusterDecrease;
    [SerializeField] private Slider verticalThrusterIncrease;
    [SerializeField] private Slider verticalThrusterDecrease;

    [SerializeField] private Slider forwardSpeedPositive;
    [SerializeField] private Slider forwardSpeedNegative;
    [SerializeField] private Slider verticalSpeedPositive;
    [SerializeField] private Slider verticalSpeedNegative;

    [SerializeField] private Transform orientationTsfm;

    [SerializeField] private Text gameName;
    [SerializeField] private Text songName;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(ShowTitle());
    }

    void Update()
    {
        
    }

    private IEnumerator ShowTitle()
    {
        gameName.rectTransform.sizeDelta += new Vector2(10f * Time.deltaTime, 0);

        yield return null;

        if (gameName.rectTransform.rect.width < 60)
            StartCoroutine(ShowTitle());
        else
            StartCoroutine(HideTitle());
    }

    private IEnumerator HideTitle()
    {
        gameName.rectTransform.sizeDelta -= new Vector2(15f * Time.deltaTime, 0);

        yield return null;

        if (gameName.rectTransform.rect.width > 0)
            StartCoroutine(HideTitle());
    }

    public void SetOrientation(Transform avionTsfm)
    {
        orientationTsfm.localRotation = Quaternion.AngleAxis(180, Vector3.up) * avionTsfm.rotation;
    }

    public void SetSongName(string name)
    {
        songName.text = name;
    }

    public void UpdateLeftEngineThrusterIncrease(float speed)
    {
        leftEngineThrusterIncrease.value = speed;
    }

    public void UpdateLeftEngineThrusterDecrease(float speed)
    {
        leftEngineThrusterDecrease.value = speed;
    }

    public void UpdateRightEngineThrusterIncrease(float speed)
    {
        rightThrusterIncrease.value = speed;
    }

    public void UpdateRightEngineThrusterDecrease(float speed)
    {
        rightThrusterDecrease.value = speed;
    }

    public void UpdateVerticalEngineThrusterIncrease(float speed)
    {
        verticalThrusterIncrease.value = speed;
    }

    public void UpdateVerticalEngineThrusterDecrease(float speed)
    {
        verticalThrusterDecrease.value = speed;
    }

    public void UpdateForwardSpeed(float speed)
    {
        if (speed > 0)
            forwardSpeedPositive.value = speed;
        else if (speed < 0)
            forwardSpeedNegative.value = -speed;
        else
        {
            forwardSpeedPositive.value = 0;
            forwardSpeedNegative.value = 0;
        }
    }

    public void UpdateVerticalSpeed(float speed)
    {
        if (speed > 0)
            verticalSpeedPositive.value = speed;
        else if (speed < 0)
            verticalSpeedNegative.value = -speed;
        else
        {
            verticalSpeedPositive.value = 0;
            verticalSpeedNegative.value = 0;
        }
    }
}
