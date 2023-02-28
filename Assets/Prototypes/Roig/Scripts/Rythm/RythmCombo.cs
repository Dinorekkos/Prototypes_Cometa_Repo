using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RythmCombo", menuName = "RythmGame/RythmCombo")]
public class RythmCombo : ScriptableObject
{
    [SerializeField] private List<RythmNote> _NotesSequence;
    [SerializeField] private RythmAttack _Attack;

    public List<RythmNote> NotesSequence { get => _NotesSequence; }
    public RythmAttack Attack { get => _Attack; }

    public bool IsComboStillGoing(int index, RythmNote note)
    {
        return _NotesSequence[index] == note;
    }

    public bool IsComboFinished(List<RythmNote> currentCombo)
    {
        return _NotesSequence.SequenceEqual(currentCombo);
    }
}
