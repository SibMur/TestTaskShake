using UnityEngine;

public class EnemyRewardSpawner : MonoBehaviour
{
    [SerializeField, Range(0,100)] private float _extraDropRate;
    [Space]
    [SerializeField] private GameObject _diamondPrefab;
    [SerializeField] private GameObject[] _extraPrefabs;


    private void OnDestroy() {
        Instantiate(_diamondPrefab, transform.position, Quaternion.identity);

        float rate = Random.Range(0, 101);
        if(rate < _extraDropRate) {
            int index = Random.Range(0, _extraPrefabs.Length);
            Instantiate(_extraPrefabs[index], transform.position + Vector3.forward/2, Quaternion.identity);
        }
    }
}
