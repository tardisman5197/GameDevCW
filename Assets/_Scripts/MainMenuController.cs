using System.Collections;
using System.Collections.Generic;
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
        SceneManager.LoadSceneAsync("LevelSelect");
    }

    public void ButtonHandlerLoad()
    {

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
