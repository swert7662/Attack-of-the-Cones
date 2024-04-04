using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private GameEvent _powerupCollectedEvent;
    [SerializeField] private PowerupList _powerupList;

    private CollectibleFollowers collectibleFollowers;

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
                break;
            case CollectibleState.FollowPlayer:
                FollowPlayer();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && followTarget == null)
        {
            collectibleFollowers = collider.GetComponent<CollectibleFollowers>();
            if (collectibleFollowers != null)
            {
                followTarget = collectibleFollowers.AddFollower(this);
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
            if (collectibleFollowers != null)
            {
                int index = collectibleFollowers.GetFollowerIndex(this.transform);
                collectibleFollowers.DropFollowersAtIndex(Mathf.Max(0, index - 1));
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

    //// Method to handle the item being picked up and despawned immediately after
    private void PickUp()
    {
        _powerupCollectedEvent.Raise(this, _powerupList.Category.ToString());
        Despawn(false);
    }

    public void Despawn(bool breakChain)
    {
        if (collectibleFollowers != null &&  currentState == CollectibleState.FollowPlayer)
        {
            if (breakChain == true)
            {
                collectibleFollowers.DropFollowersAtIndex(collectibleFollowers.GetFollowerIndex(this.transform));
            }
            else
            {
                collectibleFollowers.RemoveFollowerAtIndex(collectibleFollowers.GetFollowerIndex(this.transform));
            }
        }
        //Destroy(this.gameObject);
        StartCoroutine(DestroyNextFrame());
    }

    public void ResetToIdle()
    {
        currentState = CollectibleState.Idle;
        followTarget = null;
    }

    public PowerupList.PowerUpCategory GetPowerupCategory()
    {
        return _powerupList.Category;
    }

    IEnumerator DestroyNextFrame()
    {
        // Optionally perform any pre-destruction cleanup here
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
}
