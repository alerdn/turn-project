using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseMenuButton : MonoBehaviour
{
    public Button Button { get; protected set; }
    public TMP_Text ButtonText { get; protected set; }

    public abstract void Execute();

    protected void InitComponents()
    {
        Button ??= GetComponent<Button>();
        ButtonText ??= GetComponentInChildren<TMP_Text>();
    }
}
