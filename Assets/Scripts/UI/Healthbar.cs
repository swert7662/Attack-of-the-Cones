using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;

    //private float  _targetFillAmount;

    public void UpdateHealthbar (float maxHealth, float currentHealth)
    {
        //_targetFillAmount = currentHealth / maxHealth;
        _healthbarSprite.fillAmount = currentHealth / maxHealth;
    }

    private void Update()
    {
       // This can be useful for something like damage over time, where you want the healthbar to update smoothly and not instantly
        // transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position); for 3D
       // _healthbarSprite.fillAmount = Mathf.Lerp(_healthbarSprite.fillAmount, _targetFillAmount, Time.deltaTime * 1f);
    }
}
