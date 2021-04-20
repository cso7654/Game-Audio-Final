using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Ship : MonoBehaviour {

    public Vector3 position;
    public Vector3 direction;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 accelerationDirection;

    public float speed = 0.1f;
    public float maxSpeed = 0.5f;
    public float deltaSpeed = 0.1f;
    public float deceleration = 0.5f;
    public float turnSpeed = 4f;

    public float health = 1f;

    public GameObject bulletPrefab;
    public float bulletsPerSecond;
    public float timeSinceLastFire;
    public float bulletSpeed;

    public ParticleSystem thrusters;

    public GameObject collisionManager;
    public GameObject gui;

    public StudioEventEmitter orientationFMOD;
    public StudioEventEmitter thrusterFMOD;
    public StudioEventEmitter ambientCreakFMOD;

    public bool invincible;
    float flickerTimer;

	// Use this for initialization
	void Start () {
        position = new Vector3(0, 0);
        direction = new Vector3(1, 0);
        velocity = new Vector3(0, 0);
        acceleration = new Vector3(0, 0);
        thrusters = gameObject.GetComponentInChildren<ParticleSystem>();
        thrusters.Stop();
        //fmodEmitter = GetComponent<StudioEventEmitter>();
	}
	
	// Update is called once per frame
	void Update () {
        Turning();

        Acceleration();

        ambientCreakFMOD.SetParameter("Health", health);

        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
        if (speed < -maxSpeed)
        {
            speed = -maxSpeed;
        }

        acceleration = speed * accelerationDirection * Time.deltaTime;
        velocity += acceleration;

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            velocity *= deceleration;
            speed *= deceleration;

            if (Mathf.Abs(speed) <= 0.0005f || velocity.magnitude <= 0.0005f)
            {
                speed = 0;
                velocity = Vector3.zero;
            }
        }

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        position += velocity;

        Wrap();

        transform.position = position;

        float zAngle = Mathf.Atan2(direction.y, direction.x);
        zAngle *= Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, zAngle);

        UpdateBullets();

        Flicker();
    }

    void Turning()
    {
        int turnVal = 0;

        if (Input.GetKey(KeyCode.E))
        {
            turnVal = 1;
            direction = Quaternion.Euler(0, 0, -turnSpeed / 2) * direction;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            turnVal = 1;
            direction = Quaternion.Euler(0, 0, turnSpeed / 2) * direction;
        }
        if (Input.GetKey(KeyCode.D))
        {
            turnVal = 2;
            direction = Quaternion.Euler(0, 0, -turnSpeed) * direction;
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnVal = 2;
            direction = Quaternion.Euler(0, 0, turnSpeed) * direction;
        }

        orientationFMOD.SetParameter("Turning", turnVal);

        if (orientationFMOD.IsPlaying() && turnVal == 0)
        {
            orientationFMOD.Stop();
        }else if (!orientationFMOD.IsPlaying() && turnVal > 0)
        {
            orientationFMOD.Play();
        }
    }

    void Acceleration()
    {
        thrusterFMOD.SetParameter("Health", health);

        if (Input.GetKey(KeyCode.W))
        {
            speed += deltaSpeed;
            accelerationDirection = direction;
            thrusters.Play();
            if (!thrusterFMOD.IsPlaying())
            {
                thrusterFMOD.Play();
            }
            ParticleSystem.ShapeModule shape = thrusters.shape;
            shape.rotation = new Vector3(0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            speed -= deltaSpeed;
            accelerationDirection = direction;
            thrusters.Play();
            if (!thrusterFMOD.IsPlaying())
            {
                thrusterFMOD.Play();
            }
            ParticleSystem.ShapeModule shape = thrusters.shape;
            shape.rotation = new Vector3(180, 0);
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            thrusters.Stop();
            thrusterFMOD.Stop();
        }
    }

    void Wrap()
    {
        //Wrapping
        //Use some variables for world size because constants make me want to stop being alive
        float edgeX = Camera.main.GetComponent<PlayField>().width / 2;
        float edgeY = Camera.main.GetComponent<PlayField>().height / 2;
        float deltaX = Camera.main.GetComponent<PlayField>().center.x;
        float deltaY = Camera.main.GetComponent<PlayField>().center.y;

        if (position.x > edgeX + 1 - deltaX)
        {
            position.x = -(edgeX + 0.5f) - deltaX;
        }
        if (position.x < -(edgeX + 1) - deltaX)
        {
            position.x = edgeX + 0.5f - deltaX;
        }
        if (position.y > edgeY + 1 - deltaY)
        {
            position.y = -(edgeY + 0.5f) - deltaY;
        }
        if (position.y < -(edgeY + 1) - deltaY)
        {
            position.y = edgeY + 0.5f - deltaY;
        }
    }

    void UpdateBullets()
    {
        //Bullets
        timeSinceLastFire += Time.deltaTime;

        if ((Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space)) && timeSinceLastFire >= 1f / bulletsPerSecond)
        {
            //Reset time
            timeSinceLastFire = 0;

            //Create new bullet
            GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
            //Set velocity and position
            Bullet bulletBehavior = bullet.GetComponent<Bullet>();

            bulletBehavior.position = position;
            bulletBehavior.direction = direction;
            bulletBehavior.speed = bulletSpeed;
            bulletBehavior.velocity = direction * bulletSpeed;
            bulletBehavior.collisionManager = collisionManager;

            collisionManager.GetComponent<CollisionDetection>().objects.Add(bullet);
        }
    }

    void Flicker()
    {
        flickerTimer += Time.deltaTime;
        if (flickerTimer > 0.1)
        {
            flickerTimer = 0;
        }

        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        if (invincible && flickerTimer == 0)
        {
            //Switch between colors to make a flickering/blinking effect to show invincibility time
            sr.color = sr.color == Color.white ? Color.red: Color.white;
        }
        
        //Make sure ship is never red when it isn't invincible
        if (!invincible)
        {
            sr.color = Color.white;
        }
    }
}
