using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedalBounds : MonoBehaviour
{
    //public Transform UpLeftPoint;
    //public Transform DownRightPoint;
    private Transform parent;
    [SerializeField] private Vector3 BoxBound;
    private Vector3 downPoint;
    private Vector3 upPoint;
    private Vector3 Up1;
    private Vector3 Up2;
    private Vector3 Up3;
    private Vector3 Down1;
    private Vector3 Down2;
    private Vector3 Down3;

    //Go to the oriantation of the VR glasses and the turn 90 degrees to the right. That is the oriantation I am looking at.
    //[SerializeField] private bool isUpLeftXBigger;
    //[SerializeField] private bool isUpLeftYBigger;
    //[SerializeField] private bool isUpLeftZBigger;

    // Start is called before the first frame update
    void Start()
    {
        if(parent == null)
            parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //Compansate with rotation!
        downPoint = transform.position+parent.transform.localRotation *( new Vector3(-BoxBound.x/2, -BoxBound.y/2, -BoxBound.z/2));
        upPoint = transform.position+parent.transform.localRotation *(new Vector3(BoxBound.x/2, BoxBound.y/2, BoxBound.z/2));
        //Get all the combinations :D
        Up1 = transform.position+parent.transform.localRotation *(new Vector3(BoxBound.x/2, BoxBound.y/2, -BoxBound.z/2));
        Up2 = transform.position+parent.transform.localRotation *(new Vector3(-BoxBound.x/2, BoxBound.y/2, -BoxBound.z/2));
        Up3 = transform.position+parent.transform.localRotation *( new Vector3(-BoxBound.x/2, BoxBound.y/2, BoxBound.z/2));
        Down1 = transform.position+parent.transform.localRotation *(new Vector3(BoxBound.x/2, -BoxBound.y/2, -BoxBound.z/2));
        Down2 = transform.position+parent.transform.localRotation *( new Vector3(BoxBound.x/2, -BoxBound.y/2, BoxBound.z/2));
        Down3 = transform.position+parent.transform.localRotation *( new Vector3(-BoxBound.x/2, -BoxBound.y/2, BoxBound.z/2));

    }
    public bool isPedalInBox(Transform pedalTransform)
    {
        //Vector3 downPoint = parent.transform.rotation * DownRightPoint.position;
        //Vector3 upPoint = parent.transform.rotation * UpLeftPoint.position;
        //Debug.Log("X Condition: "+ (downPoint.x < pedalTransform.position.x) + ", "+ (upPoint.x > pedalTransform.position.x) + " "+ (downPoint.x < pedalTransform.position.x && upPoint.x > pedalTransform.position.x));
        //Debug.Log("Y Condition: "+ (downPoint.y < pedalTransform.position.y) + ", "+ (upPoint.y > pedalTransform.position.y) + " "+ (downPoint.y < pedalTransform.position.y && upPoint.y > pedalTransform.position.y));
       // Debug.Log("Z Condition: "+ (downPoint.z < pedalTransform.position.z) + ", "+ (upPoint.z > pedalTransform.position.z) + " "+ (downPoint.z < pedalTransform.position.z && upPoint.z > pedalTransform.position.z));
       // Debug.Log(pedalTransform.gameObject.name+" here!"+ pedalTransform.position+ "BoundBox: "+ downPoint+ " "+ upPoint);
        if(!(downPoint.x < pedalTransform.position.x && upPoint.x > pedalTransform.position.x))
            return false;
        if(!(downPoint.y < pedalTransform.position.y && upPoint.y > pedalTransform.position.y))
            return false;
        if(!(downPoint.z < pedalTransform.position.z && upPoint.z > pedalTransform.position.z))
            return false;
        Debug.DrawLine(pedalTransform.position, pedalTransform.position + (Vector3.up *5), Color.green);
        return true;
    }

    private void OnDrawGizmos() {
        if(parent == null)
        {
            parent = transform.parent;
        }
        if(Application.isEditor)
        {
            //Compansate with rotation!
            downPoint = transform.position+parent.transform.localRotation *( new Vector3(-BoxBound.x/2, -BoxBound.y/2, -BoxBound.z/2));
            upPoint = transform.position+parent.transform.localRotation *(new Vector3(BoxBound.x/2, BoxBound.y/2, BoxBound.z/2));
            //Get all the combinations :D
            Up1 = transform.position+parent.transform.localRotation *(new Vector3(BoxBound.x/2, BoxBound.y/2, -BoxBound.z/2));
            Up2 = transform.position+parent.transform.localRotation *(new Vector3(-BoxBound.x/2, BoxBound.y/2, -BoxBound.z/2));
            Up3 = transform.position+parent.transform.localRotation *( new Vector3(-BoxBound.x/2, BoxBound.y/2, BoxBound.z/2));
            Down1 = transform.position+parent.transform.localRotation *(new Vector3(BoxBound.x/2, -BoxBound.y/2, -BoxBound.z/2));
            Down2 = transform.position+parent.transform.localRotation *( new Vector3(BoxBound.x/2, -BoxBound.y/2, BoxBound.z/2));
            Down3 = transform.position+parent.transform.localRotation *( new Vector3(-BoxBound.x/2, -BoxBound.y/2, BoxBound.z/2));
        }
 

        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(upPoint, Up1);
        Gizmos.DrawLine(Up1, Up2);
        Gizmos.DrawLine(Up2, Up3);
        Gizmos.DrawLine(Up3, upPoint);

        Gizmos.DrawLine(downPoint, Down1);
        Gizmos.DrawLine(Down1, Down2);
        Gizmos.DrawLine(Down3, Down2);
        Gizmos.DrawLine(Down3, downPoint);

        Gizmos.DrawLine(upPoint, Down2);
        Gizmos.DrawLine(Up1, Down1);
        Gizmos.DrawLine(downPoint, Up2);
        Gizmos.DrawLine(Down3, Up3);
    }
}
