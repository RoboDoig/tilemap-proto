using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterControl currentCharacter {get; private set;}
    int characterIndex = 0;
    private GameTiles gameTiles;
    private PlayerInterface playerInterface;

    // Start is called before the first frame update
    void Start()
    {
        currentCharacter = CharacterControl.activeCharacters[characterIndex];
        playerInterface = GetComponent<PlayerInterface>();
        gameTiles = GameTiles.instance;

        PlayerCheck();
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

        PlayerCheck();
    }

    bool CheckWinState(CharacterControl character) {
        foreach(Vector3Int goalTileLocation in gameTiles.goalTileLocations) {
            float distance = (character.GetCurrentCell() - goalTileLocation).magnitude;
            if (distance < 3) {
                return true;
            }
        }

        return false;
    }

    void PlayerCheck() {
        if (currentCharacter.isPlayer) {
            playerInterface.updateAction = playerInterface.InControlUpdate;
        } else {
            playerInterface.updateAction = playerInterface.OutControlUpdate;

            // if this is not a player, send it to a random location
            currentCharacter.SetRandomPath();
            AdvanceTurn();
        }
    }
}
