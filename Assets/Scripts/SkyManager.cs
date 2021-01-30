using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
    public Material materialSky;
    private Color prevSkyColor;
    private Color nextSkyColor;
    private float prevAtmo;
    private float nextAtmo;
    private Vector3 prevSunDir;
    private Vector3 nextSunDir;
    private float prevSunSize;
    private float nextSunSize;
    private float timeLerp = 0;
    private float durationLerpLong = 20;
    private float durationLerpShort = 5;
    private bool transition = false;
    // Start is called before the first frame update
    void Start()
    {
        prevSkyColor = new Color(Random.value, Random.value, Random.value);
        nextSkyColor = new Color(Random.value, Random.value, Random.value);
        prevSunDir = new Vector3(0, 0.5f, 0.8f);
        nextSunDir = prevSunDir;
    }

    // Update is called once per frame
    void Update()
    {
        timeLerp -= Time.deltaTime;
        if (timeLerp <= 0)
        {
            transition = !transition;

            if (transition)
            {
                timeLerp = durationLerpShort;
                prevSkyColor = nextSkyColor;
                nextSkyColor = new Color(Random.value, Random.value, Random.value);
                prevAtmo = nextAtmo;
                nextAtmo = Random.value;
                prevSunDir = nextSunDir;
                //nextSunDir = Quaternion.AngleAxis(Random.value * 180, new Vector3(0,1,0)) * prevSunDir;
                nextSunDir = new Vector3(Random.value*2-1, (Random.value*2-1)*0.2f, Random.value * 2 - 1);
                prevSunSize = nextSunSize;
                nextSunSize = Mathf.Min(0.8f, Mathf.Max(0.3f, Random.value));
            }
            else
            {
                timeLerp = durationLerpLong;
            }
            
        }

        if (transition)
        {
            float lerpVal = 1 - Mathf.Clamp01(timeLerp / durationLerpShort);
            materialSky.SetColor("_ColorDay", Color.Lerp(prevSkyColor, nextSkyColor, lerpVal));
            materialSky.SetFloat("_DayLerp", Mathf.Lerp(prevAtmo, nextAtmo, lerpVal));
            materialSky.SetVector("_SunDir", Vector3.Lerp(prevSunDir, nextSunDir, lerpVal));
            materialSky.SetFloat("_SunSize", Mathf.Lerp(prevSunSize, nextSunSize, lerpVal));
        }
        
    }
}
