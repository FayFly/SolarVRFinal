using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ToggleTextUpdate : MonoBehaviour
{
    public GameObject textfield;
    Text text;
    // Start is called before the first frame update
    void Start() {
        text = textfield.GetComponent<Text>();
        text.enabled = true;
    }
    public void ToggleTextDisplay()
    {
        text.enabled = !text.enabled;
    }
}