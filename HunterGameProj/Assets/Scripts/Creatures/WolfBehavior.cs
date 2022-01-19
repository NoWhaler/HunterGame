using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = System.Random;

public class WolfBehavior : SteeringBehavior
{    

    [SerializeField]
    private Behaviors behavior;
    
    private Transform _target;

    [SerializeField]
    private float _wanderSpeed;

    [SerializeField]
    private float _chaseSpeed;

    private Transform _closestTarget;

    [SerializeField]
    private float _walkRange; 
    
    [SerializeField]
    private float _chaseRange;       

    [SerializeField]
    private float _dieWithoutCatch;

    private double changeSpot = 0;

    private Vector3 _targetPosition;

    private Vector3 randomPosition;    

    private float _additionalLifeTime = 5f;    

    private float closestTargetDistance;    

    private float _timeBeforeNewDirectionIsChosen = 0.1f;

    private float timeBeforeNewDirectionIsChosen;

    new void Start(){
        base.Start();        
        FindNewSpot();
        timeBeforeNewDirectionIsChosen = Time.time + _timeBeforeNewDirectionIsChosen;
    }

    private void Update(){

        if (Time.time > _dieWithoutCatch){
            DieWithoutCatch();
        }

        Vector3 cliff = AvoidCliffs();

        _closestTarget = FindTarget();        

        if (closestTargetDistance < (_chaseRange * _chaseRange)){

            behavior = Behaviors.CHASE;            
            if (_closestTarget != null){

                _target = _closestTarget; 
                _speed = _chaseSpeed;
            }
            else{

                behavior = Behaviors.WANDER;   
                _speed = _wanderSpeed;             
            }
            
        }
        else if (closestTargetDistance > _walkRange * _walkRange){

            behavior = Behaviors.WANDER;
            _speed = _wanderSpeed;            
        }
        else{
            if (_closestTarget == null){

                behavior = Behaviors.WANDER;
                _speed = _wanderSpeed;
            }
        }
        
        Vector3 steeringBehavior = Vector3.zero;      
        
        switch (behavior){
            case Behaviors.WANDER:

                steeringBehavior = Wander(_targetPosition);

                if (timeBeforeNewDirectionIsChosen <= Time.time){

                    FindNewSpot();
                    timeBeforeNewDirectionIsChosen = Time.time + _timeBeforeNewDirectionIsChosen;
                }      
                break;

            case Behaviors.CHASE:

                steeringBehavior = Chase(_target.position);

                break;            
        }    

        ApplyForce(steeringBehavior);
        ApplyForce(cliff * 2f);
        ApplySteeringToMotion();
    }     

    private void FindNewSpot(){
        Random randomValue = new Random();
        changeSpot += randomValue.NextDouble() * 2 - 1;
        randomPosition = new Vector2((float) Math.Cos(changeSpot), (float) Math.Sin(changeSpot));
        _targetPosition = transform.GetChild(0).position + randomPosition;
    }    

    private Transform FindTarget(){
        List<Transform> targets = GameObject.FindGameObjectsWithTag("FriendlyCreature").Select(target => target.transform.parent.gameObject.transform).ToList();
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

    private void DieWithoutCatch(){
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Rabbit") || other.CompareTag("Hunter") || other.CompareTag("Rabbit")){
            Destroy(other.gameObject);
            _dieWithoutCatch += _additionalLifeTime;
        }

        if (other.CompareTag("Bullet")){
            Destroy(gameObject);
        } 
    }    

}
