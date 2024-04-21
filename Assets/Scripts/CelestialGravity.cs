using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;
public class CelestialGravity : MonoBehaviour
{
    public bool containsSatellites = false;
    private const float physicsTimeStep = 0.01f;
    private float satelliteRatio = 0f;
    GameObject[] celestials; // store all the celestials objects in the system
    // public GameObject[] satellites;
    // Start is called before the first frame update
    void Start()
    {
        celestials = GameObject.FindGameObjectsWithTag("Celestial");
        //InitialVelocityCircular();
        ManualRotateStart();
    }
    void FixedUpdate()
    {
        if (!containsSatellites) { CalculateGravity(); }
        else { CalculateGravityWithSatellites(); }
        //Debug.Log("FixedUpdate "+celestials[1].transform.position);
    }
    private void Update()
    {
        if (containsSatellites) { ManualRotate(); }
        //Debug.Log("Update "+celestials[1].transform.position);
    }
    private void LateUpdate()
    {
        //Debug.Log("LateUpdate " + celestials[1].transform.position);
    }
    // function to apply gravity force
    void CalculateGravity(){
        foreach (GameObject obj1 in celestials){
            foreach (GameObject obj2 in celestials){
                if (!obj1.Equals(obj2)){ // two different celestials
                    float m1 = obj1.GetComponent<Rigidbody>().mass;
                    float m2 = obj2.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(obj1.transform.position, obj2.transform.position);
                    // apply gravity force G*m1*m2/r^2, need to adjust the parameters to have resonable period.
                    obj1.GetComponent<Rigidbody>().AddForce((obj2.transform.position - obj1.transform.position).normalized *
                        (UniverseSettings.gravitationalConstant * (m1 * m2) / (r * r)));
                }
            }
        }
    }
    void CalculateGravityWithSatellites()
    {
        float kratio;
        foreach (GameObject obj1 in celestials)
        {
            CelestialBody cb1 = obj1.GetComponent<CelestialBody>();
            foreach (GameObject obj2 in celestials)
            {
                CelestialBody cb2 = obj2.GetComponent<CelestialBody>();
                if (!obj1.Equals(obj2))
                { // two different celestials
                    if (cb1.isSatellite) { //if a planet is satellite, only consider the gravity from its mother planet.
                        if(cb1.motherPlanet.name == obj2.name) {
                            kratio = satelliteRatio;
                        } else { kratio = 0; }
                    }
                    else {
                        if (cb2.isSatellite) { kratio = 0; } else { kratio = 1; }
                    }
                    float m1 = obj1.GetComponent<Rigidbody>().mass;
                    float m2 = obj2.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(obj1.transform.position, obj2.transform.position);
                    // apply gravity force G*m1*m2/r^2, need to adjust the parameters to have resonable period.
                    obj1.GetComponent<Rigidbody>().AddForce((obj2.transform.position - obj1.transform.position).normalized *
                        (kratio*UniverseSettings.gravitationalConstant * (m1 * m2) / (r * r)));
                }
            }
        }
    }
    // function to calculate initial velocity (not used)
    void InitialVelocityCircular(){
        foreach (GameObject obj1 in celestials){
            foreach (GameObject obj2 in celestials){
                if (!obj1.Equals(obj2)){ // two different celestials
                    float m2 = obj2.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(obj1.transform.position, obj2.transform.position);
                    // set initial velocity v0=sqrt(G*m2/r) for circular motion
                    obj1.GetComponent<Rigidbody>().velocity += obj1.transform.right * Mathf.Sqrt((UniverseSettings.gravitationalConstant * m2) / r);
                }
            }
        }
    }
    // function to calculate initial velocity (not used)
    void InitialVelocityElliptic(){
        foreach (GameObject obj1 in celestials){
            foreach (GameObject obj2 in celestials){
                if (!obj1.Equals(obj2)){ // two different celestials
                    float m2 = obj2.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(obj1.transform.position, obj2.transform.position);
                    float a = r;
                    // set initial velocity v0=G*m2*(2/r+1/a), a is the length of the semi-major axis for elliptic motion
                    obj1.GetComponent<Rigidbody>().velocity += obj1.transform.right * (UniverseSettings.gravitationalConstant * m2 * (2 / r + 1 / a));
                }
            }
        }
    }
    // function to Auto Orient a Object (not used)
    void AutoOrient(Vector3 down){
        float autoOrientSpeed = 1f;
        Quaternion orientationDirection = Quaternion.FromToRotation(-transform.up, down) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, orientationDirection, autoOrientSpeed * Time.deltaTime);
    }
    void ManualRotateStart()
    {
        foreach (GameObject obj1 in celestials)
        {
            CelestialBody cb1 = obj1.GetComponent<CelestialBody>();
            if (cb1.isSatellite)
            {
                Quaternion initDirection = Quaternion.LookRotation((obj1.transform.position - cb1.motherPlanet.transform.position), Vector3.up);
                cb1.currentAngleToMotherPlanet = initDirection.eulerAngles.y;
            }
        }
    }
    void ManualRotate()
    {
        Vector3 unitDir = new Vector3(1f, 0, 1f).normalized;
        foreach (GameObject obj1 in celestials) {
            CelestialBody cb1 = obj1.GetComponent<CelestialBody>();
            if (cb1.isSatellite) {
                //obj1.transform.position = (obj1.transform.position - cb1.motherPlanet.transform.position).normalized * cb1.distanceToMotherPlanet + cb1.motherPlanet.transform.position;
                //obj1.transform.RotateAround(cb1.motherPlanet.transform.position, Vector3.up, cb1.satelliteOrbitSpeed * Time.deltaTime);
                //Quaternion startDirection = Quaternion.LookRotation((obj1.transform.position - cb1.motherPlanet.transform.position), Vector3.up);
                Quaternion endDirection = Quaternion.Euler(new Vector3(0f, (cb1.currentAngleToMotherPlanet + cb1.satelliteOrbitSpeed * Time.deltaTime), 0f));
                Vector3 endDir = endDirection * Vector3.forward;
                Quaternion endDirection2 = Quaternion.LookRotation(endDir, Vector3.up);
                cb1.currentAngleToMotherPlanet = endDirection2.eulerAngles.y;
                Vector3 newPosWithRotation = endDir.normalized * cb1.distanceToMotherPlanet + cb1.motherPlanet.transform.position;
                obj1.transform.position = newPosWithRotation;
            }
        }
    }
}