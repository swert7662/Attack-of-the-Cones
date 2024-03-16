using UnityEngine;
using UnityEngine.Rendering; // Make sure to include this for the SortingGroup

public class DynamicSort : MonoBehaviour
{
    [SerializeField] private SortingGroup sortingGroup;
    [SerializeField] private int sortFactor;

    void Update()
    {
        if (sortingGroup != null)
        {
            // Use the absolute value of the Y position and multiply by a factor if needed to scale the sorting order appropriately
            // We use Mathf.FloorToInt to ensure we get an integer value for the sorting order
            // Multiply by -1 because in Unity, a higher sorting order means the object will be rendered on top.
            sortingGroup.sortingOrder = Mathf.FloorToInt(transform.position.y * -(sortFactor)); // Adjust the factor (-100) as necessary for your game
        }
    }
}
