using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingArea : MonoBehaviour
{
    private CharacterControl characterControl;
    private GuardAI guardAI;
    private GameTiles gameTiles;
    public Vector3Int lastHeardPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        gameTiles = GameTiles.instance;
        characterControl = GetComponentInParent<CharacterControl>();
        guardAI = GetComponentInParent<GuardAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D col) {
        CharacterControl heardCharacter = col.transform.GetComponent<CharacterControl>();
        if (heardCharacter.characterName == "found" && heardCharacter.currentMoveSpeed > 0.1f) {
            lastHeardPosition = heardCharacter.GetCurrentCell();
            guardAI.Alert(lastHeardPosition);   
        }
    }
}
