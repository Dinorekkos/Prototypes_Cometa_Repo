using UnityEngine;

public class RythmAttackSpawner : MonoBehaviour
{
    private RythmMissile _AttackToSpawn;
    private GameObject _CurrentTarget;

    public void OnEnemyDetected(GameObject target)
    {
        _CurrentTarget = target;
    }

    public void OnComboMade(RythmCombo combo)
    {
        if (_CurrentTarget != null)
        {
            _AttackToSpawn = Instantiate(combo.Attack.AttackPrefab, transform.position, Quaternion.identity).GetComponent<RythmMissile>();
            _AttackToSpawn.SetTargetRotation(_CurrentTarget.transform);
        }
    }
}
