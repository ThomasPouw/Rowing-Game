using System.Collections;
using System.Collections.Generic;
using ProceduralToolkit;
using UnityEngine;

public class PedalBounds : MonoBehaviour
{
    public Transform UpLeftPoint;
    public Transform DownRightPoint;

//Go to the oriantation of the VR glasses and the turn 90 degrees to the right. That is the oriantation I am looking at.
    //[SerializeField] private bool isUpLeftXBigger;
    //[SerializeField] private bool isUpLeftYBigger;
    //[SerializeField] private bool isUpLeftZBigger;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool isPedalInBox(Transform pedalTransform)
    {
        Debug.Log("X Condition: "+ (DownRightPoint.position.x > pedalTransform.position.x) + ", "+ (UpLeftPoint.position.x < pedalTransform.position.x));
        Debug.Log("Y Condition: "+ (DownRightPoint.position.y < pedalTransform.position.y) + ", "+ (UpLeftPoint.position.y > pedalTransform.position.y));
        Debug.Log("Z Condition: "+ (DownRightPoint.position.z > pedalTransform.position.z) + ", "+ (UpLeftPoint.position.z < pedalTransform.position.z));
        //Debug.Log(pedalTransform.gameObject.name+" here!"+ pedalTransform.position+ "BoundBox: "+ UpLeftPoint.position+ " "+ DownRightPoint.position);
        if(!(DownRightPoint.position.x > pedalTransform.position.x && UpLeftPoint.position.x < pedalTransform.position.x))
            return false;
        if(!(DownRightPoint.position.y < pedalTransform.position.y && UpLeftPoint.position.y > pedalTransform.position.y))
            return false;
        if(!(DownRightPoint.position.z > pedalTransform.position.z && UpLeftPoint.position.z < pedalTransform.position.z))
            return false;
        Debug.DrawLine(pedalTransform.position, pedalTransform.position + (Vector3.up *5), ColorE.green);
        return true;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        //Get all the combinations :D
        Vector3 Up1 = new Vector3(UpLeftPoint.position.x, UpLeftPoint.position.y, DownRightPoint.position.z);
        Vector3 Up2 = new Vector3(DownRightPoint.position.x, UpLeftPoint.position.y, DownRightPoint.position.z);
        Vector3 Up3 = new Vector3(DownRightPoint.position.x, UpLeftPoint.position.y, UpLeftPoint.position.z);
        Vector3 Down1 = new Vector3(UpLeftPoint.position.x, DownRightPoint.position.y, DownRightPoint.position.z);
        Vector3 Down2 = new Vector3(UpLeftPoint.position.x, DownRightPoint.position.y, UpLeftPoint.position.z);
        Vector3 Down3 = new Vector3(DownRightPoint.position.x, DownRightPoint.position.y, UpLeftPoint.position.z);
        
        Gizmos.DrawLine(UpLeftPoint.position, Up1);
        Gizmos.DrawLine(Up1, Up2);
        Gizmos.DrawLine(Up2, Up3);
        Gizmos.DrawLine(Up3, UpLeftPoint.position);

        Gizmos.DrawLine(DownRightPoint.position, Down1);
        Gizmos.DrawLine(Down1, Down2);
        Gizmos.DrawLine(Down3, Down2);
        Gizmos.DrawLine(Down3, DownRightPoint.position);

        Gizmos.DrawLine(UpLeftPoint.position, Down2);
        Gizmos.DrawLine(Up1, Down1);
        Gizmos.DrawLine(DownRightPoint.position, Up2);
        Gizmos.DrawLine(Down3, Up3);


    }
}
