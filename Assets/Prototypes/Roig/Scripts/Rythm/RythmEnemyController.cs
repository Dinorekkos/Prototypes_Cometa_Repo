using UnityEngine;

public class RythmEnemyController : MonoBehaviour
{
    [SerializeField] private float _Speed = 4f;
    [SerializeField] private ElementalTypes _WeakType;

    private GameObject _Target;

    void OnEnable()
    {
        _Target = GameObject.FindGameObjectWithTag("Player");    
    }

    // Update is called once per frame
    void Update()
    {
        if (_Target == null) return;
        // Move our position a step closer to the target.
        var step = _Speed * Time.deltaTime; // calculate distance to move
        Vector3 positionToMove = Vector3.MoveTowards(transform.position, _Target.transform.position, step);
        positionToMove.y = 1.63f;
        transform.position = positionToMove;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player")
        {
            RythmMissile missile = other.transform.parent.gameObject.GetComponent<RythmMissile>();
            if (missile != null)
            {
                if (missile.ElementalType == _WeakType)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
