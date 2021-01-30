using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avion : MonoBehaviour
{
    [SerializeField] private Transform leftEngine;
    [SerializeField] private Transform rightEngine;
    [SerializeField] private Transform verticalEngine;
    [SerializeField] private GameObject cockpitCamera;
    [SerializeField] private GameObject leftEngineCamera;
    [SerializeField] private GameObject rightEngineCamera;
    [SerializeField] private GameObject verticalEngineCamera;
    [SerializeField] private Slider leftEngineThruster;
    [SerializeField] private Slider rightEngineThruster;
    [SerializeField] private Slider verticalEngineThruster;
    [SerializeField] private float maxThrust;
    [SerializeField] private float minVelocityLiftForce;
    [SerializeField] private float maxLiftForce;
    [SerializeField] private float thrusterIncreaseSpeed;
    [SerializeField] private float verticalThrusterChangeSpeed;
    [SerializeField] private float rotationSpeed;

    private Rigidbody thisRigidbody;

    private Vector2 rotationInput;
    private float currentLeftEngineThrust;
    private float currentRightEngineThrust;
    private float currentVerticalEngineThrust;
    private float currentLiftForce;

    private void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        cockpitCamera.SetActive(true);
        leftEngineCamera.SetActive(false);
        rightEngineCamera.SetActive(false);
        verticalEngineCamera.SetActive(false);

        rotationInput = new Vector2(0, 0);
        currentLeftEngineThrust = 0;
        currentRightEngineThrust = 0;
        currentVerticalEngineThrust = 0;
        currentLiftForce = 0;
    }

    private void Update()
    {
        rotationInput.x = Input.GetAxis("Horizontal");
        rotationInput.y = Input.GetAxis("Vertical");

        if (Input.GetButton("LeftEngineThrusterIncrease"))
            leftEngineThruster.value += thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("RightEngineThrusterIncrease"))
            rightEngineThruster.value += thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("LeftEngineThrusterDecrease"))
            leftEngineThruster.value -= thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("RightEngineThrusterDecrease"))
            rightEngineThruster.value -= thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("LiftThruster"))
            verticalEngineThruster.value += verticalThrusterChangeSpeed * Time.deltaTime;
        else
            verticalEngineThruster.value -= verticalThrusterChangeSpeed * Time.deltaTime;

        if (Input.GetButtonDown("CockpitCamera"))
        {
            cockpitCamera.SetActive(true);
            leftEngineCamera.SetActive(false);
            rightEngineCamera.SetActive(false);
            verticalEngineCamera.SetActive(false);
        }

        if (Input.GetButtonDown("LeftEngineCamera"))
        {
            cockpitCamera.SetActive(false);
            leftEngineCamera.SetActive(true);
            rightEngineCamera.SetActive(false);
            verticalEngineCamera.SetActive(false);
        }

        if (Input.GetButtonDown("RightEngineCamera"))
        {
            cockpitCamera.SetActive(false);
            leftEngineCamera.SetActive(false);
            rightEngineCamera.SetActive(true);
            verticalEngineCamera.SetActive(false);
        }

        if (Input.GetButtonDown("LiftEngineCamera"))
        {
            cockpitCamera.SetActive(false);
            leftEngineCamera.SetActive(false);
            rightEngineCamera.SetActive(false);
            verticalEngineCamera.SetActive(true);
        }

        UpdateUI();
    }

    private void FixedUpdate()
    {
        // Thrusters
        thisRigidbody.AddForceAtPosition(transform.forward * currentLeftEngineThrust * maxThrust, leftEngine.position);
        thisRigidbody.AddForceAtPosition(transform.forward * currentRightEngineThrust * maxThrust, rightEngine.position);

        // Vertical thrusters
        thisRigidbody.AddForce(transform.up * currentVerticalEngineThrust * maxThrust);

        // Rotation
        thisRigidbody.MoveRotation(thisRigidbody.rotation * Quaternion.Euler(rotationInput.y * rotationSpeed * Time.deltaTime, 0, -rotationInput.x * rotationSpeed * Time.deltaTime));
    }

    private void UpdateUI()
    {
        
    }

    public void SetLeftEngineThrust(float value)
    {
        currentLeftEngineThrust = value;
    }

    public void SetRightEngineThrust(float value)
    {
        currentRightEngineThrust = value;
    }

    public void SetVerticalEngineThrust(float value)
    {
        currentVerticalEngineThrust = value;
    }
}
