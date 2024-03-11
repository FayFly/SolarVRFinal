using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipseOrbit : MonoBehaviour
{
    public Transform orbitingObject;
    public EllipseBase orbitPath;

    [Range(0f, 1f)]
    public float orbitProgress = 0f;
    public float orbitPeriod = 5f;
    public bool orbitActive = true;

    // Start is called before the first frame update
    void Start()
    {
        if (orbitingObject == null)
        {
            orbitActive = false;
        }
        else
        {
            SetOrbitingObjectPosition();
            StartCoroutine(AnimateOrbit());
        }
        
    }

    void SetOrbitingObjectPosition(){
        Vector2 orbitPos = orbitPath.Evaluate(orbitProgress);
        orbitingObject.localPosition = new Vector3(orbitPos.x, 0, orbitPos.y);
    }

    IEnumerator AnimateOrbit(){
        if (orbitPeriod < 0.1f)
        {
            orbitPeriod = 0.1f;
        }
        float orbitSpeed = 1f / orbitPeriod;
        while (orbitActive)
        {
            orbitProgress += Time.deltaTime * orbitSpeed;
            orbitProgress %= 1f;
            SetOrbitingObjectPosition();
            yield return null;
        }
    }
}
