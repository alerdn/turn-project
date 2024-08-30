using UnityEngine;

public class MoveEffect : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Init(Unit unit, string animationName)
    {
        string prefix = unit.Type switch
        {
            UnitType.Player => "AFCT_Player_",
            UnitType.Enemy => "AFCT_Enemy_",
            _ => ""
        };

        string animation = prefix + animationName;
        _animator.CrossFadeInFixedTime(animation, .1f);
    }


    public void OnEffectEnd()
    {
        Destroy(gameObject);
    }
}