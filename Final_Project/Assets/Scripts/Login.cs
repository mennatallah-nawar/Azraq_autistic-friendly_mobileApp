using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    //public InputField betInput;
    public static string username1 = null;
    public static string password1 = null;
    [SerializeField] public JSONReader JsonObject;
    public static string token = null;
    public static string error = null;
    public GameObject WrongPasswordPanel;
    public GameObject NoUserPanel;
    //PagesNav pagenav = new PagesNav();
    //GameObject gameObject = new GameObject(); 
    //PagesNav myScript = gameObject.AddComponent<PagesNav>();
    //public GameObject GameObject;
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

        print("yala");
        StartCoroutine(LoginRequest(username1, password1));
        print("zeft");
        //StartCoroutine(multiple());

    }



    IEnumerator LoginRequest(string username1, string password1)
    {
        string url = "https://azraq-ermoszz3qq-uc.a.run.app/login";


        UnityWebRequest request = new UnityWebRequest(url, "POST");
        string loginJson = "{\"username\":\"" + username1 + "\",\"password\":\"" + password1 + "\"}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(loginJson);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseBody = request.downloadHandler.text;
            print(responseBody.Length);
            if (responseBody.Length == 83)
            {
                WrongPasswordPanel.SetActive(true);
            }
            else if (responseBody.Length == 88)
            {
                NoUserPanel.SetActive(true);
            }
            else
            {
                JsonObject = JsonUtility.FromJson<JSONReader>(responseBody);
                token = JsonObject.token;
                Debug.Log(loginJson);
                print(request.GetResponseHeader("Content-Length"));
                //print(loginJson);
                Debug.Log(JsonObject.token);
                yield return new WaitForSeconds(2);
                OpenHome();
            }
        }
        else
        {
            // string responseBody = request.downloadHandler.text;
            // JsonObject = JsonUtility.FromJson<JSONReader>(responseBody);
            // error = JsonObject.WWWAuthenticate;
            // print(responseBody);

            //print(loginJson);
            //Debug.LogError(request.error);
        }
        request.Dispose();
    }



}
