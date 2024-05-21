using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JustPtrck.Shaders.Water;
using static JustPtrck.Shaders.Water.Floater;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Water Mesh for bounds")]
    [SerializeField] private WaterManager WaterMesh;
    [SerializeField] private float[] bounds;
    [Header("Variables")]
    [SerializeField] private GameObject StoreLocation;
    [SerializeField] private List<Transform> SpawnedObjects;
    [SerializeField]public short ObjectAmount 
    {
        get {return objectAmount;}
        set 
        {
            if(value <= objectAmount)
            {
                List<Transform> objects = SpawnedObjects.GetRange(objectAmount, value - objectAmount);
                for (int i = 0; i < objects.Count; i++)
                {
                    Destroy(objects[i]);
                }
                SpawnedObjects.RemoveRange(objectAmount, value - objectAmount);
                objectAmount = value;
            } 
            else
            {
                short old = objectAmount;
                objectAmount = value;
                Spawn(old);
            }
        }
    }
    [SerializeField] public float ObjectPoints; //Needs a Getter setter added.
    [SerializeField] private short objectAmount;
    [SerializeField] private GameObject prefabOBJ;
    // Start is called before the first frame update
    private void OnEnable() {
        Mesh M = WaterMesh.GetMesh();
        Vector3 max = M.bounds.max;
        Vector3 min = M.bounds.min;
        bounds = new float[4]{min.x, max.x, min.z, max.z};
        Spawn(0);
        
    }
    /// <summary>
    /// Recursion! Spawns as many version of the object as you ask. The counter is how many items it already has spawned.
    /// 
    /// </summary>
    public bool Spawn(short BuoyCounter, GameObject spawnedObject = null)
    {
    
        float X = Random.Range(bounds[0], bounds[1]);
        float Z = Random.Range(bounds[2], bounds[3]);

        if(SpawnedObjects.Find(T => (int)T.position.x == (int)X && (int)T.position.z == (int)Z) != null)
            return Spawn(BuoyCounter, spawnedObject);
        if(spawnedObject == null)
        {
            spawnedObject =Instantiate(prefabOBJ, new Vector3(X, 0, Z), transform.rotation);
            SpawnedObjects.Add(spawnedObject.transform);
            spawnedObject.transform.parent = StoreLocation.transform;
        }
        BuoyCounter++;
        Floater F1 =spawnedObject.transform.GetChild(0).GetComponent<Floater>();
        SetFloaterCoords(F1, spawnedObject.transform);
        F1 =spawnedObject.transform.GetChild(1).GetComponent<Floater>();
        SetFloaterCoords(F1, spawnedObject.transform);
        
        if(objectAmount <= BuoyCounter) return true;
        return Spawn(BuoyCounter);
    }
    private void SetFloaterCoords(Floater floater, Transform parentObj)
    {
        Anchor anchor = floater.anchorPoint;
        anchor.position = new Vector3(anchor.position.x + parentObj.transform.position.x, anchor.position.y + parentObj.transform.position.y, anchor.position.z + parentObj.transform.position.z);
        floater.anchorPoint = anchor;
    }
}
