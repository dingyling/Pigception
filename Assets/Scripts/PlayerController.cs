using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxSpeed;
    public float speed;
    public int jumpHeight;
    private Rigidbody2D rb2d;
    private GameObject[] gameObjects;
    private GameObject walkedOver = null;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update () {
        if(Input.GetKey("left"))
        {
            if(rb2d.velocity.x > -maxSpeed)
            {
                rb2d.AddForce(new Vector2(-speed, 0));
            }
            transform.eulerAngles = new Vector2(0,180);
        } 

        if (Input.GetKey("right"))
        {
            if (rb2d.velocity.x < maxSpeed)
            {
                rb2d.AddForce(new Vector2(speed, 0));
            }
            transform.eulerAngles = new Vector2(0,0);
        }

        if (Input.GetKey("up"))
        {
            Jump();
        }

        if (Input.GetKey("space"))
        {
            if(walkedOver != null)
            {
                //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
                if (!walkedOver.CompareTag("Finish"))
                {
                    gameObjects = GameObject.FindGameObjectsWithTag(walkedOver.tag);
                    //Debug.Log(gameObjects);
                    for (int i = 0; i < gameObjects.Length; i++)
                    {
                        //Debug.Log(gameObjects[i].layer);

                        if (gameObjects[i].layer == 9)
                        {
                            Destroy(gameObjects[i]);
                        }
                        else if (gameObjects[i].layer == 10)
                        {
                            //Destroy(gameObjects[i]);
                            gameObjects[i].GetComponent<Collider2D>().enabled = false;
                        }
                    }
                }
            }
        }
    }

    void Jump()
    {
        if (rb2d.velocity.y == 0)
        {
            {
                rb2d.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            }
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

}
