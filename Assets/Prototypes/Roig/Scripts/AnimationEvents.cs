using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent<string> _animationEvent;

    public void SendAnimationEvent(string eventName)
    {
        _animationEvent?.Invoke(eventName);
    }
}
