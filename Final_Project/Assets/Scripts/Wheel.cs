using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public class Wheel : MonoBehaviour
{
    private int randomvalue;
    private float timeInterval;
    private bool spinningAllowed;
    private int finalAngle;

    public static string FeelingFromWheel;

    public GameObject BravooPanel ;
    public GameObject TryAgainPanel ;
    public GameObject SpinFirst;

    public GameObject ErrorPanel;


    //private string feeling;


    //public TMP_Text feelingText;

    public void Start()
    {
        if (FrontCamera.WaitResult == true || FrontCamera.RequestError == true)
        {
            //prevent spinning wheel till show result
            spinningAllowed = false;
            CheckOutAct();
        }
        else
        {
            spinningAllowed = true;
        }
    }


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
                FeelingFromWheel = "Happy";
                //feelingText.text = feeling;
                break;
            case 60:
                FeelingFromWheel = "Fear";
                break;
            case 120:
                FeelingFromWheel = "Surprise";
                break;
            case 180:
                FeelingFromWheel = "Angry";
                break;
            case 240:
                FeelingFromWheel = "Sad";
                break;
            case 300:
                FeelingFromWheel = "Disgust";
                break;
        }
        Debug.Log("Feeling From Wheel:" + FeelingFromWheel);
        //WriteString();
        //spinningAllowed = true;
    }

    public void WriteString()
    {
        string path = Application.persistentDataPath + "/feelings.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(FeelingFromWheel);
        writer.Close();
        StreamReader reader = new StreamReader(path);
        //Print the text from the file
        Debug.Log("feeling saved");
        Debug.Log(path);
        reader.Close();
    }

    public void changeScene()
    {
        if (!spinningAllowed && !FrontCamera.WaitResult)
        {
            Debug.Log("camera opened");
            SceneManager.LoadScene("FrontCamera");
        }
        else
        {
            SpinFirst.SetActive(true);
            Invoke("CloseNotifications", 5);
            //Debug.Log("Spin wheel first !!!");
        }
    }

    void CloseNotifications()
    {
        TryAgainPanel.SetActive(false);
        BravooPanel.SetActive(false);
        SpinFirst.SetActive(false);
        ErrorPanel.SetActive(false);
    }

    public void CheckOutAct()
    {
        Debug.Log("Prediction:" + FrontCamera.prediction);
        
        
        if (FrontCamera.prediction == FeelingFromWheel)
        {
            BarController.progress++;
            BravooPanel.SetActive(true);
        }
        else if (FrontCamera.RequestError == true)
        {
            ErrorPanel.SetActive(true);
        }
        else
        {
            TryAgainPanel.SetActive(true);
        }

        Invoke("CloseNotifications", 5);
        FrontCamera.WaitResult = false;
        FrontCamera.RequestError = false;
        FrontCamera.prediction = null;
        Start();
    }
}