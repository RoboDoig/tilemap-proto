using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataTileSwitch", menuName = "WorldTiles/DataTileSwitch", order = 1)]
public class DataTileSwitch : DataTile
{
    public DataTileSwitch flippedVersion;
    public DataTile targetEffect;

    public override void Interact(Vector3Int cellLocation, CharacterControl interactingCharacter)
    {
        base.Interact(cellLocation, interactingCharacter);

        GameTiles.instance.tilemapObstacles.SetTile(cellLocation, flippedVersion);

        AudioManager.instance.AudioSwitch();
    }
}
