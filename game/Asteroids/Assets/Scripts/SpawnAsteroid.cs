using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SpawnAsteroid : MonoBehaviour
{
    float timeSinceLastSpawn;

    public GameObject prefab;
    public float timeBetweenSpawns;
    public float averageSpeed;
    public float speedVariance;

    float fieldWidth;
    float fieldHeight;
    Vector2 fieldCenter;

    public StudioEventEmitter breakFMOD;

    public GameObject collisionManager;
    public GameObject gui;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Increment time
        timeSinceLastSpawn += Time.deltaTime;

        //Check if an asteroid should spawn
        if (timeSinceLastSpawn > timeBetweenSpawns)
        {
            //No need to update vars unless we're spawning a new asteroid
            UpdateFieldVars();

            //Generate a point to spawn at
            Vector2 randPoint = RandomEdgePoint();
            Vector3 point = new Vector3(randPoint.x, randPoint.y, 0);

            //Get angle of point to center
            float baseTheta = Mathf.Atan2(fieldCenter.y - point.y, fieldCenter.x - point.x);
            //Get a random displacement of up to pi/4 radians away from the angle.
            float deltaTheta = Random.Range(-Mathf.PI / 4f, Mathf.PI / 4f);

            //Generate a random variance based on the max variance
            float deltaVel = Random.Range(-speedVariance, speedVariance);

            GameObject asteroid = Instantiate(prefab, new Vector3(point.x, point.y, 0), Quaternion.Euler(0, 0, Mathf.Rad2Deg * (baseTheta + deltaTheta)));

            //Apply speed and angle
            Asteroid astBehavior = asteroid.GetComponent<Asteroid>();
            astBehavior.speed = averageSpeed + deltaVel;
            astBehavior.position = point;
            astBehavior.direction = Quaternion.Euler(0, 0, Mathf.Rad2Deg * (baseTheta + deltaTheta)) * new Vector3(1, 0);
            astBehavior.velocity = astBehavior.speed * astBehavior.direction;
            astBehavior.collisionManager = collisionManager;
            astBehavior.gui = gui;
            astBehavior.asteroidSpawner = gameObject;
            astBehavior.breakFMOD = breakFMOD;

            collisionManager.GetComponent<CollisionDetection>().objects.Add(asteroid);

            timeSinceLastSpawn = 0;
        }
    }

    void UpdateFieldVars()
    {
        fieldWidth = Camera.main.GetComponent<PlayField>().width;
        fieldHeight = Camera.main.GetComponent<PlayField>().height;
        fieldCenter = Camera.main.GetComponent<PlayField>().center;
    }

    Vector2 RandomEdgePoint()
    {
        float rand = Random.Range(0f, 1f);
        Vector2 point;

        //Randomly select either horizontal or vertical edges. 50/50 chance of rand value being below 0.5 to select between 2 options
        if (rand < 0.5)
        {
            //Horizontal

            //Generate a random x value along the horizontal
            float x = Random.Range(fieldCenter.x - fieldWidth / 2 - 1, fieldCenter.x + fieldWidth / 2 + 1);

            //Randomly select top or bottom. 50/50 chance of rand value being below 0.25 since at this point we know it is below 0.5, otherwise we wouldn't be here, would we?
            if (rand < 0.25)
            {
                //Top
                point = new Vector2(x, fieldCenter.y + fieldHeight / 2 + 1);
            }
            else
            {
                //Bottom
                point = new Vector2(x, fieldCenter.y - fieldHeight / 2 - 1);
            }
        }
        else
        {
            //Vertical

            //Generate a random y value along the vertical
            float y = Random.Range(fieldCenter.y - fieldHeight / 2 - 1, fieldCenter.y + fieldHeight / 2 + 1);

            //Randomly select left or right. 50/50 chance of rand value being below 0.75 after getting here
            if (rand < 0.75)
            {
                //Left
                point = new Vector2(fieldCenter.x - fieldWidth / 2 - 1, y);
            }
            else
            {
                //Right
                point = new Vector2(fieldCenter.x + fieldWidth / 2 + 1, y);
            }
        }

        return point;
    }
}
