using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipCollision : CollisionBehavior
{
    public float invincibilityTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibilityTime > 0)
        {
            gameObject.GetComponent<Ship>().invincible = true;
            invincibilityTime -= Time.deltaTime;
        }
        else
        {
            gameObject.GetComponent<Ship>().invincible = false;
        }
    }

    public override void Collide(GameObject obj)
    {
        if (obj.GetComponent<Asteroid>() != null && invincibilityTime <= 0)
        {
            //Collided with an asteroid
            Lives lives = gameObject.GetComponent<Ship>().gui.GetComponent<Lives>();
            lives.LifeCount -= 1;
            invincibilityTime = 3;

            if (lives.LifeCount == 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
