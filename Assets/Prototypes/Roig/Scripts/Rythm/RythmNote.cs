using UnityEngine;

[CreateAssetMenu(fileName = "RythmNote", menuName = "RythmGame/RythmNotes")]
public class RythmNote : ScriptableObject
{
    public enum NoteType
    {
        Red,
        Blue
    }

    [SerializeField] private NoteType _Note;
    [SerializeField] private AudioClip _NoteClip;

    public NoteType Note { get => _Note; }
    public AudioClip NoteClip { get => _NoteClip; }
}
