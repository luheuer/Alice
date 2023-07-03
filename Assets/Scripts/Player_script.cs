using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_script : MonoBehaviour
{
    //VARIABLES
    // --- movement---
    private float _speed = 15f;
    private float _jumpingSpeed = 15f;
    private bool is_Grounded = true;
    private bool is_Locked = false;
    
    //--- size and collisions and sprite managment---
    [SerializeField]
    private Rigidbody RB;
    private float _size = 1;
    private float _standardSize = 1;
    private bool facing_Right = true;
    private Animator anim; 
    
    //--- trigger and scene managment ---
    [SerializeField]
    private GameObject myScenemanager;
    private bool _drank = false;
    private bool _ate = false;
    private string _triggerTag;
    private Vector3 _spawn = new Vector3(0f,0f,0f);
    private string currentScene;

    //--- for textbox ---
    [SerializeField]
    private Textboxmanager_script theTextbox;
    
    
    void Start()
    {
      anim = GetComponent<Animator>();
      currentScene = SceneManager.GetActiveScene().name;
      //The player starts the first room falling, so the apropriate animation is triggert
      if(currentScene == "First_room")
      {
        anim.SetBool("jumping", true);
      }
    }

  
    void Update()
    {
        //MOVEMENT
        // Only if the player is not currently locked in place
        if(!is_Locked)
        {
          float horizontal_Input = Input.GetAxis("Horizontal");
          if(horizontal_Input != 0)
          {
            // making sure the sprite is facing the right direction and the proper animation is active
            if(horizontal_Input >0 && !facing_Right)
            {
              flip();
            } else if(horizontal_Input < 0 && facing_Right)
            {
              flip();
            }
            anim.SetBool("running", true);
          } else
          {
            anim.SetBool("running", false);
          }
          transform.Translate(Vector3.right * Time.deltaTime * _speed * horizontal_Input);
        
          //JUMPING
          if (Input.GetKeyDown("space") && is_Grounded)
          {
            anim.SetBool("jumping",true);
            RB.velocity += new Vector3(0f,_jumpingSpeed, 0f);
          }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
      //enables the player to jump after touching down on a proper object 
      //(Mushrooms are not in the current version due to time reasons, but the interaction was coded already so I left it in)
      if(collision.gameObject.tag == "Ground" || collision.gameObject.CompareTag("Mushroom"))
      {
        is_Grounded = true;
        anim.SetBool("jumping",false);
        // If the player were to land on a Mushroom, they are catapulted upwards automatically
        if(collision.gameObject.CompareTag("Mushroom"))
        {
          RB.velocity += new Vector3(0f,_jumpingSpeed*1.5f, 0f);
        }
      }
    }

     void OnCollisionExit(Collision collision)
    {
      // Prevents the player from double-jumping and triggers the jumping animation
      if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Mushroom"))
      {
        is_Grounded = false;
        anim.SetBool("jumping", true);
      }
    }

    void OnTriggerEnter(Collider trigger)
    {
      _triggerTag = trigger.tag;
      // Possible Triggers are checked to initiate the proper response
      switch(_triggerTag)
      {
        case "Cake":
          // A cake causes the Player to grow within a certain limit...
          if(_size < 2)
          {
            _size += 1;
            this.SetSize(_size);
            //(Special reaction if the Player eats a cake for the first time)
            if(!_ate)
            {
              anim.SetBool("running", false);
              _ate = true;
              setLocked(true);
              theTextbox.EnableTextbox();
              theTextbox.ReloadScript(trigger.GetComponent<TextTrigger_script>().GetText());
              theTextbox.SetCurrentLine(trigger.GetComponent<TextTrigger_script>().GetStartLine());
              theTextbox.SetEndLine(trigger.GetComponent<TextTrigger_script>().GetEndLine());
            }
            Destroy(trigger.gameObject);
          }
          break;
        case "Drink":
          // ... and a Drink does the same in the other direction.
          // Both Items are one use only
          if(_size > 0)
          {
            _size -= 1;
            this.SetSize(_size);
            //(Special reaction to the first drink)
            if(!_drank)
            {
              anim.SetBool("running", false);
              _drank = true;
              setLocked(true);
              theTextbox.EnableTextbox();
              theTextbox.ReloadScript(trigger.GetComponent<TextTrigger_script>().GetText());
              theTextbox.SetCurrentLine(trigger.GetComponent<TextTrigger_script>().GetStartLine());
              theTextbox.SetEndLine(trigger.GetComponent<TextTrigger_script>().GetEndLine());
            }
            Destroy(trigger.gameObject);
          }
          break;
        case "CHeckpoint":
          // The Checkpoint sets the new Spawnpoint, according to the current scene 
          // again the if clause is an artifact of higher ambitions
          if(currentScene == "First_room")
          {
            _spawn = new Vector3(240f,55f,0f);
            _standardSize = 2;
            anim.SetBool("running", false);
            setLocked(true);
            
            //Text reaction by Alice to the Checkpoint
            //(Also hints to the player how to respawn)
            theTextbox.EnableTextbox();
            theTextbox.ReloadScript(trigger.GetComponent<TextTrigger_script>().GetText());
            theTextbox.SetCurrentLine(trigger.GetComponent<TextTrigger_script>().GetStartLine());
            theTextbox.SetEndLine(trigger.GetComponent<TextTrigger_script>().GetEndLine());
            Destroy(trigger.gameObject);
          }
          break;
        case "Goal":
          // Goal causes the next scene to load
          if(currentScene == "First_room")
          {
            SceneManager.LoadScene("Second_room");
          }
          break;
        case "TextTrigger":
          // If the player comes into contact with a Text Trigger, the text box returns with new content
          anim.SetBool("running", false);
          setLocked(true);
          theTextbox.EnableTextbox();
          theTextbox.ReloadScript(trigger.GetComponent<TextTrigger_script>().GetText());
          theTextbox.SetCurrentLine(trigger.GetComponent<TextTrigger_script>().GetStartLine());
          theTextbox.SetEndLine(trigger.GetComponent<TextTrigger_script>().GetEndLine());
          Destroy(trigger.gameObject);
          break;

      }
      
    }
    

    public void SetSize(float size)
    { //Sets the size of the player and adjusts speed and jumping speed according to the current size.
      _size = size;
      switch(size){
        case 0:
          //making sure the resized sprite still faces the right direction.
          if(facing_Right)
          {
            transform.localScale = new Vector3(2f,2f,2f);  
          } else {
            transform.localScale = new Vector3(-2f,2f,2f);
          }
          
          _speed = 17.5f;
          _jumpingSpeed = 12.5f;
          break;
        case 1:
          if(facing_Right)
          {
            transform.localScale = new Vector3(3.5f,3.5f,3.5f);
          } else {
            transform.localScale = new Vector3(-3.5f,3.5f,3.5f);
          }
          _speed = 15f;
          _jumpingSpeed = 15f;
          break;
        case 2:
          if(facing_Right)
          {
            transform.localScale = new Vector3(4.5f,4.5f,4.5f);
          } else {
            transform.localScale = new Vector3(-4.5f,4.5f,4.5f);
          }
          _speed = 12.5f;
          _jumpingSpeed = 17.5f;
          break;  
      }  
    }

    //Misc functions (mostly for the Scenemanagment)
    public Vector3 getSpawn()
    {
      return this._spawn;
    }

    public float getStandardSize()
    {
      return this._standardSize;
    }
    
    public void flip()
    { //Function to flip the way the Player-sprite is facing
      // Switches the way the player is labelled as facing
      facing_Right = !facing_Right;

      // Flips the Sprite by multiplying the player's x local scale by -1
      Vector3 scale = transform.localScale;
      scale.x *= -1;
      transform.localScale = scale;
    }

    public void setLocked(bool state)
    {
      this.is_Locked = state;
    }
}