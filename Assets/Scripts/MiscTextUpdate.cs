using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MiscTextUpdate : MonoBehaviour
{
    public Slider slider;
    public GameObject sliderText;
    Text textfield;
    // Start is called before the first frame update
    void Start() {
        textfield = sliderText.GetComponent<Text>();
        textfield.text = slider.value.ToString();
    }

    // Update is called once per frame
    void Update() {
        textfield.text = slider.value.ToString();
    }
}