using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JakeScript : MonoBehaviour
{
    public int age;
    public float speed;
    public string characterName;
    public bool isAlive;
    public float health;
    public Text healthText;
    public GameObject gameOver;

    public SpriteRenderer sprite;
    public GameObject bulletFinn;
    public float bulletForce;




    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bulletFinnClone = Instantiate(bulletFinn, transform.position, transform.rotation);
            bulletFinnClone.GetComponent<Rigidbody2D>().AddForce(bulletForce * -transform.right, ForceMode2D.Impulse);
            Destroy(bulletFinnClone, 1);
        }
    }




    void Damage(float damageAmount)
    {
        health -= damageAmount;
        healthText.text = "Health: " + health;
        if (health <= 0)
        {
            gameOver.SetActive(true);
            Destroy(gameObject);
        }
    }


    void JakeMove()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            //sprite.flipX = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            //transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            //sprite.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = "Health: " + health;

        if (isAlive == true)
        {
            print(characterName);
        }
        else
        {
            print("Not Alive");
        }

    }

    // Update is called once per frame
    void Update()
    {
        JakeMove();
        Shoot();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {      
        if (collision.gameObject.tag == "Bmo")
        {
            Destroy(collision.gameObject);
            Damage(2);
        }

        if (collision.gameObject.tag == "Ball")
        {
            Damage(health);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }



}
