using System.Collections;
using System.Collections.Generic;
using JustPtrck.Shaders.Water;
using UnityEngine;

public class PeddalBoatVR : MonoBehaviour
{
    [Header("Boat Parameters")]
    [SerializeField] private float speedMod = 10f;   // m/s^2
    //[SerializeField] private float maxVelocity = 5f;    // m/s 
    [SerializeField, Range(0, 90)] private float rudderMaxAngle = 40f;
    [SerializeField] private float rotationSpeed = 2f;

    [Header("Transform References")]
    [SerializeField] private Transform rudder;
    [SerializeField] private Transform motor;
    [SerializeField] private Floater floater;


    [Header("Paddel References")]
    [SerializeField] private float SteeringAngle;
    [SerializeField] private float PedalSpeed;
    [SerializeField] private Transform LeftPaddelAxis;
    [SerializeField] private Transform LeftPaddelPoint;

    [SerializeField] private float LeftPaddelAngle;

    [SerializeField] private Transform RightPaddelAxis;
    [SerializeField] private Transform RightPaddelPoint;
    [SerializeField] private float RightPaddelAngle;
    [Header("VR Information")]
    [SerializeField] private XRInputTranslator xrInput;
    [SerializeField] private PedalBounds pedalBounds;
    [SerializeField] private GameObject LeftVRControlPoint;
    [SerializeField] private GameObject RightVRControlPoint;
    private Vector3 lastLeftVelocity;
    private Vector3 lastRightVelocity;
    [Header("Sound Manager")]
    [SerializeField] private RowingSoundManager rowingSoundManager;
    
    private Rigidbody rb;
    //private float rudderAngle;
    private float motorForce;
    // Start is called before the first frame update
    private void OnEnable() {
        rb = gameObject.GetComponent<Rigidbody>() ? gameObject.GetComponent<Rigidbody>() : gameObject.AddComponent<Rigidbody>();
        floater = gameObject.GetComponent<Floater>() ? gameObject.GetComponent<Floater>() : gameObject.AddComponent<Floater>();
        RightPaddelPoint = RightPaddelAxis.GetChild(0).GetChild(0).transform;
        LeftPaddelPoint = LeftPaddelAxis.GetChild(0).GetChild(0).transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rudderAngle = 0;
        //ClampTransform(LeftPaddelAxis.transform, LeftPaddelPoint.transform, new Vector3(0,0,0), new Vector3(40,40,40));
        //ClampTransform(RightPaddelAxis.transform, RightVRControlPoint.transform, new Vector3(320,120,0), new Vector3(360,240,0));
        //Debug.Log(LeftPaddelPoint.transform);
        if(pedalBounds.isPedalInBox(LeftVRControlPoint.transform))
            LeftPaddelAxis.transform.LookAt(LeftVRControlPoint.transform);
        if(pedalBounds.isPedalInBox(RightVRControlPoint.transform))
            RightPaddelAxis.transform.LookAt(RightVRControlPoint.transform);
        bool left = isAbleToPaddel(LeftPaddelPoint.position);
        bool right = isAbleToPaddel(RightPaddelPoint.position);
        if(left && right)
        {
            rowingSoundManager.PlaySoundEffect(true);
            rowingSoundManager.PlaySoundEffect(false);
            Vector3 leftAcceleration = (xrInput.controller.pos.LeftVelocity- lastLeftVelocity)/Time.deltaTime;
            Vector3 rightAcceleration = (xrInput.controller.pos.RightVelocity- lastRightVelocity)/Time.deltaTime;
            lastLeftVelocity = xrInput.controller.pos.LeftVelocity;
            lastRightVelocity = xrInput.controller.pos.RightVelocity;

            motorForce = speedMod * (leftAcceleration.z > rightAcceleration.z ? leftAcceleration.z : rightAcceleration.z) * Mathf.Cos(Mathf.Deg2Rad * rudderAngle); 
        }
        else if(left)
        {
            rowingSoundManager.PlaySoundEffect(true);
            Vector3 leftAcceleration = (xrInput.controller.pos.LeftVelocity- lastLeftVelocity)/Time.deltaTime;
            lastLeftVelocity = xrInput.controller.pos.LeftVelocity;
            motorForce = speedMod * leftAcceleration.z * Mathf.Cos(Mathf.Deg2Rad * rudderAngle); 
            rudderAngle = rudderMaxAngle * leftAcceleration.z;
        }
        else if(right)
        {
            rowingSoundManager.PlaySoundEffect(false);
            Vector3 rightAcceleration = (xrInput.controller.pos.RightVelocity- lastRightVelocity)/Time.deltaTime;
            lastRightVelocity = xrInput.controller.pos.RightVelocity;
            motorForce = speedMod * -rightAcceleration.z * Mathf.Cos(Mathf.Deg2Rad * rudderAngle);
            rudderAngle = rudderMaxAngle * rightAcceleration.z;
        }
       
        if (floater.floatType == Floater.FloaterType.Ideal)
             IdealMove(motorForce, rudderAngle);
        else if (motor.position.y < WaveManager.instance.GetDisplacementFromGPU(motor.position).y)
            rb.AddForceAtPosition(rudder.forward * motorForce, motor.position, ForceMode.Acceleration);
    }

    private void IdealMove(float speed, float rudderAngle)
    {
        Floater.Anchor anchor = new Floater.Anchor();
        anchor.position = floater.anchorPoint.position + (transform.forward * speed * Time.deltaTime);
        anchor.rotationAngle = floater.anchorPoint.rotationAngle - rudderAngle * rotationSpeed * Time.deltaTime;
        floater.anchorPoint = anchor;
    }
    private bool isAbleToPaddel(Vector3 pos)
    {
        Vector3 normal = new Vector3();
        Vector3 point = WaveManager.instance.GetDisplacementFromGPU(pos, ref normal);
        //Debug.DrawLine(pos, point); 
        return pos.y < point.y;
    }
    private void OnDrawGizmos() {
       // Gizmos.color = Color.blue;
       // Gizmos.DrawLine(RightPaddelAxis.position, xrInput.controller.RightController.transform.position);
    }
    private void ClampTransform(Transform PedalAxis, Transform VRPoint, Vector3 minClamp, Vector3 maxClamp)
    {
        PedalAxis.LookAt(VRPoint);
        Debug.Log(PedalAxis.gameObject.name+" "+PedalAxis.rotation.eulerAngles);
        Quaternion newQuat = PedalAxis.rotation;
        newQuat.eulerAngles = new Vector3(
            Mathf.Clamp(PedalAxis.rotation.x, minClamp.x, maxClamp.x), 
            Mathf.Clamp(PedalAxis.rotation.y, minClamp.y, maxClamp.y), 
            Mathf.Clamp(PedalAxis.rotation.z, minClamp.z, maxClamp.z));
        PedalAxis.rotation = newQuat;

    }
}
