using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private PlayerStats _player;
    [SerializeField] private Transform _truck;
    [SerializeField] private GameEvent _powerupCollectedEvent;
    [SerializeField] private GameEvent _powerupSuctionEvent;
    [SerializeField] private PowerupList _powerupList;

    private CollectibleFollowers CollectibleFollowers;

    public Transform followTarget;  
    public float speed = 5f;
    public float stoppingDistance = 5f;

    private CollectibleState currentState = CollectibleState.Idle;

    public enum CollectibleState
    {
        Idle,
        FollowPlayer
    }

    private void Update()
    {
        switch (currentState)
        {
            case CollectibleState.Idle:
                Debug.Log("Collectible is idle");
                break;
            case CollectibleState.FollowPlayer:
                Debug.Log("Collectible is following player");
                FollowPlayer();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && followTarget == null)
        {
            CollectibleFollowers = collider.GetComponent<CollectibleFollowers>();
            if (CollectibleFollowers != null)
            {
                followTarget = CollectibleFollowers.AddFollower(this);
            }
            currentState = CollectibleState.FollowPlayer;
        }
        else if (collider.gameObject.CompareTag("Truck") && currentState == CollectibleState.FollowPlayer)
        {
            PickUp();
        }
    }
    private void FollowPlayer()
    {
        if (followTarget == null || !followTarget.gameObject.activeInHierarchy)
        {
            if (CollectibleFollowers != null)
            {
                int index = CollectibleFollowers.GetFollowerIndex(this.transform);
                CollectibleFollowers.DropFollowersAtIndex(Mathf.Max(0, index - 1));
            }

            // Reset this collectible's state to Idle
            ResetToIdle();
            return;
        }

        float distance = Vector3.Distance(transform.position, followTarget.position);
        if (distance > stoppingDistance)
        {
            Vector3 direction = (followTarget.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
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
        _powerupCollectedEvent.Raise(this, _powerupList.Category.ToString());
        // Assuming this transform is the follower to be removed
        int index = CollectibleFollowers.GetFollowerIndex(this.transform);
        // Directly remove this follower without adjusting the index
        CollectibleFollowers.RemoveFollowerAtIndex(index);
        Despawn();
    }


    public PowerupList.PowerUpCategory GetPowerupCategory()
    {
        return _powerupList.Category;
    }

    private void Despawn()
    {
        Destroy(this.gameObject);
        //ObjectPoolManager.DespawnObject(this.gameObject);
    }

    public void ResetToIdle()
    {
        currentState = CollectibleState.Idle;
        followTarget = null;
    }
}
