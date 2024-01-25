using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerupManager : MonoBehaviour
{
    [SerializeField] private GameObject _chainLightning;
    [SerializeField] bool _isChainLightningActive = true;

    private void OnEnable()
    {
        Projectile.OnAdditionalEffectsTrigger += HandleAdditionalEffectsTrigger;
    }

    private void OnDisable()
    {
        Projectile.OnAdditionalEffectsTrigger -= HandleAdditionalEffectsTrigger;
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
        if (GameManager.Instance.IsCooldownElapsed("ElectricSpawner", 1f)) // 1f is the cooldown duration in seconds
        {
            GameObject lightningGO = ObjectPoolManager.SpawnObject(_chainLightning, position, Quaternion.identity, ObjectPoolManager.PoolType.Projectile);
            lightningGO.GetComponent<ElectricSpawner>().StartChainAttack(target);
        }
    }
}
