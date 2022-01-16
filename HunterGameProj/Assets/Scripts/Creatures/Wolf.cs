using UnityEngine;
using System.Linq;

public class Wolf : MonoBehaviour
{

    [SerializeField]
    private GameObject _target;    

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
        waitTime = startWaitTime;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        _rigidBody = GetComponent<Rigidbody2D>();
    }


    private void Update() {        

        if (Time.time > _dieWithoutCatch){
            DieWithoutCatch();
        }
        FindTarget();
        if (_target==null){
            _walk = true;
        }
        else{

            _walk = false;
        }
        
        if (_walk){
            Walk();            
        }
        else{
            Chase();
        } 
    }

    private void DieWithoutCatch(){
        Destroy(gameObject);
    }

    private void Walk(){
        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, _baseSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpot.position) < 0.1f){
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

    private void FindTarget(){

        var catchTarget = Physics2D.OverlapCircleAll(transform.position, _chaseRange).ToList<Collider2D>();

        catchTarget = catchTarget.Where(t => t.tag !="Wolf").ToList();
        catchTarget = catchTarget.Where(t => t.tag !="Cliff").ToList();
        
        if (catchTarget.Count > 0){
            float minDist = 100f;
            GameObject target = null;
            foreach (var potentialTarget in catchTarget){           
                if (potentialTarget.gameObject.tag == "Hunter"
                   || potentialTarget.gameObject.tag == "Rabbit"
                   || potentialTarget.gameObject.tag == "Deer"){
                    var distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y),
                                   new Vector2(potentialTarget.gameObject.transform.position.x,
                                   potentialTarget.gameObject.transform.position.y));
                    if (distance <= minDist &&
                        distance >0.5f){
                        minDist = distance;                    
                        target = potentialTarget.gameObject;
                    }
                }
            }
            _target = target;
        }
        else{          
            _target = null;
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

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, _chaseRange);
    }

}
