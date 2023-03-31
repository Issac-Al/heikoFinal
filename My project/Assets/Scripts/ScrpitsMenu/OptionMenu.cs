using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    public GameObject mainCanvas, abilityCanvas, optionCanvas, controlCanvas, scrollView;
    public AudioSource AudioSourceMusic, AudioSourceSFX;
    public Scrollbar sliderMusic, sliderSFX;

    public void ToOptions()
    {
        mainCanvas.SetActive(false);
        optionCanvas.SetActive(true);
    }

    public void ToAbilities()
    {
        mainCanvas.SetActive(false);
        abilityCanvas.SetActive(true);
    }

    public void ToControl()
    {
        mainCanvas.SetActive(false);
        controlCanvas.SetActive(true);
    }

    public void ReturnFromAbilities()
    {
        mainCanvas.SetActive(true);
        abilityCanvas.SetActive(false);
    }

    public void ReturnFromControl()
    {
        mainCanvas.SetActive(true);
        controlCanvas.SetActive(false);
    }

    public void ReturnFromOptions()
    {
        mainCanvas.SetActive(true);
        optionCanvas.SetActive(false);
    }

    public void ToMainMenu()
    {
        AudioSourceMusic.Pause();
        SceneManager.LoadScene(0);
    }

    public void ReloadScene()
    {
        AudioSourceMusic.Pause();
        SceneManager.LoadScene(1);
    }

    public void MusicVolume()
    {
        AudioSourceMusic.volume = sliderMusic.value;
    } 

    public void SFXVolume()
    {
        AudioSourceSFX.volume = sliderSFX.value;
    }
}
