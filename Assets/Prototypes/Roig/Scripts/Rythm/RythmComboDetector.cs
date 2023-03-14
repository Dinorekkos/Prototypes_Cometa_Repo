using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RythmComboDetector : MonoBehaviour
{
    [SerializeField] private List<RythmCombo> _Combos;
    [SerializeField] private float _TimeToResetCombo = 1f;
    [SerializeField] private AudioSource _NoteAudioSource;
    [SerializeField] private RythmNote _BlueNote;
    [SerializeField] private RythmNote _RedNote;
    [SerializeField] private UnityEvent<RythmCombo> _OnComboMade;

    private List<RythmCombo> _AvailableCombos = new List<RythmCombo>();
    private int _NoteIndex = 0;

    private List<RythmNote> _CurrentNotes = new List<RythmNote>();

    void Start()
    {
        ResetCombo();
    }

    public void OnRedNote(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnNotePlayed(_RedNote);
        }
    }

    public void OnBlueNote(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnNotePlayed(_BlueNote);
        }
    }

    public void OnNotePlayed(RythmNote note)
    {
        bool foundCombo = false;
        PlayNoteSound(note);
        for (int i = _AvailableCombos.Count - 1; i >= 0; i--)
        {
            if (_AvailableCombos[i].IsComboStillGoing(_NoteIndex, note))
            {
                if (_AvailableCombos[i].IsComboFinished(_CurrentNotes))
                {
                    _OnComboMade?.Invoke(_AvailableCombos[i]);
                    ResetCombo();
                    foundCombo = true;
                    break;
                }
            }
            else
            {
                _AvailableCombos.Remove(_AvailableCombos[i]);
                if (_AvailableCombos.Count <= 0)
                {
                    StopCoroutine("ComboTimerCoroutine");
                    ResetCombo();
                    return;
                }
            }
        }
        if (!foundCombo)
        {
            _NoteIndex++;
            StopCoroutine("ComboTimerCoroutine");
            StartCoroutine("ComboTimerCoroutine");
        }
    }

    private void PlayNoteSound(RythmNote note)
    {
        _CurrentNotes.Add(note);
        //if (_NoteAudioSource.isPlaying) return;
        _NoteAudioSource.clip = note.NoteClip;
        _NoteAudioSource.Play();
    }

    IEnumerator ComboTimerCoroutine()
    {
        yield return new WaitForSeconds(_TimeToResetCombo);
        ResetCombo();
    }

    private void ResetCombo()
    {
        _CurrentNotes.Clear();
        _NoteIndex = 0;
        _AvailableCombos.Clear();
        _AvailableCombos = new List<RythmCombo>(_Combos);
    }
}
