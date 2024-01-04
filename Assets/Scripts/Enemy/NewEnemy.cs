using UnityEngine;

public class NewEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyStats _enemyStats;

    [SerializeField] private ParticleSystem _deathSprinkles;
    [SerializeField] private Healthbar _healthbar;

    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    private Transform _target;
    private Animator _animator;
    private DamageFlash _damageFlash;
    private Vector3 _originalScale;

    private void Awake()
    {
        _target = GameManager.Instance._playerTransform;
        _animator = GetComponentInChildren<Animator>();
        _damageFlash = GetComponentInChildren<DamageFlash>();
        _originalScale = _animator.transform.localScale;
        MaxHealth = _enemyStats.maxHealth;
        CurrentHealth = _enemyStats.maxHealth;
    }

    private void Update()
    {
        FlipTowardsTarget();

        if (Vector3.Distance(transform.position, _target.position) > _enemyStats.attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _enemyStats.speed * Time.deltaTime);
        }
    }

    #region Health Functions

    public void Damage(float damageAmount)
    {
        // debug that says how much damage was taken
        Debug.Log($"{gameObject.name} took {damageAmount} damage");
        _animator.SetTrigger("Hit");
        _damageFlash.Flash();
        CurrentHealth -= damageAmount;        
        _healthbar.UpdateHealthbar(_enemyStats.maxHealth, CurrentHealth);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        ResetEnemy();
        GameObject deathSprinkleInstance = ObjectPoolManager.SpawnObject(_deathSprinkles.gameObject, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Particle);

        if (deathSprinkleInstance != null)
        {
            ParticleSystem deathSprinklesPS = deathSprinkleInstance.GetComponent<ParticleSystem>();

            var emissionModule = deathSprinklesPS.emission;
            emissionModule.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, (short)_enemyStats.expPoints) });

            deathSprinklesPS.Play();
        }
        ObjectPoolManager.DespawnObject(this.gameObject);
    }

    private void ResetEnemy()
    {
        CurrentHealth = _enemyStats.maxHealth;
        _healthbar.UpdateHealthbar(_enemyStats.maxHealth, CurrentHealth);
        _animator.transform.localScale = _originalScale;

        //IsAggroed = false;
        //IsWithinStrikingDistance = false;
    }

    private void FlipTowardsTarget()
    {
        bool shouldFlip = (transform.position.x > _target.position.x && _animator.transform.localScale.x > 0) ||
                          (transform.position.x < _target.position.x && _animator.transform.localScale.x < 0);

        if (shouldFlip)
        {
            Vector3 scale = _animator.transform.localScale;
            scale.x *= -1;
            _animator.transform.localScale = scale;
        }
    }


    #endregion
}
