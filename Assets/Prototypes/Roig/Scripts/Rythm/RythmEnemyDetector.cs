using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RythmEnemyDetector : MonoBehaviour
{
    [SerializeField] private float _DetectionRange = 4f;
    [SerializeField] private LayerMask _EnemiesLayerMask;
    [SerializeField] private UnityEvent<GameObject> _OnEnemyDetected;
    private GameObject _CurrentEnemy;

    private RaycastHit[] enemyHits;

    Vector3 targetRotation = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        DetectEnemies();
        RotateToEnemy();
    }

    private void DetectEnemies()
    {
        enemyHits = Physics.SphereCastAll(transform.position, _DetectionRange, Vector3.forward, 1f, _EnemiesLayerMask);
        float minDistanceToEnemy = Mathf.Infinity;
        if (enemyHits.Length == 0)
        {
            _CurrentEnemy = null;
            minDistanceToEnemy = Mathf.Infinity;
            _OnEnemyDetected?.Invoke(_CurrentEnemy);
        }
        else
        {
            foreach (var enemy in enemyHits) 
            {
                float distance = Vector3.Distance(enemy.collider.gameObject.transform.position, transform.position);
                if (distance <= minDistanceToEnemy)
                {
                    minDistanceToEnemy = distance;
                    _CurrentEnemy = enemy.collider.gameObject;
                    _OnEnemyDetected?.Invoke(_CurrentEnemy);
                }
            }
        }
    }

    private void RotateToEnemy()
    {
        if (_CurrentEnemy == null) return;
        targetRotation = Quaternion.LookRotation(_CurrentEnemy.transform.position - transform.position).eulerAngles;
        transform.rotation = Quaternion.Euler(Vector3.Scale(targetRotation, Vector3.up));
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _DetectionRange);    
    }
}
