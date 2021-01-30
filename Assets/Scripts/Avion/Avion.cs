using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avion : MonoBehaviour
{
    [SerializeField] private Transform leftEngine;
    [SerializeField] private Transform rightEngine;
    [SerializeField] private Camera cockpitCamera;
    [SerializeField] private Camera leftEngineCamera;
    [SerializeField] private Camera rightEngineCamera;
    [SerializeField] private Slider leftEngineThruster;
    [SerializeField] private Slider rightEngineThruster;
    [SerializeField] private Text liftForceText;
    [SerializeField] private float maxThrust;
    [SerializeField] private float minVelocityLiftForce;
    [SerializeField] private float maxLiftForce;
    [SerializeField] private float thrusterIncreaseSpeed;
    [SerializeField] private float rotationSpeed;

    private Rigidbody thisRigidbody;

    private Vector2 rotationInput;
    private float currentLeftEngineThrust;
    private float currentRightEngineThrust;
    private float currentLiftForce;

    private void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();

        rotationInput = new Vector2(0, 0);
        currentLeftEngineThrust = 0;
        currentRightEngineThrust = 0;
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

        if (Input.GetButtonDown("CockpitCamera"))
        {
            cockpitCamera.enabled = true;
            leftEngineCamera.enabled = false;
            rightEngineCamera.enabled = false;
        }

        if (Input.GetButtonDown("LeftEngineCamera"))
        {
            cockpitCamera.enabled = false;
            leftEngineCamera.enabled = true;
            rightEngineCamera.enabled = false;
        }

        if (Input.GetButtonDown("RightEngineCamera"))
        {
            cockpitCamera.enabled = false;
            leftEngineCamera.enabled = false;
            rightEngineCamera.enabled = true;
        }

        UpdateUI();
    }

    private void FixedUpdate()
    {
        // Rotation
        thisRigidbody.MoveRotation(thisRigidbody.rotation * Quaternion.Euler(rotationInput.y * rotationSpeed * Time.deltaTime, 0, -rotationInput.x * rotationSpeed * Time.deltaTime));

        // Thrusters
        thisRigidbody.AddForceAtPosition(transform.forward * currentLeftEngineThrust * maxThrust, leftEngine.position);
        thisRigidbody.AddForceAtPosition(transform.forward * currentRightEngineThrust * maxThrust, rightEngine.position);

        // Lift force
        if (Vector3.Dot(thisRigidbody.velocity, transform.forward) >= minVelocityLiftForce)
        {
            Debug.DrawRay(transform.position, Vector3.Project(thisRigidbody.velocity, transform.forward) * 100f);

            currentLiftForce = Mathf.Lerp(maxLiftForce, 0, Vector3.Angle(Vector3.Project(thisRigidbody.velocity, transform.forward), transform.forward) / 45f);
            thisRigidbody.AddForce(Vector3.up * currentLiftForce);
        }
    }

    private void UpdateUI()
    {
        liftForceText.text = currentLiftForce.ToString();
    }

    public void SetLeftEngineThrust(float value)
    {
        currentLeftEngineThrust = value;
    }

    public void SetRightEngineThrust(float value)
    {
        currentRightEngineThrust = value;
    }
}
