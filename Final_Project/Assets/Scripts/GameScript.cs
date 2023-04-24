using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    [SerializeField] private Transform emptyspace = null;
    //[SerializeField] private Transform fady = null;
    [SerializeField] public Transform last = null;
    [SerializeField] public Transform first = null;
    [SerializeField] public Transform middle = null;
    [SerializeField] public Transform red = null;
    [SerializeField] public Transform blue = null;
    [SerializeField] public Transform yellow = null;
    [SerializeField] private Transform fadyyy = null;
    [SerializeField] private Transform emptyspace1 = null;
    
    private Camera _camera;
    
    public SpriteRenderer _sprite;
    
    void Start()
    {
        
        _camera = Camera.main;
        _sprite = GetComponent<SpriteRenderer>();
        emptyspace.position = new Vector2(-9.8f, 0.21f);
        //fady.position = new Vector2(0.0f, 0.0f);
        last.position = new Vector2(-9.59f, -5.21f);
        print(last.position);
        first.position = new Vector2(-9.94f,5.56f);
        middle.position = new Vector2(-9.8f,0.21f);

        red.position = new Vector2(9.96f,0.04f);
        blue.position = new Vector2(10.01f,-5.04f);
        yellow.position = new Vector2(9.9949f,5.27f);
       
        fadyyy.position = new Vector2(-9.80f, 0.21f);
        print(fadyyy);
    }

    // Update is called once per frame  
    void Update()
    {
        //_sprite.color = Color.red;
        // if(last.position.x < -9.5){
        //         print("hehe");
        //         //_sprite.color = Color.green;
        //     }

       // print(emptyspace.position);
        if(Input.GetMouseButtonDown(0))
        {
           
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if(hit)
            {
                if (Vector2.Distance(emptyspace.position, hit.transform.position) < 100 && hit.transform.position.x < 0)
                {
                    Vector2 lastEmptySpcePosition = emptyspace.position;
                    TilesScript thisTile = hit.transform.GetComponent<TilesScript>();
                    
                    print(emptyspace.position);
                    print("hi");

                    if(hit.transform.position == emptyspace.position){
                        print(hit.transform.position);
                        print(emptyspace.position);
                        print("of");
                        thisTile.targetPosition = emptyspace.position;
                    }


                    emptyspace.position = thisTile.targetPosition;
                    thisTile.targetPosition = lastEmptySpcePosition;


                    print(hit.transform.position);
                    print(emptyspace.position);
                    print(lastEmptySpcePosition);
                    print(thisTile.targetPosition);
                    //print(emptyspace.position);
                    // if(hit.transform.position == emptyspace.position && emptyspace.position == fady.position ){
                    //     print(hit.transform.position);
                    //     print(emptyspace.position);
                    //     print("of");
                    //     emptyspace.position = thisTile.targetPosition;
                    // }

                    // if(hit.transform.position == emptyspace.position && emptyspace.position == fadyyy.position ){
                    //     print(hit.transform.position);
                    //     print(emptyspace.position);
                    //     print("of");
                    //     emptyspace.position = lastEmptySpcePosition;
                    // }
                  
                }

                if (Vector2.Distance(emptyspace1.position, hit.transform.position) < 100 && hit.transform.position.x > 0)
                {
                    Vector2 lastEmptySpcePosition1 = emptyspace1.position;
                    TilesScript thisTile = hit.transform.GetComponent<TilesScript>();
                    
                    if(hit.transform.position == emptyspace1.position){
                        print(hit.transform.position);
                        print(emptyspace.position);
                        print("of");
                        thisTile.targetPosition = emptyspace1.position;
                    }

                    emptyspace1.position = thisTile.targetPosition;
                    thisTile.targetPosition = lastEmptySpcePosition1;
                    print("kharaaaaaaaaa");
                    
                  
                }
            }

        }
    }
}