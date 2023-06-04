using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class UploadPhoto : MonoBehaviour
{
    string path;

    public RawImage image;
    string UploadImage_URL;
    [SerializeField] public JSONReader JsonObject;
    public static string Image_Url = null;
    public static bool flag = false;



    void Start()
    {
        if (flag == true)
        {
            StartCoroutine(DownloadImage(Image_Url));
        }

    }

    public void UploadImageeee()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
          {

              if (path != null)
              {

                  print(path);
                  StartCoroutine(Upload(path));

                  //path.Dispose();

                  print("hello");

              }
          }, "Select an image", "image/*");


        // Show file picker

    }
    IEnumerator Upload1(string path)
    {
        UploadImage_URL = "https://azraq-ermoszz3qq-uc.a.run.app/postImage";
        //UnityWebRequest request = new UnityWebRequest(UploadImage_URL, "POST");

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", File.ReadAllBytes(path));
        byte[] formData = form.data;

        print(form);
        //print(formData);


        UnityWebRequest request = UnityWebRequest.Post(UploadImage_URL, form);
        //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        //request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("x-access-token", Login.token);
        request.SetRequestHeader("Content-Type", "application/json");
        print(Login.token);

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request Sent");
            JsonObject = JsonUtility.FromJson<JSONReader>(request.downloadHandler.text);
            Image_Url = JsonObject.Image_Url;
            Debug.Log(request.downloadHandler.text);
            Debug.Log(Image_Url);
            StartCoroutine(DownloadImage(Image_Url));

        }
        else
        {
            Debug.LogError(request.error);
        }

    }

    IEnumerator Upload2(string path)
    {
        UploadImage_URL = "https://blu-lfoazpk3ca-uc.a.run.app/postImage";
        // Convert the texture to a byte array
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", File.ReadAllBytes(path));
        byte[] formData = form.data;
        //byte[] imageData = image.EncodeToPNG();

        // Create a new UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Post(UploadImage_URL, "multipart/form-data");

        // Set the x-access-token header with the access token value
        request.SetRequestHeader("x-access-token", Login.token);

        // Set the request body data
        string boundary = "------------------------" + System.DateTime.Now.Ticks.ToString("x");
        byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
        request.uploadHandler = new UploadHandlerRaw(formData);
        request.uploadHandler.contentType = "application/octet-stream";
        //request.uploadHandler.chunkedTransfer = false;

        // Set the content type and boundary for the multipart/form-data request
        string contentType = "multipart/form-data; boundary=" + boundary;
        request.SetRequestHeader("Content-Type", contentType);

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Get the response data
        string responseText = request.downloadHandler.text;

        // Do something with the response data
        Debug.Log(responseText);
    }


    IEnumerator Upload(string path)
    {
        UploadImage_URL = "https://azraq-ermoszz3qq-uc.a.run.app/postImage";
        WWWForm form = new WWWForm();



        form.AddBinaryData("file", File.ReadAllBytes(path));
        //form.AddField("UserID", "1");

        using (UnityWebRequest request = UnityWebRequest.Post(UploadImage_URL, form))
        {
            //request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("x-access-token", Login.token);
            //request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.responseCode == 200)
            {
                Debug.Log("Request Sent");
                JsonObject = JsonUtility.FromJson<JSONReader>(request.downloadHandler.text);
                Image_Url = JsonObject.Image_Url;
                Debug.Log(request.downloadHandler.text);
                Debug.Log(Image_Url);
                flag = true;
                StartCoroutine(DownloadImage(Image_Url));

            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.result);
                Debug.Log("Error Code" + request.responseCode);
            }



        }

    }


    public IEnumerator DownloadImage(string imageUrl)
    {


        string i = "https://azraq-ermoszz3qq-uc.a.run.app";
        using (UnityWebRequest request = UnityWebRequest.Get(i + imageUrl))
        {
            request.SetRequestHeader("x-access-token", Login.token);
            yield return request.SendWebRequest();

            if (request.responseCode == 200)
            {
                Debug.Log("Request received");

                if (request.result == UnityWebRequest.Result.Success)
                {


                    Texture2D texture = new Texture2D(1, 1);
                    texture.LoadImage(request.downloadHandler.data);

                    print(request.downloadHandler.data.Length);
                    print(request.downloadHandler.data);
                    print(texture);
                    image.texture = texture;

                }

            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.result);
                Debug.Log("Error Code" + request.responseCode);
            }

        }
    }





}

