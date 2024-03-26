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

        //ResetFollower(followers[index]);
        followers.RemoveAt(index);
        if (followers.Count > 0)
        {
            Collectible followerCollectible = followers[0].GetComponent<Collectible>();
            if (followerCollectible != null)
            {
                followerCollectible.followTarget = this.transform;
            }
        }
        /*
        for (int i = index; i < followers.Count; i++)
        {
            Collectible followerCollectible = followers[i].GetComponent<Collectible>();
            // The new follow target is the predecessor in the list, or this transform if it's now the first follower
            Transform newFollowTarget = i > 0 ? followers[i - 1] : this.transform;
            if (followerCollectible != null)
            {
                followerCollectible.followTarget = newFollowTarget;
            }
        }
        */
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
