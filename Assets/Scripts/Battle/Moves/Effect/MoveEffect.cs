using System.Collections.Generic;
using UnityEngine;

public class MoveEffect : MonoBehaviour
{
    [SerializeField] private string _moveName;
    [SerializeField] private List<AudioSource> _audioSources;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Init(Unit unit)
    {
        string prefix = unit.Type switch
        {
            UnitType.Player => "AFCT_Player_",
            UnitType.Enemy => "AFCT_Enemy_",
            _ => ""
        };

        string animation = prefix + _moveName;
        _animator.CrossFadeInFixedTime(animation, .1f);
    }

    public void PlayAudio(int index)
    {
        int audiosCount = _audioSources?.Count ?? 0;
        if (audiosCount > 0 && index < audiosCount)
        {
            _audioSources[index].Play();
        }
    }

    public void OnEffectEnd()
    {
        Destroy(gameObject);
    }
}