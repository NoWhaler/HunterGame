using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Wolf : MonoBehaviour
{

    [SerializeField]
    private Transform _target;    

    [Header("Properties")]

    [SerializeField]
    private float _baseSpeed;

    [SerializeField]
    private float _chaseSpeed;

    [SerializeField]
    private float _chaseRange;    

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

    [SerializeField]
    private float _dieWithoutCatch;    

    private bool _chase = false;

    private bool _walk = true;

    private Rigidbody2D _rigidBody;      

    private void Start(){

        _rigidBody = GetComponent<Rigidbody2D>();
        waitTime = startWaitTime;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }


    private void Update() {        

        if (Time.time > _dieWithoutCatch){
            DieWithoutCatch();
        }
        if (_target==null){
            _walk = true;
        }
        else{

            _walk = false;
        }
        
        if (_walk){
            Walk();            
        }
        else
        {
            Chase();
        } 
    }

    private void DieWithoutCatch(){
        Destroy(gameObject);
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
    
    private void Chase(){       
        
        if (!_walk && _target){
            var moveVector = Vector2.MoveTowards(transform.position, _target.transform.position, _chaseSpeed * Time.deltaTime);
            transform.position = new Vector2(moveVector.x, moveVector.y);
        }        
    }    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Rabbit") || other.CompareTag("Hunter") || other.CompareTag("Deer")){
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Bullet")){
            Destroy(gameObject);
        } 
    }

}
