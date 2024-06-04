using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using JustPtrck.Shaders.Water;
using UnityEngine.XR.Interaction.Toolkit;
public class PeddalBoat : MonoBehaviour
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




    private Rigidbody rb;
    private float rudderAngle;
    private float motorForce;



    private void OnEnable()
    {
            rb = gameObject.GetComponent<Rigidbody>() ? gameObject.GetComponent<Rigidbody>() : gameObject.AddComponent<Rigidbody>();
            floater = gameObject.GetComponent<Floater>() ? gameObject.GetComponent<Floater>() : gameObject.AddComponent<Floater>();
            RightPaddelPoint = RightPaddelAxis.GetChild(0).GetChild(0).transform;
            LeftPaddelPoint = LeftPaddelAxis.GetChild(0).GetChild(0).transform;
            eclipsePeddal(ref LeftPaddelAngle, LeftPaddelAxis, PedalSpeed, true);
            eclipsePeddal(ref RightPaddelAngle, RightPaddelAxis, PedalSpeed , false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (motor.position.y < WaveManager.instance.GetDisplacementFromGPU(motor.position).y)
        //     rb.AddForceAtPosition(motor.forward * motorForce, motor.position, ForceMode.Acceleration);
        if(SteeringAngle != 0)
        {
            if(SteeringAngle > 0)
            {
                eclipsePeddal(ref LeftPaddelAngle, LeftPaddelAxis, SteeringAngle, true);
                rudderAngle = isAbleToPaddel(LeftPaddelPoint.position) ? rudderMaxAngle * SteeringAngle : 0;
            }
            else
            {
                eclipsePeddal(ref RightPaddelAngle, RightPaddelAxis, SteeringAngle, false);
                rudderAngle = isAbleToPaddel(RightPaddelPoint.position) ? rudderMaxAngle * SteeringAngle : 0;
            }
        }
        if(PedalSpeed != 0)
        {
            eclipsePeddal(ref LeftPaddelAngle, LeftPaddelAxis, PedalSpeed, true);
            eclipsePeddal(ref RightPaddelAngle, RightPaddelAxis, PedalSpeed , false);
        }
            
        float curVelo = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        // if (rudder.position.y < WaveManager.instance.GetDisplacementFromGPU(rudder.position).y)
        //     rb.AddForceAtPosition(rudder.right * curVelo * Mathf.Sin(Mathf.Deg2Rad * rudderAngle), rudder.position, ForceMode.Force);
        // Debug.Log($"Velocity: {curVelo}");


        // TEMP Formula for force
        if (floater.floatType == Floater.FloaterType.Ideal)
             IdealMove(motorForce);
        else if (motor.position.y < WaveManager.instance.GetDisplacementFromGPU(motor.position).y)
            rb.AddForceAtPosition(rudder.forward * motorForce, motor.position, ForceMode.Acceleration);
    }
    public void OnAccelerate(InputValue value)
    {
        PedalSpeed = value.Get<float>();
        //Debug.Log(PedalSpeed);
        motorForce = acceleration * PedalSpeed * Mathf.Cos(Mathf.Deg2Rad * rudderAngle);
        // Debug.Log($"Accelerate Input: {mod}");
        return;
    }
    public void OnSteering(InputValue value)
    {
        SteeringAngle = value.Get<float>();
        rudderAngle = rudderMaxAngle * SteeringAngle;
        /*if(value.Get<float>() > 0)
        {
            LeftPaddelAngle += ((Mathf.PI)/5) * Time.deltaTime;
            LeftPaddelAxis.transform.rotation= Quaternion.Euler(0f, Mathf.Cos(LeftPaddelAngle)*4, Mathf.Sin(LeftPaddelAngle)*5);
            Debug.Log(Quaternion.Euler(0f, Mathf.Cos(LeftPaddelAngle)*4, Mathf.Sin(LeftPaddelAngle)*5));
        }
        
        float angle = (2*Mathf.PI)/5 * value.Get<float>();
        //https://discussions.unity.com/t/circular-motion-via-the-mathematical-circle-equation/89830
        Debug.Log(angle+ "  "+ value.Get<float>()); */ //Does not work, due to onsteerign only getting the event ones.
        // Debug.Log($"Steering Input: {value.Get<float>()}");
        //rudder.transform.localRotation = Quaternion.Euler(0f, rudderAngle, 0f); //Will be motion based. Why? Because I like making problems.
        return;
    }
    /// <summary>
    /// Calculates the Arc that the pedals go in WASD mode.
    /// </summary>
    /// <param name="PeddalAngle"> The reference to which side you are changing the angle of.</param>
    /// <param name="PaddelAxis"> The transform of the point on where the pedal rests. You can find it by looking for the pedals and seeing the parent of it.</param>
    /// <param name="force"> How much force is there? You find that force by looking at the InputSystem</param>
    /// <param name="Left">Just making sure that the pedals are not messed up. As one is mirrored so you have to change some numbers.</param>
    /// <returns></returns>
    private void eclipsePeddal(ref float PeddalAngle, Transform PaddelAxis, float force, bool Left)
    {
        PeddalAngle += ((Mathf.PI)) * Time.deltaTime * Mathf.Abs(force); 
        float SinAngle = Mathf.Sin(PeddalAngle)*40;
       // Debug.Log("SinAngle: "+ SinAngle);
        if(Left)
            SinAngle = SinAngle > 0f ? SinAngle * 0.5f : SinAngle * 1.2f;
        else
            SinAngle = SinAngle < 0f ? SinAngle * 0.5f : SinAngle * 1.2f;
        PaddelAxis.localRotation=  Quaternion.Euler(0f, Mathf.Cos(PeddalAngle)*30, SinAngle);
        //Debug.Log(Quaternion.Euler(0f, Mathf.Cos(PeddalAngle)*30, SinAngle));
    }
    private void IdealMove(float speed)
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
}
