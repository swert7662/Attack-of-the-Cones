using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip _shootSFX;
    [SerializeField] private AudioClip _impactSFX;  
    [SerializeField] private AudioClip _lightningStrikeSFX;
    [SerializeField] private AudioClip _sprinkleSFX;


    // Start is called before the first frame update
    private void OnEnable()
    {
        Projectile.OnProjectileShoot += HandleProjectileShot;
        Projectile.OnProjectileImpact += HandleProjectileImpact;            
        ElectricSpawner.OnLightningStrike += HandleLightningStrike;
        SprinkleCollector.OnSprinklePickup += HandleSprinklePickup;
    }

    private void OnDisable()
    {
        Projectile.OnProjectileImpact -= HandleProjectileImpact;
        Projectile.OnProjectileShoot -= HandleProjectileShot;
        ElectricSpawner.OnLightningStrike -= HandleLightningStrike;
        SprinkleCollector.OnSprinklePickup -= HandleSprinklePickup;
    }
    private void HandleProjectileShot()
    {
        PlaySound(_shootSFX, true);
    }

    private void HandleProjectileImpact(Vector2 hitPoint, Vector2 hitNormal)
    {
        PlaySound(_impactSFX, true);
    }

    private void HandleLightningStrike(Vector2 vector)
    {
        PlaySound(_lightningStrikeSFX, true);
    }
    private void HandleSprinklePickup(int pickupCount)
    {
        pickupCount = Mathf.Min(pickupCount, 5); // Cap at 5 plays

        for (int i = 0; i < pickupCount; i++)
        {
            PlaySound(_sprinkleSFX, false);
        }
    }

    private void PlaySound(AudioClip clip, bool randomOn)
    {
        float volume = .8f;
        if (randomOn)
        {
            float pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            volume = UnityEngine.Random.Range(0.6f, .8f);
            AudioManager.Instance.PlaySound(clip, volume, pitch);
        }
        else
        {
            AudioManager.Instance.PlaySoundNoPitch(clip, volume);
        }
    }
}
