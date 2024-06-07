using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JustPtrck.Shaders.Water;
using static JustPtrck.Shaders.Water.Floater;
using UnityEngine.Events;
using System.ComponentModel;
using System.Linq;

public class RowingGameLevelManager : MonoBehaviour
{
    [Header("Terrain")]
    [SerializeField] private WaterManager WaterMesh;
    private float[] bounds;
    [SerializeField] private Terrain Mountains;  /// This is needed to get the sampleheight. To make sure that no buoy is placed inside of the mountain border.
    [Header("Buoy")]
    [SerializeField] private GameObject prefabOBJ;
    [SerializeField] private GameObject StoreLocation;
    [SerializeField] private List<Transform> SpawnedObjects;
    [SerializeField][Description("Where from the center of the gameObject does the single Bouy spawn?")] private List<Vector3> BuoyLocation;
    [SerializeField] private float CantSpawnBuoyRadius;
    [SerializeField] private float CantSpawnBoatRadius;
    [SerializeField]public short ObjectAmount 
    {
        get {return objectAmount;}
        set 
        {
            Debug.Log(value+ " "+ objectAmount);
            if(value < objectAmount)
            {
                List<Transform> objects = SpawnedObjects.GetRange(objectAmount, objectAmount- value);
                for (int i = 0; i < objects.Count; i++)
                {
                    Destroy(objects[i].gameObject);
                }
                SpawnedObjects.RemoveRange(objectAmount, objectAmount- value);
            } 
            else
            {
                short old = objectAmount;
                objectAmount = value;
                Spawn(old);
            }
            objectAmount = value;
        }
    }
    [SerializeField] public float ObjectPoints; //Needs a Getter setter added.
    [SerializeField] private short objectAmount;

    [HideInInspector]public UnityEvent<float> ProvidePoints;

    private Transform BoatLocation;

    // Start is called before the first frame update
    private void OnEnable() {
        BoatLocation = GameObject.FindGameObjectWithTag("Boat").transform;
        Mesh M = WaterMesh.GetMesh();
        Vector3 max = new Vector3(65,0,65);//M.bounds.max;
        Vector3 min = new Vector3(-65,0,-65);//M.bounds.min;
        bounds = new float[4]{min.x, max.x, min.z, max.z};


        //ProvidePoints.AddListener(points.ChangeText);
        Spawn(0);
    }

    public void ChangePlaceEvent(Transform T)
    {
        ProvidePoints.Invoke(ObjectPoints);
        Spawn(objectAmount, T.gameObject);
    }
    /// <summary>
    /// Recursion! Spawns as many version of the object as you ask. The counter is how many items it already has spawned.
    /// 
    /// </summary>
    public bool Spawn(short BuoyCounter, GameObject spawnedObject = null)
    {
        float X = Random.Range(bounds[0], bounds[1]);
        float Z = Random.Range(bounds[2], bounds[3]);
        if(!isAbleToSpawn(X, Z))
            return Spawn(BuoyCounter, spawnedObject);
        if(spawnedObject == null)
        {
            spawnedObject =Instantiate(prefabOBJ);
            spawnedObject.transform.GetComponentInChildren<BuoyTriggerCentered>().changePlace.AddListener(ChangePlaceEvent);
            spawnedObject.transform.parent = StoreLocation.transform;
            SpawnedObjects.Add(spawnedObject.transform);
        }
        float Angle = Random.Range(0f,180f);
        spawnedObject.transform.position = new Vector3(X, 0, Z);
        spawnedObject.transform.rotation = Quaternion.Euler(0,Angle, 0);
        List<Floater> TempFloater = spawnedObject.transform.GetComponentsInChildren<Floater>().ToList();
        //Change this to keep in mind rotation.
        for (int i = 0; i < TempFloater.Count; i++)
        {
            Floater F1 =TempFloater[i].GetComponent<Floater>();
            SetFloaterCoords(F1, spawnedObject.transform, BuoyLocation[i]);
        }
        BuoyCounter++;
        if(objectAmount <= BuoyCounter) return true;
        return Spawn(BuoyCounter);
    }
    private void SetFloaterCoords(Floater floater, Transform parentObj, Vector3 anchorpoint)
    {
        Anchor anchor = floater.anchorPoint;
        anchorpoint = parentObj.transform.rotation * anchorpoint;
        anchor.position = new Vector3(anchorpoint.x + parentObj.transform.position.x, anchorpoint.y + parentObj.transform.position.y, anchorpoint.z + parentObj.transform.position.z);
        floater.anchorPoint = anchor;
    }
    ///Checks if the spawning conditions are met.
    ///Those being:
    /// -Is it touching the boat?
    /// -Is it touching another gate/ buoy pair?
    /// -Is it touching the terrain? 
    ///     -For this we do it 4 times to check the cardinal directions. If the Mountain sampleheight is higher then 0 then you can assume its either going through the mountain or clipping in it.
    private bool isAbleToSpawn(float X, float Z)
    {
        if(Vector2.Distance(new Vector2(BoatLocation.position.x, BoatLocation.position.z), new Vector2(X,Z)) < CantSpawnBoatRadius)
            return false;
        if(SpawnedObjects.Find(T => Vector2.Distance(new Vector2(T.position.x, T.position.z), new Vector2(X,Z)) < CantSpawnBuoyRadius))
            return false;
        for (int i = 0; i < 3; i++)
        {
            Vector3 point= new Vector3(X,0,Z)+ Quaternion.Euler(0, 90*i, 0) * (Vector3.forward * CantSpawnBuoyRadius);
            if((Mountains.SampleHeight(point)) +Mountains.transform.position.y > 0)
            {

                return false;
            }
        }
        return true;  
    }
    private void OnDrawGizmos() {
        if(!Application.isPlaying) 
        ///Thomas Pouw likes to see the circles as soon as possible. But does not like the errors it causes in Editor.
            return;
        Gizmos.color = Color.red;
        for (int i = 0; i < SpawnedObjects.Count; i++)
        {
            Gizmos.DrawWireSphere(SpawnedObjects[i].position, CantSpawnBuoyRadius/2);
        }
        Gizmos.DrawWireSphere(BoatLocation.position, CantSpawnBoatRadius/2);
    }
}
