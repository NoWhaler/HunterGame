using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = System.Random;

public class DeerBehavior : SteeringBehavior
{

    [SerializeField]
    private Behaviors behavior;

    private Transform _target;

    private Transform _closestAvoidTarget;

    [SerializeField]
    private float _walkRange; 
    
    [SerializeField]
    private float _avoidRange;

    private double changeSpot = 0;

    private Vector3 _targetPosition;

    private Vector3 randomPosition;  

    private float closestTargetDistance;    

    private float _timeBeforeNewDirectionIsChosen = 0.1f;

    private float timeBeforeNewDirectionIsChosen; 

    private List<GameObject> flock; 


    new void Start(){
        base.Start();        
        FindNewSpot();
        timeBeforeNewDirectionIsChosen = Time.time + _timeBeforeNewDirectionIsChosen;
    }

    private void Update() {

        flock = GameObject.FindGameObjectsWithTag("Deer").Select(flock => flock.transform.parent.gameObject).ToList();

        Vector3 cliff = AvoidCliffs();

        Vector3 avoidance = Avoid(flock); 

        Vector3 cohesion = Cohesion(flock);  

        Vector3 align = Alingment(flock);     

        _closestAvoidTarget = DetectThreat();

        Vector3 steeringBehavior = Vector3.zero;

        switch (behavior){
            case Behaviors.WANDER:
                steeringBehavior = Wander(_targetPosition);
                if (timeBeforeNewDirectionIsChosen <= Time.time)
                {
                    FindNewSpot();
                    timeBeforeNewDirectionIsChosen = Time.time + _timeBeforeNewDirectionIsChosen;
                }
                break;
                

            case Behaviors.AVOID:
                steeringBehavior = Escape(_target.position);
                break;
                

            case Behaviors.COHESION:

                break;
        }

        ApplyForce(steeringBehavior);
        ApplyForce(cohesion * 2f);
        ApplyForce(avoidance * 2f);
        ApplyForce(align);
        ApplyForce(cliff * 2f);
        ApplySteeringToMotion();
    }

    public Vector3 Cohesion(List<GameObject> flock){

        float neighborDistance = 5f;
        Vector3 sum = new Vector3();
        int count = 0;
        foreach (var deerCreature in flock){
            float d = Vector3.Distance(transform.position, deerCreature.transform.position);
            if ((d > 0) && (d < neighborDistance)){
                sum +=  deerCreature.transform.position;
                count++;
            }
        }

        if (count > 0){
            sum /= count;
            return Wander(sum); 
        }
        else{
            return Vector3.zero;
        }        
    } 

    public Vector3 Avoid(List<GameObject> flock){
        float desiredSeparation = 2f;
        Vector3 sum = new Vector3();
        int count = 0;
        foreach (var deerCreature in flock){
            float d = Vector3.Distance(transform.position, deerCreature.transform.position);
            if ((d > 0) && (d < desiredSeparation)){
                Vector3 difference = transform.position - deerCreature.transform.position;
                difference.Normalize();
                sum += difference;
                count++;

            }
        }

        if (count > 0){
            sum /= count;
            sum.Normalize();
            sum *= speed;
            Vector3 steer = Vector3.ClampMagnitude(sum - velocity, force);
            return steer;
        }  

        return Vector3.zero;     
        
    }       

    public Vector3 Alingment(List<GameObject> flock){
        float neighborDistance = 5f;
        Vector3 sum = new Vector3();
        int count = 0;
        foreach (var deerCreature in flock){
            float d = Vector3.Distance(transform.position, deerCreature.transform.position);
            if ((d > 0) && (d < neighborDistance)){
                sum +=  deerCreature.transform.position;
                count++;
            }
        }

        if (count < 0){
            sum /= count;
            sum.Normalize();
            sum *= speed;
            Vector3 steer = sum - velocity;
            return steer;
        }
        else{
            return Vector3.zero;
        }       
    }

    private void FindNewSpot(){
        Random randomValue = new Random();
        changeSpot += randomValue.NextDouble() * 2 - 1;
        randomPosition = new Vector2((float) Math.Cos(changeSpot), (float) Math.Sin(changeSpot));
        _targetPosition = transform.GetChild(0).position + randomPosition;
    } 

    private Transform DetectThreat(){
        List<Transform> targets = GameObject.FindGameObjectsWithTag("EnemyCreature").Select(target => target.transform.parent.gameObject.transform).ToList();
        targets.Remove(gameObject.transform);
        Transform target = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform potentialTarget in targets)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float distanceToTarget = directionToTarget.sqrMagnitude;
            if(distanceToTarget < closestDistance)
            {
                closestTargetDistance = distanceToTarget;
                closestDistance = distanceToTarget;
                target = potentialTarget;
            }
        }
        return target;
    }  

    private void OnTriggerEnter2D(Collider2D other) {       

        if (other.CompareTag("Bullet")){
            Destroy(gameObject);
        } 
    }  

    
}
