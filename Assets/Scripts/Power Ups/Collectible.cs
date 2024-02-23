using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private PowerUpType _powerUpType;
    [SerializeField] private Player _player;
    [SerializeField] private Transform _truck;
    [SerializeField] private GameEvent _powerupCollectedEvent;
    [SerializeField] private GameEvent _powerupSuctionEvent;
    [SerializeField] private PowerupList _powerupList;

    public Transform followTarget;  
    public float speed = 5f;
    public float stoppingDistance = 5f;

    public static event Action<PowerUpType> OnPowerupPickup;

    private void Update()
    {
        if (followTarget == null) return;

        float distance = Vector3.Distance(transform.position, followTarget.position);
        if (distance > stoppingDistance)
        {
            Vector3 direction = (followTarget.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && followTarget == null)
        {
            var manager = collider.GetComponent<PowerupFollowers>();
            if (manager != null)
            {
                followTarget = manager.AddFollower(this);
            }
        }
        else if (collider.gameObject.CompareTag("Truck"))
        {
            _powerupCollectedEvent.Raise(this, _powerupList.Category.ToString());
            _powerupSuctionEvent.Raise();
            PickUp();
        }
    }

    public void SetFollowTruck()
    {
        if (followTarget != null)
        {
            followTarget = _truck;
        }
    }

    //// Method to handle the item being picked up and despawned immediately after
    private void PickUp()
    {
        //AddWeaponPowerup();

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
        Damage,
        FireRate,
        Health,
        Speed,
        Suction,
        ExpUp,
        ChainLightning,
        Fireball,
    }
}
