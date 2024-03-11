using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;
public class UIControl : MonoBehaviour
{
    public GameObject mainUI;
    public string nextScene;
    public Material defSkyMat;
    public Material otherSkyMat;
    public Slider fovSlider;
    public Slider playerScaleSlider;
    public Slider playerHeightSlider;
    public GameObject planetButtonPrefab;
    GameObject planetButtonsParent;

    private bool activeMainUI = false;
    private bool isDefSkybox = true;
    private float scale = 1.0f;
    private float height = 150f;
    private float fov = 90f;
    GameObject xrOrigin;
    GameObject[] bodies;
    public GameObject notesMain;
    GameObject playerControl;
    Player playerControlScript;
    NotesControl notesMainScript;
    GameObject orbitInfoPlot;
    OrbitInfoPlot orbitInfoPlotScript;
    Camera myCam;
    float initialFov;
    GameObject cameraSystems;
    CameraSystemControl cameraSystemCtrl;
    GameObject cameraButtonsParent;
    int cameraCounts;
    // Start is called before the first frame update
    void Start() {
        //mainUI = GameObject.FindWithTag("MainUI"); 
        //notesMain = GameObject.FindWithTag("NotesMain"); //Cannot use FindWithTag is the GameObject is not active;
        RenderSettings.skybox = defSkyMat;
        isDefSkybox = true;
        ToggleMainUI();
        xrOrigin = GameObject.FindWithTag("XROrigin");
        myCam = xrOrigin.transform.GetChild(0).GetChild(0).GetComponent<Camera>();
        if (myCam != null ) { initialFov = myCam.fieldOfView; }
        playerControl = GameObject.FindWithTag("PlayerControl");
        orbitInfoPlot = GameObject.FindWithTag("OrbitInfoPlot");
        orbitInfoPlotScript = orbitInfoPlot.GetComponent<OrbitInfoPlot>();
        playerControlScript = playerControl.GetComponent<Player>();
        notesMainScript = notesMain.GetComponent<NotesControl>();
        planetButtonsParent = mainUI.transform.GetChild(1).GetChild(2).gameObject;
        InstantiatePlanetButtons();
        cameraSystems = GameObject.FindWithTag("CustomCameras");
        if (cameraSystems != null) { 
            cameraSystemCtrl = cameraSystems.GetComponent<CameraSystemControl>(); 
            cameraCounts = cameraSystems.transform.childCount;
            cameraButtonsParent = mainUI.transform.GetChild(2).GetChild(2).gameObject;
            InstantiateCameraButtons();
        }
        else { cameraCounts = 0; }
    }
    public void Update()
    {
        // Debug.Log(playerScaleSlider.value);
    }
    public void ToggleMenuButtonPressed(InputAction.CallbackContext context) {
        if (context.performed) {
            ToggleMainUI();
        }
    }
    public void ToggleMainUI() { // toggle between hide and show main UI
        if (activeMainUI) {
            mainUI.SetActive(false);
            activeMainUI = false;
        }
        else if(!activeMainUI) {
            mainUI.SetActive(true);
            activeMainUI = true;
        }
    }
    public void ToggleSkybox()
    { // toggle between hide and show main UI
        if (isDefSkybox) {
            RenderSettings.skybox = otherSkyMat;
            isDefSkybox = false;
        }
        else if (!isDefSkybox) {
            RenderSettings.skybox = defSkyMat;
            isDefSkybox = true;
        }
    }
    public void PauseScene() {
        Time.timeScale = 0;
    }
    public void ContinueScene() {
        Time.timeScale = 1;
    }
    public void LoadNextScene() {
        SceneManager.LoadScene(nextScene);
    }
    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame() {
        Application.Quit();
    }
    public void Resize() {
        scale = playerScaleSlider.value;
        xrOrigin.transform.localScale = Vector3.one * scale;
    }
    public void AdjustHeight() {
        height = playerHeightSlider.value;
        xrOrigin.transform.position = new Vector3(xrOrigin.transform.position.x, height, xrOrigin.transform.position.z);
    }
    public void AdjustFov()
    {
        fov = fovSlider.value;
        myCam.fieldOfView = fov;
    }
    private void InstantiatePlanetButtons()
    {
        GameObject buttonObj;
        Button button;
        bodies = FindObjsWithTagOrdered("Celestial");
        for (int i = 0; i < bodies.Length; i++)
        {
            GameObject bodycopy = new GameObject();
            bodycopy = bodies[i];
            buttonObj = Instantiate(planetButtonPrefab, planetButtonsParent.transform, false);
            String name = bodies[i].name;
            buttonObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = name.Substring(1, name.Length - 1);
            button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(delegate { SetPlanet(bodycopy); });
        }
    }
    public void SetPlanet(GameObject obj)
    {
        playerControlScript.planet = obj;
        playerControlScript.isPlanetUpdated = true;
        notesMainScript.planet = obj;
        notesMainScript.isPlanetUpdated = true;
        orbitInfoPlotScript.planet = obj;
        orbitInfoPlotScript.isPlanetUpdated = true;
        //Debug.Log("Button is clicked"+obj.name);
    }
    private void InstantiateCameraButtons()
    {
        GameObject buttonObj;
        Button button;
        for (int i = 0; i < cameraCounts; i++)
        {
            int icopy = i;
            buttonObj = Instantiate(planetButtonPrefab, cameraButtonsParent.transform, false);
            Text tmptxt = buttonObj.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            tmptxt.text = cameraSystems.transform.GetChild(i).name;
            tmptxt.fontSize = 18;
            button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(delegate { SelectCustomCam(icopy); });
        }
    }
    public void SelectCustomCam(int i)
    {
        cameraSystemCtrl.camIndex = i;
    }
    private GameObject[] FindObjsWithTagOrdered(string tag)
    {
        GameObject[] foundObs = GameObject.FindGameObjectsWithTag(tag);
        Array.Sort(foundObs, CompareObNames);
        return foundObs;
    }
    private int CompareObNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }
}
