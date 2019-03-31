using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject exitPannel;

    public AudioSource click;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // ButtonHandlerPlay is called when the new game button
    // is pressed
    public void ButtonHandlerPlay()
    {
        // New game so delete saves
        try {
            Directory.Delete("saves", true);
        } catch
        {

        }
        SceneManager.LoadSceneAsync("LevelSelect");

    }

    // ButtonHandlerLoad is called when the load game button
    // is pressed
    public void ButtonHandlerLoad()
    {
        SceneManager.LoadSceneAsync("LevelSelect");
    }

    // ButtonHandlerControls is called if the controls button
    // is pressed
    public void ButtonHandlerControls()
    {

    }

    // ButtonHandlerExit is called if the exit yes button
    // is pressed
    public void ButtonHandlerExit()
    {
        Debug.Log("Quiting Application");
        Application.Quit();
    }

    // ButtonHandlerExitPromtToggle is called if the exit button
    // or the exit no button is pressed
    public void ButtonHandlerExitPromptToggle()
    {
        exitPannel.SetActive(!exitPannel.activeSelf);
    }

    public void PlayClickSound()
    {
        click.Play();
    }
}
