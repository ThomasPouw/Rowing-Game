using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JustPtrck.Shaders.Water;
using UnityEngine.Rendering;
using static JustPtrck.Shaders.Water.WaveManager;

public class Hover : MonoBehaviour
{
    public float height = 1f;

    // Update is called once per frame
    void Update()
    {
       WaveManager.instance.GetDisplacementFromGPU(transform.position, CallBackHover);
    }
    private void CallBackHover(AsyncGPUReadbackRequest asyncGPUReadbackRequest)
    {
        DN[] dn = asyncGPUReadbackRequest.GetData<DN>(0).ToArray();
        float disp = dn[0].displacement.y+ height;
        transform.position += Vector3.up * (disp - transform.position.y);
    }
}
