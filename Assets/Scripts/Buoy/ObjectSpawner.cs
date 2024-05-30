using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JustPtrck.Shaders.Water;
using static JustPtrck.Shaders.Water.Floater;
using UnityEngine.Events;

public class RowingGameLevelManager : MonoBehaviour
{
    [Header("Water Mesh for bounds")]
    [SerializeField] private WaterManager WaterMesh;
    [SerializeField] private float[] bounds;
    [Header("Floating Objects")]
    [SerializeField] private GameObject StoreLocation;
    [SerializeField] private List<Transform> SpawnedObjects;
    [SerializeField] private float CantSpawnRadius;
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
    [SerializeField] private GameObject prefabOBJ;
    [SerializeField] private Dictionary<Vector3, Vector3> misses = new Dictionary<Vector3, Vector3>();

    [HideInInspector]public UnityEvent<float> ProvidePoints;
    public PointsUI points;
    // Start is called before the first frame update
    private void OnEnable() {
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
        if(SpawnedObjects.Find(T => Vector2.Distance(new Vector2(T.position.x, T.position.z), new Vector2(X,Z)) < CantSpawnRadius)){
            //Vector3 T = SpawnedObjects.Find(T => Vector2.Distance(new Vector2(T.position.x, T.position.z), new Vector2(X,Z)) < CantSpawnRadius).position;
            //misses.Add(T, new Vector3(X,0,Z));
            return Spawn(BuoyCounter, spawnedObject);
        }      
        float Angle = Random.Range(0f,180f);
        if(spawnedObject == null)
        {
            spawnedObject =Instantiate(prefabOBJ, new Vector3(X, 0, Z), transform.rotation);
            spawnedObject.transform.GetChild(2).GetComponent<BuoyTriggerCentered>().changePlace.AddListener(ChangePlaceEvent);
            SpawnedObjects.Add(spawnedObject.transform);
            spawnedObject.transform.parent = StoreLocation.transform;
        }
        else{
            spawnedObject.transform.position = new Vector3(X, 0, Z);
            spawnedObject.transform.rotation = transform.rotation;
        }
        BuoyCounter++;
        //Change this to keep in mind rotation.
        Floater F1 =spawnedObject.transform.GetChild(0).GetComponent<Floater>();
        SetFloaterCoords(F1, spawnedObject.transform, new Vector3(-3f, -.025f, 0), Angle);
        F1 =spawnedObject.transform.GetChild(1).GetComponent<Floater>();
        SetFloaterCoords(F1, spawnedObject.transform, new Vector3(3f, -.025f, 0), Angle);
        
        if(objectAmount <= BuoyCounter) return true;
        return Spawn(BuoyCounter);
    }
    private void SetFloaterCoords(Floater floater, Transform parentObj, Vector3 anchorpoint, float angle)
    {
        Anchor anchor = floater.anchorPoint;
        //Debug.Log(angle);
        //Debug.Log("Pre: "+anchorpoint.x);
        anchorpoint.x += 3 * Mathf.Cos((Mathf.Deg2Rad *angle) * (Mathf.PI/180));
        //Debug.Log("Post: "+anchorpoint.x);
        anchor.position = new Vector3(anchorpoint.x + parentObj.transform.position.x, anchorpoint.y + parentObj.transform.position.y, anchorpoint.z + parentObj.transform.position.z);
        floater.anchorPoint = anchor;
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        for (int i = 0; i < SpawnedObjects.Count; i++)
        {
            Gizmos.DrawWireSphere(SpawnedObjects[i].position, CantSpawnRadius/2);
        }
        /*if(misses == null) return;
        foreach (KeyValuePair<Vector3, Vector3> item in misses)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(item.Key, 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(item.Value, 0.1f);
            Gizmos.color = Color.black;
            Gizmos.DrawLine(item.Key, item.Value);
        }*/
    }
}
