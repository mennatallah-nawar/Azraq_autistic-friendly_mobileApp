using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class match : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    Vector2 _offset, _orginialPosition, _slotPosition, _slot1Position, _slot2Position, _slot3Position;
    bool _dragging, _placed;

    
    void Start(){
        _slotPosition = new Vector2(-9.94f, 5.56f);
        _slot1Position = new Vector2(-9.8f, 0.21f);
        _slot2Position = new Vector2(-9.59f, -5.21f);
        print("nela");
        
    }
   
    void Awake(){
        _orginialPosition = transform.position;
       
   
    }
    void Update()
    {
        //Init(_slot);
        if(_placed) return;
        if(_dragging){
            var mousePos = GetMousePos();
            transform.position = mousePos;
            // if(transform.parent != transform.position){
            //     transform.position = mousePos - _offset;
            // }
        
        }
        return;
       
    }
    void OnMouseUp()
    {
        //Init(_slot);
        print(_orginialPosition);
        print(transform.position);
       //if(Vector2.Distance(transform.position, _slot.transform.position) < 3){
        if(_orginialPosition.x < -9.9 && _orginialPosition.y > 5.5 ){
            transform.position = _slot1Position;
            //_slot.Placed();
            print(_slotPosition);
           
            _placed = true;
       }
       else if(_orginialPosition.x < -2.4 && _orginialPosition.y <-1.7 && transform.position.x < 8.3 && transform.position.x > 8.2 && transform.position.y > 2 ){
            transform.position = _slot3Position;
            _placed = true;
       }
       else if(_orginialPosition.x < 3 && _orginialPosition.y <-1.6 && transform.position.x < 2.93 && transform.position.x > 2.88 && transform.position.y > 2.1 && transform.position.y < 2.35 ){
            transform.position = _slot2Position;
            _placed = true;
       }
       else if(_orginialPosition.x > 8 && _orginialPosition.y <-1.6 && transform.position.x < -2.3 && transform.position.x > -2.7 && transform.position.y > 2 && transform.position.y < 2.4 ){
            transform.position = _slot1Position;
           _placed = true;
         }
       else{
            transform.position = _orginialPosition;
            //transform.position = GetMousePos();
            _dragging = false;
       }
       
    }
    void OnMouseDown(){
        _dragging = true;
        _offset = GetMousePos() - (Vector2)transform.position;
    }

    Vector2 GetMousePos(){
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
