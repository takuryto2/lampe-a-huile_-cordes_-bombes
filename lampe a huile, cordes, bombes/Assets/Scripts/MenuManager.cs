using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<AudioManager>().PlaySound("musique");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("Simon_MainMenu");
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
}
