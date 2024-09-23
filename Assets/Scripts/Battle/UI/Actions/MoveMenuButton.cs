using System;
using System.Threading.Tasks;
using UnityEngine;

public class MoveMenuButton : BaseMenuButton
{
    [SerializeField] private MoveData _move;
    [SerializeField] private FightBattleMenu _fightMenu;
    [SerializeField] private bool _overrideName;

    private bool _isInitialized;

    protected override void OnEnable()
    {
        base.OnEnable();

        VerifyInitialization();
    }

    public void Init(MoveData move, FightBattleMenu fightMenu = null)
    {
        _move = move;
        _fightMenu ??= fightMenu;
    }

    private void VerifyInitialization()
    {
        if (_move && !_isInitialized)
        {
            if (!_overrideName)
            {
                ButtonText.text = _move.Name;
            }
            _isInitialized = true;
        }
    }

    public void Execute()
    {
        bool canUseMove = _move && _move.EnergyCost <= PlayerController.Instance.Unit.EnergyAmount;
        if (canUseMove)
        {
            _fightMenu.SelectAction(_move);
            _fightMenu.HideMenu();
        }
        else
        {
            Debug.LogError("Jogador não pode usar este movimento");
        }
    }
}