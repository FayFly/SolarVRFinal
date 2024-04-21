using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class AmbientLightControl : MonoBehaviour
{
    public float maxIntensity = 1.0f;
    SunLightControl sunLight;
    GameObject xrOrigin;
    Light ambientLight;
    const float rotationSpeed = 0.2f;
    // Start is called before the first frame update
    void Start() {
        sunLight = FindObjectOfType<SunLightControl>();
        ambientLight = GetComponent<Light>();
        xrOrigin = GameObject.FindWithTag("XROrigin");
        transform.rotation = CalcAmbientLightRot();
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, CalcAmbientLightRot(), Time.deltaTime * rotationSpeed);
        float sunDir = Vector3.Dot(sunLight.transform.forward, transform.forward);
        float idir = 1 - Mathf.Clamp01(sunDir); //0: sun in the same dir; 1: sun direction is perpendicular;
        float intensityMult = Mathf.Clamp01((idir - 0.5f) * 2);
        ambientLight.intensity = intensityMult * maxIntensity;
    }
    private void OnValidate()
    {
        ambientLight.intensity = maxIntensity;
    }
    Quaternion CalcAmbientLightRot()
    {
        GameObject[] celestials = GameObject.FindGameObjectsWithTag("Celestial");
        Vector3 nearestPlanet = Vector3.zero;
        float nearestD = float.PositiveInfinity;
        for (int i=0; i<celestials.Length; i++) {
            float rs = (xrOrigin.transform.position - celestials[i].transform.position).sqrMagnitude;
            if (rs < nearestD) {
                nearestD = rs;
                nearestPlanet = celestials[i].transform.position;
            }
        }
        Vector3 targetDir = (nearestPlanet - xrOrigin.transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        return targetRot;
    }
}