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


    //[Header("Paddel References")]
    [SerializeField]private Transform LeftPaddelAxis;
    [SerializeField]private Transform LeftPaddelPoint;

    [SerializeField]private Transform RightPaddelAxis;
    [SerializeField]private Transform RightPaddelPoint;
    [Header("VR Information")]
    [SerializeField] private XRInputTranslator xrInput;
    [SerializeField] private PedalBounds pedalBounds;
    [SerializeField] private GameObject LeftVRControlPoint;
    [SerializeField] private GameObject RightVRControlPoint;
    private bool isAbleToRow = false;
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
        isAbleToRow = false;
    }
    private void Start() {
        isAbleToRow = false;
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
        if(isAbleToRow)
            return;
        bool left = isAbleToPaddel(LeftPaddelPoint.position);
        bool right = isAbleToPaddel(RightPaddelPoint.position);
        if(left && right)
        {
            rowingSoundManager.PlaySoundEffect(true);
            rowingSoundManager.PlaySoundEffect(false);

            motorForce = speedMod * (xrInput.controller.pos.LeftVelocity.z+ xrInput.controller.pos.RightVelocity.z) * Mathf.Cos(Mathf.Deg2Rad * rudderAngle); 
        }
        else if(left)
        {
            rowingSoundManager.PlaySoundEffect(true);
            motorForce = speedMod * (xrInput.controller.pos.LeftVelocity.z) * Mathf.Cos(Mathf.Deg2Rad * rudderAngle); 
            rudderAngle = rudderMaxAngle * (xrInput.controller.pos.LeftVelocity.z);
        }
        else if(right)
        {
            rowingSoundManager.PlaySoundEffect(false);
            motorForce = speedMod * -(xrInput.controller.pos.RightVelocity.z) * Mathf.Cos(Mathf.Deg2Rad * rudderAngle);
            rudderAngle = rudderMaxAngle * -(xrInput.controller.pos.RightVelocity.z);
        }
       
        if (floater.floatType == Floater.FloaterType.Ideal)
             IdealMove(motorForce, rudderAngle);
        else if (motor.position.y < WaveManager.instance.GetDisplacementFromGPU(motor.position).y)
            rb.AddForceAtPosition(rudder.forward * motorForce, motor.position, ForceMode.Acceleration);
    }
    public void isAbleToMove()
    {
        isAbleToRow = !isAbleToRow;
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
    public void SetVRPoints()
    {
        LeftVRControlPoint.transform.position= new Vector3(LeftVRControlPoint.transform.position.x, LeftVRControlPoint.transform.position.y, LeftPaddelAxis.transform.position.z);
        RightVRControlPoint.transform.position= new Vector3(RightVRControlPoint.transform.position.x, RightVRControlPoint.transform.position.y, RightPaddelAxis.transform.position.z);
    }
}
