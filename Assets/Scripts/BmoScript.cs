using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BmoScript : MonoBehaviour
{
    public float health;

    void Damage(float damageAmount)
    {
        health -= damageAmount;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "BulletFinn(Clone)")
        {
            Destroy(collision.gameObject);
            Damage(1);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
