using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.InputSystem;
public class NotesControl : MonoBehaviour
{
    public GameObject planet;
    public bool isPlanetUpdated = false;
    public GameObject notesUI;
    public GameObject screenUI;
    public AudioSource audioSource;
    public VideoPlayer videoPlayer;
    AudioSource videoPlayerAudioSource;
    private bool activeNotesUI = false;
    private bool activeScreenUI = false;
    private bool activeNotesParent = false;
    EducationSystemInfo educationSystemInfo;
    EducationNotes educationNotes;
    EducationVideo educationVideo;
    GameObject xrOrigin;
    Text nameText;
    Text contentText;
    string contentString;
    private bool isAudioAvailable = false;
    private bool isVideoAvailable = false;
    // Start is called before the first frame update
    void Start() {
        // get planet GameObject
        //planet = transform.parent.gameObject;
        ManualUpdateNotes();
        // xr origin
        xrOrigin = GameObject.FindWithTag("XROrigin");
        HideBoth();
        if (!activeNotesParent) { DisplayNotesParent(); }
    }
    private void Update()
    {
        if(isPlanetUpdated) 
        { 
            ManualUpdateNotes();
            isPlanetUpdated = false;
        }
    }
    private void LateUpdate() {
        //AlignOrienation();
    }
    public void ManualUpdateNotes()
    {
        educationSystemInfo = planet.GetComponent<EducationSystemInfo>();
        educationNotes = educationSystemInfo.ReturnEducationNotes();
        educationVideo = educationSystemInfo.ReturnEducationVideo();
        notesUI = transform.GetChild(0).gameObject;
        screenUI = transform.GetChild(1).gameObject;
        // assign notes
        nameText = transform.GetChild(0).GetChild(0).GetComponent<Text>(); // Notes - Scroll Notes - Header
        contentText = transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>(); // Notes - Scroll Notes - Scroll View - Viewport - Content - Scroll Text
        // assign audio
        audioSource = GetComponent<AudioSource>();
        contentString = "";
        if (educationNotes != null)
        {
            nameText.text = educationNotes.planetName;
            for (int i = 0; i < educationNotes.lines.Count; i++)
            {
                contentString += educationNotes.lines[i];
                contentString += "\n";
            }
            contentText.text = contentString;
            if(educationNotes.audioClip != null)
            {
                audioSource.clip = educationNotes.audioClip;
                isAudioAvailable = true;
            }
            else
            {
                isAudioAvailable = false;
            }
        }
        else
        {
            nameText.text = "N.A.";
            contentText.text = "No information provided.\n";
            isAudioAvailable = false;
        }
        // assign video
        videoPlayer = this.transform.GetChild(1).GetComponent<VideoPlayer>();
        videoPlayerAudioSource = this.transform.GetChild(1).GetComponent<AudioSource>();
        if(educationVideo!=null)
        {
            if (educationVideo.isUrl)
            {
                videoPlayer.source = VideoSource.Url;
                videoPlayer.url = educationVideo.videoUrl;
                videoPlayer.controlledAudioTrackCount = 1;
                videoPlayer.EnableAudioTrack(0, true);
                videoPlayer.SetTargetAudioSource(0, videoPlayerAudioSource);
                videoPlayerAudioSource.volume = 1.0f;
            }
            else
            {
                videoPlayer.source = VideoSource.VideoClip;
                videoPlayer.clip = educationVideo.clip;
                videoPlayer.controlledAudioTrackCount = 1;
                videoPlayer.EnableAudioTrack(0, true);
                videoPlayer.SetTargetAudioSource(0, videoPlayerAudioSource);
                videoPlayerAudioSource.volume = 1.0f;
            }
            isVideoAvailable = true;
        }
        else
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = "";
            isVideoAvailable = false;
        }
    }
    public void ToggleNotesPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleNotes();
        }
    }
    public void ToggleScreenPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleScreen();
        }
    }
    public void ToggleNotes()
    {
        if (!activeNotesUI) {
            DisplayNotes();
            PlayAudio();
        } else if (activeNotesUI) {
            HideNotes();
            StopAudio();
        }
    }
    public void ToggleScreen() {
        if (!activeScreenUI){
            DisplayScreen();
            PlayVideo();
        } else if (activeScreenUI) {
            HideScreen();
            StopVideo();
        }
    }
    public void PlayAudio(){
        audioSource.Play();
    }
    public void PauseAudio(){
        audioSource.Pause();
    }
    public void StopAudio(){
        audioSource.Stop();
    }
    public void PlayVideo()
    {
        videoPlayer.Play();
        videoPlayerAudioSource.Play();
    }
    public void PauseVideo()
    {
        videoPlayer.Pause();
        videoPlayerAudioSource.Pause();
    }
    public void StopVideo()
    {
        videoPlayer.Stop();
        videoPlayerAudioSource.Stop();
    }
    public void DisplayNotes(){
        notesUI.SetActive(true);
        activeNotesUI = true;
    }
    public void HideNotes(){
        notesUI.SetActive(false);
        activeNotesUI = false;
    }
    public void DisplayScreen(){
        screenUI.SetActive(true);
        activeScreenUI = true;
    }
    public void HideScreen(){
        screenUI.SetActive(false);
        activeScreenUI = false;
    }
    public void DisplayBoth()
    {
        this.gameObject.SetActive(true);
        activeNotesParent = true;
        DisplayNotes();
        DisplayScreen();
    }
    public void HideBoth(){
        HideNotes();
        HideScreen();
        this.gameObject.SetActive(false);
        activeNotesParent = false;
    }
    public void DisplayNotesParent()
    {
        this.gameObject.SetActive(true);
        activeNotesParent = true;
    }
    public void AlignOrienation(){
        // transform.LookAt(xrOrigin.transform.position);
        Vector3 targetDir = (this.transform.position - xrOrigin.transform.position ).normalized;
        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
    }
}