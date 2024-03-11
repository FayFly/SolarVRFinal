using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SunLightControl : MonoBehaviour
{
    //GameObject xrOrigin;
    // Change light direction to point towards earth
    GameObject earth;
    GameObject[] celestials;
    void Start()
    {
        //xrOrigin = GameObject.FindWithTag("XROrigin");
        celestials = GameObject.FindGameObjectsWithTag("Celestial");
        for (int i = 0; i < celestials.Length; i++)
        {
            if (celestials[i].name == "dEarth")
            {
                earth = celestials[i];
            }
        }
        transform.position = (earth.transform.position - Vector3.zero).normalized * 80;
        transform.LookAt(earth.transform.position);
    }

    private void Update()
    {
        transform.position = (earth.transform.position - Vector3.zero).normalized * 80;
        transform.LookAt(earth.transform.position);
    }
}