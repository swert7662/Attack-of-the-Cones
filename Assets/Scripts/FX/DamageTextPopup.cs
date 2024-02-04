using TMPro;
using UnityEngine;

public class DamageTextPopup : MonoBehaviour
{
    [SerializeField] private GameObject _damagePopupPrefab;

    private float popupOffsetDirection = 1;
    private int currentMaxSortingOrder = 0;

    public void HandleDamagePopup(Component sender, object data)
    {
        if (data is DamagedData)
        {
            Vector3 extents = ((DamagedData)data).Extents;
            float damageTaken = ((DamagedData)data).DamageAmount;
            GameObject damagedObject = ((DamagedData)data).GameObjectSender;

            Vector3 spawnPoint = damagedObject.transform.position;

            if (damagedObject == this.gameObject && damageTaken > 0)
            {
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
        else {Debug.LogWarning("Invalid data type for HandleDamagePopup : " + data.ToString());}
    }
}
