using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = System.Random;

public class RabbitBehavior : SteeringBehavior
{

    [SerializeField]
    private Behaviors behavior;
    
    private Transform _target;

    private Transform _closestEscapeTarget;

    [SerializeField]
    private float _wanderSpeed;

    [SerializeField]
    private float _escapeSpeed;

    [SerializeField]
    private float _threatRange;

    [SerializeField]
    private float _walkRange;            

    private double changeSpot = 0;

    private Vector3 _targetPosition;

    private Vector3 randomPosition;
  
    private float closestThreatDistance;    

    private float _timeBeforeNewDirectionIsChosen = 0.1f;

    private float timeBeforeNewDirectionIsChosen;
    
    new void Start(){   
        base.Start();        
        FindNewSpot();
        timeBeforeNewDirectionIsChosen = Time.time + _timeBeforeNewDirectionIsChosen;
    }

    private void Update(){

        _closestEscapeTarget = DetectThreat();
        if (closestThreatDistance < (_threatRange * _threatRange))
        {
            behavior = Behaviors.ESCAPE;
            if (_closestEscapeTarget != null)
            {
                _target = _closestEscapeTarget;
                speed = _escapeSpeed;
            }
            else
            {
                behavior = Behaviors.WANDER;
                speed = _wanderSpeed;
            }
        } else if (closestThreatDistance > _walkRange * _walkRange)
        {
            behavior = Behaviors.WANDER;
            speed = _wanderSpeed;
        }
        else
        {
            if (_closestEscapeTarget == null)
            {
                behavior = Behaviors.WANDER;
                speed = _wanderSpeed;
            }
        }
        Vector3 cliff = AvoidCliffs();
        Vector3 steer = Vector3.zero;
        switch (behavior)
        {
            case Behaviors.WANDER:
                steer = Wander(_targetPosition);
                if (timeBeforeNewDirectionIsChosen <= Time.time)
                {
                    FindNewSpot();
                    timeBeforeNewDirectionIsChosen = Time.time + _timeBeforeNewDirectionIsChosen;
                }
                break;
            case Behaviors.ESCAPE:
                steer = Escape(_target.position);
                break;
        }
        ApplyForce(steer);
        ApplyForce(cliff * 1.2f);
        ApplySteeringToMotion();
    }

    private void FindNewSpot(){
        Random randomValue = new Random();
        changeSpot += randomValue.NextDouble() * 2 - 1;
        randomPosition = new Vector2((float) Math.Cos(changeSpot), (float) Math.Sin(changeSpot));
        _targetPosition = transform.GetChild(0).position + randomPosition;
    }

    private Transform DetectThreat(){
        List<Transform> targets = GameObject.FindGameObjectsWithTag("Creatures").Select(target => target.transform.parent.gameObject.transform).ToList();
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
                closestThreatDistance = distanceToTarget;
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
