using UnityEngine;
using UnityEngine.Events;

public class RythmTriggerDetector : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject> OnTriggerDetected;

    void OnTriggerEnter(Collider other)
    {
        OnTriggerDetected?.Invoke(other.gameObject);
    }
}
