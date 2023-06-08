using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PagesNav : MonoBehaviour
{
    public static bool OpenConv = false;
    public void OpenGameMapScene()
    {
        SceneManager.LoadScene("GamesMenu");
    }

    public void OpenGlassesScene()
    {
        OpenConv = false;
        Debug.Log("camera opened");
        SceneManager.LoadScene("BackCamera");
    }

    public void OpenConvScene()
    {
        OpenConv = true;
        Debug.Log("camera opened");
        SceneManager.LoadScene("BackCamera");
    }

    public void OpenSettingsScene()
    {
        SceneManager.LoadScene("Settings");
    }

    public void OpenGame1_1()
    {
        SceneManager.LoadScene("Game1_1");
    }

    public void Replay()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void OpenGame1()
    {
        SceneManager.LoadScene("Game1");
    }

    public void OpenGame2()
    {
        SceneManager.LoadScene("Wheel");
    }

    public void OpenGame3()
    {
        SceneManager.LoadScene("Game3");
    }

    public void OpenGame4()
    {
        SceneManager.LoadScene("Game4");
    }

    public void OpenHomePage()
    {
        SceneManager.LoadScene("Home");
    }

    public void OpenConnectedDevices()
    {
        SceneManager.LoadScene("ConnectedDevices");
    }

    public void OpenLogin()
    {
        SceneManager.LoadScene("Login");
    }

    public void OpenSignup()
    {
        SceneManager.LoadScene("Signup");
    }

    public void OpenStart()
    {
        SceneManager.LoadScene("Start");
    }
}
