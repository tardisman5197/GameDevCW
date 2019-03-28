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
    public Transform startTransform;
    // players stores the previous lives of a player.
    public List<GameObject> players;

    public GameObject infoUI;

    [Serializable]
    public struct LvlSave
    {
        public int deaths;

        public LvlSave(int deaths)
        {
            this.deaths = deaths;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();
        noOfEnemyDeaths = 0;
        SpawnArmy();
        NewPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (army.transform.childCount <= noOfEnemyDeaths)
        {
            Debug.Log("Won!!");
            Save();
            SceneManager.LoadSceneAsync("LevelSelect");
        }
    }

    // AddPlayer resets the player's previous character
    // to the start position and adds the player to the list.
    void AddPlayer(GameObject newPlayer)
    {
        newPlayer.GetComponent<CharacterMotor>().dead = true;
        newPlayer.GetComponent<HealthController>().player = false;
        players.Add(newPlayer);
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
            players[i].GetComponent<CharacterMotor>().Reset();
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
        ResetScene();

        Debug.Log("New player");
        NewPlayer();

        infoUI.GetComponent<ScoreUI>().AddDeath();
    }

    // NewPlayer 
    void NewPlayer()
    {
        // Update the start position
        startTransform.position = new Vector3(startTransform.position.x - 1, startTransform.position.y, startTransform.position.z);

        // Create the new player
        Debug.Log("Creating new player");
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

    // Save creates an xml document contating the lvl information
    public void Save()
    {
        System.IO.Directory.CreateDirectory("saves");
        string filename = "saves/lvl1.xml";
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
}
