using System.Collections.Generic;
using UnityEngine;

public class CollectibleFollowers : MonoBehaviour
{
    private List<Transform> followers = new List<Transform>();

    public Transform AddFollower(Collectible newFollower)
    {
        Transform followTarget = followers.Count == 0 ? this.transform : followers[followers.Count - 1];

        followers.Add(newFollower.transform);

        return followTarget;
    }

    public void RemoveFollowerAtIndex(int index)
    {
        Debug.Log("Removing follower at index: " + index);
        if (index < 0 || index >= followers.Count)
        {
            Debug.LogError("Index out of bounds for removing a follower.");
            return;
        }

        Debug.Log("Removed follower: " + followers[index]);
        followers.RemoveAt(index);

        SetFollowerTarget(index);
    }

    public void DropFollowersAtIndex(int index)
    {
        Debug.Log("Dropping followers at index: " + index);
        if (index == 0)
        {
            ResetFollowers();
            return;
        }

        for (int i = index; i < followers.Count; i++)
        {
            ResetFollower(followers[i]);
        }

        int countToRemove = followers.Count - index;
        followers.RemoveRange(index, countToRemove);
    }

    public int GetFollowerIndex(Transform follower)
    {
        return followers.IndexOf(follower);
    }

    private void SetFollowerTarget(int index)
    {
        if (index == 0 && followers.Count > 0)
        {
            Collectible firstFollowerCollectible = followers[0].GetComponent<Collectible>();
            if (firstFollowerCollectible != null)
            {
                firstFollowerCollectible.followTarget = this.transform;
            }
        }
        // If a follower was removed from somewhere else in the list (not the first one),
        // update the followTarget of the next follower in the list (now at the index of the removed follower)
        else if (index < followers.Count) // Check if there's a follower after the removed one
        {
            // The follower that was following the removed follower should now follow
            // the predecessor of the removed follower.
            // Since we've already removed the follower at 'index', the new follower at 'index'
            // (previously at index + 1) should now follow the one at index - 1.
            Collectible nextFollowerCollectible = followers[index].GetComponent<Collectible>();
            if (nextFollowerCollectible != null)
            {
                // Set its new followTarget to the follower that was ahead of the removed follower
                Transform newFollowTarget = (index - 1 >= 0) ? followers[index - 1] : this.transform;
                nextFollowerCollectible.followTarget = newFollowTarget;
            }
        }
    }

    public void ResetFollowers()
    {
        followers.RemoveAll(item => item == null); // Clean up nulls first
        foreach (var follower in followers)
        {
            ResetFollower(follower);
        }
        followers.Clear();
    }


    private static void ResetFollower(Transform follower)
    {
        if (follower == null)
        {
            return;
        }

        Collectible collectible = follower.GetComponent<Collectible>();
        if (collectible != null)
        {
            collectible.ResetToIdle();
        }
    }
}
