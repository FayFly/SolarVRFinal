using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : MonoBehaviour
{
    public bool isSatellite = false;
    public GameObject motherPlanet;
    public float satelliteOrbitSpeed=0f;
    public float distanceToMotherPlanet = 0f;
    public float currentAngleToMotherPlanet = 0f;
    public bool setInitParams = false;
    public float radius = 10.0f;
    public float mass = 1.0f;
    public Vector3 initPos = Vector3.zero;
    public Vector3 initVelocity = Vector3.zero;
    Rigidbody rb;
    void Awake() {
        rb = GetComponent<Rigidbody>();
        if (setInitParams) {
            rb.mass = mass;
            rb.velocity = initVelocity;
            this.transform.position = initPos;
            this.transform.localScale = Vector3.one * radius;
        }
    }
    public void CalculateGravity(CelestialBody[] allBodies)
    {
        foreach (CelestialBody otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float r = Vector3.Distance(otherBody.transform.position, this.transform.position);
                // apply gravity force G*m1*m2/r^2, need to adjust the parameters to have resonable period.
                rb.AddForce((otherBody.transform.position - rb.transform.position).normalized *
                    (UniverseSettings.gravitationalConstant * (rb.mass * otherBody.mass) / (r * r)));
            }
        }
    }
    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        foreach (CelestialBody otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float sqrDst = (otherBody.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rb.position - rb.position).normalized;
                Vector3 acceleration = UniverseSettings.gravitationalConstant * otherBody.mass / sqrDst * forceDir;
                rb.velocity += acceleration * timeStep;
            }
        }
    }
    public void UpdatePosition(float timeStep)
    {
        rb.MovePosition(rb.position + rb.velocity * timeStep);

    }
}