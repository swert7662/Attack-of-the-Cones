using Com.LuisPedroFonseca.ProCamera2D.TopDownShooter;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Effect", menuName = "Power Ups/HealthEffect", order = 2)]
public class HealthEffect : PowerUpEffect
{
    [SerializeField] Health Health;
    [SerializeField] private float _healAmount;
    [SerializeField] private GameEvent OnHealthEvent;

    public override void Apply()
    {
        Health.CurrentHealth += _healAmount;
        OnHealthEvent.Raise();

        base.Apply();
    }
}