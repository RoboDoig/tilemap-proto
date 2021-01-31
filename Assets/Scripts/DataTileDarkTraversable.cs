using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataTileDarkTraversable", menuName = "WorldTiles/DataTileDarkTraversable", order = 1)]
public class DataTileDarkTraversable : DataTile
{
    public override void VisionInteract(Vector3Int cellLocation, bool inVision, CharacterControl currentCharacter)
    {
        base.VisionInteract(cellLocation, inVision, currentCharacter);
        if (!inVision && currentCharacter.characterName == "lost") {
            GameTiles.instance.worldTileData[cellLocation.x, cellLocation.y].traversable = true;
        } else {
            GameTiles.instance.worldTileData[cellLocation.x, cellLocation.y].traversable = false;
        }
        // if (inVision) {
        //     GameTiles.instance.worldTileData[cellLocation.x, cellLocation.y].traversable = false;
        // } else {
        //     GameTiles.instance.worldTileData[cellLocation.x, cellLocation.y].traversable = true;
        // }
    }
}
