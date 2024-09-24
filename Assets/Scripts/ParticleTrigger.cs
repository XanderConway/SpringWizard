using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem system1;
    public ParticleSystem system2;
    public ParticleSystem system3;
    public ParticleSystem system4;

    void Start()
    {
        system1.Clear();
        system2.Clear();
        system3.Clear();
        system4.Clear();
    }

}
