using UnityEngine;
using Cinemachine;

public class CameraEffects : MonoBehaviour
{
    public static float previousTimeScale = 1f;
    
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineBasicMultiChannelPerlin multiChannelPerlin;
    private bool isTiming;
    private float shakeTime = 0;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        multiChannelPerlin = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (isTiming && Time.unscaledTime > shakeTime)
        {
            multiChannelPerlin.m_AmplitudeGain = 0;
            multiChannelPerlin.m_FrequencyGain = 0;
            isTiming = false;
            shakeTime = 0;
        }
    }

    public void ShakeScreen(float amplitude, float length)
    {
        multiChannelPerlin.m_AmplitudeGain = amplitude;
        multiChannelPerlin.m_FrequencyGain = 20f;
        isTiming = true;
        shakeTime = Time.unscaledTime + length;
    }
}
