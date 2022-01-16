using UnityEngine;
using System.Linq;

public class Deer : MonoBehaviour
{
    [Header("Properties")]

    [SerializeField]
    private float _baseSpeed;    

    [SerializeField]
    private float _detectionThreatRange; 

    private Vector2 _threatWalkDirection;   

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

    private float _threatTime;

    private float _calmDownTime = 1f;

    private bool _avoidThreat = false;

    private bool _grouping = false;

    private bool _walk = true;  

    private Rigidbody2D _rigidBody;    

    private void Start(){
        _avoidThreat = false;
        _rigidBody = GetComponent<Rigidbody2D>();
        waitTime = startWaitTime;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    private void Update() {     

        DetectThreat();

        if (_threatTime <= 0){
            _avoidThreat = false;

        }
        if (_walk){
            Walk();         
        }
        else{
            AvoidThreat();
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

    private void AvoidThreat(){
        var moveVector = Vector2.MoveTowards(transform.position, _threatWalkDirection * 3, _baseSpeed * Time.deltaTime);
        transform.position = new Vector2(moveVector.x, moveVector.y);
    } 
    
    private void DetectThreat(){
        var threat = Physics2D.OverlapCircleAll(transform.position, _detectionThreatRange).ToList<Collider2D>();
        
        threat = threat.Where(t => t.tag !="Deer").ToList();
        threat = threat.Where(t => t.tag !="Rabbit").ToList();

        if (threat.Count > 0){
            Vector2 runDirection = new Vector2();
            foreach (var scareCreature in threat){
                Vector2 direction = new Vector2();
                if (scareCreature.gameObject.tag == "Cliff" && 
                    scareCreature.gameObject.tag == "Wolf" &&
                    scareCreature.gameObject.tag == "Hunter"){
                     direction = scareCreature.transform.position - transform.position;
                }
                else{
                    direction = transform.position - scareCreature.transform.position;
                }                            
                runDirection += direction;
            }
            _threatWalkDirection = runDirection;
            _avoidThreat = true;
            _threatTime = _calmDownTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {       

        if (other.CompareTag("Bullet")){
            Destroy(gameObject);
        } 
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, _detectionThreatRange);
    }
}
