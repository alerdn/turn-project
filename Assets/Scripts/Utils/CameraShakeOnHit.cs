using System;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

[Serializable]
public class CameraShakeSetting
{
    public float AmplitudeGain;
    public float FrequencyGain;
    public float Duration;
}

public class CameraShakeOnHit : MonoBehaviour
{
    [SerializeField] private float _hitAmplitudeGain = 3f;
    [SerializeField] private float _hitFrequencyGain = 5f;
    [SerializeField] private float _hitDuration = .25f;

    private CinemachineBasicMultiChannelPerlin _cameraShake;
    private Task _shakingTask;
    private bool _isShaking;

    private void Start()
    {
        _cameraShake = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cameraShake.m_AmplitudeGain = 0;
        _cameraShake.m_FrequencyGain = 0;
        _shakingTask = null;
        _isShaking = false;
    }

    public void ShakeOnHit(CameraShakeSetting setting = null)
    {
        if (_isShaking)
        {
            _shakingTask.Dispose();
            _cameraShake.m_AmplitudeGain = 0f;
            _cameraShake.m_FrequencyGain = 0f;
            _isShaking = false;
        }

        if (setting != null)
        {
            _hitAmplitudeGain = setting.AmplitudeGain;
            _hitFrequencyGain = setting.FrequencyGain;
            _hitDuration = setting.Duration;
        }

        _shakingTask = Shake(_hitAmplitudeGain, _hitFrequencyGain, _hitDuration);
    }

    private async Task Shake(float amplitudeGain, float frequencyGain, float duration)
    {
        _isShaking = true;

        _cameraShake.m_AmplitudeGain = amplitudeGain;
        _cameraShake.m_FrequencyGain = frequencyGain;
        await Task.Delay(Mathf.RoundToInt(duration * 1000f));

        _cameraShake.m_AmplitudeGain = 0f;
        _cameraShake.m_FrequencyGain = 0f;

        _isShaking = false;
    }
}
