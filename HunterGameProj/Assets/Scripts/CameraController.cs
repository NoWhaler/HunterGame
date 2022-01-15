using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _followCharacter;
    
    void FixedUpdate()
    {
        this.transform.position = new Vector3(_followCharacter.position.x, _followCharacter.position.y, this.transform.position.z);
    }
}
