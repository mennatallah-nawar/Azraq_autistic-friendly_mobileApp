using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using TMPro;
public class BackCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture BackCam;
    private Texture defaultBackground;
    public RawImage background;
    public AspectRatioFitter fit;

    public GameObject CaptureButton;

    public byte[] bytes;

    public byte[] Audiobytes;
    private string BackCamPrediction;

    private bool BackCameraRequestError = false;

    private bool startConv = false;

    private bool RecordingFlag = false;
    private AudioClip recordedClip;
    public GameObject BravoPanel;

    public GameObject TryAgainPanel;
    public GameObject HappyPanel;
    public GameObject AngerPanel;

    public GameObject SadPanel;

    public GameObject SurpPanel;

    public GameObject DisPanel;

    public GameObject FearPanel;

    public GameObject StartConvButton;

    public GameObject YouCanSay;

    public GameObject relistenButton;

    public GameObject ErrorPanel;

    public AudioSource FeedbackAudio;

    public AudioSource audioStart;

    public AudioSource audioStop;

    private string IncrementProgressss;


    [SerializeField] public JSONReader JsonObject;


    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                BackCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }

        }
        if (BackCam == null)
        {
            Debug.Log("unable to find back camera");
            return;
        }

        BackCam.Play();
        background.texture = BackCam;
        camAvailable = true;
    }

    void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)BackCam.width / (float)BackCam.height;
        fit.aspectRatio = ratio;

        float scaleY = BackCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -BackCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void TakePhoto()
    {

        StartCoroutine("Screenshot");
        Debug.Log("Photo saved");
        Invoke("Wait", 3);

    }

    private IEnumerator Screenshot()
    {

        yield return new WaitForEndOfFrame();

        Texture2D screenShot = ScreenCapture.CaptureScreenshotAsTexture();

        //Encode to a JPG
        bytes = screenShot.EncodeToJPG();

        //Save on PC
        File.WriteAllBytes(Application.dataPath + "/Photo.jpg", bytes);

        Destroy(screenShot);
    }

    public void Wait()
    {
        if (BackCam != null)
        {
            BackCam.Stop();
            background.enabled = false;
            CaptureButton.SetActive(false);
        }
        StartCoroutine("Upload");
    }

    public void Back()
    {
        if (BackCam != null)
        {
            BackCam.Stop();
        }
        SceneManager.LoadScene("Home");
    }

    public void closepanels()
    {
        TryAgainPanel.SetActive(false);
    }

    public void OpenNotification()
    {
        BackCam.Stop();

        if (BackCameraRequestError == true)
        {
            ErrorPanel.SetActive(true);
        }
        if (BackCamPrediction == "Happy")
        {
            HappyPanel.SetActive(true);
        }

        if (BackCamPrediction == "Sad")
        {
            SadPanel.SetActive(true);
        }

        if (BackCamPrediction == "Angry")
        {
            AngerPanel.SetActive(true);
        }

        if (BackCamPrediction == "Fear")
        {
            FearPanel.SetActive(true);
        }

        if (BackCamPrediction == "Surprise")
        {
            SurpPanel.SetActive(true);
        }

        if (BackCamPrediction == "Disgust")
        {
            DisPanel.SetActive(true);
        }

        if (BackCamPrediction == "Contempt")
        {
            AngerPanel.SetActive(true);
        }

        if (BackCamPrediction == "Neutral")
        {
            HappyPanel.SetActive(true);
        }

        if (PagesNav.OpenConv && !BackCameraRequestError)
        {
            StartConvButton.SetActive(true);
        }
        else
        {
            Invoke("Back", 5);
        }
    }

    private IEnumerator Upload()
    {
        string UploadImage_URL = "https://azraq-ermoszz3qq-uc.a.run.app/predict";
        WWWForm form = new WWWForm();

        form.AddBinaryData("file", bytes);
        //form.AddField("UserID", "1");

        using (UnityWebRequest request = UnityWebRequest.Post(UploadImage_URL, form))
        {
            request.SetRequestHeader("x-access-token", Login.token);
            yield return request.SendWebRequest();
            if (request.responseCode == 200)
            {
                Debug.Log("Request Sent");
                //Debug.Log("Response:" + request.downloadHandler.text);

                JsonObject = JsonUtility.FromJson<JSONReader>(request.downloadHandler.text);
                BackCamPrediction = JsonObject.prediction;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                BackCameraRequestError = true;
                Debug.Log(request.result);
                Debug.Log("Error Code" + request.responseCode);
            }

        }

        OpenNotification();
    }

    public void GetAudio()
    {
        startConv = true;
        StartCoroutine("GetAudioClip");
        YouCanSay.SetActive(true);
        StartConvButton.SetActive(false);
    }

    public void RepeatAudio()
    {
        if (!RecordingFlag)
        {
            FeedbackAudio.Play();
            Invoke("Startaudio", 7);
            Invoke("StartRecord", 8);
            Invoke("Stopaudio", 13);
            Invoke("SaveRecord", 14);
        }
    }

    public void Startaudio()
    {
        audioStart.Play();
    }

    public void StartRecord()
    {
        RecordingFlag = true;
        audioStart.Stop();
        recordedClip = Microphone.Start(null, true, 10, 44100);
    }

    public void Stopaudio()
    {
        audioStop.Play();
    }

    public void SaveRecord()
    {
        //stop record
        Microphone.End(null);
        RecordingFlag = false;
        audioStop.Stop();
        // Save recorded clip as WAV file
        SavWav.Save("recordedAudio", recordedClip);

        string filee = SavWav.filePath;
        SavWav.filePath = SavWav.filePath.Replace("\\", "/");

        StartCoroutine(SendWavRequest(filee));

    }

    IEnumerator GetAudioClip()
    {
        string GetAudio_URL = "https://azraq-ermoszz3qq-uc.a.run.app/audio";

        if (startConv && PagesNav.OpenConv)
        {
            Debug.Log("all flags high");

            using (UnityWebRequest GetAudioReq = UnityWebRequestMultimedia.GetAudioClip(GetAudio_URL, AudioType.WAV))
            {
                GetAudioReq.SetRequestHeader("x-access-token", Login.token);
                yield return GetAudioReq.SendWebRequest();
                Debug.Log(GetAudioReq.responseCode);
                if (GetAudioReq.responseCode == 200)
                {
                    AudioClip myClip = DownloadHandlerAudioClip.GetContent(GetAudioReq);
                    FeedbackAudio.clip = myClip;
                    FeedbackAudio.Play();
                    relistenButton.SetActive(true);

                    Invoke("Startaudio", 7);
                    Invoke("StartRecord", 8);
                    Invoke("Stopaudio", 13);
                    Invoke("SaveRecord", 14);

                }
                else
                {
                    Debug.Log(GetAudioReq.error);
                }
            }

        }
    }

    IEnumerator SendWavRequest(string filePath)
    {

        string url = "https://azraq-ermoszz3qq-uc.a.run.app/speech";
        // Create a new WWWForm object to hold the form data
        WWWForm form = new WWWForm();

        byte[] wavData = File.ReadAllBytes(filePath);

        // Add the WAV file to the form data
        form.AddBinaryData("file", wavData, "recordedAudio.wav", "audio/wav");

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {

            request.SetRequestHeader("x-access-token", Login.token);
            yield return request.SendWebRequest();

            // Check the response status code and response data
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("WAV file sent successfully");
                Debug.Log("Response Code: " + request.responseCode);
                Debug.Log("Response Data: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error sending WAV file: " + request.error);
            }
            if (request.responseCode == 200)
            {
                //match
                BravoPanel.SetActive(true);
                if (!PagesNav.StartConvFromHome)
                {
                    //Increment Social Script progress
                    if (Login.SocialScriptProgress<3)
                    {
                        BarController.progress++;
                        Login.SocialScriptProgress++;
                        StartCoroutine(IncrementProgress(Login.SocialScriptProgress));
                    }
                }
                else
                {
                    PagesNav.StartConvFromHome = false;
                }
                Invoke("Back", 3);
            }
            else if (request.responseCode == 201)
            {
                //mismatch
                TryAgainPanel.SetActive(true);
                Invoke("closepanels", 3);
            }

        }
    }

    IEnumerator IncrementProgress(int SocialProgress)
    {
        // convert int SocialProgress -> string IncrementProgressss
        IncrementProgressss = SocialProgress.ToString();
        string url = "https://azraq-ermoszz3qq-uc.a.run.app/progress";
        WWWForm form = new WWWForm();
        form.AddField("progress",IncrementProgressss);
        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {

            request.SetRequestHeader("x-access-token", Login.token);
            yield return request.SendWebRequest();
            string responseBody = request.downloadHandler.text;
            Debug.Log(responseBody);

            print(request.responseCode);
            // if (request.responseCode == 200)
            // {

            // }
            request.Dispose();
        }
    }
}