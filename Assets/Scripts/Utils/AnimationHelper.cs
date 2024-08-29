using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public event Action ImpactEvent;

    private void OnImpact()
    {
        ImpactEvent?.Invoke();
    }
}
