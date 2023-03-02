using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RythmPlayerHealth : MonoBehaviour
{
    [SerializeField] private int _MaxHealth = 100;
    [SerializeField] private UnityEvent<int> _OnDamageTaken;
    [SerializeField] private UnityEvent _OnDeath;

    private bool canBeDamaged = true;

    void Start()
    {
        _OnDamageTaken?.Invoke(_MaxHealth);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<RythmEnemyController>() != null)
        {
            if (canBeDamaged)
            {
                _MaxHealth -= 10;
                _OnDamageTaken?.Invoke(_MaxHealth);
                if (_MaxHealth <= 0)
                {
                    _OnDeath?.Invoke();
                    return;
                }
                canBeDamaged = false;
                StartCoroutine("DamageCooldown");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<RythmEnemyController>() != null)
        {
            if (canBeDamaged)
            {
                _MaxHealth -= 10;
                _OnDamageTaken?.Invoke(_MaxHealth);
                if (_MaxHealth <= 0)
                {
                    _OnDeath?.Invoke();
                    return;
                }
                canBeDamaged = false;
                StartCoroutine("DamageCooldown");
            }
        }
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(1f);
        canBeDamaged = true;
    }
}
