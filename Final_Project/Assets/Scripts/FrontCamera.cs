using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class FrontCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture frontCam;
    private Texture defaultBackground;
    public RawImage background;
    public AspectRatioFitter fit;

    public GameObject CaptureButton;

    public static bool WaitResult = false;

    public static bool RequestError = false;

    public static string prediction = null;

    public byte[] bytes;

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
            if (devices[i].isFrontFacing)
            {
                frontCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }

        }
        if (frontCam == null)
        {
            Debug.Log("unable to find front camera");
            return;
        }

        frontCam.Play();
        background.texture = frontCam;
        camAvailable = true;
    }


    //Update is called once per frame
    void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)frontCam.width / (float)frontCam.height;
        fit.aspectRatio = ratio;

        float scaleY = frontCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -frontCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void TakePhoto()
    {

        StartCoroutine("Screenshot");
        Debug.Log("SelfiePhoto saved");
        if (frontCam != null)
        {
            frontCam.Stop();
            background.enabled = false;
            CaptureButton.SetActive(false);
        }
        Invoke("Wait", 2);

    }

    private IEnumerator Screenshot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenShot = ScreenCapture.CaptureScreenshotAsTexture();

        //Encode to a JPG
        bytes = screenShot.EncodeToJPG();
        //Save on PC
        File.WriteAllBytes(Application.dataPath + "/SelfiePhoto.jpg", bytes);

        Destroy(screenShot);
    }

    public void Wait()
    {
        StartCoroutine("Upload");
    }

    public void Back()
    {
        if (frontCam != null)
        {
            frontCam.Stop();
        }
        SceneManager.LoadScene("Wheel");
    }

    private IEnumerator Upload()
    {
        string UploadImage_URL = "https://blu-lfoazpk3ca-uc.a.run.app/predict";
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
                WaitResult = true;
                //Debug.Log("Response:" + request.downloadHandler.text);

                JsonObject = JsonUtility.FromJson<JSONReader>(request.downloadHandler.text);
                prediction = JsonObject.prediction;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                RequestError = true;
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
        Back();
    }
}