using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour //Class to control the player character
{

    public Animator animator;
    public float speed;
    bool jump = false;
    public HealthBar healthBar;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody rigidbodyComponent;
    public bool isGrounded;
    public int maxHealth = 20;
    public int currentHealth;
    public bool faceRight;
    private float invincibleTimer;
    [SerializeField] private LayerMask platformMask;
    [SerializeField] private GameObject displayGO;
    [SerializeField] private GameObject displayYW;
    private string gameState;
    private float sceneDelay;
    public AudioSource dieAudio;
    public AudioSource endAudio;
    public AudioSource hitAudio;
    public AudioSource jumpAudio;
    public AudioSource squishAudio;
    //Declaring Variables

    private void Start() //Start method is called once when the script is loaded
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rigidbodyComponent = GetComponent<Rigidbody>();
        speed = 4f;
        gameState = "PC";
        displayGO.SetActive(false);
        displayYW.SetActive(false);
        //Setting up/resetting starting values such as health and gameState. Identifying and locating the player.
    }

    void TakeDamage(int damage) //Method to deal damage to the player
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth); 
    }

    void Update() // Update method is called repeatedly to check for user input and animations
    {
        animator.SetFloat("playerSpeed", Mathf.Abs(horizontalInput)+ Mathf.Abs(verticalInput)); //Determines if the player is moving to animate it.
        switch (gameState) { //Switch statement to determine what state the game is in (Is the player alive? Dead? Did they win?)
            case "PC": //If player is alive, lets them move
                doMove();
                break;
            case "GO": //If player has died, stops them from moving and sends them to the main menu after displaying GAME OVER
                displayGO.SetActive(true);
                horizontalInput = 0;
                verticalInput = 0;
                jump = false;
                rigidbodyComponent.velocity = new Vector3(0, 0, 0);
                if (Time.time > sceneDelay)
                { SceneManager.LoadScene("Menu"); }
                    break;
            case "YW": //If player won the game, stops them from moving and sends them to the main menu after displaying YOU WIN
                displayYW.SetActive(true);
                horizontalInput = 0;
                verticalInput = 0;
                jump = false;
                rigidbodyComponent.velocity = new Vector3(0, 0, 0);
                if (Time.time > sceneDelay)
                { SceneManager.LoadScene("Menu"); }
                break;
        }
        

    }
    void FixedUpdate() //Method that updates every frame for 60 frames. Used for physics and detection of collisions as well as elements.
    {
        if (currentHealth <= 0) //Checking if the player is dead
        {
            if (gameState != "GO") //Defines delay, plays death animation and audio only once
            {
                transform.localRotation = Quaternion.Euler(75, 0, 45);
                dieAudio.Play();
                sceneDelay = Time.time + 5;
            }
            gameState = "GO"; //Changes game state
            
        }
        rigidbodyComponent.velocity = new Vector3(horizontalInput * speed, rigidbodyComponent.velocity.y, verticalInput * speed); //Setting player's speed and movement.
       if (!isGrounded) //Checking if the player is grounded and allowing them to jump accordingly
        { return; }
        if (jump == true) {
            doJump();
        }
        
    }

    public void doMove() //Method to detect player input and set their movement/animation accordingly.
    {
        if (Input.GetKey(KeyCode.A))
        { horizontalInput = -1; }
        if (Input.GetKey(KeyCode.D))
        { horizontalInput = 1; }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        { horizontalInput = 0; }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        { horizontalInput = 0; }

        if (Input.GetKey(KeyCode.S))
        { verticalInput = -1; }
        if (Input.GetKey(KeyCode.W))
        { verticalInput = 1; }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W))
        { verticalInput = 0; }
        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        { verticalInput = 0; }

        //Detecting player input and resetting input if necessary.

        if (horizontalInput < 0)
        {
            faceRight = false;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (horizontalInput > 0)
        {
            faceRight = true;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        //Animating player depending on which direction they are moving
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        //Checking if player is grounded and allowing them to jump if so.
    }

    public void OnTriggerStay(Collider collider) //Method to detect collision between player's feet and platforms/Goons
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Platforms")) //States the player is grounded if they are standing on a platform
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Goomba")) //Kills the Goon if Flavio jumps on it and plays audio
        {
            Destroy(collider.gameObject);
            squishAudio.Play();
        }
    }
    public void OnTriggerExit(Collider collider)//Method to check if the player is airborne to animate them.
    {isGrounded = false;
    animator.SetBool("isJumping", true);
    }
        public void doJump() //Method to make the player jump.
    {
        rigidbodyComponent.AddForce(Vector3.up * 13, ForceMode.VelocityChange);
        jumpAudio.Play();
        jump = false;
    }

    public void OnCollisionStay(Collision collider) //Method to detect if the player is touching a goomba with their body to deal damage or to make the player win when they catch Prince Peach.
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Goomba"))
        {
            if (Time.time > invincibleTimer) //Gives the player a 1 second invincibility timer while taking a 5th of their health away. Also plays hurt noise.
            {
                invincibleTimer = Time.time + 1;
                TakeDamage(4);
                if (currentHealth > 0) { hitAudio.Play(); }
                
            }
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Peach")) //If the player interacts with Prince Peach, it changes the game state and plays game end audio
        {
            if (gameState != "YW") //Sets the delay only once and plays the secret message.
            {
                sceneDelay = Time.time + 30; //30 second delay lets the player bask in glory while they hear a secret message
                endAudio.Play();
            }
            gameState = "YW";
            
        }

    }

}
