using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Home : MonoBehaviour
{
    
    //public static string user = null;
    
    public TextMeshProUGUI myText;
    // Start is called before the first frame update
    void Start()
    {
        myText.text = Login.username1;
    }

    // Update is called once per frame
    void Update()
    {
       // print(user);
    }
}
