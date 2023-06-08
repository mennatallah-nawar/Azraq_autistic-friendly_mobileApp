using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using TMPro;

public class Signup : MonoBehaviour
{
    //public InputField betInput;
    public static string username = null;
    public static string firstname = null;
    public static string lastname = null;
    public static string age = null;
    public static string password = null;
    [SerializeField] public JSONReader JsonObject;
    public static string token = null;
    public GameObject TryAgainPanel;
    public GameObject UserNamePanel;
    public GameObject PasswordPanel;
    public GameObject DataPanel;

    public GameObject showPass;

    public GameObject hidePass;
    public TMP_InputField PassInputField;

    void Start()
    {
        hidePass.SetActive(true);
    }
    public void usernamee(string s)
    {
        username = s;
    }

    public void firstnamee(string s)
    {
        firstname = s;
    }

    public void lastnamee(string s)
    {
        lastname = s;
    }

    public void Enterage(string s)
    {
        age = s;
        //Debug.Log(age);
    }

    public void passwordd(string s)
    {
        password = s;
    }


    public void OpenLoginpage()
    {
        SceneManager.LoadScene("Login");
    }
    public void OnLoginButtonClick()
    {
        
        if (username == null | firstname == null | lastname == null | password == null)
        {
            
            DataPanel.SetActive(true);
        }
        else if (username.Length < 3)
        {
            
            UserNamePanel.SetActive(true);
        }
        else if (password.Length < 6)
        {
            
            PasswordPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(SignupRequest(username, firstname, lastname, age, password));
            //Debug.Log(age);
            OpenLoginpage();
        }
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
    IEnumerator SignupRequest(string username, string firstname, string lastname, string age, string password)
    {
        string url = "https://azraq-ermoszz3qq-uc.a.run.app/signup";
        print(age);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        string loginJson = "{\"username\":\"" + username + "\",\"firstname\":\"" + firstname + "\" ,\"lastname\":\"" + lastname + "\" ,\"age\":\"" + age + "\" ,\"password\":\"" + password + "\"}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(loginJson);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        print(loginJson);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseBody = request.downloadHandler.text;

            Debug.Log(loginJson);


        }
        else
        {
            //Debug.LogError(request.error);
            show();
        }
        request.Dispose();
    }

    void show()
    {
        TryAgainPanel.SetActive(true);
    }

}
