using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlanetRotation : MonoBehaviour
{
    public Vector3 axis;
    public Transform sun;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        axis = new Vector3(0, Random.Range(0f, 1f), Random.Range(0f, 1f));
        speed = Random.Range(5f, 100f);
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(sun.position, axis, speed * Time.deltaTime);
    }
}
