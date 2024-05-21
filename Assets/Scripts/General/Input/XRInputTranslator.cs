using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInputTranslator : MonoBehaviour
{
    [SerializeField] private ActionBasedController LeftVRController;
    [SerializeField] private ActionBasedController RightVRController;

    public bool LeftMoved = false;
    public bool RightMoved = false;
    public float verticalMovementNormal;
    public float horizontalMovementNormal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(LeftVRController.positionAction.reference);
        Debug.Log(RightVRController.positionAction.reference);
        Debug.Log(LeftVRController.rotationAction.reference);
        Debug.Log(RightVRController.rotationAction.reference);
        Debug.Log(LeftVRController.trackingStateAction.reference);
        Debug.Log(RightVRController.trackingStateAction.reference);
    }
}
