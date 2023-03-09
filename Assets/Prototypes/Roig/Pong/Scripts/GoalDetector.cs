using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class GoalDetector : NetworkBehaviour
{
    public bool isPlayer1Goal = true;
    public UnityEvent<bool> OnGoal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>() != null)
        {
            OnGoal?.Invoke(isPlayer1Goal);
        }
    }
}
