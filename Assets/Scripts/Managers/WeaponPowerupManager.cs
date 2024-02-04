using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerupManager : MonoBehaviour
{
    [SerializeField] private GameObject _chainLightning;
    [SerializeField] private Projectile _fireball;

    private GameObject player;
    private PlayerController playerController;

    private bool _isChainLightningActive = false;

    private void Start()
    {
        //player = GameManager.Instance._playerGameObject;
        player = GameObject.FindGameObjectWithTag("Player"); // <--- replace with SO?
        if (player != null) { playerController = player.GetComponent<PlayerController>(); }
        else { Debug.LogWarning("Player not found by WeaponPowerupManager"); }
    }

    private void OnEnable()
    {
        Projectile.OnAdditionalEffectsTrigger += HandleAdditionalEffectsTrigger;
        Collectible.OnPowerupPickup += HandlePowerupPickup;
    }

    private void OnDisable()
    {
        Projectile.OnAdditionalEffectsTrigger -= HandleAdditionalEffectsTrigger;
        Collectible.OnPowerupPickup -= HandlePowerupPickup;
    }

    private void HandlePowerupPickup(Collectible.PowerUpType powerUp)
    {
        Debug.Log("A " + powerUp + " powerup picked up");

        switch (powerUp)
        {
            case Collectible.PowerUpType.ChainLightning:
                HandleChainLightningPickup();
                break;
            case Collectible.PowerUpType.Fireball:
                HandleFireballPickup();
                break;
            default:
                Debug.LogError("Powerup type not handled");
                break;
        }
    }

    private void HandleChainLightningPickup()
    {
        if(!_isChainLightningActive)
        {
            _isChainLightningActive = true;
        }
    }
    private void HandleFireballPickup()
    {
        playerController.ChangeWeapon(_fireball);
    }

    private void HandleAdditionalEffectsTrigger(GameObject target, Vector2 position)
    {
        if (_isChainLightningActive)
        {
            ChainLightning(target, position);
        }
    }

    private void ChainLightning(GameObject target, Vector2 position)
    {
        if (GameManager.Instance.IsCooldownElapsed("ElectricSpawner", 2f)) // 1f is the cooldown duration in seconds
        {
            GameObject lightningGO = ObjectPoolManager.SpawnObject(_chainLightning, position, Quaternion.identity, ObjectPoolManager.PoolType.Projectile);
            lightningGO.GetComponent<ChainLightningSpawner>().StartChainAttack(target);
        }
    }
}
