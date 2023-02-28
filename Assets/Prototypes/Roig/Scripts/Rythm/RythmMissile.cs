using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmMissile : MonoBehaviour
{
    [SerializeField] private float _Speed = 3f;
    [SerializeField] private float _TimeAlive = 3f;

    private float TimeCounter = 0;
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * _Speed * Time.deltaTime;

        TimeCounter += Time.deltaTime;
        if (TimeCounter > _TimeAlive)
        {
            Destroy(gameObject);
        }
    }
}
