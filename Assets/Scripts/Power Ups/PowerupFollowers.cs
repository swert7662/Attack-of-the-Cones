using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupFollowers : MonoBehaviour
{
    private Queue<Transform> followers = new Queue<Transform>();
    private Transform lastFollower;

    void Start()
    {
        // Initialize lastFollower to the player's transform at the start
        ResetFollowers();
    }

    public Transform AddFollower(Collectible newFollower)
    {
        // Add the new follower to the queue
        followers.Enqueue(newFollower.transform);

        // If it's the first follower, it follows the player directly.
        // For subsequent followers, they follow the last follower in the queue.
        Transform followTarget = lastFollower;

        // Update lastFollower to be the newFollower's transform
        lastFollower = newFollower.transform;

        // Return the followTarget for the new follower to follow
        return followTarget;
    }

    private void RemoveAllFollowers()
    {
        followers.Clear();
    }

    public void ResetFollowers()
    {
        RemoveAllFollowers();
        lastFollower = transform;
    }
}
