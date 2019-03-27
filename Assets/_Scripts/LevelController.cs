using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // army stores the starting layout of the enemy
    public GameObject army;
    // player stores the prefab of a player
    public GameObject player;
    public Transform startTransform;
    // players stores the previous lives of a player.
    public List<GameObject> players;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();
        SpawnArmy();
        NewPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    // NewPlayer 
    void NewPlayer()
    {
        // Update the start position
        startTransform.position = new Vector3(startTransform.position.x + 1, startTransform.position.y, startTransform.position.z);

        // Create the new player
        Debug.Log("Creating new player");
        GameObject newPlayer = Instantiate(player, startTransform);
        newPlayer.GetComponent<CharacterMotor>().startTransform = startTransform;
        newPlayer.GetComponent<HealthController>().player = true;

        // Set the camera to the new player
        this.gameObject.GetComponent<CameraController>().target = newPlayer.transform.Find("CameraTarget");
    }
}
