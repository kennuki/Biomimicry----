using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanNoise : MonoBehaviour
{
    private Renderer render;
    private void Start()
    {
        render = GetComponent<Renderer>();
        OriginStrenth = render.material.GetFloat("_Strength");
        OriginNoiseAmount = render.material.GetFloat("_NoiseAmount");
    }

    float OriginNoiseAmount;
    float OriginStrenth;
    float Strenth,_Strenth = 0;
    public float RunTime, DelayTime;
    float _RunTime, _DelayTime;
    float counter1=0,counter2=0;
    private void Update()
    {
        _RunTime = Mathf.Clamp(Random.Range(RunTime - 1, RunTime + 1),0,4);
        _DelayTime = Mathf.Clamp(Random.Range(DelayTime - 2, DelayTime + 2), 0, 10);
        Strenth = Random.Range(OriginStrenth + 0.5f, -OriginStrenth - 0.5f);
        counter1 += Time.deltaTime;
        if (counter1 > _DelayTime)
        {
            counter2 += Time.deltaTime;
            if (counter2 < _RunTime / 4)
            {
                _Strenth += (Time.deltaTime / _RunTime) * Strenth * 4;
            }
            else if (counter2 > _RunTime / 4 * 3 && counter2 < _RunTime)
            {
                _Strenth -= (Time.deltaTime / _RunTime) * Strenth * 4;
            }
            else
            {
                _Strenth = 0;
                counter2 = 0;
                counter1 = 0;
            }
            render.material.SetFloat("_NoiseAmount", Random.Range(OriginNoiseAmount - 5, OriginNoiseAmount + 35));
            render.material.SetFloat("_Strength", _Strenth);
        }



    }
}
