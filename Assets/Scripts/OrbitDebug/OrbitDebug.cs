using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitDebug : MonoBehaviour
{
    public int numSteps = 1000;
    public float timeStep = 0.1f;
    public bool relativeToBody;
    public CelestialBody centralBody;
    public float lineWidth = 10;
    public bool useThickLines = true;

    GameObject[] bodies;
    GameObject[] virtualBodies;

    // Start is called before the first frame update
    void Start(){
        bodies = GameObject.FindGameObjectsWithTag("Celestial");
        virtualBodies = new GameObject[bodies.Length];
        // Initialize virtual bodies (don't want to move the actual bodies)
        for (int i = 0; i < virtualBodies.Length; i++){
            virtualBodies[i] = Instantiate(bodies[i]);
            bodies[i].SetActive(false);
            virtualBodies[i].SetActive(true);
        }

        if (Application.isPlaying){
            HideOrbits();
        }
    }

    // Update is called once per frame
    void Update(){
        if (!Application.isPlaying){
            ShowOrbits();
        }
    }
    void ShowOrbits(){
        var drawPoints = new Vector3[bodies.Length][];
        int referenceIndex = 0;
        Vector3 referenceBodyInitPos = Vector3.zero;
        for (int i = 0; i < virtualBodies.Length; i++) {
            drawPoints[i] = new Vector3[numSteps];
            if (bodies[i] == centralBody && relativeToBody){
                referenceIndex = i;
                referenceBodyInitPos = virtualBodies[i].transform.position;
            }
        }
        // Simulate
        for (int step = 0; step < numSteps; step++)
        {
            Vector3 referenceBodyPos = (relativeToBody) ? virtualBodies[referenceIndex].transform.position : Vector3.zero;
            // Update velocities
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                virtualBodies[i].GetComponent<Rigidbody>().velocity += CalculateAcceleration(i, virtualBodies) * timeStep;
            }
            // Update positions
            for (int i = 0; i < virtualBodies.Length; i++){
                Vector3 newPos = virtualBodies[i].transform.position + virtualBodies[i].GetComponent<Rigidbody>().velocity * timeStep;
                virtualBodies[i].transform.position = newPos;
                if (relativeToBody)
                {
                    var referenceOffset = referenceBodyPos - referenceBodyInitPos;
                    newPos -= referenceOffset;
                }
                if (relativeToBody && i == referenceIndex)
                {
                    newPos = referenceBodyInitPos;
                }

                drawPoints[i][step] = newPos;
            }
        }

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++){
            var pathColour = virtualBodies[bodyIndex].GetComponent<LineRenderer>().startColor;
            if (useThickLines)
            {
                var lineRenderer = virtualBodies[bodyIndex].GetComponent<LineRenderer>();
                lineRenderer.enabled = true;
                lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                lineRenderer.SetPositions(drawPoints[bodyIndex]);
                lineRenderer.startColor = pathColour;
                lineRenderer.endColor = pathColour;
                lineRenderer.widthMultiplier = lineWidth;
            }
            else
            {
                for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++) {
                    Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColour);
                }

                // Hide renderer
                var lineRenderer = virtualBodies[bodyIndex].GetComponent<LineRenderer>();
                if (lineRenderer) {
                    lineRenderer.enabled = false;
                }
            }

        }
    }

    Vector3 CalculateAcceleration(int i, GameObject[] virtualBodies) {
        Vector3 acceleration = Vector3.zero;
        for (int j = 0; j < virtualBodies.Length; j++){
            if (i != j) {
                float sqrDst = (virtualBodies[j].transform.position - virtualBodies[i].transform.position).sqrMagnitude;
                Vector3 forceDir = (virtualBodies[j].transform.position - virtualBodies[i].transform.position).normalized;
                acceleration = UniverseSettings.gravitationalConstant * virtualBodies[j].GetComponent<Rigidbody>().mass / sqrDst * forceDir;
            }
        }
        return acceleration;
    }

    void HideOrbits(){
        // Draw paths
        for (int bodyIndex = 0; bodyIndex < bodies.Length; bodyIndex++)
        {
            var lineRenderer = bodies[bodyIndex].GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
        }
    }
}
