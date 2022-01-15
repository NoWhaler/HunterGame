using UnityEngine;

public class CliffDeath : MonoBehaviour
{       
    private void OnTriggerEnter2D(Collider2D other)
    {            

        if (other.CompareTag("Hunter")){
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Creatures")){
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Bullet")){
            Destroy(other.gameObject);
        }
    }
}
