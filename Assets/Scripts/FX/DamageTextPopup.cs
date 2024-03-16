using UnityEngine;
using TMPro; // Ensure you have this using directive for TextMeshPro

public class DamageTextPopup : MonoBehaviour
{
    [SerializeField] private GameObject _normalDamagePopupPrefab;
    [SerializeField] private GameObject _fireDamagePopupPrefab;
    [SerializeField] private GameObject _lightningDamagePopupPrefab;

    private float popupOffsetDirection = 1;
    private int currentMaxSortingOrder = 0;

    public void HandleDamagePopup(Component sender, object data)
    {
        if (data is DamagedData damagedData)
        {
            Vector3 extents = damagedData.Extents;
            float damageTaken = damagedData.DamageAmount;
            GameObject damagedObject = damagedData.GameObjectSender;
            DamageType damageType = damagedData.DamageType;

            Vector3 spawnPoint = damagedObject.transform.position;

            if (damagedObject == this.gameObject && damageTaken > 0)
            {
                float randomOffset = UnityEngine.Random.Range(-0.5f, 0.5f);
                spawnPoint.y += extents.y; // Adjust for height            
                spawnPoint.x += extents.x * popupOffsetDirection + randomOffset;

                // Toggle the direction for the next popup
                popupOffsetDirection *= -1;

                // Determine which prefab to use based on the damage type
                GameObject damageTextPrefab = _normalDamagePopupPrefab; // Default to normal
                switch (damageType)
                {
                    case DamageType.Fire:
                        damageTextPrefab = _fireDamagePopupPrefab;
                        break;
                    case DamageType.Lightning:
                        damageTextPrefab = _lightningDamagePopupPrefab;
                        break;
                        // Add other cases as needed
                }

                // Spawn the selected prefab
                DamagePopupFX damagePopup = ObjectPoolManager.SpawnObject<DamagePopupFX>(damageTextPrefab, spawnPoint, Quaternion.identity);
                if (damagePopup != null)
                {
                    damagePopup.SetDamageTextValue(damageTaken);

                    TextMeshPro tmp = damagePopup.gameObject.GetComponent<TextMeshPro>();
                    if (tmp != null)
                    {
                        tmp.sortingOrder = ++currentMaxSortingOrder;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Invalid data type for HandleDamagePopup: " + data.ToString());
        }
    }
}
