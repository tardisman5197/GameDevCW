using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    // army stores the starting layout of the enemy
    public GameObject army;
    public int noOfEnemyDeaths;

    // player stores the prefab of a player
    public GameObject player;

    // startTransform stores the location of the next player to spawn
    public Transform startTransform;

    // players stores the previous lives of a player.
    private class PastPlayer {
        public Dictionary<int, CharacterMotor.Action> actions;
        public Vector3 position;
        public Quaternion rotation;

        public PastPlayer(Dictionary<int, CharacterMotor.Action> actions, Vector3 position, Quaternion rotation)
        {
            this.actions = actions;
            this.position = position;
            this.rotation = rotation;
        }
    }
    // players is the past lives of a player
    private List<PastPlayer> players;

    // infoUI is the HUD 
    public GameObject infoUI;

    // won variables
    public bool won;
    public GameObject wonUI;

    public GameObject respawnUI;

    // LvlSave is used to store the score of the player
    [Serializable]
    public struct LvlSave
    {
        public int deaths;

        public LvlSave(int deaths)
        {
            this.deaths = deaths;
        }
    }

    public string filename;

    // Start is called before the first frame update
    void Start()
    {
        won = false;
        players = new List<PastPlayer>();
        noOfEnemyDeaths = 0;

        // setup the scene
        SpawnArmy();
        NewPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player has won
        if (army.transform.childCount <= noOfEnemyDeaths && !won)
        {
            Debug.Log("Won!!");
            won = true;
            wonUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }

    // AddPlayer resets the player's previous character
    // to the start position and adds the player to the list.
    void AddPlayer(GameObject newPlayer)
    {
        players.Add(new PastPlayer(
            newPlayer.GetComponent<CharacterMotor>().GetActions(),
            newPlayer.GetComponent<CharacterMotor>().startPosition,
            newPlayer.GetComponent<CharacterMotor>().startRotation)
        );
    }

    // CleanUpScene removes all the players and armies from the scene.
    void CleanUpScene()
    {
        // Remove all the enmeys from the scene
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i =0; i<enemys.Length; i++)
        {
            Destroy(enemys[i]);
        }

        // Remove all the players from the scene
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerObjs.Length; i++)
        {
            Destroy(playerObjs[i]);
        }
    }

    // SpawnArmy spawns the army in their starting positions.
    void SpawnArmy()
    {
        // Spawn the army in the orignal positions
        Instantiate(army);
    }

    // SpawnPlayers spawns all the previous players.
    void SpawnPlayers()
    {
        Debug.Log("Spawning Players");
        for (int i=0; i<players.Count; i++)
        {
            GameObject newPlayer = Instantiate(player, players[i].position, players[i].rotation);
            newPlayer.GetComponent<CharacterMotor>().Reset(players[i].actions);
            newPlayer.GetComponent<HealthController>().player = false;
        }
    }

    // ResetScene clears the level and respawns the players and army. 
    void ResetScene()
    {
        noOfEnemyDeaths = 0;
        CleanUpScene();
        SpawnArmy();
        SpawnPlayers();
    }

    // PlayerDeath restarts the level and adds the player to the
    // players list.
    public void PlayerDeath(GameObject player)
    {

        AddPlayer(player);

        respawnUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    // NewPlayer 
    void NewPlayer()
    {
        // Update the start position
        startTransform.position = new Vector3(startTransform.position.x - 1, startTransform.position.y, startTransform.position.z + 1);

        // Create the new player
        GameObject newPlayer = Instantiate(player, startTransform);
        newPlayer.GetComponent<CharacterMotor>().startPosition = startTransform.position;
        newPlayer.GetComponent<CharacterMotor>().startRotation = startTransform.rotation;
        newPlayer.GetComponent<HealthController>().player = true;

        // Set the camera to the new player
        this.gameObject.GetComponent<CameraController>().target = newPlayer.transform.Find("CameraTarget");
    }

    // EnemyDeath is used to update the number of enmeys killed.
    public void EnemyDeath()
    {
        noOfEnemyDeaths++;
    }

    // Save creates an xml document contati g the lvl information
    public void Save()
    {
        System.IO.Directory.CreateDirectory("saves");
        XmlDocument xmlDocument = new XmlDocument();
        LvlSave state = new LvlSave(players.Count);
        XmlSerializer serializer = new XmlSerializer(typeof(LvlSave));
        using (MemoryStream stream = new MemoryStream())
        {
            serializer.Serialize(stream, state);
            stream.Position = 0;
            xmlDocument.Load(stream);
            xmlDocument.Save(filename);
        }
    }

    // LevelSelect saves and goes back to the level select
    // scene
    public void LevelSelect()
    {
        Save();
        SceneManager.LoadSceneAsync("LevelSelect");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    // Mainmenu saves and goes back to the main menu
    // scene
    public void MainMenu()
    {
        Save();
        SceneManager.LoadSceneAsync("MainMenu");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;
    }

    // Resume is called when the respawn button is pressed
    public void Respawn()
    {
        ResetScene();

        Debug.Log("New player");
        NewPlayer();

        infoUI.GetComponent<ScoreUI>().AddDeath();

        respawnUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
}
