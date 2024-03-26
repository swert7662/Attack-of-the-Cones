using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : NewEnemy 
{
    [SerializeField] private List<GameObject> _powerupDropList;
    [SerializeField] private SpriteRenderer _currentPowerupSprite;

    private GameObject _selectedPowerupPrefab;
    private float statMultiplier = 3.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        MaxHealth *= statMultiplier;
        CurrentHealth = MaxHealth;
        AttackDamage *= statMultiplier;

        _selectedPowerupPrefab = SelectRandomPowerup();
        UpdateSprite();
    }

    public void SetPowerupDrop(PowerupList.PowerUpCategory dropType)
    {
        foreach (GameObject powerupObject in _powerupDropList)
        {
            // Attempt to get the Collectible component on the powerupObject
            if (powerupObject.TryGetComponent<Collectible>(out Collectible powerupCollectible))
            {
                if (powerupCollectible.GetPowerupCategory() == dropType)
                {
                    _selectedPowerupPrefab = powerupObject;
                    UpdateSprite();
                    break;
                }
            }
        }
    }


    private GameObject SelectRandomPowerup()
    {
        if (_powerupDropList == null || _powerupDropList.Count == 0)
        {
            Debug.LogError("Powerup drop list is empty or not assigned.");
            return null;
        }

        int randomIndex = Random.Range(0, _powerupDropList.Count);
        return _powerupDropList[randomIndex];
    }

    private void UpdateSprite()
    {
        if (_selectedPowerupPrefab != null)
        {
            SpriteRenderer powerupSpriteRenderer = _selectedPowerupPrefab.GetComponent<SpriteRenderer>();
            if (powerupSpriteRenderer != null)
            {
                _currentPowerupSprite.sprite = powerupSpriteRenderer.sprite;
            }
            else
            {
                Debug.LogWarning("Selected powerup prefab does not have a SpriteRenderer component.");
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Elite enemy cannot be promoted further.");
    }

    public override void Die()
    {
        if (_selectedPowerupPrefab != null)
        {
            Instantiate(_selectedPowerupPrefab, transform.position, Quaternion.identity);
        }

        base.Die();        
    }
}
