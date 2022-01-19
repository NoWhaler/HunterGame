using UnityEngine;

public class CreaturesSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _deerPrefab;
    [SerializeField]
    private GameObject _wolfPrefab;
    [SerializeField]
    private GameObject _rabbitPrefab;
    
    [SerializeField]
    private int _deerCount;
    [SerializeField]
    private int _wolfCount;
    [SerializeField]
    private int _rabbitCount;

    private Vector2 RandomSpawnPointOnSquare(){
        float x = Random.Range(-25f, 25f);
        float y = Random.Range(-25f, 25f);
        return new Vector2(x, y);
    }

    private void Start (){
        for (int i = 0; i < _deerCount; i++){
            Instantiate(_deerPrefab, RandomSpawnPointOnSquare(), Quaternion.identity);
        }
        for (int i = 0; i < _wolfCount; i++){
            Instantiate(_wolfPrefab, RandomSpawnPointOnSquare(), Quaternion.identity);
        }
        for (int i = 0; i < _rabbitCount; i++){
            Instantiate(_rabbitPrefab, RandomSpawnPointOnSquare(), Quaternion.identity);
        }
    }       
}
