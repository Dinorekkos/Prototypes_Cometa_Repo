using UnityEngine;

public class RythmMissile : MonoBehaviour
{
    [SerializeField] private float _Speed = 3f;
    [SerializeField] private float _TimeAlive = 3f;
    [SerializeField] private ElementalTypes _ElementalType;

    private float TimeCounter = 0;

    Vector3 targetRotation = Vector3.zero;

    public ElementalTypes ElementalType { get => _ElementalType; }

    public void SetTargetRotation(Transform _target)
    {
        targetRotation = Quaternion.LookRotation(_target.position - transform.position).eulerAngles;
        transform.rotation = Quaternion.Euler(Vector3.Scale(targetRotation, Vector3.up));
    }

    public void OnTriggerDetected(GameObject _object)
    {
        Destroy(gameObject);
    }

    void Update()
    {
        transform.position += transform.forward * _Speed * Time.deltaTime;

        TimeCounter += Time.deltaTime;
        if (TimeCounter > _TimeAlive)
        {
            Destroy(gameObject);
        }
    }
}
