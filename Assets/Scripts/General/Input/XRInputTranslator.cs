using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInputTranslator : MonoBehaviour
{
    [SerializeField] private XRController LeftVRController;
    [SerializeField] private XRController RightVRController;

    public bool LeftMoved = false;
    public bool RightMoved = false;
    [SerializeField]public Controller controller;
    // Start is called before the first frame update
    private void OnEnable() 
    {
    }
    void Start()
    {
        controller.LeftController = LeftVRController.gameObject;
        controller.RightController = RightVRController.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if(!LeftMoved) return;
        GetViveControls();
    }
    private void GetViveControls()
    {
        LeftVRController.inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out controller.pos.LeftPosition);
        //Debug.Log("Left Controller Primairy in/out: "+ controller.pos.LeftPosition);
        RightVRController.inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out controller.pos.RightPosition);
        //Debug.Log("Right Controller Primairy in/out: "+ controller.pos.RightPosition);
        LeftVRController.inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out controller.rot.LeftRotation);
        //Debug.Log("Left Controller Primairy rotation: "+ controller.rot.LeftRotation);
        RightVRController.inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out controller.rot.RightRotation);
        //Debug.Log("Right Controller Primairy rotation: "+ controller.rot.RightRotation);
        RightVRController.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out controller.pos.LeftVelocity);
        //Debug.Log("Left Controller Primairy velocity: "+ controller.pos.LeftVelocity);
        LeftVRController.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out controller.pos.RightVelocity);
        //Debug.Log("Right Controller Primairy velocity: "+ controller.pos.RightVelocity);
        /*Debug.Log("Left Controller Primairy in/out: "+ controller.pos.LeftPosition+ " Right Controller Primairy in/out: "+ controller.pos.RightPosition+ 
        "|| Left Controller Primairy rotation: "+ controller.rot.LeftRotation + " Right Controller Primairy rotation: "+ controller.rot.RightRotation+ 
        "|| Left Controller Primairy velocity: "+ controller.pos.LeftVelocity+ " Right Controller Primairy velocity: "+ controller.pos.RightVelocity);*/
    }
}
[System.Serializable]
public class Controller
{
    public GameObject LeftController;
    public GameObject RightController;
    public position pos;
    public rotation rot;
}
[System.Serializable]
public class position
{
    public Vector3 LeftPosition;
    public Vector3 LeftVelocity;
    public Vector3 RightPosition;
    public Vector3 RightVelocity;
}
[System.Serializable]
public class rotation
{
    public Quaternion LeftRotation;
    public float LeftVelocity;
    public Quaternion RightRotation;
    public float RightVelocity;
}