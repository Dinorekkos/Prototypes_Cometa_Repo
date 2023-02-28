using UnityEngine;
using System.Collections;
using ManagerPooling;

public class MoveObject : MonoBehaviour
{
    
    [SerializeField]
    private DataForPool data = default;

    private Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 7f));
        transform.position = pos;
        transform.rotation = Quaternion.identity;
        StopCoroutine(Delay());
        StartCoroutine(Delay());
    }

    private void FixedUpdate()
    {
        body.AddForce(Vector3.right * data.Speed);
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(data.TimerToDisable);
        PoolManager.BackToPool(gameObject, data.IdGroup);
    }
}
