using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmEnemyAnimationController : MonoBehaviour
{
    [SerializeField] private string deathAnimationName;
    [SerializeField] private Animator animator;

    public void OnDeath()
    {
        animator.Play(deathAnimationName);
    }
}
