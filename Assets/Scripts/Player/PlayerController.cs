using UnityEngine;

public class PlayerController : ControllerBase
{
    public static PlayerController Instance { get; private set; }

    public InputReader InputReader => _input;
    public Gun Gun => _gun;
    public PlayerCombatComponent CombatComponent => _combatComponent;
    public PlayerAnimationComponent AnimationComponent => _animationComponent;
    public PlayerInventoryComponent InventoryComponent => _inventoryComponent;

    [Header("Input")]
    [SerializeField] private InputReader _input;

    [Header("Components")]
    [SerializeField] private Gun _gun;
    [SerializeField] private PlayerCombatComponent _combatComponent;
    [SerializeField] private PlayerMovementComponent _movementComponent;
    [SerializeField] private PlayerAnimationComponent _animationComponent;
    [SerializeField] private PlayerInventoryComponent _inventoryComponent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _animationComponent.Init(Unit.Animator);
        _movementComponent.Init(_input, _animationComponent);
        _input.EnableMovementInputs();
    }
}
