using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class FrontCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture frontCam;
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

        for(int i = 0; i < devices.Length; i++)
        {
            if(devices[i].isFrontFacing)
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
        ScreenCapture.CaptureScreenshot("SelfiePhoto.png");

        //Texture2D photo = new Texture2D(frontCam.width, frontCam.height);
        //photo.SetPixels(frontCam.GetPixels());
        //photo.Apply();
        ////Encode to a PNG
        //byte[] bytes = photo.EncodeToPNG();
        //string path = Application.persistentDataPath + "photo.png";
        //File.WriteAllBytes(path, bytes);

        Debug.Log("SelfiePhoto saved");
        frontCam.Stop();
        BarController.progress++;
        SceneManager.LoadScene("Wheel");
    }
}
