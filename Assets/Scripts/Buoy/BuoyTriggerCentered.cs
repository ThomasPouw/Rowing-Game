using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;


public class BuoyTriggerCentered : MonoBehaviour
{

    public UnityEvent<Transform> changePlace;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //PutDistanceOnTrigger();
        //MoveTrigger();
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
