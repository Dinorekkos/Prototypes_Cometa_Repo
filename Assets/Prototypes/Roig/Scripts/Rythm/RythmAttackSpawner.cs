using UnityEngine;

public class RythmAttackSpawner : MonoBehaviour
{
    private GameObject _AttackToSpawn;

    public void OnComboMade(RythmCombo combo)
    {
        _AttackToSpawn = Instantiate(combo.Attack.AttackPrefab, transform.position, Quaternion.identity);
    }
}
