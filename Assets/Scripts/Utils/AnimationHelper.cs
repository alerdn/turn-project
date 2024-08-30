using System;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public event Action<int> ImpactsEvent;

    public void OnImpact(int index)
    {
        ImpactsEvent?.Invoke(index);
    }
}
