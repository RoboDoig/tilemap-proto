using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataTileBreakable", menuName = "WorldTiles/DataTileBreakable", order = 1)]
public class DataTileBreakable : DataTile
{
    public override void Interact(Vector3Int cellLocation, CharacterControl interactingCharacter)
    {
        base.Interact(cellLocation, interactingCharacter);
        DataTile tile = (DataTile)GameTiles.instance.tilemapObstacles.GetTile(cellLocation);

        if (interactingCharacter.characterName == "found") {
            GameTiles.instance.tilemapObstacles.SetTile(cellLocation, null);
            GameTiles.instance.worldTileData[cellLocation.x, cellLocation.y].blocksVision = false;
            GameTiles.instance.worldTileData[cellLocation.x, cellLocation.y].traversable = true;
        }
    }
}
