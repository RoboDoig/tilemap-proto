using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class SightArea : MonoBehaviour
{
    private CharacterControl characterControl;
    private GameTiles gameTiles;
    public List<CharacterControl> seenCharacters;

    // Start is called before the first frame update
    void Start()
    {
        gameTiles = GameTiles.instance;
        characterControl = GetComponentInParent<CharacterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D col) {
        // Check if player character is in sight area bounding polygon
        CharacterControl seenCharacter = col.transform.GetComponent<CharacterControl>();
        bool playerCharacterIn = false;
        if (seenCharacter != null && seenCharacter.isPlayer) {
            playerCharacterIn = true;
        }

        // If we found a player character, draw a sight line to it and see if it is blocked
        if (playerCharacterIn) {
            Vector3Int currentPosition = characterControl.GetCurrentCell();
            Vector3Int targetPosition = seenCharacter.GetCurrentCell();
            List<Vector3Int> sightLine = gameTiles.GetBresenhamLineCells(new Vector2Int(currentPosition.x, currentPosition.y), new Vector2Int(targetPosition.x, targetPosition.y));

            sightLine = sightLine.OrderBy(x => Vector3Int.Distance(currentPosition, x)).ToList();
            foreach (Vector3Int sightCell in sightLine) {
                DataTile tile = (DataTile)gameTiles.tilemapFloor.GetTile(sightCell);
                if (gameTiles.worldTileData[sightCell.x, sightCell.y].blocksVision) {
                    return;
                }
            }

            Vector3 lookDirection = characterControl.lookDirection;
            Vector3 seenDirection = characterControl.transform.position - seenCharacter.transform.position;
            float dotProduct = Vector3.Dot(lookDirection, seenDirection);

            // If we are roughly in front of guard
            if (dotProduct < -0.8) {
                // If none of the tiles were vision blocking then we have sight
                if (!seenCharacters.Contains(seenCharacter)) {seenCharacters.Add(seenCharacter);}
            }
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        CharacterControl seenCharacter = col.transform.GetComponent<CharacterControl>();
        if (seenCharacter != null && seenCharacter.isPlayer) {
            seenCharacters.Remove(seenCharacter);
        }
    }
}
