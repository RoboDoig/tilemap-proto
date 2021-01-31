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
        // Check if we can knock out guard around this tile
        if (interactingCharacter.characterName == "lost") {
            for (int x = -1; x < 2; x++) {
                for (int y = -1; y < 2; y++) {
                    foreach (CharacterControl character in CharacterControl.activeCharacters) {
                        Vector3Int testLocation = interactingCharacter.GetCurrentCell() + new Vector3Int(x, y, 0);
                        if (!character.isPlayer && character.GetCurrentCell()==testLocation) {
                            character.GetComponent<GuardAI>().Incapacitate();

                            AudioManager.instance.AudioGuardKO();
                            ScreenShake.instance.DoShake(0.5f, 0.5f, 1f);
                        }
                    }
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
