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

    public virtual void Interact(Vector3Int cellLocation, CharacterControl interactingCharacter) {
        // Check if we can knock out a guard here
        if (interactingCharacter.characterName == "lost") {
            Debug.Log("lost interacting");
            foreach (CharacterControl character in CharacterControl.activeCharacters) {
                if (!character.isPlayer && character.GetCurrentCell()==cellLocation) {
                    character.GetComponent<GuardAI>().Incapacitate();
                }
            }
        }
    }

    public virtual void VisionInteract(Vector3Int cellLocation, bool inVision) {

    }
}
