using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataTileWin", menuName = "WorldTiles/DataTileWin", order = 1)]
public class DataTileWin : DataTile
{
    public override void Interact(Vector3Int cellLocation, CharacterControl interactingCharacter)
    {
        interactingCharacter.ReachedGoal();
        if (interactingCharacter.characterName == "found") {
            AudioManager.instance.AudioFoundWin();
        } else if (interactingCharacter.characterName == "lost") {
            AudioManager.instance.AudioLostWin();
        }
    }
}
