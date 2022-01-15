using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooting : MonoBehaviour
{
    [SerializeField]
    private float _bulletSpeed;

    IEnumerator DestroyBulletAfterTime(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

     private void OnEnable() {

        StartCoroutine(DestroyBulletAfterTime());

    }
    
    private void Update()
    {
        transform.Translate(Vector3.up * _bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other){        
        Destroy(gameObject);
    }
}
