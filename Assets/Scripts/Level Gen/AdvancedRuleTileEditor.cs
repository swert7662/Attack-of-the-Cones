#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(AdjacentRuleTile))]
    [CanEditMultipleObjects]
    public class AdvancedRuleTileEditor : RuleTileEditor
    {
        public Texture2D AnyIcon;
        public Texture2D SpecifiedIcon;
        public Texture2D NothingIcon;

        public override void RuleOnGUI(Rect rect, Vector3Int position, int neighbor)
        {
            switch (neighbor)
            {
                case AdjacentRuleTile.Neighbor.Any:
                    GUI.DrawTexture(rect, AnyIcon);
                    return;
                case AdjacentRuleTile.Neighbor.Specified:
                    GUI.DrawTexture(rect, SpecifiedIcon);
                    return;
                case AdjacentRuleTile.Neighbor.Nothing:
                    GUI.DrawTexture(rect, NothingIcon);
                    return;
            }

            base.RuleOnGUI(rect, position, neighbor);
        }
    }
}
#endif