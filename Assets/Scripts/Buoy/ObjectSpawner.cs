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
    [SerializeField] private List<Transform> SpawnedObjects;
    [SerializeField]private short ObjectAmount;
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
    /// Recursion! Think it as a kind of solved grandfathers paradox
    /// </summary>
    private bool Spawn(short BuoyCounter)
    {
    
        float X = Random.Range(bounds[0], bounds[1]);
        float Z = Random.Range(bounds[2], bounds[3]);

        if(SpawnedObjects.Find(T => (int)T.position.x == (int)X && (int)T.position.z == (int)Z) != null)
        {
            return Spawn(BuoyCounter);
        }
        GameObject NB =Instantiate(prefabOBJ, new Vector3(X, 0, Z), transform.rotation);
        SpawnedObjects.Add(NB.transform);
        BuoyCounter++;

        Floater F1 =NB.transform.GetChild(0).GetComponent<Floater>();
        SetFloaterCoords(F1, NB.transform);
        F1 =NB.transform.GetChild(1).GetComponent<Floater>();
        SetFloaterCoords(F1, NB.transform);
        
        if(ObjectAmount <= BuoyCounter) return true;
        return Spawn(BuoyCounter);
    }
    private void SetFloaterCoords(Floater floater, Transform parentObj)
    {
        Anchor anchor = floater.anchorPoint;
        anchor.position = new Vector3(anchor.position.x + parentObj.transform.position.x, anchor.position.y + parentObj.transform.position.y, anchor.position.z + parentObj.transform.position.z);
        floater.anchorPoint = anchor;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
