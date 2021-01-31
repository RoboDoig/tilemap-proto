using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void StartTutorial() {
        SceneManager.LoadScene("TutorialLevel1", LoadSceneMode.Single);
    }

    public void StartGame() {
        SceneManager.LoadScene("Level0", LoadSceneMode.Single);
    }
}
