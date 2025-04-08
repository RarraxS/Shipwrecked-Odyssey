using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsModel : MonoBehaviour
{
    [SerializeField] private GameObject rainParticleEmmiter;
    private Light2D sunlight;

    [SerializeField] private List<DayState> sunnyDayStates, 
        rainyDayStates;
    [SerializeField] private float rainingProbability;


    private bool raining = false;
    private float timeTimer;
    private int indexDayState = 0;
}

[Serializable]
public class DayState
{
    public byte hour;

    public Color ilumination;
}