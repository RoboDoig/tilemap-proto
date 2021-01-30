using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterControl currentCharacter {get; private set;}
    int characterIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentCharacter = CharacterControl.activeCharacters[characterIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceTurn() {
        characterIndex++;
        if (characterIndex > CharacterControl.activeCharacters.Count-1) {
            characterIndex = 0;
        }

        currentCharacter = CharacterControl.activeCharacters[characterIndex];
    }
}
