using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.VisualScripting;

[CreateAssetMenu]
public class AdjacentRuleTile : RuleTile<AdjacentRuleTile.Neighbor> {
    public bool alwaysConnect;
    public bool checkSelf;
    public TileBase[] tileToConnect;

    private Sprite specifiedSprite;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int This = 1;
        public const int NotThis = 2;
        public const int Any = 3;
        public const int Specified = 4;
        public const int Nothing = 5;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        specifiedSprite = GetSpriteFromTile(tile);
        switch (neighbor) {
            case Neighbor.This: return Check_This(tile);
            case Neighbor.NotThis: return Check_NotThis(tile);
            case Neighbor.Any: return Check_Any(tile);
            case Neighbor.Specified: return Check_Specified(tile, specifiedSprite);
            case Neighbor.Nothing: return Check_Nothing(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }    

    private bool Check_This(TileBase tile)
    {
        if (!alwaysConnect)
        {
            return tile == this;
        }
        else
        {
            return tileToConnect.Contains(tile);
        }
    }

    private bool Check_NotThis(TileBase tile)
    {
        return tile != this; // Redundant since its already handled by default, but for clarity
    }

    private bool Check_Any(TileBase tile)
    {
        // If checkSelf is true, return true if tile is not null
        if (checkSelf)
        {
            return tile != null;
        }
        // If checkSelf is false, return true if tile is not null and not this
        else
        {
            return tile != null && tile != this;
        }
    }

    private bool Check_Specified(TileBase tile, Sprite specifiedSprite)
    {
        Sprite tileSprite = GetSpriteFromTile(tile);
        if (tileSprite = specifiedSprite)
        {
            return false;
        }
        else 
            return true;
    }

    private bool Check_SameSprite(TileBase tile, Sprite currentTileSprite)
    {
        Sprite tileSprite = GetSpriteFromTile(tile);
        return tileSprite != currentTileSprite;
    }

    private bool Check_Nothing(TileBase tile)
    {
        return tile == null;
    }

    private Sprite GetSpriteFromTile(TileBase tile)
    {
        if (tile is Tile)
        {
            return (tile as Tile).sprite;
        }
        return null;
    }
}