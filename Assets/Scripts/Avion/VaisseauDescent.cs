using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaisseauDescent : MonoBehaviour
{
    private Rigidbody thisRigidbody;
    private UIManager uiManager;
    private MusicPlaylist musicPlaylist;
    public Transform playerBodyBase;
    private Transform playerBody = null;

    //Key per engine (thurst is opposite)
    private KeyCode kBack = KeyCode.Z;
    private KeyCode kFront = KeyCode.S;
    private KeyCode kLeftPerp = KeyCode.D;
    private KeyCode kRightPerp = KeyCode.Q;
    private KeyCode kBottom = KeyCode.E;
    private KeyCode kUp = KeyCode.A;
    private KeyCode kBurst = KeyCode.LeftShift;
    private KeyCode kLeaveVaisseau = KeyCode.R;

    private float forceBack = 0;
    private float forceFront = 0;
    private float forceLeftPerp = 0;
    private float forceRightPerp = 0;
    private float forceBottom = 0;
    private float forceUp = 0;

    private float autoAvoidForce = 0;

    private bool lockVaisseau = false;
    private bool vaisseauDocked = false;


    private KeyCode kSlower = KeyCode.Space;

    [Range(0.0f, 1.0f)]
    public float movePower = 1;
    [Range(0.0f, 1.0f)]
    public float nosePower = 1;

    [Range(0.0f, 1.0f)]
    public float dragNoBreak = 0.1f;
    [Range(0.0f, 1.0f)]
    public float dragAutoBreak = 0.8f;

    public float maxSpeed = 500;
    private float realMaxSpeed = 500;
    public float maxAngularSpeed = 0.7f;

    Vector2 noseInput;

    float distanceToColliderBottom = 0;

    public void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        uiManager = FindObjectOfType<UIManager>();
        musicPlaylist = GetComponentInChildren<MusicPlaylist>();
        distanceToColliderBottom = GetComponent<BoxCollider>().size.y / 2 - GetComponent<BoxCollider>().center.y;
    }


    public void UpdateUI()
    {
        uiManager.SetSongName(musicPlaylist.GetNameOfCurrentSong());

        uiManager.UpdateLeftEngineThrusterIncrease(forceLeftPerp);
        uiManager.UpdateLeftEngineThrusterDecrease(forceBack);
        uiManager.UpdateRightEngineThrusterIncrease(forceRightPerp);
        uiManager.UpdateRightEngineThrusterDecrease(forceBack);
        uiManager.UpdateVerticalEngineThrusterIncrease(forceBottom);
        uiManager.UpdateVerticalEngineThrusterDecrease(forceUp);

        uiManager.UpdateForwardSpeed(GetForwardVelocity()/ maxSpeed);
        uiManager.UpdateVerticalSpeed(GetVerticalVelocity() / maxSpeed);
    }

    public void Update()
    {
        UpdateUI();

        if (uiManager.CursorLocked)
        {
            noseInput += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        forceBack = Mathf.Clamp01(forceBack + (Input.GetKey(kBack) && !lockVaisseau ? 1 : -10) * Time.deltaTime);
        forceFront = Mathf.Clamp01(forceFront + (Input.GetKey(kFront) && !lockVaisseau ? 1 : -10) * Time.deltaTime);
        forceLeftPerp = Mathf.Clamp01(forceLeftPerp + (Input.GetKey(kLeftPerp) && !lockVaisseau ? 1 : -10) * Time.deltaTime);
        forceRightPerp = Mathf.Clamp01(forceRightPerp + (Input.GetKey(kRightPerp) && !lockVaisseau ? 1 : -10) * Time.deltaTime);
        forceBottom = Mathf.Clamp01(forceBottom + (Input.GetKey(kBottom) && !lockVaisseau ? 1 : -10) * Time.deltaTime);
        forceUp = Mathf.Clamp01(forceUp + (Input.GetKey(kUp) && !lockVaisseau ? 1 : -10) * Time.deltaTime);

        if (Input.GetKeyDown(kLeaveVaisseau))
        {
            if (!lockVaisseau && vaisseauDocked)
            {
                playerBody = GameObject.Instantiate<Transform>(playerBodyBase);
                playerBody.position = transform.position + transform.right * 10;
                playerBody.rotation = transform.rotation;
                playerBody.GetComponent<Controller>().SetWantedDirection(transform.forward);

                CameraManager.sharedInstance.walkCamera.GetComponent<FirstPersonView>().enabled = true;
                CameraManager.sharedInstance.walkCamera.GetComponent<FirstPersonView>().MyBody = playerBody.GetComponent<Body>();
                CameraManager.sharedInstance.walkCamera.GetComponent<FirstPersonView>().MyController = playerBody.GetComponent<Controller>();
                CameraManager.sharedInstance.SetCamMode(CameraManager.CamMode.WALKING);
                lockVaisseau = true;
            }
            else if(Vector3.Distance(playerBody.transform.position,transform.position) < 30)
            {
                CameraManager.sharedInstance.walkCamera.GetComponent<FirstPersonView>().enabled = false;
                CameraManager.sharedInstance.SetCamMode(CameraManager.CamMode.FLYING);
                Destroy(playerBody.gameObject);
                lockVaisseau = false;
            }
            
        }
    }

    public void FixedUpdate()
    {
           
        float forwardPush = Input.GetKey(kBurst) ? 800 : 500;
        float speed = thisRigidbody.velocity.magnitude;

        if (Input.GetKey(kBurst))
            realMaxSpeed = Mathf.Min(maxSpeed * 1.5f, realMaxSpeed + Time.deltaTime * 500);
        else
            realMaxSpeed = Mathf.Max(maxSpeed, realMaxSpeed - Time.deltaTime * 500);

        
        //Deplacement 
        thisRigidbody.AddForce(forceBack * transform.forward * movePower * forwardPush * thisRigidbody.mass);
        thisRigidbody.AddForce(-forceFront * transform.forward * movePower * 200 * thisRigidbody.mass);
        thisRigidbody.AddForce(forceLeftPerp * transform.right * movePower * 200 * thisRigidbody.mass);
        thisRigidbody.AddForce(-forceRightPerp * transform.right * movePower * 200 * thisRigidbody.mass);
        thisRigidbody.AddForce(forceBottom * transform.up * movePower * 300 * thisRigidbody.mass);
        thisRigidbody.AddForce(-forceUp * transform.up * movePower * 300 * thisRigidbody.mass);

        float moveForces = forceBack + forceFront + forceLeftPerp + forceRightPerp + forceBottom + forceUp;
        
        //Nose
        if ((Cursor.lockState == CursorLockMode.Locked || uiManager.CursorLocked ) && !lockVaisseau)
        {
            if (Input.GetMouseButton(0))
                thisRigidbody.AddTorque(transform.forward * nosePower * 50 * thisRigidbody.mass);
            if (Input.GetMouseButton(1))
                thisRigidbody.AddTorque(-transform.forward * nosePower * 50 * thisRigidbody.mass);

            //Stabilisateur
            if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            {
                float upAlign = Vector3.Dot(transform.up, Vector3.up);
                float rotCorrection = 1 - upAlign;
                float sensCorrection = -Mathf.Sign(Vector3.Dot(transform.right, Vector3.up));
                float rotCorrecSpeedCur = Vector3.Dot(transform.forward, thisRigidbody.angularVelocity);
                
                if (rotCorrection < 0.02f)
                {
                    if(Mathf.Sign(rotCorrecSpeedCur) != sensCorrection ||  
                       (Mathf.Sign(rotCorrecSpeedCur) == sensCorrection && Mathf.Abs(rotCorrecSpeedCur) < Mathf.Pow(rotCorrection,0.1f)))
                        thisRigidbody.AddTorque(transform.forward * 300 * thisRigidbody.mass * rotCorrection * sensCorrection);
                }
            }

            if (noseInput.magnitude > 1)
                noseInput = noseInput.normalized * 1.5f;

            thisRigidbody.AddTorque(transform.up * nosePower * 30 * thisRigidbody.mass * noseInput.x);
            thisRigidbody.AddTorque(-transform.right * nosePower * 30 * thisRigidbody.mass * noseInput.y);
            noseInput = Vector2.zero;
        }

        //Damp
        if (Input.GetKey(kSlower))
        {
            thisRigidbody.drag = dragAutoBreak * 2.0f;
            thisRigidbody.angularDrag = dragAutoBreak * 2.0f;
        }
        else
        {
            thisRigidbody.drag = dragNoBreak * 2.0f;
            thisRigidbody.angularDrag = dragNoBreak * 2.0f;
        }

        //Limit
        
        if (speed > realMaxSpeed)
        {
            thisRigidbody.velocity = thisRigidbody.velocity.normalized * realMaxSpeed;
            speed = realMaxSpeed;
        }

        if (thisRigidbody.angularVelocity.sqrMagnitude > maxAngularSpeed * maxAngularSpeed)
        {
            thisRigidbody.angularVelocity = thisRigidbody.angularVelocity.normalized * maxAngularSpeed;
        }

        //Atterissage
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.SphereCast(new Ray(transform.position,-transform.up), 2.0f, out hitInfo))
        {
            Vector3 normal;
            float normVariability;
            GetMeanGroundNormal(out normal, out normVariability);

            float distToGround = Mathf.Max(0,hitInfo.distance - distanceToColliderBottom);
            float align = 0;
            float distAutoAlign = 100;
            if (distToGround < distAutoAlign && normVariability < 0.2)
            {
                float unaligned = 1- Vector3.Dot(normal, transform.up);
                if (unaligned > float.Epsilon)
                {
                    align = 100 * unaligned * 1-(distToGround / distAutoAlign);
                    Vector3 vecRot = Vector3.Cross(transform.up, hitInfo.normal).normalized;
                    float currentRot = Vector3.Dot(thisRigidbody.angularVelocity, vecRot);
                    thisRigidbody.AddTorque(vecRot * (align - currentRot) * thisRigidbody.mass);
                    
                }
            }

            float breaking = 0;
            float speedTowardSurface = Vector3.Dot(-hitInfo.normal, thisRigidbody.velocity);

            RaycastHit hitInfoOneSec = new RaycastHit();
            Physics.SphereCast(new Ray(transform.position + thisRigidbody.velocity, -transform.up), 2.0f, out hitInfoOneSec);
            float distToGroundInOneSec = Mathf.Max(0, hitInfoOneSec.distance - distanceToColliderBottom);
            float distAutoAvoid = 150;
            float mindDistGround = Mathf.Min(distToGround, distToGroundInOneSec);
            if (mindDistGround < distAutoAvoid && speedTowardSurface > 0)
            {                
                autoAvoidForce = Mathf.Clamp01(autoAvoidForce+Time.deltaTime/2.0f) * 290;
                breaking = (1-(mindDistGround / distAutoAvoid)) * thisRigidbody.mass * autoAvoidForce;
                thisRigidbody.AddForce(hitInfo.normal * breaking);
            }
            else
            {
                autoAvoidForce = 0;
            }

            if (distToGround <  10 && speed < 1)
            {
                if(moveForces - forceUp < float.Epsilon)
                {
                    thisRigidbody.angularDrag = 1000;
                    thisRigidbody.drag = 1000;
                    vaisseauDocked = true;
                }
                else
                {
                    vaisseauDocked = false;
                }
            }

            Debug.Log(distToGround + " : align = " + align + " autoavoid = " + breaking + " normvar = " + normVariability + " maxSpeed = "+ realMaxSpeed);
        }
    }

    private bool GetMeanGroundNormal(out Vector3 normal, out float variability)
    {
        float steps = 2;
        float largeurScan = 30;
        float scanStep = largeurScan / (steps*2);
        variability = 0;

        RaycastHit hitinfo = new RaycastHit();
        normal = Vector3.zero;
        float nbNorm = 0;
        for (float x = -steps ; x <= steps ; x++)
        {
            for (float y = -steps ; y <= steps ; y++)
            {
                Vector3 startPos = transform.position + transform.right * x * scanStep;
                startPos += transform.forward * y * scanStep;
                
                Debug.DrawLine(startPos, startPos  - transform.up * 5, Color.gray);

                if (Physics.SphereCast(new Ray(startPos, -transform.up), 2.0f, out hitinfo))
                {
                    if (normal.sqrMagnitude > float.Epsilon)
                        variability += 1-Vector3.Dot(normal.normalized, hitinfo.normal);
                    normal += hitinfo.normal;
                    nbNorm++;
                }
            }
        }
        
        if(nbNorm > 0)
        {
            normal = normal.normalized;
            variability /= nbNorm;
            //Debug.DrawLine(transform.position, transform.position + normal * 5,Color.Lerp(Color.green,Color.red,variability));
            return true;
        }

        return false;
    }

    private float GetForwardVelocity()
    {
        return Vector3.Dot(thisRigidbody.velocity, transform.forward);
    }

    private float GetVerticalVelocity()
    {
        return Vector3.Dot(thisRigidbody.velocity, transform.up);
    }

}


