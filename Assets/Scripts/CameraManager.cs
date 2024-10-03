using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [field: SerializeField] public float FallSpeedDampingChangeThreshold { get; private set; } = -15f;
    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

    [Header("Controls for lerping the Y Damping during player jump/fall")]
    [SerializeField] private float _fallPanAmount = .25f;
    [SerializeField] private float _fallPanTime = .35f;

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;
    private float _normYPanAmount;
    private Coroutine _lerpYPanRoutine;

    protected override void Awake()
    {
        base.Awake();

        foreach (var camera in _allVirtualCameras)
        {
            if (camera.enabled)
            {
                _currentCamera = camera;
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        _normYPanAmount = _framingTransposer.m_YDamping;
    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanRoutine = StartCoroutine(LerpYDampingRoutine(isPlayerFalling));
    }

    private IEnumerator LerpYDampingRoutine(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        // Grab the starting dampoing amount
        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampingAmount = 0f;

        // Determina the end damping amount
        if (isPlayerFalling)
        {
            endDampingAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampingAmount = _normYPanAmount;
        }

        // Lerp the pan amount
        float elapsedTime = 0f;
        while (elapsedTime < _fallPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampingAmount, elapsedTime / _fallPanTime);
            _framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }

        IsLerpingYDamping = false;
    }
}
