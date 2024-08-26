using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private EnemyController _enemy;
    [SerializeField] private Transform _enemyPosition;

    private bool _isBattleStarted;

    private void Start()
    {
        _isBattleStarted = false;
        _camera.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isBattleStarted && other.TryGetComponent(out PlayerController playerController))
        {
            _isBattleStarted = true;
            _camera.SetActive(true);

            BattleManager.Instance.OnBattleEnded += OnBattleEnded;
            BattleManager.Instance.StartBattle(playerController, _playerPosition.position, _enemy, _enemyPosition.position);
        }
    }

    private void OnBattleEnded()
    {
        _camera.SetActive(false);
    }
}
