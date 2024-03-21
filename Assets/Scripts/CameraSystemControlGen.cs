using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSystemControlGen : MonoBehaviour
{
    GameObject toBeActiveCam;
    bool isCustomCamEnabled = false;
    public int camIndex = 0;
    int totalCustomCameras;
    int camCount;
    // Start is called before the first frame update
    void Start()
    {
        totalCustomCameras = transform.childCount;
        camCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleCustomCamPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleCustomCam();
        }
    }
    void ToggleCustomCam()
    { // toggle between hide and show a custom camera
        if (isCustomCamEnabled)
        {
            DisableCam(camIndex);
            isCustomCamEnabled = false;
        }
        else if (!isCustomCamEnabled)
        {
            EnableCam(camIndex);
            isCustomCamEnabled = true;
        }
    }
    public void BrowseCustomCamPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            BrowseCustomCam();
        }
    }
    void BrowseCustomCam()
    {
        if (camCount == 0)
        {
            EnableCam(camCount);
            camCount++;
        }
        else if (camCount < totalCustomCameras)
        {
            EnableCam(camCount);
            DisableCam(camCount - 1);
            camCount++;
        }
        else if (camCount == totalCustomCameras)
        {
            DisableCam(camCount - 1);
            camCount = 0;
        }
    }
    void EnableCam(int i)
    {
        toBeActiveCam = transform.GetChild(i).gameObject;
        toBeActiveCam.GetComponent<Camera>().depth = 10;
    }
    void DisableCam(int i)
    {
        toBeActiveCam = transform.GetChild(i).gameObject;
        toBeActiveCam.GetComponent<Camera>().depth = -10;
    }
}
