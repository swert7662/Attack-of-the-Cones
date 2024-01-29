using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextPopup : MonoBehaviour
{
    [SerializeField] private GameObject _damagePopupPrefab;

    private float popupOffsetDirection = 1;
    private int currentMaxSortingOrder = 0;

    private void OnEnable()
    {
        var damageTaker = GetComponentInParent<IHealth>(); // Assuming IHealth interface is used for all damageable entities
        if (damageTaker != null)
        {
            // Debug log saying how many times this has subscribed
            damageTaker.OnDamageTaken -= HandleDamagePopup;
            damageTaker.OnDamageTaken += HandleDamagePopup;
        }
    }

    private void OnDisable()
    {
        var damageTaker = GetComponentInParent<IHealth>(); // Assuming IHealth interface is used for all damageable entities
        if (damageTaker != null)
        {
            // Debug log saying how many times this has subscribed
            damageTaker.OnDamageTaken -= HandleDamagePopup;
        }
    }

    private void HandleDamagePopup(GameObject damagedObject, Vector2 extents, float damageTaken)
    {
        Vector3 spawnPoint = damagedObject.transform.position;

        
        if (damagedObject == this.gameObject && damageTaken > 0)
        {
            Debug.Log("DamagePopup: " + damagedObject.name + " took " + damageTaken + " damage");
            float randomOffset = UnityEngine.Random.Range(-0.5f, 0.5f);
            spawnPoint.y += extents.y; // Adjust for height            
            spawnPoint.x += extents.x * popupOffsetDirection + randomOffset;

            // Toggle the direction for the next popup
            popupOffsetDirection *= -1;

            GameObject go = ObjectPoolManager.SpawnObject(_damagePopupPrefab.gameObject, spawnPoint, Quaternion.identity);
            DamagePopupFX damagePopup = go.GetComponent<DamagePopupFX>();
            if (damagePopup != null)
            {
                damagePopup.SetDamageTextValue(damageTaken);

                TextMeshPro tmp = go.GetComponent<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.sortingOrder = ++currentMaxSortingOrder;
                }
            }
        }
        
    }
}
