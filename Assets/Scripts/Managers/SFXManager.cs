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
        //ChainLightningSpawner.OnLightningStrike += HandleLightningStrike;
        //SprinkleCollector.OnSprinklePickup += HandleSprinklePickup;
    }

    private void OnDisable()
    {
        Projectile.OnProjectileImpact -= HandleProjectileImpact;
        Projectile.OnProjectileShoot -= HandleProjectileShot;
        //ChainLightningSpawner.OnLightningStrike -= HandleLightningStrike;
        //SprinkleCollector.OnSprinklePickup -= HandleSprinklePickup;
    }
    private void HandleProjectileShot()
    {
        PlaySound(_shootSFX, true);
    }

    private void HandleProjectileImpact(Vector2 hitPoint, Vector2 hitNormal)
    {
        PlaySound(_impactSFX, true);
    }

    public void HandleLightningStrike()
    {
        PlaySound(_lightningStrikeSFX, true);
    }
    public void HandleSprinklePickup()
    {
        PlaySound(_sprinkleSFX, false);
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
