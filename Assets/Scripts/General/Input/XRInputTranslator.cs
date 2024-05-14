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
        //verticalMovementNormal = LeftVRController
        Debug.Log("LeftVRController POS"+ LeftVRController.positionAction.reference);
        Debug.Log("RightVRController POS"+ RightVRController.positionAction.reference);

        Debug.Log("LeftVRController ROT"+ LeftVRController.rotationAction.reference);
        Debug.Log("RightVRController ROT"+ RightVRController.rotationAction.reference);
    }
}
