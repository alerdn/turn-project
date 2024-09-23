using UnityEngine;

public class PlayerController : ControllerBase
{
    public static PlayerController Instance { get; private set; }

    public Gun Gun => _gun;
    public InputReader InputReader => _input;

    [Header("Input")]
    [SerializeField] private InputReader _input;

    [Header("Components")]
    [SerializeField] private Gun _gun;
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
        _movementComponent.Init(_input, Unit);
        _input.EnableMovementInputs();
    }
}
