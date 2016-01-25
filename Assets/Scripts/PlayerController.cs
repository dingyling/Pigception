using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxSpeed;
    public float speed;
    public int jumpHeight;

    private GameObject[] Board;
    private LevelBuilder levelBuilder;
    private Rigidbody2D rb2d;
    private GameObject[] gameObjects;
    private GameObject walkedOver = null;
    private bool isGrounded = true;
    private float jumpDelay;
    private bool hasJumped;

    void Start()
    {
        hasJumped = false;
        rb2d = GetComponent<Rigidbody2D>();
        Board = GameObject.FindGameObjectsWithTag("GameController");
        levelBuilder = Board[0].GetComponent<LevelBuilder>();
    }

    void Update() {
        if (Input.GetKey("left"))
        {
            if (rb2d.velocity.x > -maxSpeed)
            {
                rb2d.AddForce(new Vector2(-speed, 0));
            }
            transform.eulerAngles = new Vector2(0, 180);
        }

        if (Input.GetKey("right"))
        {
            if (rb2d.velocity.x < maxSpeed)
            {
                rb2d.AddForce(new Vector2(speed, 0));
            }
            transform.eulerAngles = new Vector2(0, 0);
        }

        if (Input.GetKeyDown("up"))
        {
            //if (Physics2D.Raycast(transform.position, Vector3.down, 1.0f) == true ) {
            if (isGrounded)
            {
                Jump();
                hasJumped = true;
                jumpDelay = Time.time;
                isGrounded = false;
            }
            
            //}
        }

        if (Input.GetKey("left ctrl"))
        {
            levelBuilder.LoadNextLevel();
        }

        if (Input.GetKey("space"))
        {
            if (walkedOver != null)
            {
                if (!walkedOver.CompareTag("Finish"))
                {
                    gameObjects = GameObject.FindGameObjectsWithTag(walkedOver.tag);
                    for (int i = 0; i < gameObjects.Length; i++)
                    {
                        if (gameObjects[i].layer == 9)
                        {
                            Destroy(gameObjects[i]);
                        }
                        else if (gameObjects[i].layer == 10)
                        {
                            gameObjects[i].GetComponent<Collider2D>().enabled = false;
                        }
                    }
                }
                else
                {
                    levelBuilder.LoadNextLevel();
                }
            }
        }

        Debug.Log(rb2d.velocity.x);
    }

    void ResetJump()
    {
        //Debug.Log(Time.time - jumpDelay);
       // Debug.Log(GameObject.FindWithTag("otherColliders").GetComponent<BoxCollider2D>);
        //if (Time.time - jumpDelay > 0.2) {
            isGrounded = true;
        //}
        
    }

    void Jump()
    {
        if(rb2d.velocity.y == 0)
        {
            rb2d.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        walkedOver = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        walkedOver = null;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        
        //  other.collider.IsTouching(Colli1);
        if (!isGrounded)
        {
            ResetJump();
        }
    }
    

}
