using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Shoot")]
    [Space(5)]
    public GameObject bullet;
    public Transform bulletPoint;
    public float bulletForce;
    public Transform player;

    [Header("Components")]
    [Space(5)]
    public Animator anim;

    [Header("Drop")]
    public GameObject[] dropObject;
    public GameObject explosion;



    void Explode()
    {
        GameObject explosionClone = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(explosionClone, 0.7f);
    }


    void DropObjects()
    {

        float dropChance = Random.Range(0, 101);
        if (dropChance <= 30 & dropChance >= 10)
        {
            GameObject ammoClone = Instantiate(dropObject[0], transform.position, transform.rotation);
            Destroy(ammoClone, 3);
        }
        else if(dropChance <= 10)
        {
            GameObject healClone = Instantiate(dropObject[1], transform.position, transform.rotation);
            Destroy(healClone, 2);
        }
        else if (dropChance >= 30 & dropChance <= 40)
        {
            GameObject buffClone = Instantiate(dropObject[2], transform.position, transform.rotation);
            Destroy(buffClone, 2);
        }
    }

    void SpawnBullet()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.Normalize();
        if(gameObject.name == "Enemy(Clone)")
        {
            anim.SetTrigger("Shoot");
        }
        GameObject bulletClone = Instantiate(bullet, bulletPoint.position, transform.rotation);
        bulletClone.GetComponent<Rigidbody2D>().AddForce(directionToPlayer * bulletForce, ForceMode2D.Impulse);
        Destroy(bulletClone, 4);
    }


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBullet", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "BulletPlayer")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            DropObjects();
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Explode();
        }
    }

}
