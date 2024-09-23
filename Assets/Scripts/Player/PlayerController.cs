using UnityEngine;

public class PlayerController : ControllerBase
{
    public static PlayerController Instance { get; private set; }

    public InputReader InputReader => _input;
    public Gun Gun => _gun;
    public PlayerCombatComponent CombatComponent => _combatComponent;

    [Header("Input")]
    [SerializeField] private InputReader _input;

    [Header("Components")]
    [SerializeField] private Gun _gun;
    [SerializeField] private PlayerCombatComponent _combatComponent;
    [SerializeField] private PlayerMovementComponent _movementComponent;

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
        _movementComponent.Init(_input, Unit.transform);
        _input.EnableMovementInputs();
    }
}
