using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "DataTile", menuName = "WorldTiles/DataTile", order = 1)]
public class DataTile : Tile
{
    public bool traversable;
    public bool blocksVision;
    public bool isWinLocation;
}
