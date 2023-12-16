using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinkleParticleSystem : MonoBehaviour
{
    private ParticleSystem _sprinkles;

    void Start()
    {
        _sprinkles = GetComponent<ParticleSystem>();
    }
}
