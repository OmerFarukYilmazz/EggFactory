using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour
{
    private ParticleSystem pSystem;   
    public void Awake()
    {
        pSystem = gameObject.GetComponent<ParticleSystem>();

    }

    public void Play()
    {
        pSystem.Simulate(Time.unscaledDeltaTime, true, true);        
    }

    public void Update()
    {
        pSystem.Simulate(Time.unscaledDeltaTime, true, false);
    }
}
