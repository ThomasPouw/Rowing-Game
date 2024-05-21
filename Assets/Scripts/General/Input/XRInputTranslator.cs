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
    private Vector3 verticalMovementPrev;
    private Vector3 horizontalMovementPrev;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        LeftVRController.positionAction.action.performed += F => Debug.Log("LeftVRController.positionAction: "+F.ReadValue<Vector3>());
        LeftVRController.positionAction.action.performed += F => Debug.Log("LeftVRController Distance: "+Vector3.Distance(F.ReadValue<Vector3>(), Vector3.zero));
        RightVRController.positionAction.action.performed += F => Debug.Log("RightVRController.positionAction: "+F.ReadValue<Vector3>());

        LeftVRController.rotationAction.action.performed += F => Debug.Log("LeftVRController.rotationAction: "+F.ReadValue<Vector3>());
        RightVRController.rotationAction.action.performed += F => Debug.Log("RightVRController.rotationAction "+F.ReadValue<Vector3>());

        LeftVRController.trackingStateAction.action.performed += F => Debug.Log("LeftVRController.trackingStateAction: "+F.ReadValue<Vector3>());
        RightVRController.trackingStateAction.action.performed += F => Debug.Log("RightVRController.trackingStateAction: "+F.ReadValue<Vector3>());
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(RightVRController.positionAction.reference.action);
        Debug.Log(LeftVRController.rotationAction.reference.action);
        Debug.Log(RightVRController.rotationAction.reference.action);
        Debug.Log(LeftVRController.trackingStateAction.reference.action);
        Debug.Log(RightVRController.trackingStateAction.reference.action);*/
    }
}
