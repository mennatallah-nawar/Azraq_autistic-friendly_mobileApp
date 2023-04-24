using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class BackCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture BackCam;
    private Texture defaultBackground;
    public RawImage background;
    public AspectRatioFitter fit;



    // Start is called before the first frame update
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


    //Update is called once per frame
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
        ScreenCapture.CaptureScreenshot("Photo.png");

        //Texture2D photo = new Texture2D(BackCam.width, BackCam.height);
        //photo.SetPixels(BackCam.GetPixels());
        //photo.Apply();
        ////Encode to a PNG
        //byte[] bytes = photo.EncodeToPNG();
        //string path = Application.persistentDataPath + "photo.png";
        //File.WriteAllBytes(path, bytes);

        Debug.Log("Photo saved");
        BarController.progress++;
        BackCam.Stop();
        SceneManager.LoadScene("Home");
    }
}
