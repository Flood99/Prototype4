using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject focalPoint;
    public float speed = 1;
    private bool hasPowerup = false;
    private float powerupStrength = 15;
    public GameObject powerupIndicator;
    // Start is called before the first frame update
     IEnumerator PowerupCountdownRoutine() 
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        Debug.Log("Powerup over");
        powerupIndicator.gameObject.SetActive(false);
    }
    void Start()
    {
       
        rb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        rb.AddForce(focalPoint.transform.forward*forwardInput*speed);
        powerupIndicator.transform.position = transform.position;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            Debug.Log("Hit power");
            if(hasPowerup != true)
            {
                Debug.Log("collected power");
                hasPowerup = true;
                Destroy(other.gameObject);
                powerupIndicator.gameObject.SetActive(true);
                StartCoroutine(PowerupCountdownRoutine());
            }
          
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb =  rb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
             
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            Debug.Log("Collision with " + collision.gameObject.name + " With Powerup");

        }

    }
   
}

