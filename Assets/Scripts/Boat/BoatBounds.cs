using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBounds : MonoBehaviour
{
    [SerializeField] private float AmountChecksOnLine;
    [SerializeField] private float CheckLengthX;
    [SerializeField] private float CheckLengthY;
    [SerializeField] private float CheckLengthZ;
    [SerializeField] private Terrain terrain;
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

    private void Start()
    {
        CheckLengthX = BoxBound.x / AmountChecksOnLine;
        CheckLengthY = BoxBound.y / AmountChecksOnLine;
        CheckLengthZ = BoxBound.z / AmountChecksOnLine;
    }
    // Update is called once per frame
    void Update()
    {
        if(parent == null)
        {
            parent = transform.parent;
        }
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
    public bool isTouchingMountain()
    {
        for (int i = 0; i < CheckLengthX; i++)
        {
            for (int ii = 0; ii < CheckLengthY; ii++)
            {
                for (int iii = 0; iii < CheckLengthZ; iii++)
                {
                    Vector3 sample = downPoint+parent.transform.localRotation *(new Vector3(CheckLengthX*i, CheckLengthY*ii, CheckLengthZ*ii));
                    Debug.DrawLine(sample, sample + new Vector3(0.1f, 0.1f, 0.1f), Color.yellow);
                    if((terrain.SampleHeight(sample)) +terrain.transform.position.y > 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
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
 

        Gizmos.color = Color.red;

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
