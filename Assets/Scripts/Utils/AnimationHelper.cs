using System;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public event Action<int> ImpactsEvent;
    public event Action<MoveEffect> SpawnEffectEvent;

    public void OnImpact(int index)
    {
        ImpactsEvent?.Invoke(index);
    }

    public void SpawnEffect(MoveEffect effect)
    {
        if (!effect) return;

        SpawnEffectEvent?.Invoke(effect);
    }
}
