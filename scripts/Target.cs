using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRB;
    private GameManager gameManager;
    public ParticleSystem explosionParticle;

    private float minSpeed = 10f;
    private float maxSpeed = 14f;
    private float maxTorque = 10f;
    private float xRange = 4;
    private float ySpawnPos = -1;
    public int pointValue = 0;
    public float mouseSpeed;

    void Update()
    {
        mouseSpeed = Mathf.Abs(Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y"));
    }

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        targetRB = GetComponent<Rigidbody>();
        targetRB.AddForce(RandomForce(), ForceMode.Impulse);
        targetRB.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
    }

    Vector3 RandomForce() {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }
    float RandomTorque() {
        return Random.Range(-maxTorque, maxTorque);
    }
    Vector3 RandomSpawnPos() {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    private void OnMouseOver()
    {
        if (gameManager.isGameActive && Input.GetMouseButton(0) && mouseSpeed > 0.25)
        {
            if (gameObject.CompareTag("Bad"))
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.UpdateScore(pointValue);
            }

        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sensor") && !gameObject.CompareTag("Bad") && gameManager.isGameActive)
        {
            gameManager.UpdateScore(-pointValue*3);
            Destroy(gameObject);
        }
    }
}
