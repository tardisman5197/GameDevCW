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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonHandlerPlay()
    {
        // New game so delete saves
        Directory.Delete("saves", true);
        SceneManager.LoadSceneAsync("LevelSelect");

    }

    public void ButtonHandlerLoad()
    {
        SceneManager.LoadSceneAsync("LevelSelect");
    }

    public void ButtonHandlerControls()
    {

    }

    public void ButtonHandlerExit()
    {
        Debug.Log("Quiting Application");
        Application.Quit();
    }

    public void ButtonHandlerExitPromptToggle()
    {
        exitPannel.SetActive(!exitPannel.activeSelf);
    }

    public void PlayClickSound()
    {
        click.Play();
    }
}
