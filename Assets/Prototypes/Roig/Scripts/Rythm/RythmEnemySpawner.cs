using System.Collections;
using UnityEngine;

public class RythmEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _EnemiesToSpawn;
    [SerializeField] private float _SpawnTime = 4f;

    void Start()
    {
        StartCoroutine("SpawnEnemy");
    }

    IEnumerator SpawnEnemy()
    {
        while (true) 
        {
            yield return new WaitForSeconds(_SpawnTime + Random.Range(0, _SpawnTime));
            Instantiate(_EnemiesToSpawn[Random.Range(0, _EnemiesToSpawn.Length)], transform.position, Quaternion.identity);
        }
    }
}
