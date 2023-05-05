using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UploadPhoto : MonoBehaviour
{
    string path;
    
    public RawImage image;
    
    //public static RawImage i = Home.image1;
    
   public void Start(){
        image.texture.wrapMode = TextureWrapMode.Clamp;
        image.texture.filterMode = FilterMode.Point;
        image.texture.anisoLevel = 0;
       
        
   }
    
    public void UploadImageeee() {
     NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
       {

            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                
                
                
                
                
                
                print(path);
                WWW www = new WWW("file:///" + path);
                image.texture = www.texture;
                print("hello");
                // Create a byte array from the texture data
                
                //byte[] bytes = texture.EncodeToPNG();
                
                //Upload the byte array to your server using UnityWebRequest
                //StartCoroutine(Upload(bytes));
            }
        }, "Select an image", "image/*");
        
    
    // Show file picker
    
}
    



    private IEnumerator SendRequest(UnityWebRequest www) 
    {
    yield return www.SendWebRequest();

    if (www.result != UnityWebRequest.Result.Success) {
        Debug.Log("Error uploading image: " + www.error);
    } else {
        Debug.Log("Image uploaded successfully!");
    }
    }


    IEnumerator DownloadImage(string imageUrl) 
    {
       using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl)) {
       yield return www.SendWebRequest();
    
      if (www.result == UnityWebRequest.Result.Success) {
      Texture2D texture = DownloadHandlerTexture.GetContent(www);
      // Display the texture in a UI image or 3D object
      } 
      else {
            Debug.Log("Error downloading file: " + www.error);
        }
      }
    }
   

    

}
