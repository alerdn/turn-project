using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainBattleMenu : MonoBehaviour
{
    [SerializeField] private BattleMenu _mainMenu;
    [SerializeField] private BattleMenu _moveMenu;

    private void OnEnable()
    {
        _mainMenu.ShowMenu();
        _moveMenu.HideMenu();
    }
}
