using System;
using System.Threading.Tasks;
using UnityEngine;

public class MoveMenuButton : BaseMenuButton
{
    [SerializeField] private MoveData _move;
    [SerializeField] private BattleMenu _fightMenu;
    [SerializeField] private bool _overrideName;

    private bool _isInitialized;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_move && !_isInitialized)
        {
            if (!_overrideName)
            {
                ButtonText.text = _move.Name;
            }
            Button.onClick.AddListener(Execute);
            _isInitialized = true;
        }
    }

    private void Start()
    {

    }

    public void Init(MoveData move, BattleMenu fightMenu)
    {
        _move = move;
        _fightMenu = fightMenu;
    }

    private void Execute()
    {
        bool canUseMove = _move.EnergyCost <= PlayerController.Instance.PlayerUnit.EnergyAmount;
        if (canUseMove)
        {
            BattleManager.Instance.RoundMovesChosen.Add(new() { Type = UnitType.Player, Move = _move });
            _fightMenu.HideMenu();
        }
        else
        {
            Debug.LogError("Jogador não possui energia para usar este movimento");
        }
    }
}