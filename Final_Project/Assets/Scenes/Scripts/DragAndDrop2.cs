using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop2 : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    Vector2 _offset, _orginialPosition, _slotPosition, _slot1Position, _slot2Position, _slot3Position;
    bool _dragging, _placed;
    private Slot _slot;
    
    public static DragAndDrop2 itemBeingDragged;

    public void Init(Slot slot){
        print(slot);
       // _renderer.sprite = slot.Renderer.sprite;
        _slot = slot;
        
    }
    void Start(){
        _slotPosition = new Vector2(-7.88f, 2.32f);
        _slot1Position = new Vector2(-2.51f, 2.3f);
        _slot2Position = new Vector2(2.97f, 2.25f);
        _slot3Position = new Vector2(8.27f, 2.27f);
        print("nela");
        //transform.position = GetMousePos();
        //transform.position = GetMousePos();
    }
    
   
    void Awake(){
        _orginialPosition = transform.position;
        //_parentPosition = (Vector2)transform.position;
   
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
        
        if(_orginialPosition.x < -7.7 && _orginialPosition.y < -1.7 && transform.position.x < -7.8 && transform.position.x > -8 && transform.position.y > 2.3 ){
            transform.position = _slotPosition;
            //_slot.Placed();
            print(_slotPosition);
            print("bitch");
            _placed = true;
       }
       else if(_orginialPosition.x > -2.6 && _orginialPosition.x < 0 && _orginialPosition.y < -1.7 && transform.position.x < 8.3 && transform.position.x > 8.2 && transform.position.y > 2 ){
            transform.position = _slot3Position;
            _placed = true;
       }
       else if(_orginialPosition.x < 3 && _orginialPosition.y <-1.6 && transform.position.x < 3 && transform.position.x > 2.88 && transform.position.y > 2.1 && transform.position.y < 2.35 ){
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
