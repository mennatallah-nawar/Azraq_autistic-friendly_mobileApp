using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NextScene : MonoBehaviour
{
    // Start is called before the first frame update
   
    public void OpenGame1_1()
    {
        SceneManager.LoadScene("Game1_1");
    }

    public void Replay(){
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void OpenGame1()
    {
        SceneManager.LoadScene("Game1");
    }

    public void OpenGame3()
    {
        SceneManager.LoadScene("Game3");
    }
}