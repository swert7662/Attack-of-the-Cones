using UnityEngine;
using System.Reflection;

public class Stats : ScriptableObject
{
    public void ApplyModifier(StatModifier modifier)
    {
        FieldInfo field = GetType().GetField(modifier.statName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
        {
            if (field.FieldType == typeof(float))
            {
                float currentValue = (float)field.GetValue(this);
                if (modifier.isAdditive)
                {
                    Debug.Log($"Adding float {modifier.value} to {modifier.statName}");
                    field.SetValue(this, currentValue + modifier.value);
                }
                else
                {
                    // Assuming baseValue needed for some reason; typically, you'd calculate a multiplicative modifier differently.
                    float baseValue = currentValue; // Simplification for the example; adjust as needed.
                    float modifiedValue = baseValue * modifier.value;
                    field.SetValue(this, currentValue + modifiedValue);
                }
            }
            else if (field.FieldType == typeof(int))
            {
                int currentValue = (int)field.GetValue(this);
                if (modifier.isAdditive)
                {
                    int modifierValueAsInt = Mathf.RoundToInt(modifier.value);
                    field.SetValue(this, currentValue + modifierValueAsInt);
                }
            }
            else if (field.FieldType == typeof(short))
            {
                short currentValue = (short)field.GetValue(this);
                if (modifier.isAdditive)
                {
                    // Safely convert float to short
                    short modifierValueAsShort = (short)Mathf.Clamp(modifier.value, short.MinValue, short.MaxValue);
                    field.SetValue(this, (short)(currentValue + modifierValueAsShort));
                }
            }
            else if (field.FieldType == typeof(bool))
            {
                field.SetValue(this, modifier.value > 0);
            }
        }
        else
        {
            Debug.LogWarning($"Field not found: {modifier.statName}");
        }
    }
}
