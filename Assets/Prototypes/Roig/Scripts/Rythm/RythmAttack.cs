using UnityEngine;

[CreateAssetMenu(fileName = "RythmAttack", menuName = "RythmGame/RythmAttack")]
public class RythmAttack : ScriptableObject
{
    [SerializeField] private GameObject _AttackPrefab;
    [SerializeField] private int _ManaCost;

    public GameObject AttackPrefab { get => _AttackPrefab; }
    public int ManaCost { get => _ManaCost; }
}
