using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CharacterControl currentCharacter {get; private set;}
    public GameObject turnIndicator;
    public SpriteRenderer turnIndicatorRenderer;
    public Vector3 turnIndicatorOffset;
    int characterIndex = 0;
    private GameTiles gameTiles;
    private PlayerInterface playerInterface;

    void Awake() {
        if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCharacter = CharacterControl.activeCharacters[characterIndex];
        playerInterface = GetComponent<PlayerInterface>();
        gameTiles = GameTiles.instance;
        turnIndicatorRenderer = turnIndicator.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        turnIndicator.transform.position = currentCharacter.transform.position + turnIndicatorOffset;

        Vector3Int currentCharacterCell = currentCharacter.GetCurrentCell();
        if (!GameTiles.instance.worldTileData[currentCharacterCell.x, currentCharacterCell.y].playerVisible) {
            turnIndicatorRenderer.enabled = false;
        } else {
            turnIndicatorRenderer.enabled = true;
        }
    }

    public void AdvanceTurn() {
        StartCoroutine(EndTurn());

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

        characterIndex++;
        if (characterIndex > CharacterControl.activeCharacters.Count-1) {
            characterIndex = 0;
        }

        currentCharacter = CharacterControl.activeCharacters[characterIndex];

        if (currentCharacter.isPlayer) {
            playerInterface.updateAction = playerInterface.InControlUpdate;
        } else {
            playerInterface.updateAction = playerInterface.OutControlUpdate;
            GuardAI guardAI = currentCharacter.GetComponent<GuardAI>();
            currentCharacter.SetPath(guardAI.nextDestination);
            guardAI.PlanNextMove();
            AdvanceTurn();
        }
    }

    IEnumerator EndTurn() {
        playerInterface.updateAction = playerInterface.OutControlUpdate;
        yield return new WaitForSeconds(2);
        PlayerCheck();
    }

    public void LoseState() {
        Debug.Log("LOSE!");
    }
}
