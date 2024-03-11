using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSystemControl : MonoBehaviour
{
    public GameObject earth;
    public GameObject moon;
    public GameObject sun;
    GameObject[] celestials;
    public GameObject earthSunCam;
    public GameObject earthMoonCam;
    public GameObject overEarthCam;
    public GameObject overSystemCam;
    public GameObject moonEarthCam;
    public GameObject sunEarthCam;
    GameObject toBeActiveCam;
    bool isCustomCamEnabled = false;
    public int camIndex = 0;
    int totalCustomCameras;
    int camCount;
    //LightSystem
    GameObject lightSystem;
    Transform nightLightEarth;
    Transform nightLightMoon;
    // Start is called before the first frame update
    void Start()
    {
        celestials = GameObject.FindGameObjectsWithTag("Celestial");
        for (int i = 0; i < celestials.Length; i++)
        {
            if (celestials[i].name == "aSun")
            {
                sun = celestials[i];
            }
            if (celestials[i].name == "dEarth")
            {
                earth = celestials[i];
            }
            if (celestials[i].name == "eMoon")
            {
                moon = celestials[i];
            }
        }
        //earthSunCam = GameObject.FindWithTag("EarthSunCam");
        //earthMoonCam = GameObject.FindWithTag("EarthMoonCam");
        //overEarthCam = GameObject.FindWithTag("OverEarthCam");
        //overSystemCam = GameObject.FindWithTag("OverSystemCam");
        //moonEarthCam = GameObject.FindWithTag("MoonEarthCam");
        //sunEarthCam = GameObject.FindWithTag("SunEarthCam");
        earthSunCam = transform.GetChild(0).gameObject;
        earthMoonCam = transform.GetChild(1).gameObject;
        overEarthCam = transform.GetChild(2).gameObject;
        overSystemCam = transform.GetChild(3).gameObject;
        moonEarthCam = transform.GetChild(4).gameObject;
        sunEarthCam = transform.GetChild(5).gameObject;
        SetEarthSunCam();
        SetEarthMoonCam();
        SetOverEarthCam();
        SetMoonEarthCam();
        SetSunEarthCam();
        totalCustomCameras = transform.childCount;
        camCount = 0;
        //LightSystem
        lightSystem = GameObject.FindWithTag("LightSystem");
        nightLightEarth = lightSystem.transform.GetChild(0);
        nightLightMoon = lightSystem.transform.GetChild(1);
        SetNightLightEarth();
        SetNightLightMoon();
    }

    // Update is called once per frame
    void Update()
    {
        SetEarthSunCam();
        SetEarthMoonCam();
        SetOverEarthCam();
        SetMoonEarthCam();
        SetSunEarthCam();
        SetNightLightEarth();
        SetNightLightMoon();
    }
    void SetEarthSunCam()
    {
        earthSunCam.transform.position = earth.transform.position + earth.transform.localScale.x / 2 * (sun.transform.position - earth.transform.position).normalized;
        earthSunCam.transform.LookAt(sun.transform.position);
    }
    void SetEarthMoonCam()
    {
        earthMoonCam.transform.position = earth.transform.position + earth.transform.localScale.x/2 * (moon.transform.position-earth.transform.position).normalized;
        //earthMoonCam.transform.position = earth.transform.position + earth.transform.localScale.x / 2*1.1f * (earth.transform.position - sun.transform.position).normalized;
        earthMoonCam.transform.LookAt(moon.transform.position);
        //Vector3 targetDir = (earth.transform.position - sun.transform.position).normalized;
        //Quaternion targetRot = Quaternion.LookRotation(targetDir);
        //earthMoonCam.transform.rotation = targetRot;
    }
    void SetOverEarthCam()
    {
        overEarthCam.transform.position = new Vector3(earth.transform.position.x, overEarthCam.transform.position.y, earth.transform.position.z);
    }
    void SetMoonEarthCam()
    {
        moonEarthCam.transform.position = moon.transform.position + moon.transform.localScale.x / 2 * (earth.transform.position - moon.transform.position).normalized;
        moonEarthCam.transform.LookAt(earth.transform.position);
    }
    void SetSunEarthCam()
    {
        sunEarthCam.transform.position = (earth.transform.position - sun.transform.position).normalized * 40 + sun.transform.position;
        sunEarthCam.transform.LookAt(earth.transform.position);
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
            isCustomCamEnabled =true;
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
            DisableCam(camCount-1);
            camCount++;
        } else if (camCount == totalCustomCameras)
        {
            DisableCam(camCount - 1);
            camCount = 0;
        }
    }
    void EnableCam(int i)
    {
        toBeActiveCam = transform.GetChild(i).gameObject;
        toBeActiveCam.GetComponent<Camera>().depth=10;
    }
    void DisableCam(int i)
    {
        toBeActiveCam = transform.GetChild(i).gameObject;
        toBeActiveCam.GetComponent<Camera>().depth = -10;
    }
    void SetNightLightEarth()
    {
        nightLightEarth.transform.position = earth.transform.position + earth.transform.localScale.x * (earth.transform.position - sun.transform.position).normalized;
        nightLightEarth.transform.LookAt(earth.transform.position);
    }
    void SetNightLightMoon()
    {
        //nightLightMoon.transform.position = earth.transform.position + earth.transform.localScale.x * (earth.transform.position - sun.transform.position).normalized;
        //Vector3 newPos = earth.transform.position + earth.transform.localScale.x * 2 * (earth.transform.position - sun.transform.position).normalized;
        //nightLightMoon.transform.LookAt(newPos);
        nightLightMoon.transform.position = earth.transform.position + earth.transform.localScale.x * (moon.transform.position - earth.transform.position).normalized;
        nightLightMoon.transform.LookAt(moon.transform.position);
    }
}
