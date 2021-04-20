using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Asteroid : MonoBehaviour
{
    public Vector3 position;
    public Vector3 direction;
    public Vector3 velocity;

    public float speed = 0.1f;
    public float splitAngleRange;

    public GameObject collisionManager;
    public GameObject asteroidSpawner;
    public GameObject explosionParticles;

    public StudioEventEmitter breakFMOD;
    public StudioEventEmitter destroyFMOD;

    public int stage = 0;

    public GameObject gui;

    bool destroy;
    bool split;

    // Use this for initialization
    void Start()
    {
        float brightness = Random.Range(0.25f, 1);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(brightness, brightness, brightness, 255);
    }

    // Update is called once per frame
    void Update()
    {
        position += velocity;

        transform.position = position;

        float zAngle = Mathf.Atan2(direction.y, direction.x);
        zAngle *= Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, zAngle);

        //Clearing
        float edgeX = Camera.main.GetComponent<PlayField>().width / 2;
        float edgeY = Camera.main.GetComponent<PlayField>().height / 2;
        float deltaX = Camera.main.GetComponent<PlayField>().center.x;
        float deltaY = Camera.main.GetComponent<PlayField>().center.y;

        if (position.x > edgeX + 1 - deltaX ||
            position.x < -(edgeX + 1) - deltaX ||
            position.y > edgeY + 1 - deltaY ||
            position.y < -(edgeY + 1) - deltaY)
        {
            destroy = true;
        }

        if (split)
        {
            split = false;

            //Create two smaller asteroids
            GameObject ast1 = Instantiate(gameObject);
            ast1.transform.localScale = new Vector3(0.15f, 0.15f, 1);
            Asteroid ast1Behavior = ast1.GetComponent<Asteroid>();
            //Speed up and modify angle
            ast1Behavior.speed *= 1.25f;
            ast1Behavior.direction = Quaternion.Euler(0, 0, Random.Range(-splitAngleRange, 0f)) * direction;
            ast1Behavior.velocity = ast1Behavior.speed * ast1Behavior.direction;
            ast1Behavior.stage = 1;
            collisionManager.GetComponent<CollisionDetection>().objects.Add(ast1);

            GameObject ast2 = Instantiate(gameObject);
            ast2.transform.localScale = new Vector3(0.15f, 0.15f, 1);
            Asteroid ast2Behavior = ast2.GetComponent<Asteroid>();
            //Speed up and modify angle
            ast2Behavior.speed *= 1.25f;
            ast2Behavior.direction = Quaternion.Euler(0, 0, Random.Range(0f, splitAngleRange)) * direction;
            ast2Behavior.velocity = ast2Behavior.speed * ast2Behavior.direction;
            ast2Behavior.stage = 1;
            collisionManager.GetComponent<CollisionDetection>().objects.Add(ast2);


            destroy = true;
        }
        if (destroy)
        {
            collisionManager.GetComponent<CollisionDetection>().objects.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void Break()
    {
        //Create explosion
        GameObject explosion = Instantiate(explosionParticles, asteroidSpawner.transform);
        explosion.transform.position = gameObject.transform.position;
        explosion.GetComponent<ParticleSystem>().Play();

        if (stage == 0)
        {
            //Big asteroid

            split = true;

            //Play sound
            breakFMOD.Play();

            //Increase score by 10
            gui.GetComponent<Score>().Points += 10;
        }
        else
        {
            //Small asteroid

            destroy = true;

            //Increase score by 20
            gui.GetComponent<Score>().Points += 20;

            //Play sound
            destroyFMOD.Play();

            //Increase asteroid spawn rate and speed
            asteroidSpawner.GetComponent<SpawnAsteroid>().timeBetweenSpawns -= 0.05f;
            asteroidSpawner.GetComponent<SpawnAsteroid>().averageSpeed += 0.001f;
        }

    }
}
