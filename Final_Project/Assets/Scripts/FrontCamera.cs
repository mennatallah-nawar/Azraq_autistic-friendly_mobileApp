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

    public static bool WaitResult = false;

    public static bool RequestError = false;

    public static string prediction = null;

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

        
        string name = "SelfiePhoto" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".jpg";
        
        //////////////Save on PC/////////
        
        //First Method//
        // Texture2D screenShot = ScreenCapture.CaptureScreenshotAsTexture();
  
        // Texture2D newScreenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        // newScreenShot.SetPixels(screenShot.GetPixels());
        // newScreenShot.Apply();


        //Second Method//
        // Texture2D photo = new Texture2D(Screen.width,Screen.height, TextureFormat.RGB24, false);
        // photo.ReadPixels(new Rect(0, 0 , Screen.width / 2, Screen.height),0,0);
        // photo.Apply();

        // Texture2D photo = new Texture2D(frontCam.width, frontCam.height);
        // photo.SetPixels(frontCam.GetPixels());
        // photo.Apply();

        //Encode to a JPG
        // byte[] bytes = newScreenShot.EncodeToJPG();
        // File.WriteAllBytes(Application.dataPath + "/SelfiePhoto.jpg", bytes);


        // ////////////Save on android//////
        // NativeGallery.SaveImageToGallery(newScreenShot,"Azraq App SelfiePhotos",name);

        // Debug.Log("SelfiePhoto saved");
        //Invoke("Wait", 2);
        WaitResult = true;
        Back();
    }

    public void Wait()
    {
        StartCoroutine(Upload());
    }

    public void Back()
    {
        if (frontCam != null)
        {
            frontCam.Stop();
        }
        SceneManager.LoadScene("Wheel");
    }

    IEnumerator Upload()
    {
        string UploadImage_URL = "https://fersystem-lfoazpk3ca-lm.a.run.app/predict";
        WWWForm form = new WWWForm();

        form.AddBinaryData("file", File.ReadAllBytes(Application.dataPath + "/SelfiePhoto.jpg"));
        //form.AddField("UserID", "1");

        using (UnityWebRequest request = UnityWebRequest.Post(UploadImage_URL, form))
        {
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