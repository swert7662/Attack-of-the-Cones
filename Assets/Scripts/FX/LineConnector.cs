using System;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    [SerializeField] private Vector3 _startPoint;
    [SerializeField] private Vector3 _endPoint;
    [SerializeField] private Player player;

    private Vector3 startPoint;
    private Vector3 endPoint;

    private Transform originTransform;
    private Transform targetTransform;

    private LineRenderer _lineRenderer;
    private MidpointCalculator _midpointCalculator;

    private bool playerOrigin = false;

    private void OnEnable()
    {
        ObjectPoolManager.DespawnObject(gameObject, .2f);
    }
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        if (_lineRenderer == null)
        {
            Debug.LogError("LineConnector: No LineRenderer component found on " + gameObject.name);
            return;
        }
        _lineRenderer.positionCount = 5;
        _midpointCalculator = new MidpointCalculator();
    }
    public void Initialize(Vector3 startPoint, Vector3 endPoint, float duration)
    {
        Debug.Log("LineConnector Initialize called");
        SetPoints(startPoint, endPoint);
        ObjectPoolManager.DespawnObject(gameObject, duration);
    }

    public void InitializeEndPointOnly(Vector3 endPoint, float duration)
    {
        Debug.Log("LineConnector Initialize called");
        SetPoints(transform.position, endPoint);
        ObjectPoolManager.DespawnObject(gameObject, duration);
    }

    private void Update()
    {
        //if (playerOrigin)
        //{
        //    _startPoint = player.Position;
        //}

        UpdateLineMidpointNEW(_startPoint, _endPoint);
    }

    private void UpdateLineMidpointNEW(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3 center = _midpointCalculator.CalculateRandomOffsetMidpoint(startPoint, endPoint);
        Vector3 midpointA = _midpointCalculator.CalculateRandomOffsetMidpoint(startPoint, center);
        Vector3 midpointB = _midpointCalculator.CalculateRandomOffsetMidpoint(center, endPoint);
        UpdateLinePositionsNEW(startPoint, endPoint, center, midpointA, midpointB);
    }

    private void UpdateLinePositionsNEW(Vector3 startPoint, Vector3 endPoint, Vector3 center, Vector3 midpointA, Vector3 midpointB)
    {
        _lineRenderer.SetPositions(new[] { startPoint, midpointA, center, midpointB, endPoint });
    }

    private void UpdateLineMidpoint()
    {
        Vector3 center = _midpointCalculator.CalculateRandomOffsetMidpoint(_startPoint, _endPoint);
        Vector3 midpointA = _midpointCalculator.CalculateRandomOffsetMidpoint(_startPoint, center);
        Vector3 midpointB = _midpointCalculator.CalculateRandomOffsetMidpoint(center, _endPoint);
        UpdateLinePositions(center, midpointA, midpointB);
    }

    private void UpdateLinePositions(Vector3 center, Vector3 midpointA, Vector3 midpointB)
    {
        _lineRenderer.SetPositions(new[] { _startPoint, midpointA, center, midpointB, _endPoint });
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // Color for the Gizmo spheres
        Gizmos.DrawSphere(_startPoint, 0.1f); // Draw a small sphere at each point
        //Gizmos.DrawSphere(_midpointCalculator.CurrentMidpoint, 0.1f);
        Gizmos.DrawSphere(_endPoint, 0.1f);
    }

    public void SetPlayerOrigin(bool isPlayer)
    {
        playerOrigin = isPlayer;
    }
    public void SetOrigin(Transform origin)
    {
        targetTransform = origin;
    }
    public void SetTarget(Transform newTarget)
    {
        targetTransform = newTarget;
    }

    internal void SetPoints(Vector3 position1, Vector3 position2)
    {
        _startPoint = position1;
        _endPoint = position2;
    }
}
public class MidpointCalculator
{
    public Vector3 CurrentMidpoint { get; private set; }

    public Vector3 CalculateRandomOffsetMidpoint(Vector3 start, Vector3 end)
    {
        Vector3 directMidpoint = (start + end) / 2;
        Vector3 direction = (end - start).normalized;
        Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0);
        float randomOffset = UnityEngine.Random.Range(-0.5f, 0.5f);

        CurrentMidpoint = directMidpoint + perpendicular * randomOffset;
        return CurrentMidpoint;
    }
}