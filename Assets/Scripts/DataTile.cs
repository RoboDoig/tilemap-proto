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
            foreach (CharacterControl character in CharacterControl.activeCharacters) {
                if (!character.isPlayer && character.GetCurrentCell()==cellLocation) {
                    character.GetComponent<GuardAI>().Incapacitate();

                    AudioManager.instance.AudioGuardKO();
                    ScreenShake.instance.DoShake(0.5f, 0.5f, 1f);
                }
            }
        }

        foreach(Switch activeSwitch in Switch.switches) {
            if (activeSwitch.GetCurrentCell() == cellLocation) {
                activeSwitch.Toggle();
                AudioManager.instance.AudioSwitch();
            }
        }
    }

    public virtual void VisionInteract(Vector3Int cellLocation, bool inVision) {

    }
}
