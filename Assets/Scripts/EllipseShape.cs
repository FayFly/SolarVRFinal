using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class EllipseShape : MonoBehaviour
{
    LineRenderer lr;
    [Range(3, 720)]
    public int segments = 360;
    public EllipseBase ellipse;
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        CalculateEllipse();
    }
    void CalculateEllipse()
    {
        Vector3[] points = new Vector3[segments + 1];
        for (int i=0; i<segments; i++) {
            Vector2 position2D = ellipse.Evaluate((float)i / (float)segments);
            points[i] = new Vector3(position2D.x, position2D.y, 0f);
        }
        points[segments] = points[0];
        lr.positionCount = segments + 1;
        lr.SetPositions(points);
    }
    void OnValidate(){
        if (Application.isPlaying && lr!=null){
            CalculateEllipse();
        }
    }
}