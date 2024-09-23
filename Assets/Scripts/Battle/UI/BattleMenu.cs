using System;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    public event Action<ActionData> OnSelectAction;

    [SerializeField] private GameObject _frame;

    protected virtual void Start() { }

    public void SelectAction(ActionData action)
    {
        OnSelectAction?.Invoke(action);
    }

    public virtual void ShowMenu()
    {
        _frame.SetActive(true);
    }

    public virtual void HideMenu()
    {
        _frame.SetActive(false);
    }
}
