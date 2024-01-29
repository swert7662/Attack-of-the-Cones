using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true), SerializeField] private Color _color;
    [SerializeField] private float _flashDuration;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    private bool _isFlashing;
    private float _flashTimer;

    private int _flashAmount = Shader.PropertyToID("_FlashAmount");
    private int _flashColor = Shader.PropertyToID("_DamageColor");
    private void OnEnable()
    {
        // Subscribe to the OnDamageTaken event
        var damageTaker = GetComponentInParent<IHealth>(); // Assuming IHealth interface is used for all damageable entities
        if (damageTaker != null)
        {
            damageTaker.OnDamageTaken += HandleDamageTaken;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        var damageTaker = GetComponentInParent<IHealth>();
        if (damageTaker != null)
        {
            damageTaker.OnDamageTaken -= HandleDamageTaken;
        }
    }


    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }
    private void Update()
    {
        if (_isFlashing)
        {
            _flashTimer += Time.deltaTime;

            float flashLerp = Mathf.Lerp(1f, 0f, _flashTimer / _flashDuration);

            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i].SetFloat(_flashAmount, flashLerp);
            }
            // this checks if the flash is done and if so, it stops the flashing
            if (_flashTimer >= _flashDuration)
            {
                _isFlashing = false;
            }           
        }
    }
    private void HandleDamageTaken(GameObject damagedObject, Vector2 extentsUNUSED, float damageTaken)
    {
        if (damagedObject == this.gameObject)
        {
            Flash();
        }
    }

    public void Flash()
    {
        _isFlashing = true;
        _flashTimer = 0f;

        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat(_flashAmount, 1f);
            _materials[i].SetColor(_flashColor, _color);
        }
    }
}
