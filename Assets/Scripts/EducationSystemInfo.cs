using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EducationSystemInfo : MonoBehaviour
{
    public EducationNotes educationNotes;
    public EducationVideo educationVideo;
    public EducationNotes ReturnEducationNotes() {
        return educationNotes;
    }
    public EducationVideo ReturnEducationVideo() {
        return educationVideo;
    }
}