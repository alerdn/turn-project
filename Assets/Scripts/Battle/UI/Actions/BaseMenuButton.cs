using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseMenuButton : MonoBehaviour
{
    public Button Button { get; private set; }
    public TMP_Text ButtonText { get; private set; }

    protected virtual void Start()
    {
        Button = GetComponent<Button>();
        ButtonText = GetComponentInChildren<TMP_Text>();
    }
}
