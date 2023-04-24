using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarController : MonoBehaviour
{

    public static float progress = 0;
    public Slider Progressbar;

    public void Start()
    {
        Progressbar.value = progress;
    }
    
}
