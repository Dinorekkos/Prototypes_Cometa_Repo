using UnityEngine;
using ManagerPooling;

public class MouseSpawner : MonoBehaviour
{
    [SerializeField]
    private DataForPool data = default;

    [SerializeField]
    private int numMouseButton = default;
    private void Start()
    {
        PoolManager.PoolInstaller(data.Prefab,data.NumPreparedObjects,data.IdGroup);
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(numMouseButton))
        {
            PoolManager.SpawnObject(data.Prefab, data.IdGroup);
        }
    }
}
