using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class BuoyTriggerCentered : MonoBehaviour
{
    public List<Transform> Buoys = new List<Transform>();
    public BoxCollider boxCollider;
    public float hitboxOffset;

    public Material CollectedLight;
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
        float Y = ((Buoys[0].position.y + Buoys[1].position.y)/2)+hitboxOffset;
        transform.position = new Vector3(transform.position.x, Y, transform.position.z);
        transform.rotation = rot;
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        if(other.name == "Boat")
        {
            //Just move the Buoy. 
            Buoys[0].GetChild(1).gameObject.GetComponent<Renderer>().material = CollectedLight;
            Buoys[1].GetChild(1).gameObject.GetComponent<Renderer>().material = CollectedLight;
            gameObject.SetActive(false);
        }
    }
}
