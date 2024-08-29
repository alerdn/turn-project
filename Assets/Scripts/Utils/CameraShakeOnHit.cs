using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public class CameraShakeOnHit : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private float _hitAmplitudeGain = .85f;
    [SerializeField] private float _hitFrequencyGain = 2f;
    [SerializeField] private float _hitDuration = .15f;

    private CinemachineBasicMultiChannelPerlin _cameraShake;
    private bool _isShaking;

    private void Start()
    {
        _cameraShake = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cameraShake.m_AmplitudeGain = 0;
        _cameraShake.m_FrequencyGain = 0;
        _isShaking = false;
    }

    public void ShakeOnHit()
    {
        Shake(_hitAmplitudeGain, _hitFrequencyGain, _hitDuration);
    }

    private async void Shake(float amplitudeGain, float frequencyGain, float duration)
    {
        if (_isShaking) return;
        _isShaking = true;

        _cameraShake.m_AmplitudeGain = amplitudeGain;
        _cameraShake.m_FrequencyGain = frequencyGain;
        await Task.Delay(Mathf.RoundToInt(duration * 1000f));

        _cameraShake.m_AmplitudeGain = 0f;
        _cameraShake.m_FrequencyGain = 0f;

        _isShaking = false;
    }
}
