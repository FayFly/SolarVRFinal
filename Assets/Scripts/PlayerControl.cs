using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public GameObject planet; // select one planet
    public bool isPlanetUpdated = false;
    GameObject nearestPlanet;
    Rigidbody planetRigidbody;
    Orbiter planetOrbiter; 
    public float hoverDistance=20f;
    float hoverDistancePlusR = 20f;
    public float flySpeed = 200f;
    GameObject xrOrigin;
    public bool isFlyTowards = false;
    public bool isFlyFollow = false;
    public bool isFlyFollowRotation = false;
    public bool isFlyAttachTowards = false;
    public bool isFlyAttach = false;
    private bool isFlyAttachDone = false;
    float disThreshold = 2f;
    // Start is called before the first frame update
    void Start(){
        xrOrigin = GameObject.FindWithTag("XROrigin"); // xr origin
        ManualUpdatePlanet();
        //sun = GameObject.Find("aSun");
        nearestPlanet = FindNearestPlanet();
        //Debug.Log(nearestPlanet.name);
    }
    public void ManualUpdatePlanet()
    {
        planetRigidbody = planet.GetComponent<Rigidbody>();
        planetOrbiter = planet.GetComponent<Orbiter>();
        hoverDistancePlusR = hoverDistance + planet.transform.localScale.x/2.0f;
    }
    void FixedUpdate()
    {    
    }
    void Update()
    { 
        if (isPlanetUpdated)
        {
            ManualUpdatePlanet();
            isPlanetUpdated = false;
        }
        FlyTowards();
        FlyFollow();
        FlyAttach();
        FlyFollowRotation();
    }
    public void FlyTowards() 
    {
        if (isFlyTowards)
        {
            float r = Vector3.Distance(planet.transform.position, xrOrigin.transform.position); // calculate distance
            if (r < hoverDistancePlusR)
            {
                isFlyTowards = false;
                isFlyFollow = true;
                isFlyFollowRotation = false;
                isFlyAttachTowards = false;
                isFlyAttach = false;
            }
            else
            {
                Vector3 flyDirection = (planet.transform.position - xrOrigin.transform.position).normalized;
                xrOrigin.transform.LookAt(planet.transform.position);
                xrOrigin.transform.position = xrOrigin.transform.position + flySpeed * Time.deltaTime * flyDirection;
            }
        }
    }
    public void FlyFollow()
    {
        if (isFlyFollow)
        {
            Vector3 planetVelocity = planetRigidbody.velocity;
            xrOrigin.transform.position = xrOrigin.transform.position + Time.deltaTime * planetVelocity;
            xrOrigin.transform.LookAt(planet.transform.position);
        }
    }
    public void FlyFollowRotation()
    {
        if (isFlyFollowRotation)
        {
            //xrOrigin.transform.RotateAround(planet.transform.position, planet.transform.up, planetOrbiter.orbitspeed*Time.fixedDeltaTime);
            xrOrigin.transform.RotateAround(planet.transform.position, planet.transform.up, planetOrbiter.orbitspeed * Time.deltaTime);
        }
    }
    public void FlyAttach()
    {
        if (isFlyAttachTowards) {
            float r = Vector3.Distance(planet.transform.position, xrOrigin.transform.position); // calculate distance
            if (r < planet.transform.localScale.x/2.0f + disThreshold)
            {
                isFlyAttachTowards = false;
                isFlyAttach = true;
                isFlyAttachDone = false;
            }
            else
            {
                Vector3 flyDirection = (planet.transform.position - xrOrigin.transform.position).normalized;
                xrOrigin.transform.LookAt(planet.transform.position);
                xrOrigin.transform.position = xrOrigin.transform.position + flySpeed * Time.deltaTime * flyDirection;
            }
        }
        if (isFlyAttach)
        {
            Vector3 planetVelocity = planetRigidbody.velocity;
            xrOrigin.transform.position = xrOrigin.transform.position + Time.deltaTime * planetVelocity;
            //xrOrigin.transform.LookAt(planet.transform.position); // look at planet
            xrOrigin.transform.RotateAround(planet.transform.position, planet.transform.up, planetOrbiter.orbitspeed * Time.deltaTime);
            Vector3 targetDir = (xrOrigin.transform.position - planet.transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            xrOrigin.transform.rotation = Quaternion.Slerp(xrOrigin.transform.rotation, targetRot, Time.deltaTime * 5f);
            Vector3 nearestDir = (nearestPlanet.transform.position - planet.transform.position).normalized;
            Vector3 targetPos = planet.transform.position + (planet.transform.localScale.x/2.0f+ disThreshold) * nearestDir;
            float r = Vector3.Distance(targetPos, xrOrigin.transform.position); // calculate distance
            if (!isFlyAttachDone)
            {
                //if (r > disThreshold)
                //{
                    xrOrigin.transform.position = targetPos;
                    xrOrigin.transform.LookAt(nearestPlanet.transform.position);
                    //isFlyAttachDone = true;
                //}
            }
        }
    }
    public void ToggleFollowButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleFollow();
        }
    }
    public void ToggleFollowRotationButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleFollowRotation();
        }
    }
    public void ToggleFollowAttachButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleFollowAttach();
        }
    }
    public void ToggleFollow()
    {
        if(isFlyFollow)
        {
            isFlyTowards = false;
            isFlyFollow = false;
            isFlyFollowRotation = false;
            isFlyAttachTowards = false;
            isFlyAttach = false;
            isFlyAttachDone = false;
        }
        else
        {
            isFlyTowards = true;
            isFlyFollow = false;
            isFlyFollowRotation = false;
            isFlyAttachTowards = false;
            isFlyAttach = false;
            isFlyAttachDone = false;
        } 
    }
    public void ToggleFollowRotation()
    {
        if (isFlyFollowRotation) { isFlyFollowRotation = false; }
        else {isFlyFollowRotation = true; }
    }
    public void ToggleFollowAttach()
    {
        if(isFlyAttach) { isFlyAttach = false; isFlyAttachTowards = false; isFlyTowards = false; isFlyFollow = false; isFlyFollowRotation = false; isFlyAttachDone = false;}
        else { isFlyAttach = false; isFlyAttachTowards = true; isFlyAttachDone = false; isFlyTowards = false; isFlyFollow = false; isFlyFollowRotation = false; }
    }
    public GameObject FindNearestPlanet()
    {
        GameObject[] celestials = GameObject.FindGameObjectsWithTag("Celestial");
        int i_nearest = celestials.Length;
        float nearestD = float.PositiveInfinity;
        for (int i = 0; i < celestials.Length; i++)
        {
            if(planet.name!= celestials[i].name)
            {
                float rs = (planet.transform.position - celestials[i].transform.position).sqrMagnitude;
                if (rs < nearestD)
                {
                    nearestD = rs;
                    i_nearest = i;
                }
            }
        }
        return celestials[i_nearest];
    }
}
