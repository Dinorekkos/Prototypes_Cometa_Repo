using System.Collections;
using UnityEngine;

public class RythmParticle : MonoBehaviour
{
    [SerializeField] private float _TimeAlive = 1f;

    void OnEnable()
    {
        StartCoroutine(DeathTimer()); 
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(_TimeAlive);
        Destroy(gameObject);
    }
}
