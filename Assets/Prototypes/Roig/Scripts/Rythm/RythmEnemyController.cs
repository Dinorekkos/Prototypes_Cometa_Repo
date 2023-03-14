using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RythmEnemyController : MonoBehaviour
{
    [SerializeField] private float _Speed = 4f;
    [SerializeField] private float _TimeCorpseOnFloor = 2f;
    [SerializeField] private ElementalTypes _WeakType;
    [SerializeField] private UnityEvent _OnDeath;
    [SerializeField] private GameObject _DeathParticle;
    [SerializeField] private Collider _Collider;

    private GameObject _Target;
    Vector3 targetRotation = Vector3.zero;
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
        positionToMove.y = 0.8f;
        transform.position = positionToMove;
        RotateToPlayer();
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
                    _Collider.enabled = false;
                    _Target = null;
                    StartCoroutine(DestroyEnemy());
                    _OnDeath?.Invoke();
                }
            }
        }
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(_TimeCorpseOnFloor);
        Instantiate(_DeathParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void RotateToPlayer()
    {
        if (_Target == null) return;
        targetRotation = Quaternion.LookRotation(_Target.transform.position - transform.position).eulerAngles;
        transform.rotation = Quaternion.Euler(Vector3.Scale(targetRotation, Vector3.up));
    }
}
