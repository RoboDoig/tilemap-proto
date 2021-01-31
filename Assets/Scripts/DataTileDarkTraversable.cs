using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataTileDarkTraversable", menuName = "WorldTiles/DataTileDarkTraversable", order = 1)]
public class DataTileDarkTraversable : DataTile
{
    public override void VisionInteract(Vector3Int cellLocation, bool inVision)
    {
        base.VisionInteract(cellLocation, inVision);
        if (inVision) {
            GameTiles.instance.worldTileData[cellLocation.x, cellLocation.y].traversable = false;
        } else {
            GameTiles.instance.worldTileData[cellLocation.x, cellLocation.y].traversable = true;
        }
    }
}
