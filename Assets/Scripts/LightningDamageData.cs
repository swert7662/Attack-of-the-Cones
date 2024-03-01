using UnityEngine;

[System.Serializable]
public class LightningDamageData
{
    public Vector3 OriginPosition;
    public Vector3 TargetPosition;
    public bool UsePlayerPosition;

    public LightningDamageData(Vector3 originPosition, Vector3 targetPosition, bool usePlayerOrigin)
    {
        OriginPosition = originPosition;
        TargetPosition = targetPosition;
        UsePlayerPosition = usePlayerOrigin;
    }
}
