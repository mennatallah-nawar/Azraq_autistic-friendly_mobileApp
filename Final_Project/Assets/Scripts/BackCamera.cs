using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Newtonsoft.Json;

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

    private bool StartRecordFlag = false;
    private AudioClip recordedClip;
    public GameObject BravoPanel;
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

    public GameObject StartRec;

    public GameObject StopRec;

    public AudioSource FeedbackAudio;
    
   private AudioSource audioStart;
    
   public AudioClip audioClip;

   private AudioSource audioStop;
   public AudioClip audioClip1;
      

    [SerializeField] public JSONReader JsonObject;


    void Start()
    {
        // audioSource = GetComponent<AudioSource>();
        // FeedbackAudio = GetComponent<AudioSource>();
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

            // if (request.result == UnityWebRequest.Result.ProtocolError)
            // {
            //     Debug.Log(request.result);
            //     Debug.Log("Error Code" + request.responseCode);
            // }

            // if (request.result == UnityWebRequest.Result.DataProcessingError)
            // {
            //     Debug.Log(request.result);
            //     Debug.Log("Error Code" + request.responseCode);
            // }

            // if (request.result == UnityWebRequest.Result.ConnectionError)
            // {
            //     Debug.Log(request.result);
            //     Debug.Log("Error Code" + request.responseCode);
            // }
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
        if (!StartRecordFlag)
        {
            FeedbackAudio.Play();
            Invoke("StartRecord",7);
        }
        
    }
    public void StartRecord()
    {

        StartRecordFlag = true;
        StartRecordFlag = true;
        recordedClip = Microphone.Start(null, true, 10, 44100);
        

        //Convert audioclip to bytes

        //send audio
        //StartCoroutine("PostAudioClip");
    }

    public void SaveRecord()
    {
        Microphone.End(null);
        
        // Save recorded clip as WAV file
        SavWav.Save("recordedAudio", recordedClip);
       

       
        string filee = SavWav.filePath;
        SavWav.filePath = SavWav.filePath.Replace("\\", "/");
        
        StartCoroutine(SendWavRequest(filee));


    }
    public void Stopaudio(){
        audioStop = GetComponent<AudioSource>();

        // Set the audio clip to play
        audioStop.clip = audioClip1;

        audioStop.Play();
    }
    public void Startaudio(){
        audioStart = GetComponent<AudioSource>();

        // Set the audio clip to play
        audioStart.clip = audioClip;

        audioStart.Play();
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

                    Invoke("Startaudio",5);
                    Invoke("StartRecord",6);
                    Invoke("Stopaudio",10);
                    Invoke("SaveRecord",11);


                    
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
   
    
    // Create a new UnityWebRequest object to send the request
    
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
        BravoPanel.SetActive(true);
        
       }
       
    else
    {
        Debug.LogError("Error sending WAV file: " + request.error);
        //User.text = request.downloadHandler.text;
        //User.text = request.downloadHandler.text;
        //User.text = "error";
        //User.text = form.GetBinaryData.ToString();
    }
    
    };
   
    
}
    IEnumerator PostAudioClip()
    {
        string PostAudio_URL = "https://azraq-ermoszz3qq-uc.a.run.app/speech";
        WWWForm formaudio = new WWWForm();

        formaudio.AddBinaryData("file", Audiobytes);

        using (UnityWebRequest SendAudiorequest = UnityWebRequest.Post(PostAudio_URL, formaudio))
        {
            Debug.Log(SendAudiorequest.responseCode);
            SendAudiorequest.SetRequestHeader("x-access-token", Login.token);
            yield return SendAudiorequest.SendWebRequest();
            if (SendAudiorequest.responseCode == 200)
            {
                Debug.Log("Request Sent");
                Debug.Log("Response:" + SendAudiorequest.downloadHandler.text);
            }

            if (SendAudiorequest.result != UnityWebRequest.Result.Success)
            {
                BackCameraRequestError = true;
                Debug.Log(SendAudiorequest.result);
                Debug.Log("Error Code" + SendAudiorequest.responseCode);
            }
        }

    }
}
