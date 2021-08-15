using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    public Material mat;
    public float wavesDuration = 2;
    private float wavesTime;
    private bool wavesState;
    private float factor;
    public float percent = 0.05f;
    private static readonly int Attenuation = Shader.PropertyToID("_Attenuation");

    private void Start()
    {
        wavesState = false;
        factor = 0f;
        mat.SetFloat(Attenuation, factor);
    }

    void Update()
    {
        if (wavesState)
        {
            AnimationWavesShader();
        }
    }

    public void StartWaves()
    {
        wavesState = true;
        factor = 0.05f;
        wavesTime = wavesDuration;
        mat.SetFloat(Attenuation, factor);
    }

    private void AnimationWavesShader()
    {
        wavesTime -= Time.deltaTime;
        if (wavesTime <= 0)
        {
            factor = 0;
            wavesState = false;
        }
        else if (wavesTime > wavesDuration / 2f)
        {
            if (factor < 1.2f)
                factor *= 1 + percent;
        } 
        else if (wavesTime < wavesDuration / 2f)
        {
            if (factor > 0)
                factor *= 1 - percent;
        }
        
        
        mat.SetFloat(Attenuation, factor);
    }
}
