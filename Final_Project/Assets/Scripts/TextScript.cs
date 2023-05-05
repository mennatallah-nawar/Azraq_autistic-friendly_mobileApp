using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    public Vector3 correctPosition;
    public SpriteRenderer _sprite;
    public SpriteRenderer _sprite1;
    public SpriteRenderer _sprite2;
    //public GameObject ant;
    public GameScript image;
    public GameObject BravooPanel ;
    public static float c = 0;
    
    void Start()
    {
        correctPosition = transform.position;
        //i = GetComponent<GameScript>().last;
        _sprite = _sprite.GetComponent<SpriteRenderer>();
        //Sprites = GetComponent<Sprite[]>();
        _sprite1 = _sprite1.GetComponent<SpriteRenderer>();
        _sprite2 = _sprite2.GetComponent<SpriteRenderer>();
        
        //Sprites = Resources.LoadAll<Sprite>("TextScript");
    }

    

    // Update is called once per frame
    void Update()
    {
       // print(GetSpriteByName(_sprite));
       // print(Sprites);
        print(_sprite.sprite);
        if(image)
        {
        print(image.last);
        if(image.last.position.x < -9.9 && image.red.position.x > 9.97 && image.red.position.x < 10 && image.red.position.y > 0.04 ){
            //print(_sprite.sprite);
            _sprite.color = Color.green;
            
           // _sprite1.color = Color.white;
           // _sprite2.color = Color.white;
            print("farah");
            print(image.last);
            print(image.last.position);
            print("ossama");
                //_sprite.color = Color.green;
        }

         if((image.last.position.x < -9.9 && image.red.position.y < 0.04) || (image.last.position.x > -9.9 && image.red.position.y > 0.04 ) ){
            //print(_sprite.sprite);
            _sprite.color = Color.white;
           
            print("farah");
            print(image.last);
            print(image.last.position);
            print("ossama");
                //_sprite.color = Color.green;
        }

       

        if(image.first.position.x < -9.4 && image.first.position.x > -9.6 && image.yellow.position.x > 10){
            _sprite1.color = Color.green;
            
            //_sprite.color = Color.white;
            //_sprite2.color = Color.white;
            print("farah");
            print(image.last);
            print(image.last.position);
            print("zein");
            //_sprite.color = Color.green;
        }

        if((image.first.position.x < -9.4 && image.yellow.position.y > 0) || (image.first.position.y > 0 && image.yellow.position.x > 10)){
            _sprite1.color = Color.white;
            //_sprite.color = Color.white;
            //_sprite2.color = Color.white;
            print("farah");
            print(image.last);
            print(image.last.position);
            print("zein");
                //_sprite.color = Color.green;
        }

        if(image.middle.position.x < -9.7 && image.blue.position.y > 0.03 && image.blue.position.y < 5){
            _sprite2.color = Color.green;
            
           // _sprite1.color = Color.white;
           // _sprite.color = Color.white;
            print("farah");
            print(image.last);
            print(image.last.position);
            print("zeft");
                //_sprite.color = Color.green;
        }

        if((image.middle.position.x < -9.7 && image.blue.position.y < 0) || (image.middle.position.x < -9.7 && image.blue.position.y > 5) || (image.middle.position.y < 0 && image.blue.position.y > 0.03 && image.blue.position.y < 5) || (image.middle.position.y > 5 && image.blue.position.y > 0.03 && image.blue.position.y < 5)){
            _sprite2.color = Color.white;
           // _sprite1.color = Color.white;
           // _sprite.color = Color.white;
            print("farah");
            print(image.last);
            print(image.last.position);
            print("atran we nila");
                //_sprite.color = Color.green;
        }
        }
       if(_sprite.color == Color.green & _sprite1.color == Color.green & _sprite2.color == Color.green)
       { c++; }
        check();
   
    }

    public void check(){
        if(c == 1){
            BarController.progress++;
            BravooPanel.SetActive(true);
        }
    }
}
