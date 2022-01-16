using UnityEngine;

public class CliffDeath : MonoBehaviour
{       
    private void OnTriggerEnter2D(Collider2D other)
    {            

        if (other.CompareTag("Hunter")){
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Wolf") || other.CompareTag("Rabbit") || other.CompareTag("Deer")){
            Destroy(other.gameObject);
        }        
    }
}
