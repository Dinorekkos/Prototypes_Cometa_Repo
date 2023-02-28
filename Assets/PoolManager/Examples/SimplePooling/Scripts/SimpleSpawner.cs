using UnityEngine;
using ManagerPooling;

public class SimpleSpawner : MonoBehaviour
{
    [SerializeField]
    private int numPreparedObjects = default;
    private SimplePooling poolSystem;
    private void Start()
    {
        poolSystem = FindObjectOfType<SimplePooling>();
        poolSystem.PoolInstaller(numPreparedObjects);
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            poolSystem.Spawn();
        }
    }
}
