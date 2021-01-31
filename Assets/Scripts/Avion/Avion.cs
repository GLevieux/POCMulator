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
    [SerializeField] private float maxThrust;
    [SerializeField] private float minVelocityLiftForce;
    [SerializeField] private float maxLiftForce;
    [SerializeField] private float thrusterIncreaseSpeed;
    [SerializeField] private float verticalThrusterChangeSpeed;
    [SerializeField] private float rotationSpeed;

    private Rigidbody thisRigidbody;
    private UIManager uiManager;

    private Vector2 rotationInput;
    private float currentLeftEngineThrust;
    private float currentRightEngineThrust;
    private float currentVerticalEngineThrust;

    private void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        uiManager = GetComponentInChildren<UIManager>();
        cockpitCamera.SetActive(true);
        leftEngineCamera.SetActive(false);
        rightEngineCamera.SetActive(false);
        verticalEngineCamera.SetActive(false);

        rotationInput = new Vector2(0, 0);
        currentLeftEngineThrust = 0;
        currentRightEngineThrust = 0;
        currentVerticalEngineThrust = 0;
    }

    private void Update()
    {
        rotationInput.x = Input.GetAxis("Horizontal");
        rotationInput.y = Input.GetAxis("Vertical");

        if (Input.GetButton("LeftEngineThrusterIncrease"))
            currentLeftEngineThrust += thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("RightEngineThrusterIncrease"))
            currentRightEngineThrust += thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("LeftEngineThrusterDecrease"))
            currentLeftEngineThrust -= thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("RightEngineThrusterDecrease"))
            currentRightEngineThrust -= thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("LiftThruster"))
            currentVerticalEngineThrust += verticalThrusterChangeSpeed * Time.deltaTime;
        else
            currentVerticalEngineThrust -= verticalThrusterChangeSpeed * Time.deltaTime;

        currentLeftEngineThrust = Mathf.Clamp01(currentLeftEngineThrust);
        currentRightEngineThrust = Mathf.Clamp01(currentRightEngineThrust);
        currentVerticalEngineThrust = Mathf.Clamp01(currentVerticalEngineThrust);

        uiManager.UpdateLeftEngineThruster(currentLeftEngineThrust);
        uiManager.UpdateRightEngineThruster(currentRightEngineThrust);
        uiManager.UpdateVerticalThruster(currentVerticalEngineThrust);

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
}
