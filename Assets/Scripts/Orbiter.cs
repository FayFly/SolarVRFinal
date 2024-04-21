using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Orbiter : MonoBehaviour
{
    Transform planet;
    public float orbitspeed;
    Vector3 initAxis;
    // Start is called before the first frame update
    void Start()
    {
        planet = GetComponent<Transform>();
        initAxis = Quaternion.Euler(planet.eulerAngles) * Vector3.up;
    }
    // Update is called once per frame
    void Update()
    {
        planet.Rotate(Vector3.up, orbitspeed * Time.deltaTime, Space.Self); //orbitspeed = degreesPerSecond
        //planet.Rotate(initAxis, orbitspeed * Time.deltaTime, Space.World);
    }
}