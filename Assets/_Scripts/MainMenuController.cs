using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject exitPannel;

    // Start is called before the first frame update
    void Start()
    {
        
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
}
