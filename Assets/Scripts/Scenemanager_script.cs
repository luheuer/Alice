using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager_script : MonoBehaviour
{
    
    //VARIABLES

    //--- references to other game objects ---
    [SerializeField]
    private GameObject player;
     [SerializeField]
    private GameObject _CakePrefab;

    [SerializeField]
    private GameObject _DrinkPrefab;
    
    //--- utility for the prefabs ---
    private List<GameObject> cakes = new List<GameObject>();
    private List<Vector3> cake_pos_1 = new List<Vector3>{new Vector3(222f,3.5f,0f), new Vector3(128f,21.4f,0f)};
    private List<GameObject> drinks = new List<GameObject>();
    private List<Vector3> drink_pos_1 = new List<Vector3>{new Vector3(105f,17f,0f),new Vector3(320f,7.4f,0f), new Vector3(320f,26.8f,0f)};
    private GameObject _temp;

    //--- respawn and other scene managment---
    private bool r_down = false;
    private float startTime;
    private float holdTime = 2;
    private float altCounter;
    private string currentScene;
   

    

    // Start is called before the first frame update
    void Start()
    {   
        currentScene = SceneManager.GetActiveScene().name;
        switch(currentScene)
        {
            case "First_room":
                // Cakes and Drinks for the Level are instantiated and added to the List
                foreach(Vector3 position in cake_pos_1)
                {
                    _temp = Instantiate(_CakePrefab,position,Quaternion.identity);
                    cakes.Add(_temp);
                }
                foreach(Vector3 position2 in drink_pos_1)
                {
                    _temp = Instantiate(_DrinkPrefab,position2,Quaternion.identity);
                    drinks.Add(_temp);
                }
                break;
            // If we are in the Transition Scene, a counter is started    
            case "Transition":
                altCounter = Time.time;
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // The Transition Scene automatically switches to the next scene after 8 Seconds
        if(currentScene == "Transition" && (Time.time - altCounter) > 8f)
        {
            SceneManager.LoadScene("First_room");
        }

        //If the player holds down the 'R'-Key, they respawn after 2 seconds
        if(Input.GetKeyDown("r"))
        {
            r_down = true;
            startTime = Time.time;
        }

        if(Input.GetKeyUp("r"))
        {
            r_down = false;
        }
        if(((Time.time - startTime) >= holdTime) && r_down )
        {
            //restore Player to position and state of last spawnpoint
            player.transform.position = player.GetComponent<Player_script>().getSpawn(); 
            player.GetComponent<Player_script>().SetSize(player.GetComponent<Player_script>().getStandardSize());

            //cleaning up the prefabs to make room for new ones
            foreach(GameObject cake in cakes)
            {
                Destroy(cake);
            }
            foreach(GameObject drink in drinks)
            {
                Destroy(drink);
            }
            
            
            //Spawning in necessary Prefabs according to the current Scene 
            //(If case is an artifact of earlier plans to add more scenes with cakes and drinkes
            //, but was left to give an easy opportunity to add them later on)
            if(currentScene == "First_room")
            {
                foreach(Vector3 position in cake_pos_1)
                {
                    _temp = Instantiate(_CakePrefab,position,Quaternion.identity);
                    cakes.Add(_temp);
                }
                foreach(Vector3 position2 in drink_pos_1)
                {
                    _temp = Instantiate(_DrinkPrefab,position2,Quaternion.identity);
                    drinks.Add(_temp);
                }
            }
        }
    }

    public void load_Maze()
    {
        SceneManager.LoadScene("Maze");
    }

}
