using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ManagingInput : MonoBehaviour
{
    public GameObject settingA;
    public GameObject settingB;
    public GameObject settingC;
    public GameObject cameraA;
    public GameObject cameraB;
    public GameObject specialCamera;

    private void Start()
    {
        settingA.SetActive(false);
        settingB.SetActive(false);
        settingC.SetActive(false);
        specialCamera.SetActive(false);
        cameraA.SetActive(true);
        cameraB.SetActive(true);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            specialCamera.SetActive(!specialCamera.activeSelf);
            cameraA.SetActive(!specialCamera.activeSelf);
            cameraB.SetActive(!specialCamera.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            settingA.SetActive(true);
            settingB.SetActive(false);
            settingC.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            settingA.SetActive(false);
            settingB.SetActive(true);
            settingC.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            cameraA.SetActive(false);
            cameraB.SetActive(false);
            settingC.SetActive(true);
            specialCamera.SetActive(true);
        }
        
    }
}