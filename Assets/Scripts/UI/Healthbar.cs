using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private Health _health;

    public void UpdateHealthbar ()
    {
        _healthbarSprite.fillAmount = _health.CurrentHealth / _health.MaxHealth;
    }

    private void Update()
    {
       // This can be useful for something like damage over time, where you want the healthbar to update smoothly and not instantly
        // transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position); for 3D
       // _healthbarSprite.fillAmount = Mathf.Lerp(_healthbarSprite.fillAmount, _targetFillAmount, Time.deltaTime * 1f);
    }
}
