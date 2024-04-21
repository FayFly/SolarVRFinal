using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EllipseBase
{
    public float xAxis=1;
    public float yAxis=1;
    public EllipseBase(float xAxis, float yAxis)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
    }
    public Vector2 Evaluate(float t)
    {
        float angle = Mathf.Deg2Rad * 360 * t;
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        return new Vector2(x, y);
    }
}