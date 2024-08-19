using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{
    [SerializeField] private GameObject _frame;

    protected virtual void Start() { }

    public void ShowMenu()
    {
        _frame.SetActive(true);
    }

    public void HideMenu()
    {
        _frame.SetActive(false);
    }
}
