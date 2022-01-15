using UnityEngine;

public class CreaturesSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _deer;
    [SerializeField]
    private GameObject _wolf;
    [SerializeField]
    private GameObject _rabbit;
    
    [SerializeField]
    private int _deerCount;
    [SerializeField]
    private int _wolfCount;
    [SerializeField]
    private int _rabbitCount;

    private Vector2 RandomSpawnPointOnSquare(){
        float x = Random.Range(-24f, 24f);
        float y = Random.Range(-24f, 24f);
        return new Vector2(x, y);
    }

    private void Start (){
        for (int i = 0; i < _deerCount; i++){
            Instantiate(_deer, RandomSpawnPointOnSquare(), Quaternion.identity);
        }
        for (int i = 0; i < _wolfCount; i++){
            Instantiate(_wolf, RandomSpawnPointOnSquare(), Quaternion.identity);
        }
        for (int i = 0; i < _rabbitCount; i++){
            Instantiate(_rabbit, RandomSpawnPointOnSquare(), Quaternion.identity);
        }
    }       
}
