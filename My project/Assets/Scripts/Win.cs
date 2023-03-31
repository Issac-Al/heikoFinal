using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public GameObject winScreen;
    public AudioSource bgm;
    // Start is called before the first frame update
    public void WinCondition()
    {
        winScreen.SetActive(true);
    }

    public void MainScreen()
    {
        bgm.Pause();
        SceneManager.LoadScene(0);
    }
}
