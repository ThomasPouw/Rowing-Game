using System.Collections;
using System.Collections.Generic;
using JustPtrck.Shaders.Water;
using UnityEngine;

public class PeddalBoatVR : MonoBehaviour
{
    [Header("Boat Parameters")]
    [SerializeField] private float acceleration = 10f;   // m/s^2
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
        LeftPaddelAxis.LookAt(xrInput.controller.LeftController.transform.position);
        RightPaddelAxis.LookAt(xrInput.controller.RightController.transform.position);
        //RightPaddelAxis.transform.rotation.Set(RightPaddelAxis.transform.rotation.y, RightPaddelAxis.transform.rotation.x, RightPaddelAxis.transform.rotation.z, RightPaddelAxis.transform.rotation.w);
        if(xrInput.controller.pos.LeftVelocity != Vector3.zero ||  xrInput.controller.pos.RightVelocity != Vector3.zero)
        {
            if(xrInput.controller.pos.LeftVelocity != Vector3.zero)
            {
                if(xrInput.controller.pos.LeftVelocity.z < 0)
                    LeftPaddelAxis.LookAt(new Vector3(xrInput.controller.LeftController.transform.position.x, xrInput.controller.LeftController.transform.position.y, 0));
                else
                    LeftPaddelAxis.LookAt(xrInput.controller.LeftController.transform.position);
                rudderAngle += isAbleToPaddel(LeftPaddelPoint.position) ? rudderMaxAngle * Vector3.Distance(xrInput.controller.pos.LeftVelocity.normalized, Vector3.zero) : 0; 
            }
            else
            {
                if(xrInput.controller.pos.RightVelocity.z < 0)
                    RightPaddelAxis.LookAt(new Vector3(xrInput.controller.RightController.transform.position.x, xrInput.controller.RightController.transform.position.y, 0));
                else
                    RightPaddelAxis.LookAt(xrInput.controller.RightController.transform.position);
                rudderAngle -= isAbleToPaddel(RightPaddelPoint.position) ?  rudderMaxAngle * Vector3.Distance(xrInput.controller.pos.RightVelocity.normalized, Vector3.zero) : 0; 
            }
        }
       // rudderAngle = isAbleToPaddel(LeftPaddelPoint.position) ?  rudderMaxAngle * SteeringAngle : 0; //I should check this...
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
        Debug.DrawLine(pos, point);
        return pos.y < point.y;
    }
    private void OnDrawGizmos() {
       // Gizmos.color = Color.blue;
       // Gizmos.DrawLine(RightPaddelAxis.position, xrInput.controller.RightController.transform.position);
    }
}
