using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LightningStruck : MonoBehaviour
{
    private int damage; 
    private int chainAmount; 
    private float arcRange;
    private float delay; 
    private float timer; 

    private GameEvent _lightningDamageEvent;
    private GameEvent _lightningArcEvent;

    private IHealth damageable;
    private LayerMask LayerMask;

    private void Start()
    {        
        damageable = GetComponent<IHealth>();
        if (damageable == null)
        {
            Debug.LogWarning("Damageable component not found for LightningStruck: " + this.ToString());
            Destroy(this);
        }

        Debug.Log("LightningStruck LayerMask: " + LayerMask);
        LightningStrike();
    }

    public void Initialize(int damage, int chainAmount, float chainRange, float delay, GameEvent lightningDamageEvent, GameEvent lightningArcEvent, LayerMask layerMask)
    {
        this.damage = damage;
        this.delay = delay;
        this.chainAmount = chainAmount;
        this.arcRange = chainRange;
        LayerMask = layerMask;
        SetLightningDamageEvent(lightningDamageEvent);
        SetLightningArcEvent(lightningArcEvent);
    }

    private void LightningStrike()
    {
        ApplyDamage();
        ChainEffect();
        Destroy(this);
    }
    private void ChainEffect()
    {
        if (chainAmount > 0)
        {
            GameObject nextTarget = UtilityMethods.FindNextTargetWithout<LightningStruck>(transform.position, arcRange, LayerMask);
            if (nextTarget != null) 
            {
                Debug.Log("LightningStruck found next target: " + nextTarget.name);
                LightningStruck lightningStruck = nextTarget.AddComponent<LightningStruck>();
                lightningStruck.Initialize(damage, chainAmount - 1, arcRange, delay, _lightningDamageEvent, _lightningArcEvent, LayerMask);

                if (_lightningArcEvent != null)
                {
                    //Debug log to show current position of this game object
                    Debug.Log("LightningStruck current position: " + gameObject.transform.position);
                    LightningDamageData lightningDamageData = new LightningDamageData(gameObject.transform.position, nextTarget.transform.position, false);
                    _lightningArcEvent.Raise(this, lightningDamageData);
                }
            }
        }
    }

    private void ApplyDamage()
    {
        damageable.Damage(damage, DamageType.Lightning);
        if (_lightningDamageEvent != null)
        {
            _lightningDamageEvent.Raise(this, transform.position);
        }
    }

    public void SetLightningDamageEvent(GameEvent lightningDamageEvent)
    {
        _lightningDamageEvent = lightningDamageEvent;
    }
    public void SetLightningArcEvent(GameEvent lightningArcEvent)
    {
        _lightningArcEvent = lightningArcEvent;
    }

    private void OnDisable()
    {
        Destroy(this);
    }
}
