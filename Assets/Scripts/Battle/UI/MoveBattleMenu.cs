using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveBattleMenu : BattleMenu
{
    [SerializeField] private Transform _actionParent;

    public void Init(Unit unit)
    {
        List<MoveMenuButton> buttons = _actionParent.GetComponentsInChildren<MoveMenuButton>().ToList();
        for (int i = 0; i < unit.Moves.Count; i++)
        {
            MoveData move = unit.Moves[i];
            MoveMenuButton button = buttons[i];

            button.Init(move, this);
        }
    }
}