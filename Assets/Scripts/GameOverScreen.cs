using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    /*public Text scoreText;*/
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGameButton()
    {
        Application.Quit(); //only works in the compiled build
    }

}
