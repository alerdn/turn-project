using System.Collections;
using System.Collections.Generic;
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

        BattleManager.Instance.OnBattleEnded += OnBattleEnded;
        _enemy.EnemyUnit.AnimationHelper.ImpactEvent += OnImpact;
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

            BattleManager.Instance.StartBattle(playerController, _playerPosition.position, _enemy, _enemyPosition.position);
        }
    }

    private void OnImpact()
    {
        _cameraShakeEffect?.ShakeOnHit();
    }

    private void OnBattleEnded()
    {
        _camera.gameObject.SetActive(false);
        UnsubscribeEvents();
    }

    private void UnsubscribeEvents()
    {
        BattleManager.Instance.OnBattleEnded -= OnBattleEnded;
        _enemy.EnemyUnit.AnimationHelper.ImpactEvent -= OnImpact;
    }
}
