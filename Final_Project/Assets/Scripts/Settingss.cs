using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settingss : MonoBehaviour
{
    // Start is called before the first frame update
    UploadPhoto u;
    
    void Start()
    {
        if(UploadPhoto.flag == true){
            u = GameObject.FindGameObjectWithTag("need").GetComponent<UploadPhoto>();
            print(UploadPhoto.Image_Url);
            StartCoroutine(u.DownloadImage(UploadPhoto.Image_Url));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
