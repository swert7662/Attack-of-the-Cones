using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

public class ChainLightningSpawner: MonoBehaviour
{
    [SerializeField] private GameObject _lineLightningPrefab; // Reference to LineLightning prefab
    [SerializeField] private GameEvent _lightningStrikeEvent;

    [SerializeField] private PowerupStats _powerupStats;

    [SerializeField] private float _attackRange;    
    [SerializeField] private EnemyStats _enemyStats;

    private List<Transform> _chainedTargets = new();
    private List<Transform> _targetsInRange = new();
    private List<LineConnector> _lineConnectors = new();
    private System.Random _rand = new();

    private Transform StartPoint;

    private int MaxTargets => _powerupStats.ChainAmount;
    private int Damage => Mathf.Max(1, Mathf.RoundToInt(2 * _powerupStats.DamageLevel * _powerupStats.LightningDamageMultiplier));

    // --------------------------- Initialization ---------------------------
    private void OnEnable()
    {
        ResetSpawner();
    }

    private void InitializeChainAttack()
    {
        CreateEnemyChain();
        UpdateLineConnectors();
        ApplyEffectsToTargets();
        ObjectPoolManager.DespawnObject(this.gameObject);
    }

    public void InitializeChainAttackFromPosition(Vector3 startPosition)
    {
        UpdateEnemiesInRange(startPosition); // Find initial targets based on startPosition
        if (_targetsInRange.Count > 0)
        {
            Transform firstTarget = ChooseTargetFromPosition(startPosition);
            if (firstTarget != null)
            {
                AddTarget(firstTarget);
                ApplyEffectsToTargets();
            }
        }
    }

    public void StartChainAttack(Transform target)
    {
        AddTarget(target);
        InitializeChainAttack();
    }

    // Assuming this method is called to activate Tesla Coil
    public void ActivateTeslaCoil()
    {
        StartPoint = transform;
        StartChainAttack(StartPoint);
    }

    public void ActivateLightningStorm()
    {
        Debug.Log("Lightning storm activated");
        Vector3 stormStartPosition = new Vector3(UnityEngine.Random.Range(-10f, 10f), Camera.main.orthographicSize + 5f, 0); // Adjust as needed
        InitializeChainAttackFromPosition(stormStartPosition);
    }
    // --------------------------- Create Chain ---------------------------
    private void CreateEnemyChain()
    {
        for (int i = 0; i < MaxTargets - 1; i++)
        {
            if (i >= _chainedTargets.Count) { break;}

            UpdateEnemiesInRange(_chainedTargets[i].transform.position);
            Transform _targetEnemy = ChooseTarget();

            if (_targetEnemy != null) { AddTarget(_targetEnemy.transform); }
            else { break; }
        }
    }

    private void UpdateEnemiesInRange(Vector3 currentTarget)
    {
        _targetsInRange.Clear();
        var hits = Physics2D.OverlapCircleAll(currentTarget, _attackRange, _enemyStats.enemyLayerMask);

        foreach (var hit in hits)
        {
            if (!_chainedTargets.Contains(hit.gameObject.transform))
            {
                _targetsInRange.Add(hit.gameObject.transform);
                //_enemiesInRange.Add(hit.transform.parent.gameObject);
            }
        }
    }

    private Transform ChooseTarget()
    {
        var availableTargets = _targetsInRange.Except(_chainedTargets).ToList();
        if (!availableTargets.Any()) { return null; }

        Transform chosenTarget = availableTargets.OrderBy(n => _rand.Next()).FirstOrDefault();
        return chosenTarget;
    }

    private Transform ChooseTargetFromPosition(Vector3 startPosition)
    {
        // Logic similar to ChooseTarget but considering startPosition
        // For simplicity, just return the closest enemy for now
        return _targetsInRange.OrderBy(enemy => (enemy.transform.position - startPosition).sqrMagnitude).FirstOrDefault();
    }

    public void AddTarget(Transform target)
    {
        if (target != null && !_chainedTargets.Contains(target) && target.gameObject.activeInHierarchy) 
        {
            _chainedTargets.Add(target);
        }
        else { Debug.LogWarning("Invalid or duplicate target attempted to be added: " + (target != null ? target.name : "null")); }
    }


    // --------------------------- Apply Effects & Damage ---------------------------
    private void ApplyEffectsToTargets()
    {
        foreach (var target in _chainedTargets)
        {
            if (target == null || target.CompareTag("Player"))
                continue;

            IHealth damageable = target.GetComponent<IHealth>();
            if (damageable != null)
            {
                _lightningStrikeEvent.Raise(this, target.transform.position);
                damageable.Damage(Damage, DamageType.Lightning);
                //ObjectPoolManager.SpawnObject(_lightningImpactPrefab, enemy.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Particle);
            }
            else { Debug.LogWarning("Damageable component not found in the parent of " + target.name); }
        }
    }

    // --------------------------- Visual Elements ---------------------------

    private void CreateLineConnector(Vector3 startPoint, Vector3 endPoint)
    {
        LineConnector lineConnector = ObjectPoolManager.SpawnObject<LineConnector>(_lineLightningPrefab, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.Projectile);
        if (lineConnector != null)
        {
            lineConnector.Initialize(startPoint, endPoint, .2f);
            _lineConnectors.Add(lineConnector);
        }
        else { Debug.LogWarning("LineConnector component not found"); }
    }

    private void UpdateLineConnectors()
    {
        // Clean up existing line connectors
        foreach (var lineConnector in _lineConnectors)
        {
            if (lineConnector != null)
                ObjectPoolManager.DespawnObject(lineConnector.gameObject); //Destroy(lineConnector.gameObject);
        }

        _lineConnectors.Clear();

        // The same code above but with a foreach loop
        foreach (var target in _chainedTargets)
        {
            if (target == null)
                continue;

            int index = _chainedTargets.IndexOf(target);
            if (index < _chainedTargets.Count - 1)
            {
                CreateLineConnector(_chainedTargets[index].transform.position, _chainedTargets[index + 1].transform.position);
            }
        }
    }

    // --------------------------- Reset ---------------------------
    public void ResetSpawner()
    {
        // Clear all lists
        _chainedTargets.Clear();
        _targetsInRange.Clear();
    }


    // --------------------------- Debugging ---------------------------
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
