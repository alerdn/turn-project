using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public event Action ImpactEvent;

    public void OnImpact()
    {
        ImpactEvent?.Invoke();
    }
}
