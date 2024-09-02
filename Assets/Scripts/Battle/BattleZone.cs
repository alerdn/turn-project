using Cinemachine;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private EnemyController _enemy;
    [SerializeField] private Transform _enemyPosition;

    private CameraShakeOnHit _cameraShakeEffect;
    private bool _isBattleStarted;

    private void Start()
    {
        _isBattleStarted = false;
        _camera.gameObject.SetActive(false);
        _cameraShakeEffect = _camera.GetComponent<CameraShakeOnHit>();

        _enemy.EnemyUnit.ImpactEvent += OnImpact;
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isBattleStarted && other.TryGetComponent(out PlayerController playerController))
        {
            _isBattleStarted = true;
            _camera.gameObject.SetActive(true);
            BattleManager battleManager = BattleManager.Instance;

            battleManager.OnBattleEnded += OnBattleEnded;
            PlayerController.Instance.PlayerUnit.ImpactEvent += OnImpact;

            battleManager.StartBattle(playerController, _playerPosition.position, _enemy, _enemyPosition.position);
        }
    }

    private void OnImpact(CameraShakeSetting setting)
    {
        _cameraShakeEffect?.ShakeOnHit(setting);
    }

    private void OnBattleEnded()
    {
        _camera.gameObject.SetActive(false);
        UnsubscribeEvents();
    }

    private void UnsubscribeEvents()
    {
        BattleManager.Instance.OnBattleEnded -= OnBattleEnded;
        PlayerController.Instance.PlayerUnit.ImpactEvent -= OnImpact;
        _enemy.EnemyUnit.ImpactEvent -= OnImpact;
    }
}
