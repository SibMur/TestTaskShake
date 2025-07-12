using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [SerializeField] private Transform[] _targets;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _allyPrefab;
    [Space]
    [SerializeField] private int _energyThreshold = 10;
    [SerializeField] private int _startEnemies = 5;
    [SerializeField] private float _timeBetweenWaves = 10f;

    private int _wave = 0;
    private int _energy = 0;


    private void Awake() {
        if (Instance == null) 
            Instance = this;        
    }

    private void Start() {
        NextWave();
        UIManager.Instance.TurnOffTargets();
    }

    public void NextWave() {
        _wave++;
        int enemies = _startEnemies + (_wave * 2);
        for (int i = 0; i < enemies; i++) {
            var spawnIndex = Random.Range(0, _targets.Length);
            GameObject enemy = Instantiate(_enemyPrefab, _targets[spawnIndex]);
            SetEnemyTarget(enemy);
            enemy.transform.parent = null;
        }
        Invoke(nameof(NextWave), _timeBetweenWaves);
    }

    private void SetEnemyTarget(GameObject enemy) {
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        ObsticleChecker obsticle = enemy.GetComponent<ObsticleChecker>();
        obsticle.checkDistance = 30;
    }

    public void CollectDiamond() {
        _energy++;
        if (_energy >= _energyThreshold) {
            Instantiate(_allyPrefab, GameManager.Instance.LevelManager.Player.transform.position, Quaternion.identity);
            _energy = 0;
            _energyThreshold += 5;
        }
    }

    public void CollectMagnite() {
        List<Diamond> diamonds = FindObjectsOfType<Diamond>().ToList();
        for(int i = 0; i < diamonds.Count; i++) {
            Destroy(diamonds[i].gameObject);
            diamonds.RemoveAt(i);
        }
    }
}
