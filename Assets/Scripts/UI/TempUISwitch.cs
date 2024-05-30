using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class TempUISwitch : MonoBehaviour
{
    [SerializeField] public GameObject TempVRUI;
    [SerializeField] public GameObject TempComputerUI;
    private bool isLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        TempVRUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Insert))
        {
            TempVRUI.SetActive(!TempVRUI.activeInHierarchy);
            TempComputerUI.SetActive(!TempComputerUI.activeInHierarchy);
        }*/
    }
    public void OnTempUIChange(InputValue value)
    {
        if(isLocked)
        {
            TempVRUI.SetActive(!TempVRUI.activeInHierarchy);
            TempComputerUI.SetActive(!TempComputerUI.activeInHierarchy);
        }
        isLocked = !isLocked;
    }
}
