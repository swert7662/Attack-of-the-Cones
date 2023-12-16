using System;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    [SerializeField] private Vector3 _startPoint;
    [SerializeField] private Vector3 _endPoint;

    private LineRenderer _lineRenderer;
    private MidpointCalculator _midpointCalculator;

    private void Awake()
    {
        // Add a LineRenderer component if not already attached
        _lineRenderer = GetComponent<LineRenderer>();
        if (_lineRenderer == null)
        {
            Debug.LogError("LineConnector: No LineRenderer component found on " + gameObject.name);
            return;
        }
        _lineRenderer.positionCount = 3;
        _midpointCalculator = new MidpointCalculator();
    }
    public void Initialize(Vector3 startPoint, Vector3 endPoint, float duration)
    {
        Debug.Log("LineConnector Initialize called");
        SetPoints(startPoint, endPoint);
        ObjectPoolManager.DespawnObject(gameObject, duration);
    }

    private void Update()
    {
        UpdateLineMidpoint();
    }

    private void UpdateLineMidpoint()
    {
        Vector3 midpoint = _midpointCalculator.CalculateRandomOffsetMidpoint(_startPoint, _endPoint);
        UpdateLinePositions(midpoint);
    }

    private void UpdateLinePositions(Vector3 midpoint)
    {
        _lineRenderer.SetPositions(new[] { _startPoint, midpoint, _endPoint });
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // Color for the Gizmo spheres
        Gizmos.DrawSphere(_startPoint, 0.1f); // Draw a small sphere at each point
        //Gizmos.DrawSphere(_midpointCalculator.CurrentMidpoint, 0.1f);
        Gizmos.DrawSphere(_endPoint, 0.1f);
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