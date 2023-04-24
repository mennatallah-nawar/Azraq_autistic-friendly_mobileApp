using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;


public class Wheel : MonoBehaviour
{
    private int randomvalue;
    private float timeInterval;
    private bool spinningAllowed;
    private int finalAngle;

    private string feeling;


    //public TMP_Text feelingText;

    // Use this for initialization
    public void Start()
    {
        spinningAllowed = true;
    
    }

    // Update is called once per frame

    //public void Update()
    //{
    //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && spinningAllowed)

    //        StartCoroutine(Spin());
    //}

    public void StartSpin()
    {
        if (spinningAllowed)
            StartCoroutine(Spin());
    }

    public IEnumerator Spin()
    {
        spinningAllowed = false;
        randomvalue = Random.Range(20, 30);
        timeInterval = 0.1f;


        for (int i = 0; i < randomvalue; i++)
        {
            transform.Rotate(0, 0, 30f);
            if (i > Mathf.RoundToInt(randomvalue * 0.5f))
                timeInterval = 0.2f;
            if (i > Mathf.RoundToInt(randomvalue * 0.85f))
                timeInterval = 0.4f;
            yield return new WaitForSeconds(timeInterval);
        }
        if (Mathf.RoundToInt(transform.eulerAngles.z) % 60 != 0)
            transform.Rotate(0, 0, 30f);
        finalAngle = Mathf.RoundToInt(transform.eulerAngles.z);


        //Debug.Log(finalAngle);
        switch (finalAngle)
        {
            case 0:
                feeling = "happiness";
                //feelingText.text = feeling;
                break;
            case 60:
                feeling = "fear";
                break;
            case 120:
                feeling = "surprise";
                break;
            case 180:
                feeling = "anger";
                break;
            case 240:
                feeling = "sadness";
                break;
            case 300:
                feeling = "disgust";
                break;
        }
        WriteString();
        //spinningAllowed = true;
    }

    public void WriteString()
    {
        string path = Application.persistentDataPath + "/feelings.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(feeling);
        writer.Close();
        StreamReader reader = new StreamReader(path);
        //Print the text from the file
        Debug.Log("feeling saved");
        Debug.Log(path);
        reader.Close();
    }

    public void changeScene()
    {
        if (!spinningAllowed)
        {
            Debug.Log("camera opened");
            SceneManager.LoadScene("FrontCamera");
        }
        else
        {
            Debug.Log("spin wheel first");
        }
    }
}