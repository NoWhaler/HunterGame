using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HunterMovement : MonoBehaviour
{
    private PlayerControls _controls;


    [Header("Movement parameters")]
    [SerializeField]
    private float _moveSpeed;

    private Vector2 _movement;

    private Rigidbody2D _rigidBody;   

    [Header("Bullet components")]
    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private Transform _bulletDirection;

    private bool _canShoot = true;

    [Header("Amount of bullets")]
    [SerializeField]
    private int _amountBullets; 
    
    public void Die(){
        Destroy(gameObject);
    }

    private void Awake(){
        _rigidBody = GetComponent<Rigidbody2D>();
        _controls = new PlayerControls();
    }

    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable(){
        _controls.Disable();
    }

    private void Start() {        
        _controls.Player.Shoot.performed += _ => HunterShoot();
        
    }

    private void BulletsCount(int bullets){
        _amountBullets += bullets;    
    }

    private void HunterShoot(){
        if (!_canShoot && _amountBullets <= 0) return;
        BulletsCount(-1);
        Vector2 mousePosition = _controls.Player.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        GameObject bullet = Instantiate(_bullet, _bulletDirection.position, _bulletDirection.rotation);
        bullet.SetActive(true);        
        StartCoroutine(CanShoot());    
    }

    IEnumerator CanShoot(){  
        _canShoot = false;      
        yield return new WaitForSeconds(0.4f);
        _canShoot = true;
    }   

    private void OnMove(InputValue value){
        _movement = value.Get<Vector2>();
    }

    private void Movement(){      
        Vector2 currentPosition = _rigidBody.position;
        Vector2 adjustedMovement = _movement * _moveSpeed;
        Vector2 newPosition = currentPosition + adjustedMovement * Time.fixedDeltaTime;
        _rigidBody.MovePosition(newPosition);
    }

    private void RotateTowardDirection(){
        if (_movement != Vector2.zero){
            transform.rotation = Quaternion.LookRotation(Vector3.back, _movement);
        }
    }   

    private void FixedUpdate()
    {
           
        Movement();
        RotateTowardDirection();
    }
}
