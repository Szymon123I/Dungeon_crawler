using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    // Start is called before the first frame update
    public static CinemachineShake Instance {get; private set;}
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin multiChannelPerlin;
    private float shakeTimer;
    void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        multiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time, float frequency = 5){
        multiChannelPerlin.m_AmplitudeGain = intensity;
        multiChannelPerlin.m_FrequencyGain = frequency;
        shakeTimer = time;
        
    }
    void Update(){
        if (shakeTimer > 0){
            shakeTimer-=Time.deltaTime;
            if (shakeTimer <= 0){
                multiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }
}
