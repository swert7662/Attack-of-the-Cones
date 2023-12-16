using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    private CircleCollider2D _collider;
    private LayerMask _enemyLayer;

    [SerializeField] private float _damage = 10f;
    [SerializeField] private int _maxTargets = 3;

    [SerializeField] private GameObject _lightningPrefab;
    [SerializeField] private GameObject _struckByLightning;
    [SerializeField] private ParticleSystem _particleSystem;

    private GameObject _startObject;
    private GameObject _endObject;
    private Animator _animator;
    private int _singleBounce;

    private void Start()
    {
        if (_singleBounce == 0)
        {
            ObjectPoolManager.DespawnObject(gameObject);
        }

        _enemyLayer = GameManager.Instance._enemyLayer;

        _collider = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _particleSystem = GetComponent<ParticleSystem>();

        _startObject = gameObject;

        _singleBounce = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsEnemyTarget(collision) && IsNotStruckByLightning(collision))
        {
            _endObject = collision.transform.parent.gameObject;
            _maxTargets--;

            if (_maxTargets > 0)
            {
                SpawnChainLightningAtTarget(collision.transform);
            }

            ApplyDamageIfDamageable(collision);

            PlayEffects();

            // Cleanup or destroy this instance after a delay to allow effects to play
            Destroy(gameObject, 1f);
        }
    }

    private bool IsEnemyTarget(Collider2D collision)
    {
        return ((1 << collision.gameObject.layer) & _enemyLayer) != 0;
    }

    private bool IsNotStruckByLightning(Collider2D collision)
    {
        return !collision.GetComponentInChildren<EnemyLightningStruck>();
    }

    private void SpawnChainLightningAtTarget(Transform targetTransform)
    {
        ObjectPoolManager.SpawnObject(_lightningPrefab, targetTransform.position, Quaternion.identity, targetTransform);
        // Consider adding struck effect to a pool for efficiency
        Instantiate(_struckByLightning, targetTransform.position, Quaternion.identity, targetTransform);
    }

    private void ApplyDamageIfDamageable(Collider2D collision)
    {
        var damageable = collision.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            Debug.Log($"Hit {collision.gameObject.name} with lightning");
            damageable.Damage(_damage);
        }
    }

    private void PlayEffects()
    {
        _animator.StopPlayback();
        _collider.enabled = false;
        _particleSystem.Play();
        // Debug log stating which two transforms were connected by the lightning
        Debug.Log($"Lightning connected {_startObject.name} to {_endObject.name}");
        EmitParticleAtPosition(_startObject.transform.position);
        EmitParticleAtPosition(_endObject.transform.position);
    }

    private void EmitParticleAtPosition(Vector3 position)
    {
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = position;
        _particleSystem.Emit(emitParams, 1);
    }
    private void OnParticleSystemStopped()
    {
        Debug.Log("Return Lightning to Pool");
        ObjectPoolManager.DespawnObject(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (_startObject != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_startObject.transform.position, 0.5f);
        }

        if (_endObject != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_endObject.transform.position, 0.5f);

            // Draw a line to visualize the effect connection
            if (_startObject != null)
            {
                Gizmos.DrawLine(_startObject.transform.position, _endObject.transform.position);
            }
        }
    }

}
