using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [Header("Health")]
    public float health;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Slider healthBar;
    public float energy;
    public Slider energyBar;
    public float energyCost;

    [Header("Move")]
    public float speed;
    public GameObject flame;
    float moveX, moveY;
    public bool hasEnergyBuff;

    [Header("Shoot")]    
    public GameObject bullet;
    public Transform bulletPoint;
    public float bulletAmount;
    public float shootCoolDown;
    bool canShoot = true;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator anim;



    void ApplyHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            
        }
        
    }

    void Damage(float damageAmount)
    {
        health -= damageAmount;
        ApplyHearts();
        //healthBar.value = health;
        if (health == 3)
        {
            healthBar.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.yellow;
        }
        else if(health == 1)
        {
            healthBar.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
        }

        if (health <= 0)
        {          
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(2).parent = null;
            Destroy(gameObject);
        }
    }

    void Heal(float healAmount)
    {
        if(health < 5)
        {
            health += healAmount;
            ApplyHearts();
        }
        
    }

    void PlayerMove()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(speed * moveX, speed * moveY);
    }

    void PlayerShoot()
    {
        StartCoroutine(ShootCoolDown());
        bulletAmount -= 1;
        anim.SetTrigger("Shoot");
        GameObject bulletClone = Instantiate(bullet, bulletPoint.position, transform.rotation);
        Destroy(bulletClone, 2);
    }

    IEnumerator BuffEnergy()
    {
        hasEnergyBuff = true;
        energyBar.transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        hasEnergyBuff = false;
        energyBar.transform.GetChild(2).gameObject.SetActive(false);
    }

    IEnumerator ShootCoolDown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCoolDown);
        canShoot = true;
    }

    void EnergyRegen()
    {
        if(energy < 100)
        {
            energy += energyCost * Time.deltaTime;
            energyBar.value = energy;
        }
    }

    void PlayerInputs()
    {
        if(Input.GetKey(KeyCode.Space) & bulletAmount > 0 & canShoot)
        {
            PlayerShoot();
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(energy >= 1)
            {
                CancelInvoke("EnergyRegen");
                flame.SetActive(true);
                speed = 12;
                
                if(!hasEnergyBuff)
                {
                    energy -= energyCost * Time.deltaTime;
                    energyBar.value = energy;
                }
            }
            else
            {
                flame.SetActive(false);
                speed = 5;
            }
            
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            flame.SetActive(false);
            speed = 7;
            InvokeRepeating("EnergyRegen", 0, 0.01f);
        }

    }




    // Start is called before the first frame update
    void Start()
    {
        ApplyHearts();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerInputs();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Damage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "BulletEnemy(Clone)")
        {
            Destroy(collision.gameObject);
            Damage(1);
        }

        if (collision.name == "Fireball(Clone)")
        {
            Destroy(collision.gameObject);
            Damage(2);
        }

        if (collision.name == "Ammo(Clone)")
        {
            Destroy(collision.gameObject);
            bulletAmount += Random.Range(1, 11);
        }

        if (collision.name == "Heal(Clone)")
        {
            Destroy(collision.gameObject);
            Heal(1);
        }

        if (collision.name == "Buff(Clone)")
        {
            Destroy(collision.gameObject);
            StartCoroutine(BuffEnergy());
        }
    }

}
