using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMotion : MonoBehaviour
{
    [Range(-1, 1f) ]public float RangeRowX = 0;
    [Range(-1, 1f) ]public float RangeRowY = 0;
    [Range(-1, 1f) ]public float RangeRowZ = 0;
    public GameObject RowPedal;
    public Vector3 RowPedalCoords;
    public Vector3 Extention;

    // Start is called before the first frame update
    void Start()
    {
        RowPedalCoords = transform.localPosition;
        transform.position = RowPedalCoords + Extention;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = RowPedalCoords + Extention + new Vector3(RangeRowX,RangeRowY, RangeRowZ);
    }
}
