using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.HableCurve;
[RequireComponent(typeof(LineRenderer))]
public class OrbitInfoPlot : MonoBehaviour
{
    public GameObject planet; // select one planet
    public bool isPlanetUpdated = false;
    public GameObject orbitTextPrefab;
    GameObject xrOrigin;
    LineRenderer lr;
    PlanetOrbitInfor planetOrbitInfor;
    GameObject periPointText;
    GameObject apoPointText;
    GameObject periodText;
    bool showOrbitInfo = false;
    bool needUpdateInfo = true;
    // Start is called before the first frame update
    void Start()
    {
        xrOrigin = GameObject.FindWithTag("XROrigin");
        InstantiateOrbitText();
        if (planet != null){planetOrbitInfor = planet.GetComponent<PlanetOrbitInfor>();}
    }
    // Update is called once per frame
    void Update()
    {
        if (isPlanetUpdated && planet!=null)
        {
            planetOrbitInfor = planet.GetComponent<PlanetOrbitInfor>();
            needUpdateInfo = true;
            isPlanetUpdated = false;           
        }
        if (needUpdateInfo) { PlotOrbitInfo(); }
        if (showOrbitInfo)
        {
            AlignOrienation(periPointText);
            AlignOrienation(apoPointText);
            AlignOrienation(periodText);
        }
    }
    public void ToggleOrbitInfoPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePlot();
        }
    }
    void TogglePlot()
    {
        if (showOrbitInfo)
        {
            periPointText.SetActive(false);
            apoPointText.SetActive(false);
            periodText.SetActive(false);
            showOrbitInfo = false;
            lr.enabled = false;
        }
        else
        {
            periPointText.SetActive(true);
            apoPointText.SetActive(true);
            periodText.SetActive(true);
            showOrbitInfo = true;
            lr.enabled = true;
        }
    }
    void PlotOrbitInfo()
    {
        if (planetOrbitInfor.reachOnePeriod)
        {
            periPointText.transform.position = planetOrbitInfor.periPos;
            apoPointText.transform.position = planetOrbitInfor.apoPos;
            Vector3 newpos = planetOrbitInfor.periPos + (planetOrbitInfor.apoPos - planetOrbitInfor.periPos) * 0.75f;
            periodText.transform.position = newpos;
            string t = planetOrbitInfor.orbitPeriod.ToString("0.00");
            periodText.transform.GetComponent<Text>().text = "Orbital Period: " + t;
            DrawLine();
            needUpdateInfo = false;
        }
        else
        {
            periPointText.transform.position = planetOrbitInfor.periPos;
            apoPointText.transform.position = planetOrbitInfor.apoPos;
            periodText.transform.position = planet.transform.position;
            string t = planetOrbitInfor.orbitPeriod.ToString("0.00");
            periodText.transform.GetComponent<Text>().text = "Orbital Period: " + t;
            DrawLine();
        }
    }
    void DrawLine()
    {
        Vector3[] points = new Vector3[2];
        points[0] = planetOrbitInfor.periPos;
        points[1] = planetOrbitInfor.apoPos;
        lr.positionCount = 2;
        lr.SetPositions(points);
    }
    private void InstantiateOrbitText()
    {
        periPointText=Instantiate(orbitTextPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        apoPointText = Instantiate(orbitTextPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        periodText = Instantiate(orbitTextPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        periPointText.transform.GetComponent<Text>().text = "perihelion";
        apoPointText.transform.GetComponent<Text>().text = "Aphelion";
        periodText.transform.GetComponent<Text>().text = "Orbital Period";
        periPointText.SetActive(false);
        apoPointText.SetActive(false);
        periodText.SetActive(false);
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }
    public void AlignOrienation(GameObject obj)
    {
        Vector3 targetDir = (obj.transform.position - xrOrigin.transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, targetRot, Time.deltaTime * 5f);
    }
}