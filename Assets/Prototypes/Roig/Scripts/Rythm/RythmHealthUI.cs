using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RythmHealthUI : MonoBehaviour
{
    [SerializeField] private Text _HealthText;

    public void OnDamageTaken(int remainginHealth)
    {
        _HealthText.text = "Health: " + remainginHealth;
    }
}
