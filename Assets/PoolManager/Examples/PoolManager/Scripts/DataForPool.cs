using UnityEngine;

[CreateAssetMenu(fileName = "New Data Prefab", menuName = "Data Prefab", order = 0)]
public class DataForPool : ScriptableObject
{
    [SerializeField]
    private string idGroup = default;

    [SerializeField]
    private GameObject prefab = default;

    [SerializeField]
    private int numPreparedObjects  = default;

    [SerializeField]
    private float speed = default;

    [SerializeField]
    private float timerToDisable = default;

    public string IdGroup
    {
        get
        {
            return idGroup;
        }
    }

    public GameObject Prefab
    {
        get
        {
            return prefab;
        }
    }

    public int NumPreparedObjects
    {
        get
        {
            return numPreparedObjects;
        }
    }

    public float Speed
    {
        get 
        {
            return speed;
        }
    }

    public float TimerToDisable
    {
        get
        {
            return timerToDisable;
        }
    }
}
