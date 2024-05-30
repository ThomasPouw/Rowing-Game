using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;


public class BuoyTriggerCentered : MonoBehaviour
{
    public List<Transform> Buoys = new List<Transform>();
    public BoxCollider boxCollider;
    public float hitboxOffset;

    public Material CollectedLight;
    public UnityEvent<Transform> changePlace;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PutDistanceOnTrigger();
        MoveTrigger();
    }
    private void PutDistanceOnTrigger()
    {
        boxCollider.size = new Vector3(Vector3.Distance(Buoys[0].position, Buoys[1].position ), boxCollider.size.y, boxCollider.size.z);
    }
    private void MoveTrigger()
    {
        Quaternion rot = (Buoys[0].rotation * Buoys[1].rotation);
        float X = ((Buoys[0].position.x + Buoys[1].position.x)/2);
        float Y = ((Buoys[0].position.y + Buoys[1].position.y)/2)+hitboxOffset;
        float Z = ((Buoys[0].position.z + Buoys[1].position.z)/2);
        transform.position = new Vector3(X, Y, transform.position.z);
        transform.rotation = rot;
    }
    private void OnTriggerExit(Collider other) {
        Debug.Log(other.name);
        if(other.name == "Boat")
        {
            //Just move the Buoy. 
            changePlace.Invoke(transform.parent);
            /*Buoys[0].GetChild(1).gameObject.GetComponent<Renderer>().material = CollectedLight;
            Buoys[1].GetChild(1).gameObject.GetComponent<Renderer>().material = CollectedLight;
            gameObject.SetActive(false);*/
        }
    }
}
