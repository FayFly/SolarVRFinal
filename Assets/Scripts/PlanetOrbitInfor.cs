using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering;

public class PlanetOrbitInfor : MonoBehaviour
{
    public float periPoint;
    public Vector3 periPos;
    public float apoPoint;
    public Vector3 apoPos;
    public Vector3 initPos;
    public float orbitPeriod;
    public float calculatedOrbitPeriod;
    public Vector3 fastestSpeedPos;
    public float fastestOrbitSpeed;
    public Vector3 slowestSpeedPos;
    public float slowestOrbitSpeed;
    float orbitPeriodCounter = 0;
    float distanceToInitPos;
    float disThreshold = 4f;
    bool isCloser;
    bool reachPeriPoint;
    bool reachApoPoint;
    bool reachFastestSpeed;
    bool reachSlowestSpeed;
    public bool reachOnePeriod = false;
    GameObject celectialSystemInit;
    // Start is called before the first frame update
    void Start()
    {
        periPoint = float.PositiveInfinity;
        apoPoint = 0;
        fastestOrbitSpeed = 0;
        slowestOrbitSpeed = float.PositiveInfinity;
        initPos = transform.position;
        distanceToInitPos = 0;
        isCloser = false;
        if (initPos == Vector3.zero) { 
            reachPeriPoint = true; 
            reachApoPoint = true; 
            reachOnePeriod = true;
            reachFastestSpeed = true;
            reachSlowestSpeed = true;
        }
        else {
            reachPeriPoint = false;
            reachApoPoint = false;
            reachOnePeriod = false;
            reachFastestSpeed = false;
            reachSlowestSpeed = false;
        }
        orbitPeriod = 0;
        orbitPeriodCounter = 0;
        celectialSystemInit = GameObject.FindWithTag("emptyobjsysteminit");
        calculatedOrbitPeriod = CalcOrbitPeriod();
    }

    // Update is called once per frame
    void Update()
    {
        if (!reachPeriPoint) { FindNearestDistance(); }
        if(!reachApoPoint) { FindFurthestDistance();}
        if (!reachOnePeriod) { FindPeriod(); }
        if(!reachFastestSpeed) { FindFastestSpeed(); }
        if (!reachSlowestSpeed) { FindSlowestSpeed(); }
        //if (reachPeriPoint) { Debug.Log(periPoint);}
        //if (reachApoPoint) { Debug.Log(apoPoint); }
        //if (reachOnePeriod) { Debug.Log(orbitPeriod); Debug.Log(calculatedOrbitPeriod); }
    }
    public void FindPeriod()
    {
        float distance = Vector3.Distance(transform.position, initPos);
        if (distance<disThreshold && isCloser)
        {
            reachOnePeriod = true; // Reaching Period, stop Timer
            orbitPeriod = orbitPeriodCounter;
            reachPeriPoint = true;
            reachApoPoint = true;
            reachFastestSpeed = true;
            reachSlowestSpeed = true;
        }
        else
        {
            orbitPeriodCounter += Time.deltaTime;
            if (distance < distanceToInitPos)
            {
                isCloser = true;
            }
            distanceToInitPos = distance;
        }
    }
    public void FindNearestDistance()
    {
        float distance = Vector3.Distance(transform.position, Vector3.zero);
        if(distance < periPoint)
        {
            periPoint = distance;
            periPos = transform.position;
        }
    }
    public void FindFurthestDistance()
    {
        float distance = Vector3.Distance(transform.position, Vector3.zero);
        if (distance > apoPoint)
        {
            apoPoint = distance;
            apoPos = transform.position;
        }
    }
    public void FindFastestSpeed()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float speed = rb.velocity.magnitude;
        if(speed> fastestOrbitSpeed)
        {
            fastestOrbitSpeed = speed;
            fastestSpeedPos = transform.position;
        }
    }
    public void FindSlowestSpeed()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float speed = rb.velocity.magnitude;
        if (speed < slowestOrbitSpeed)
        {
            slowestOrbitSpeed = speed;
            slowestSpeedPos = transform.position;
        }
    }
    private float CalcOrbitPeriod()
    {
        CelestialSystemInit systemInit = celectialSystemInit.GetComponent<CelestialSystemInit>();
        bool isElliptic = systemInit.isElliptic;
        float ellipticRatio = systemInit.ellipticRatio;
        float r = Vector3.Distance(transform.position, Vector3.zero);
        float m = systemInit.massArray[0] + transform.GetComponent<Rigidbody>().mass;
        float t;
        if (isElliptic)
        {
            // period is 2*pi*sqrt(a^3/GM), a is the length of the semi-major axis for elliptic motion
            // a = r/ellipticRatio
            t = 2 * Mathf.PI * Mathf.Sqrt(Mathf.Pow(r / ellipticRatio, 3) / UniverseSettings.gravitationalConstant / m);
        }
        else
        {
            // period is 2*pi*sqrt(r^3/GM) for circular motion
            t = 2 * Mathf.PI * Mathf.Sqrt(Mathf.Pow(r, 3) / UniverseSettings.gravitationalConstant / m);

        }
        return t;
    }
}
