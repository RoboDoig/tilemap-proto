using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string thisLevel;
    public string nextLevel;
    public CharacterControl currentCharacter {get; private set;}
    public GameObject turnIndicator;
    private SpriteRenderer turnIndicatorRenderer;
    public Vector3 turnIndicatorOffset;
    int characterIndex;
    private GameTiles gameTiles;
    private PlayerInterface playerInterface;
    public bool winState {get; private set;}
    public bool loseState {get; private set;}

    void Awake() {
        if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

        winState = false;
        loseState = false;
        characterIndex = 0;

        currentCharacter = CharacterControl.activeCharacters[characterIndex];
        playerInterface = GetComponent<PlayerInterface>();
        gameTiles = GameTiles.instance;
        turnIndicatorRenderer = turnIndicator.GetComponent<SpriteRenderer>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (winState) {return;}
        // Have the player turn indicator follow the current character
        turnIndicator.transform.position = currentCharacter.transform.position + turnIndicatorOffset;

        // Make sure current character is in vision, otherwise turn off indicator
        Vector3Int currentCharacterCell = currentCharacter.GetCurrentCell();
        if (!GameTiles.instance.worldTileData[currentCharacterCell.x, currentCharacterCell.y].playerVisible) {
            turnIndicatorRenderer.enabled = false;
        } else {
            turnIndicatorRenderer.enabled = true;
        }
    }

    public void AdvanceTurn() {
        StartCoroutine(EndTurn());
    }

    public void CheckWinState() {
        foreach (CharacterControl characterControl in CharacterControl.activeCharacters) {
            if (characterControl.isPlayer) {
                if (!characterControl.reachedGoal) {
                    return;
                }
            }
        }

        Debug.Log("WIN");
        winState = true;

        StartCoroutine(StartLoadNextScene());
    }

    IEnumerator StartLoadNextScene() {
        CharacterControl.activeCharacters.Clear();
        Switch.switches.Clear();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }

    IEnumerator StartReloadScene() {
        CharacterControl.activeCharacters.Clear();
        Switch.switches.Clear();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(thisLevel, LoadSceneMode.Single);
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
            if (guardAI.incapacitated) {
                guardAI.Recover();
            } else {
                currentCharacter.SetPath(guardAI.nextDestination);
                guardAI.PlanNextMove();
            }
            AdvanceTurn();
        }
    }

    IEnumerator EndTurn() {
        playerInterface.updateAction = playerInterface.OutControlUpdate;
        yield return new WaitForSeconds(2);
        PlayerCheck();
    }

    public void LoseState() {
        if (!loseState) {
            AudioManager.instance.AudioLose();
            ScreenShake.instance.DoShake(2f, 0.5f, 1f);
            GameTiles.instance.PaintAllTiles(Color.red);
            loseState = true;
            StartCoroutine(StartReloadScene());
        }
    }
}
