using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterControl currentCharacter {get; private set;}
    int characterIndex = 0;
    private GameTiles gameTiles;

    // Start is called before the first frame update
    void Start()
    {
        currentCharacter = CharacterControl.activeCharacters[characterIndex];
        gameTiles = GameTiles.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceTurn() {
        // Check if we won
        bool won = true;
        foreach (CharacterControl characterControl in CharacterControl.activeCharacters) {
            if (characterControl.isPlayer) {
                if (!CheckWinState(characterControl)) {
                    won = false;
                    break;
                }
            }
        }

        if (won) {
            Debug.Log("WON!");
        }

        characterIndex++;
        if (characterIndex > CharacterControl.activeCharacters.Count-1) {
            characterIndex = 0;
        }

        currentCharacter = CharacterControl.activeCharacters[characterIndex];
    }

    bool CheckWinState(CharacterControl character) {
        foreach(Vector3Int goalTileLocation in gameTiles.goalTileLocations) {
            float distance = (character.GetCurrentCell() - goalTileLocation).magnitude;
            Debug.Log(distance);
            if (distance < 3) {
                return true;
            }
        }

        return false;
    }
}
