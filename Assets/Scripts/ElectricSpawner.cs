using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectricSpawner: MonoBehaviour
{
    [SerializeField] private GameObject _lineLightningPrefab; // Reference to LineLightning prefab
    [SerializeField] private GameObject _lightningImpactPrefab;

    [SerializeField] private float _damage;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _maxTargets;

    private List<GameObject> _chainedEnemies = new();
    private List<GameObject> _enemiesInRange = new();
    private List<LineConnector> _lineConnectors = new();
    private System.Random _rand = new();

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

    private void CreateEnemyChain()
    {
        for (int i = 0; i < _maxTargets - 1; i++)
        {
            if (i >= _chainedEnemies.Count) { break;}

            //Debug.Log("Creating enemy chain");
            UpdateEnemiesInRange(_chainedEnemies[i].transform);
            GameObject _targetEnemyGO = ChooseTarget();

            if (_targetEnemyGO != null) { AddTarget(_targetEnemyGO); }
            else { break; }
        }
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

        // Create new line connectors
        for (int i = 0; i < _chainedEnemies.Count - 1; i++)
        {
            if (_chainedEnemies[i] == null || _chainedEnemies[i + 1] == null)
            {
                Debug.LogWarning("Null enemy found in chain");
                continue; // Skip if any of the chained enemies is null
            }
            GameObject lineConnectorObject = ObjectPoolManager.SpawnObject(_lineLightningPrefab, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.Projectile);
            LineConnector lineConnector = lineConnectorObject.GetComponent<LineConnector>();
            if (lineConnector != null)
            {
                lineConnector.Initialize(_chainedEnemies[i].transform.position, _chainedEnemies[i + 1].transform.position, .3f);
                _lineConnectors.Add(lineConnector);
                //ObjectPoolManager.DespawnObject(lineConnectorObject, 0.5f);
            }
            else { Debug.LogWarning("LineConnector component not found on " + lineConnectorObject.name); }
        }
        // The same code above but with a foreach loop
        foreach (var enemy in _chainedEnemies)
        {
            if (enemy == null)
                continue;

            int index = _chainedEnemies.IndexOf(enemy);
            if (index < _chainedEnemies.Count - 1)
            {
                GameObject lineConnectorObject = ObjectPoolManager.SpawnObject(_lineLightningPrefab, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.Projectile);
                LineConnector lineConnector = lineConnectorObject.GetComponent<LineConnector>();
                if (lineConnector != null)
                {
                    lineConnector.Initialize(_chainedEnemies[index].transform.position, _chainedEnemies[index + 1].transform.position, .6f);
                    _lineConnectors.Add(lineConnector);
                    //ObjectPoolManager.DespawnObject(lineConnectorObject, 0.5f);
                }
                else { Debug.LogWarning("LineConnector component not found on " + lineConnectorObject.name); }
            }
        }
    }

    private void ApplyEffectsToTargets()
    {
        foreach (var enemy in _chainedEnemies)
        {
            if (enemy == null)
                continue;

            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(_damage);
                ObjectPoolManager.SpawnObject(_lightningImpactPrefab, enemy.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Particle);
                Debug.Log($"Hit {enemy.name} with lightning");
            }
            else
            {
                Debug.LogWarning("Damageable component not found in the parent of " + enemy.name);
            }
        }
    }

    public void StartChainAttack(GameObject target)
    {
        AddTarget(target);
        InitializeChainAttack();
    }

    // Allows other scripts to add a target to the chained enemies list
    public void AddTarget(GameObject target)
    {
        if (target != null && target.CompareTag("Enemy") && !_chainedEnemies.Contains(target) && target.activeInHierarchy)
        {
            Debug.Log("Adding target to chained enemies: " + target.name);
            _chainedEnemies.Add(target);
        }
        else
        {
            Debug.LogWarning("Invalid or duplicate target attempted to be added: " + (target != null ? target.name : "null"));
        }
    }

    private GameObject ChooseTarget()
    {
        var availableTargets = _enemiesInRange.Except(_chainedEnemies).ToList();
        if (!availableTargets.Any()) { return null; }

        //return availableTargets.OrderBy(n => _rand.Next()).FirstOrDefault();
        GameObject chosenTarget = availableTargets.OrderBy(n => _rand.Next()).FirstOrDefault();
        //Debug.Log("Chosen target: " + (chosenTarget != null ? chosenTarget.name : "null"));
        return chosenTarget;
    }

    private void UpdateEnemiesInRange(Transform currentTarget)
    {
        _enemiesInRange.Clear();
        var hits = Physics2D.OverlapCircleAll(currentTarget.position, _attackRange, GameManager.Instance._enemyLayer );

        foreach (var hit in hits)
        {
            if (hit.gameObject.CompareTag("Enemy") && !_chainedEnemies.Contains(hit.gameObject))
            {
                _enemiesInRange.Add(hit.gameObject);
                //_enemiesInRange.Add(hit.transform.parent.gameObject);
            }
        }
    }
    public void ResetSpawner()
    {
        // Clear all lists
        _chainedEnemies.Clear();
        _enemiesInRange.Clear();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
