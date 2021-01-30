using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileData
{
    public bool traversable;
    public bool blocksVision;
    public Vector3Int position;

    public WorldTileData(bool _traversable, bool _blocksVision, Vector3Int _position) {
        traversable = _traversable;
        blocksVision = _blocksVision;
        position = _position;
    }
}
