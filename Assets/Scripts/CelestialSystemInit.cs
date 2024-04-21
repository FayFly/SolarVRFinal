using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
//[RequireComponent(typeof(Rigidbody))]
public class CelestialSystemInit : MonoBehaviour
{
    public bool containsSatellites = false;
    public string[] celestialBodies;
    public bool setCelestialSystemsParams = true;
    public bool calcInitVelocities = true;
    public Vector3[] initPositions;
    public Vector3[] initVelocities;
    public float[] massArray;
    public float[] radiusArray;
    public bool isElliptic = false;
    [Range(0.2f, 1.8f)]
    public float ellipticRatio=1.0f;
    GameObject[] bodies;
    private float satelliteRatio = 0f;
    private void Awake()
    {
        bodies = FindObjsWithTagOrdered("Celestial");
        celestialBodies = new string[bodies.Length];
        for (int i=0; i<bodies.Length; i++){ 
            //Debug.Log(bodies[i].name);
            celestialBodies[i] = bodies[i].name;
        }
        if (setCelestialSystemsParams) {
            for (int i=0; i<bodies.Length; i++) {
                Rigidbody rb = bodies[i].GetComponent<Rigidbody>();
                TrailRenderer tr = bodies[i].GetComponent<TrailRenderer>();
                rb.mass = massArray[i];
                bodies[i].transform.localScale = Vector3.one * radiusArray[i];
                bodies[i].transform.position = initPositions[i];
                if(calcInitVelocities && (i != 0)) {
                    rb.velocity = CalcInitialVelocity(i);
                    float orbitPeriod = CalcOrbitPeriod(i);
                    tr.time = orbitPeriod;
                    CalcSatelliteOrbitSpeed(i, orbitPeriod);
                }
                else {
                    rb.velocity = initVelocities[i];
                }  
            }
        }
    }
    private GameObject[] FindObjsWithTagOrdered(string tag)
    {
        GameObject[] foundObs = GameObject.FindGameObjectsWithTag(tag);
        Array.Sort(foundObs, CompareObNames);
        return foundObs;
    }
    private int CompareObNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }
    private Vector3 CalcInitialVelocity(int i)
    {
        float kratio;
        float r;
        float m;
        CelestialBody cb1 = bodies[i].GetComponent<CelestialBody>();
        if (!cb1.isSatellite) {
            r = Vector3.Distance(bodies[i].transform.position, bodies[0].transform.position);
            m = massArray[0] + massArray[i];
            kratio = 1f;
        } else {
            r = Vector3.Distance(bodies[i].transform.position, cb1.motherPlanet.transform.position);
            m = massArray[i] + cb1.motherPlanet.GetComponent<Rigidbody>().mass;
            kratio = satelliteRatio;
        }
        float v;
        if (isElliptic) {
            // set initial velocity v0=G*M*(2/r-1/a), a is the length of the semi-major axis for elliptic motion
            // a = r/ellipticRatio
            v = Mathf.Sqrt(kratio*UniverseSettings.gravitationalConstant*m*(2/r-ellipticRatio/r));
        }
        else {
            // set initial velocity v0=sqrt(G*M/r) for circular motion
            v = Mathf.Sqrt(kratio*UniverseSettings.gravitationalConstant * m / r);
        }
        Vector3 velocity; 
        if (cb1.isSatellite)
        {
            Vector3 vref = cb1.motherPlanet.GetComponent<Rigidbody>().velocity;
            velocity = new Vector3(v, 0f, 0f)+0*vref;
        }
        else
        {
            velocity = new Vector3(v, 0f, 0f);
        }
        //Debug.Log(bodies[i].name + " " + r + " " + m + " " + kratio + " ");
        //Debug.Log(velocity);
        return velocity;
    }
    private float CalcOrbitPeriod(int i) {
        float kratio;
        float r;
        float m;
        CelestialBody cb1 = bodies[i].GetComponent<CelestialBody>();
        if (!cb1.isSatellite)
        {
            r = Vector3.Distance(bodies[i].transform.position, bodies[0].transform.position);
            m = massArray[0] + massArray[i];
            kratio = 1f;
        } else {
            r = Vector3.Distance(bodies[i].transform.position, cb1.motherPlanet.transform.position);
            m = massArray[i] + cb1.motherPlanet.GetComponent<Rigidbody>().mass;
            kratio = satelliteRatio;
        }
        float t;
        if (isElliptic) {
            // period is 2*pi*sqrt(a^3/GM), a is the length of the semi-major axis for elliptic motion
            // a = r/ellipticRatio
            t = 2 * Mathf.PI * Mathf.Sqrt(Mathf.Pow(r/ ellipticRatio, 3) / kratio / UniverseSettings.gravitationalConstant / m);
        }
        else {
            // period is 2*pi*sqrt(r^3/GM) for circular motion
            t = 2*Mathf.PI*Mathf.Sqrt(Mathf.Pow(r, 3)/kratio/UniverseSettings.gravitationalConstant/m);

        }
        if (cb1.isSatellite) // if planet is a satellite, get the period of its mother planet
        {
            t = cb1.motherPlanet.GetComponent<TrailRenderer>().time/12f; //12 months
        }
        return t;
    }
    private void CalcSatelliteOrbitSpeed(int i, float t)
    {
        CelestialBody cb1 = bodies[i].GetComponent<CelestialBody>();
        if (cb1.isSatellite)
        {
            float orbitSpeed = 360f / t;
            cb1.satelliteOrbitSpeed = orbitSpeed;
            bodies[i].GetComponent<Orbiter>().orbitspeed = orbitSpeed; //Time.deltaTime/Time.fixedDeltaTime
            cb1.distanceToMotherPlanet = Vector3.Distance(bodies[i].transform.position, cb1.motherPlanet.transform.position);
        }     
    }
}