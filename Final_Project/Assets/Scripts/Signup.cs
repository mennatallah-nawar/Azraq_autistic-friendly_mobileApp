using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject UserExist;
    public GameObject UserNameShort;
    public GameObject PasswordShort;
    public GameObject FillInputs;

    public GameObject UserNameCond;

    public GameObject showPass;

    public GameObject hidePass;

    public TMP_InputField UserInputField;
    public TMP_InputField PassInputField;
    public TMP_InputField FNInputField;

    public TMP_InputField LNInputField;
    public TMP_InputField AgeInputField;

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

    public void EditFields()
    {
        FillInputs.SetActive(false);
    }

    public void EditUserName()
    {
        UserNameShort.SetActive(false);
        UserNameCond.SetActive(false);
    }

    public void EditPassword()
    {
        PasswordShort.SetActive(false);
    }

    public void EditPasswordORUserName()
    {
        UserExist.SetActive(false);
    }

    public void OnLoginButtonClick()
    {

        if (string.IsNullOrEmpty(UserInputField.text) || string.IsNullOrEmpty(FNInputField.text) || string.IsNullOrEmpty(LNInputField.text) ||
        string.IsNullOrEmpty(AgeInputField.text) || string.IsNullOrEmpty(PassInputField.text))
        {
            //print("There are Empty field");
            FillInputs.SetActive(true);
        }
        else
        {
            if(!string.IsNullOrEmpty(UserInputField.text) && username.Length > 3 && !string.IsNullOrEmpty(PassInputField.text) && password.Length > 6)
            {
                StartCoroutine(SignupRequest(username, firstname, lastname, age, password));
            }
        }
        if (!string.IsNullOrEmpty(UserInputField.text) && username.Length < 3)
        {
            UserNameShort.SetActive(true);
        }
        if (!string.IsNullOrEmpty(PassInputField.text) && password.Length < 6)
        {
            PasswordShort.SetActive(true);
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
        print("hello request");
        string url = "https://azraq-ermoszz3qq-uc.a.run.app/signup";
        string loginJson = "{\"username\":\"" + username + "\",\"firstname\":\"" + firstname + "\" ,\"lastname\":\"" + lastname + "\" ,\"age\":\"" + age + "\" ,\"password\":\"" + password + "\"}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(loginJson);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            print("start request");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            print("request handeld");
            yield return request.SendWebRequest();

            print(request.responseCode);
            
            if (request.responseCode == 201)
            {
                //string responseBody = request.downloadHandler.text;
                SceneManager.LoadScene("Login");
                Debug.Log(loginJson);
            }

            else if (request.responseCode == 400)
            {
                UserExist.SetActive(true);
            }

            else if (request.responseCode == 406)
            {
                UserNameCond.SetActive(true);
            }
            request.Dispose();
        }
    }

}