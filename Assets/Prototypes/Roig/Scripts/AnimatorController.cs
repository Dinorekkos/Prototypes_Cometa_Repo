using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string IdleAnimationKey = "Idle";
    private const string RunAnimationKey = "Run";
    private const string PunchAnimationKey = "Punch";

    public void PlayRunAnimation()
    {
        _animator.Play(RunAnimationKey);
    }

    public void PlayIdleAnimation()
    {
        _animator.Play(IdleAnimationKey);
    }

    public void PlayPunchAnimation()
    {
        _animator.Play(PunchAnimationKey);
    }
}
  