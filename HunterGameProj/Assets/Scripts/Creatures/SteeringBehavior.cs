using UnityEngine;

public class SteeringBehavior : MonoBehaviour
{  

    private Vector3 _velocity;

    private Vector3 _currentPosition;

    private Vector3 _acceleration;    

    private Vector3 _startPosition; 

    public float _speed;      

    public float _force;

    public void Start(){
        _acceleration = Vector3.zero;
        _velocity = Vector3.zero;
        _currentPosition = transform.position;
        _startPosition = transform.position;
    }

    public void ApplyForce(Vector3 force)
    {
        _acceleration += force;
    }

    public void ApplySteeringToMotion()
    {
        _velocity = Vector3.ClampMagnitude(_velocity + _acceleration, _speed);
        _currentPosition += _velocity * Time.deltaTime;
        _acceleration = Vector3.zero;  
        RotateTowardTarget();
        transform.position = _currentPosition;
    }

    public Vector3 Wander(Vector3 targetPosition){
        
        Vector3 desiredVelocity = targetPosition - _currentPosition;        
        desiredVelocity.Normalize();        
        desiredVelocity *= _speed;        
        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - _velocity, _force);
        
        return steer;
    }

    public Vector3 Chase(Vector3 targetPosition){
        
        Vector3 desiredVelocity = targetPosition - _currentPosition;        
        desiredVelocity.Normalize();        
        desiredVelocity *= _speed;        
        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - _velocity, _force);
        
        return steer;
    }


    public Vector3 Escape(Vector3 targetPosition){
        Vector3 desiredVelocity = targetPosition - _currentPosition;        
        desiredVelocity.Normalize();        
        desiredVelocity *= -_speed;        
        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - _velocity, _force);
        
        return steer;
    }

    private void RotateTowardTarget()
    {
        Vector3 dirToDesiredLoc = _currentPosition - transform.position;
        dirToDesiredLoc.Normalize();
        float rotZ = Mathf.Atan2(dirToDesiredLoc.y, dirToDesiredLoc.x) * Mathf.Rad2Deg;
        rotZ -= 30;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    public Vector3 AvoidCliffs(){

        if (_currentPosition.x < -24f)
        {
            return CliffsAvoidance(new Vector3(_speed, _velocity.y));
        }
        if (_currentPosition.x > 24f)
        {
            return CliffsAvoidance(new Vector3(-_speed, _velocity.y));
        }
        if (_currentPosition.y < -24f)
        {
            return CliffsAvoidance(new Vector3(_velocity.x, _speed));
        }
        if (_currentPosition.y > 24f)
        {
            return CliffsAvoidance(new Vector3(_velocity.x, -_speed));
        }

        return Vector3.zero;
    }
    
    private Vector3 CliffsAvoidance(Vector3 desiredVelocity){
        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - _velocity, _force);
        return steer;
    }
}
