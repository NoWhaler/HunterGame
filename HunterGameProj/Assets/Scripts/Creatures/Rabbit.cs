using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    [Header("Properties")]

    [SerializeField]
    private float _baseSpeed;

    [SerializeField]
    private float _speedUpValue;

    [SerializeField]
    private float _speedUpRange;    

    private Vector2 _speedUpDirection;

    private float waitTime;

    private float startWaitTime = 0f;

    [Header("Walk values")]

    [SerializeField]
    private float minX;
    
    [SerializeField]
    private float maxX;
    
    [SerializeField]
    private float minY;
    
    [SerializeField]
    private float maxY;

    [SerializeField]
    private Transform moveSpot;      

    private bool _speedUp = false;

    private bool _walk = true;  

    private Rigidbody2D _rigidBody;    

    private void Start(){
        _speedUp = false;
        _rigidBody = GetComponent<Rigidbody2D>();
        waitTime = startWaitTime;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    private void Update() {     

        if (!_speedUp)
        {
            Walk();         
        }
        else
        {
            SpeedUp();
        }
    }    

    private void Walk(){
        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, _baseSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpot.position) < 0.2f){
            if (waitTime <= 0){
                moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                waitTime = startWaitTime;
            }else{
                waitTime -= Time.deltaTime;
            }
        }
    }

    private void SpeedUp(){
        var moveVector = Vector2.MoveTowards(transform.position, _speedUpDirection * 3, _speedUpValue * Time.deltaTime);
        transform.position = new Vector2(moveVector.x, moveVector.y);
    }    
}
