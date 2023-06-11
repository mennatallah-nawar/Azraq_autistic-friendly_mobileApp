using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using TMPro;


public class Login : MonoBehaviour
{

    public static string username1 = null;
    public static string password1 = null;
    [SerializeField] public JSONReader JsonObject;
    public static string token = null;

    public static int SocialScriptProgress = 0;
    public static string error = null;
    public GameObject WrongPasswordText;
    public GameObject NoUserText;

    public GameObject showPass;

    public GameObject hidePass;

    public GameObject FillFields;

    public static bool flag1 = false;
    public static string Image_Ur = null;

    public TMP_InputField UserInputField;
    public TMP_InputField PassInputField;

    void Start()
    {

    }
    public void usernamee(string s)
    {
        username1 = s;
    }

    public void passwordd(string s)
    {
        password1 = s;
    }

    public void OpenHome()
    {
        SceneManager.LoadScene("Home");
    }

    IEnumerator multiple()
    {
        StartCoroutine(LoginRequest(username1, password1));
        yield return new WaitForSeconds(2);
        OpenHome();
    }

    public void OnLoginButtonClick()
    {
        print("Start");
        if (string.IsNullOrEmpty( UserInputField.text ) || string.IsNullOrEmpty( PassInputField.text ))
        {
            FillFields.SetActive(true);
        }
        else
        {
            StartCoroutine(LoginRequest(username1, password1));
        }
        
        //StartCoroutine(multiple());
    }

    public void ShowPassword()
    {
        PassInputField.contentType = TMP_InputField.ContentType.Password;
        showPass.SetActive(false);
        hidePass.SetActive(true);
        PassInputField.ActivateInputField();
    }

    public void HidePassword()
    {
        PassInputField.contentType = TMP_InputField.ContentType.Standard;
        hidePass.SetActive(false);
        showPass.SetActive(true);
        PassInputField.ActivateInputField();
    }
    public void EditPasswordORUserName()
    {
        FillFields.SetActive(false);
        WrongPasswordText.SetActive(false);
        NoUserText.SetActive(false);
    }

    IEnumerator LoginRequest(string username1, string password1)
    {
        string url = "https://azraq-ermoszz3qq-uc.a.run.app/login";

        string loginJson = "{\"username\":\"" + username1 + "\",\"password\":\"" + password1 + "\"}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(loginJson);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            string responseBody = request.downloadHandler.text;
            print(request.responseCode);
            if (request.responseCode == 201)
            {
                JsonObject = JsonUtility.FromJson<JSONReader>(responseBody);
                token = JsonObject.token;
                Debug.Log(loginJson);
                //print(request.GetResponseHeader("Content-Length"));
                //print(loginJson);
                Debug.Log(JsonObject.token);
                flag1 = true;

                //yield return new WaitForSeconds(1);

                Image_Ur = JsonObject.ImageUrl;
                Debug.Log(request.downloadHandler.text);
                Debug.Log(Image_Ur);
                int.TryParse(JsonObject.Progress, out SocialScriptProgress);
                Debug.Log(SocialScriptProgress);
                //yield return new WaitForSeconds(2);
                OpenHome();
            }
            else if (request.responseCode == 403)
            {
                //wrong Password
                WrongPasswordText.SetActive(true);
            }
            else if (request.responseCode == 401)
            {
                //User Doesn't Exist
                NoUserText.SetActive(true);
            }
            request.Dispose();
        }

    }

}
