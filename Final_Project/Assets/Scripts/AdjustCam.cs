using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustCam : MonoBehaviour
{

    public bool _onStart = true;
    public bool _onUpdate;

    public float _desiredAspect = 16f/9f;

    Camera _myCam;

    int _width;
    int _height;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        
    }

}