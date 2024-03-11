using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRecenter : MonoBehaviour
{
    // recenter the scene with XR Origin at (0,0,0).
    public float distanceThreshold = 1000f;
    List<Transform> planetsT;
    GameObject xrOrigin;

    public event System.Action PostFloatingOriginUpdate;
    private void Awake()
    {
        xrOrigin = GameObject.FindWithTag("XROrigin");
        GameObject[] celestials = GameObject.FindGameObjectsWithTag("Celestial");
        planetsT = new List<Transform>();
        planetsT.Add(xrOrigin.transform);
        foreach(GameObject obj1 in celestials)
        {
            planetsT.Add(obj1.transform);
        }
    }

    private void LateUpdate()
    {
        RecenterOrigin();
        if (PostFloatingOriginUpdate != null)
        {
            PostFloatingOriginUpdate();
        }
    }

    void RecenterOrigin()
    {
        Vector3 originOffset = xrOrigin.transform.position;
        if (originOffset.magnitude > distanceThreshold){
            foreach (Transform t in planetsT){
                t.position -= originOffset;
            }
        }
    }
}
