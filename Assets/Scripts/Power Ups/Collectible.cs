using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private PowerUpType _powerUpType;

    public static event Action<PowerUpType> OnPowerupPickup;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PickUp();
        }
    }

    //// Method to handle the item being picked up and despawned immediately after
    private void PickUp()
    {
        AddWeaponPowerup();

        Despawn();
    }

    private void Despawn()
    {
        Destroy(this.gameObject);
        //ObjectPoolManager.DespawnObject(this.gameObject);
    }

    private void AddWeaponPowerup()
    {
        Debug.Log("Adding weapon powerup");
        OnPowerupPickup?.Invoke(_powerUpType);
    }

    public enum PowerUpType
    {
        None,
        ChainLightning,
        Fireball,
    }
}
