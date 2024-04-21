using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
[CreateAssetMenu(fileName = "Video", menuName = "ScriptableObjects/EducationVideo", order = 1)]
public class EducationVideo : ScriptableObject
{
    public bool isUrl = false;
    public VideoClip clip;
    public string videoUrl;
}