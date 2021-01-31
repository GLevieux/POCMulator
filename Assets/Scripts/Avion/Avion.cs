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
    [SerializeField] private float maxVelocity;
    [SerializeField] private float thrusterIncreaseSpeed;
    [SerializeField] private float verticalThrusterChangeSpeed;
    [SerializeField] private float rotationSpeed;

    private Rigidbody thisRigidbody;
    private UIManager uiManager;
    private MusicPlaylist musicPlaylist;

    private Vector2 rotationInput;
    private float currentLeftEngineThrust;
    private float currentRightEngineThrust;
    private float currentVerticalEngineThrust;

    private void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        uiManager = GetComponentInChildren<UIManager>();
        musicPlaylist = GetComponentInChildren<MusicPlaylist>();
        cockpitCamera.SetActive(true);
        leftEngineCamera.SetActive(false);
        rightEngineCamera.SetActive(false);
        verticalEngineCamera.SetActive(false);

        rotationInput = new Vector2(0, 0);
        currentLeftEngineThrust = 0;
        currentRightEngineThrust = 0;
        currentVerticalEngineThrust = 0;

        uiManager.UpdateLeftEngineThrusterIncrease(currentLeftEngineThrust);
        uiManager.UpdateLeftEngineThrusterDecrease(currentLeftEngineThrust);
        uiManager.UpdateRightEngineThrusterIncrease(currentRightEngineThrust);
        uiManager.UpdateRightEngineThrusterDecrease(currentRightEngineThrust);
        uiManager.UpdateVerticalEngineThrusterIncrease(currentVerticalEngineThrust);
        uiManager.UpdateVerticalEngineThrusterDecrease(currentVerticalEngineThrust);

        uiManager.UpdateForwardSpeed(0);
        uiManager.UpdateVerticalSpeed(0);
    }

    private void Update()
    {
        rotationInput.x = Input.GetAxis("Horizontal");
        rotationInput.y = Input.GetAxis("Vertical");

        if (Input.GetButton("LeftEngineThrusterIncrease"))
            currentLeftEngineThrust += thrusterIncreaseSpeed * Time.deltaTime;
        else if (currentLeftEngineThrust > 0)
            currentLeftEngineThrust -= thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("RightEngineThrusterIncrease"))
            currentRightEngineThrust += thrusterIncreaseSpeed * Time.deltaTime;
        else if (currentRightEngineThrust > 0)
            currentRightEngineThrust -= thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("LeftEngineThrusterDecrease"))
            currentLeftEngineThrust -= thrusterIncreaseSpeed * Time.deltaTime;
        else if (currentLeftEngineThrust < 0)
            currentLeftEngineThrust += thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("RightEngineThrusterDecrease"))
            currentRightEngineThrust -= thrusterIncreaseSpeed * Time.deltaTime;
        else if (currentRightEngineThrust < 0)
            currentRightEngineThrust += thrusterIncreaseSpeed * Time.deltaTime;

        if (Input.GetButton("VerticalThrusterIncrease"))
            currentVerticalEngineThrust += verticalThrusterChangeSpeed * Time.deltaTime;
        else if (currentVerticalEngineThrust > 0)
            currentVerticalEngineThrust -= verticalThrusterChangeSpeed * Time.deltaTime;

        if (Input.GetButton("VerticalThrusterDecrease"))
            currentVerticalEngineThrust -= verticalThrusterChangeSpeed * Time.deltaTime;
        else if (currentVerticalEngineThrust < 0)
            currentVerticalEngineThrust += verticalThrusterChangeSpeed * Time.deltaTime;

        currentLeftEngineThrust = Mathf.Clamp(currentLeftEngineThrust, -1, 1);
        currentRightEngineThrust = Mathf.Clamp(currentRightEngineThrust, -1, 1);
        currentVerticalEngineThrust = Mathf.Clamp(currentVerticalEngineThrust, -1, 1);

        if (currentLeftEngineThrust > 0)
            uiManager.UpdateLeftEngineThrusterIncrease(currentLeftEngineThrust);
        else
            uiManager.UpdateLeftEngineThrusterDecrease(-currentLeftEngineThrust);

        if (currentRightEngineThrust > 0)
            uiManager.UpdateRightEngineThrusterIncrease(currentRightEngineThrust);
        else
            uiManager.UpdateRightEngineThrusterDecrease(-currentRightEngineThrust);

        if (currentVerticalEngineThrust > 0)
            uiManager.UpdateVerticalEngineThrusterIncrease(currentVerticalEngineThrust);
        else
            uiManager.UpdateVerticalEngineThrusterDecrease(-currentVerticalEngineThrust);

        uiManager.SetOrientation(transform);
        uiManager.SetSongName(musicPlaylist.GetNameOfCurrentSong());

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

        ClampForwardVelocity();
        ClampVerticalVelocity();

        uiManager.UpdateForwardSpeed(GetForwardVelocityNormalized());
        uiManager.UpdateVerticalSpeed(GetVerticalVelocityNormalized());
    }

    private void ClampForwardVelocity()
    {
        if (GetForwardVelocity() >= maxVelocity)
        {
            thisRigidbody.velocity = (transform.forward * maxVelocity) + (transform.up * GetVerticalVelocity());
        }
        else if (GetForwardVelocity() <= -maxVelocity)
        {
            thisRigidbody.velocity = (-transform.forward * maxVelocity) + (transform.up * GetVerticalVelocity());
        }
    }

    private void ClampVerticalVelocity()
    {
        if (GetVerticalVelocity() >= maxVelocity)
        {
            thisRigidbody.velocity = (transform.forward * GetForwardVelocity()) + (transform.up * maxVelocity);
        }
        else if (GetVerticalVelocity() <= -maxVelocity)
        {
            thisRigidbody.velocity = (transform.forward * GetForwardVelocity()) + (-transform.up * maxVelocity);
        }
    }

    public float GetForwardVelocity()
    {
        return Vector3.Dot(thisRigidbody.velocity, transform.forward);
    }

    public float GetVerticalVelocity()
    {
        return Vector3.Dot(thisRigidbody.velocity, transform.up);
    }

    public float GetForwardVelocityNormalized()
    {
        return GetForwardVelocity() / maxVelocity;
    }

    public float GetVerticalVelocityNormalized()
    {
        return GetVerticalVelocity() / maxVelocity;
    }
}
