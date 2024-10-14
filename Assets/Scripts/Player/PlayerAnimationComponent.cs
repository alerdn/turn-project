using UnityEngine;

public enum PlayerAnimationState
{
    Idle,
    Walk,
    Jump,
    Fall,
    Attack
}

public class PlayerAnimationComponent : MonoBehaviour
{
    private PlayerAnimationState _currentState;
    private Animator _animator;

    public void Init(Animator animator)
    {
        _animator = animator;
    }

    public void PlayAnimation(PlayerAnimationState state)
    {
        if (_currentState == state) return;
        _currentState = state;

        switch (state)
        {
            case PlayerAnimationState.Idle:
                _animator.CrossFadeInFixedTime("ANIM_Player_Idle", .1f);
                break;
            case PlayerAnimationState.Walk:
                _animator.CrossFadeInFixedTime("ANIM_Player_Walk", .1f);
                break;
            case PlayerAnimationState.Jump:
                _animator.CrossFadeInFixedTime("ANIM_Player_Jump", .1f);
                break;
            case PlayerAnimationState.Fall:
                _animator.CrossFadeInFixedTime("ANIM_Player_Fall", .1f);
                break;
            case PlayerAnimationState.Attack:
                _animator.CrossFadeInFixedTime("ANIM_Player_Attack", .1f);
                break;
        }
    }
}