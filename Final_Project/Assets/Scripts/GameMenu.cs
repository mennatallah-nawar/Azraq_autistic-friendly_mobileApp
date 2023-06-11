using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public GameObject Closedlock1;

    public GameObject Closedlock2;

    public GameObject Closedlock3;
    void Start()
    {

        if (Login.SocialScriptProgress >= 1)
        {
            Closedlock1.SetActive(false);
        }

        if (Login.SocialScriptProgress >= 2)
        {
            Closedlock2.SetActive(false);
        }

        if (Login.SocialScriptProgress == 3)
        {
            Closedlock3.SetActive(false);
        }

    }
}
