using UnityEngine;
using Cinemachine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineBasicMultiChannelPerlin multiChannelPerlin;
    
    private bool isTiming;
    public static float previousTimeScale = 1f;
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
            Time.timeScale = 1;
        }
    }

    public void ShakeScreen(float amplitude, float length)
    {
        multiChannelPerlin.m_AmplitudeGain = amplitude;
        multiChannelPerlin.m_FrequencyGain = 20f;
        isTiming = true;
        shakeTime = Time.unscaledTime + length;
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0.75f;
    }
}
