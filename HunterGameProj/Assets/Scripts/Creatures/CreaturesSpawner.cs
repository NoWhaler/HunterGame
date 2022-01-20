using UnityEngine;

public class CreaturesSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _deerPrefab;
    [SerializeField]
    private GameObject _wolfPrefab;
    [SerializeField]
    private GameObject _rabbitPrefab;
    
    private Transform _spawnTransform;

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
            
            GenerateAnimals(_deerPrefab, Random.Range(3, 11), i);
        }
        GenerateAnimals(_wolfPrefab, _wolfCount);
        GenerateAnimals(_rabbitPrefab, _rabbitCount);       
        
    } 

    private void GenerateAnimals(GameObject prefabCreature, int count){
        for (int i = 0; i < count; i++){
            GameObject creature = Instantiate(prefabCreature, RandomSpawnPointOnSquare(), Quaternion.identity);
        }
    }   

    private void GenerateAnimals(GameObject prefabCreature, int count, int group){
        for (int i = 0; i < count; i++){
            GameObject creature = Instantiate(prefabCreature, RandomSpawnPointOnSquare(), Quaternion.identity, _spawnTransform);
        }
    }   
}
