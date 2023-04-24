
using UnityEngine;


public class TilesScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 targetPosition;
    public Vector3 correctPosition;
    //public SpriteRenderer _sprite;
    void Start()
    {
        targetPosition = transform.position;
        correctPosition = transform.position;
       // _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.05f);
        // if(targetPosition == correctPosition)
        // {
        //     _sprite.color = Color.green;
        // }
        // else
        // {
        //     _sprite.color = Color.white;
        // }
    }
}
