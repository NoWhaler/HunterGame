using UnityEngine;
using System.Linq;

public class Rabbit : MonoBehaviour
{
    [Header("Properties")]

    [SerializeField]
    private float _baseSpeed;

    [SerializeField]
    private float _threatSpeedValue;

    [SerializeField]
    private float _threatRange;    

    private Vector2 _threatRunDirection;

    private float _threatTime;

    private float _calmDownTime = 1.5f;

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

    private bool _threat = false;

    private bool _walk = true;  

    private Rigidbody2D _rigidBody;    

    private void Start(){
        _threat = false;        
        waitTime = startWaitTime;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {     
        
        FindThreatTarget();

        if (_threatTime <= 0){
            _threat = false;
        }
        if (_threatTime > 0){
            _threatTime -= Time.deltaTime;

        }
        else{
            Walk();
            _threatTime = 1;
        }

        if (!_threat){
            Walk();         
        }
        else{
            SpeedUp();
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

    private void SpeedUp(){
        var moveVector = Vector2.MoveTowards(transform.position, _threatRunDirection * 3, _threatSpeedValue * Time.deltaTime);
        transform.position = new Vector2(moveVector.x, moveVector.y);
    }    

    private void FindThreatTarget(){

        var threat = Physics2D.OverlapCircleAll(transform.position, _threatRange).ToList<Collider2D>();

        threat = threat.Where(x => x.gameObject.GetInstanceID() != gameObject.GetInstanceID()).ToList();

        if (threat.Count > 0){
            Vector3 runDirection = new Vector3();
            foreach (var scareCreature in threat){
                Vector3 direction = new Vector3();
                if (scareCreature.gameObject.tag == "Cliff"){
                     direction = scareCreature.transform.position - transform.position;
                }
                else{
                    direction = transform.position - scareCreature.transform.position;
                }                            
                runDirection += direction;
            }
            _threatRunDirection = runDirection;
            _threat = true;
            _threatTime = _calmDownTime;
        }       

    }    

    private void OnTriggerEnter2D(Collider2D other) {       

        if (other.CompareTag("Bullet")){
            Destroy(gameObject);
        } 
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, _threatRange);
    }

}
