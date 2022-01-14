using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HunterMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    private Vector2 _movement;

    private Rigidbody2D _rigidBody;

    // public Camera camera;
    
    private void Awake(){
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value){
        _movement = value.Get<Vector2>();
    }

    public void Movement(){
        Vector2 currentPosition = _rigidBody.position;
        Vector2 adjustedMovement = _movement * _moveSpeed;
        Vector2 newPosition = currentPosition + adjustedMovement * Time.fixedDeltaTime;
        _rigidBody.MovePosition(newPosition);
    }

    public void RotateTowardDirection(){
        if (_movement != Vector2.zero){
            transform.rotation = Quaternion.LookRotation(Vector3.back, _movement);
        }
    }
   

    void FixedUpdate()
    {        
        Movement();
        RotateTowardDirection();
    }
}
