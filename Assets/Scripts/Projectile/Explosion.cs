using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Explosion : MonoBehaviour
{
    private int Damage;
    private float Range;
    private LayerMask TargetLayerMask;

    public void Initialization(int damage, float range, LayerMask targetLayerMask)
    {
        Damage = damage;
        Range = range;
        TargetLayerMask = targetLayerMask;
    }

    private void Start()
    {
        Explode();
    }

    private void Explode()
    {
        List<GameObject> targets = UtilityMethods.FindAllTargetsWith<NewEnemy>(transform.position, Range, TargetLayerMask);
        foreach (GameObject target in targets)
        {
            IHealth targetHealth = target.GetComponent<IHealth>();
            if (targetHealth != null)
            {
                targetHealth.Damage(Damage, DamageType.Fire);
            }
        }
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the color of the Gizmo
        Gizmos.DrawWireSphere(transform.position, Range); // Draw a wireframe sphere with the specified range
    }
}
