using System;
using UnityEngine;

public class DynamicYSort : MonoBehaviour
{
    private int _baseSortingOrder;
    private float _ySortingOffset;
    [SerializeField] private SortableSprite[] _sortableSprites;
    [SerializeField] private Transform _sortOffsetMarker;

    private void Start()
    {
        _ySortingOffset = _sortOffsetMarker.position.y;
    }

    private void Update()
    {
        _baseSortingOrder = transform.GetSortingOrder(_ySortingOffset);
        foreach (SortableSprite sortableSprite in _sortableSprites)
        {
            sortableSprite.spriteRenderer.sortingOrder = _baseSortingOrder + sortableSprite.relativeOrder;
        }
    }
    [Serializable]
    public struct SortableSprite
    {
        public SpriteRenderer spriteRenderer;
        public int relativeOrder;
    }
}
