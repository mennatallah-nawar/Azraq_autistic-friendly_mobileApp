using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class Signup : MonoBehaviour
{
    //public InputField betInput;
    public static string username = null;
    public static string firstname = null;
    public static string lastname = null;
    public static int age = 0;
    public static string password = null;
    [SerializeField] public JSONReader JsonObject;
    public static string token = null;
    public GameObject TryAgainPanel;
    public GameObject UserNamePanel;
    public GameObject PasswordPanel;
    public GameObject DataPanel;
    
    void Start () {
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

    public void agee(int s)
    {
        age = s;
       
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
        print("yala");
        if(username == null | firstname == null | lastname == null | password == null)
        {
            print("bs yala");
            DataPanel.SetActive(true);
        }
        else if(username.Length < 3){
            print("yaa");
            UserNamePanel.SetActive(true);
        }
        else if(password.Length < 6){
            print("nela");
            PasswordPanel.SetActive(true);
        }
        else{
              StartCoroutine(SignupRequest(username, firstname, lastname, age, password));
              OpenLoginpage();
        }
    }
   


 

    
    

    IEnumerator SignupRequest(string username, string firstname, string lastname, int age, string password)
    {
        string url = "https://blu-lfoazpk3ca-uc.a.run.app/signup";
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
