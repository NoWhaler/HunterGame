using UnityEngine;

public class Deer : MonoBehaviour
{
    [Header("Properties")]

    [SerializeField]
    private float _baseSpeed;    

    [SerializeField]
    private float _detectionWolfRange;    

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

    private bool _avoidWolf = false;

    private bool _walk = true;  

    private Rigidbody2D _rigidBody;    

    private void Start(){
        _avoidWolf = false;
        _rigidBody = GetComponent<Rigidbody2D>();
        waitTime = startWaitTime;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    private void Update() {     

        if (!_avoidWolf)
        {
            Walk();         
        }
        else
        {
            AvoidWolf();
        }
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

    private void AvoidWolf(){

    } 

    private void OnTriggerEnter2D(Collider2D other) {       

        if (other.CompareTag("Bullet")){
            Destroy(gameObject);
        } 
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, _detectionWolfRange);
    }
}
