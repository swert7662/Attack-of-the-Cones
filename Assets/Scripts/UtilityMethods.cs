using System.Collections.Generic;
using UnityEngine;

public static class UtilityMethods
{
    public static GameObject FindNextTargetWith<T>(Vector3 position, float range, LayerMask enemyLayer) where T : Component
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, range, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponent<T>() == null) continue;
            return hit.gameObject;
        }
        return null; // No valid target found
    }
    public static GameObject FindNextTargetWithout<T>(Vector3 position, float range, LayerMask enemyLayer) where T : Component
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, range, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponent<T>() != null) continue;
            return hit.gameObject;
        }
        return null; // No valid target found
    }

    public static List<GameObject> FindAllTargetsWith<T>(Vector3 position, float range, LayerMask enemyLayer) where T : Component
    {
        List<GameObject> targets = new List<GameObject>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, range, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            T component = hit.GetComponent<T>();
            if (component != null)
            {
                targets.Add(hit.gameObject);
            }
        }
        return targets;
    }

    public static List<GameObject> FindAllTargetsWithout<T>(Vector3 position, float range, LayerMask enemyLayer) where T : Component
    {
        List<GameObject> targets = new List<GameObject>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, range, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            T component = hit.GetComponent<T>();
            if (component == null)
            {
                targets.Add(hit.gameObject);
            }
        }
        return targets;
    }

    public static int GetSortingOrder(this Transform transform, float yOffset = 0)
    {
        return -(int)((transform.position.y + yOffset) * 100);
    }
}
