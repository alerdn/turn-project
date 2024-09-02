using System;
using System.Collections;
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
    private Coroutine _shakeRoutine;

    private void Start()
    {
        _cameraShake = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cameraShake.m_AmplitudeGain = 0;
        _cameraShake.m_FrequencyGain = 0;
        _shakeRoutine = null;
    }

    public void ShakeOnHit(CameraShakeSetting setting = null)
    {
        if (setting != null)
        {
            _hitAmplitudeGain = setting.AmplitudeGain;
            _hitFrequencyGain = setting.FrequencyGain;
            _hitDuration = setting.Duration;
        }

        if (_shakeRoutine != null)
        {
            StopCoroutine(_shakeRoutine);
        }

        _shakeRoutine = StartCoroutine(Shake(_hitAmplitudeGain, _hitFrequencyGain, _hitDuration));
    }

    private IEnumerator Shake(float amplitudeGain, float frequencyGain, float duration)
    {
        _cameraShake.m_AmplitudeGain = amplitudeGain;
        _cameraShake.m_FrequencyGain = frequencyGain;

        yield return new WaitForSeconds(duration);

        _cameraShake.m_AmplitudeGain = 0f;
        _cameraShake.m_FrequencyGain = 0f;

        _shakeRoutine = null;
    }
}
