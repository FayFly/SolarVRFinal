using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Notes", menuName = "ScriptableObjects/EducationNotes", order = 1)]
public class EducationNotes : ScriptableObject
{
    public string planetName;
    public List<string> lines;
    public AudioClip audioClip;
}
