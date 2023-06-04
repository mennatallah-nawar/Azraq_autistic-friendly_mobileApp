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
    private string BackCamPrediction;

    private bool BackCameraRequestError = false;

    public GameObject HappyPanel;
    public GameObject AngerPanel;

    public GameObject SadPanel;

    public GameObject SurpPanel;

    public GameObject DisPanel;

    public GameObject FearPanel;

    //public GameObject NeutralPanel;

    public GameObject ErrorPanel;

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

        Invoke("Back", 5);
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
}
