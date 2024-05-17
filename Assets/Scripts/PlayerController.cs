using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject focalPoint;
   
    public float speed = 1;
    private bool hasStrengthPowerup = false;
    private bool hasShooterPowerup = false;
    private float powerupStrength = 15;
    public GameObject powerupIndicator;
    public GameObject projectile;
    public float jumpForce = 500f;
    private string state = "Grounded";
    // Start is called before the first frame update
    IEnumerator StrengthPowerupCountdownRoutine() 
    {
        yield return new WaitForSeconds(7);
        hasStrengthPowerup = false;
        Debug.Log("Powerup over");
        powerupIndicator.gameObject.SetActive(false);
    }
    IEnumerator ShooterPowerupCountdownRoutine() 
    {
        yield return new WaitForSeconds(15);
        hasShooterPowerup = false;
        Debug.Log("Powerup over");
        powerupIndicator.gameObject.SetActive(false);
        StopCoroutine(ShooterPowerupCountdownRoutine());
        CancelInvoke();
        
    }
    void BulletRing() 
    {
        for(int i = 0; i < 12; i++)
        {
            Instantiate(projectile,transform.position,Quaternion.Euler(0,i*30,0));
        }
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
        Quaternion angle = transform.rotation;
        if(state == "Grounded" && Input.GetKeyDown(KeyCode.Space))
        {
           rb.AddForce(Vector3.up*jumpForce);
           state = "InAir";
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Strength"))
        {
            Debug.Log("Hit power");
            if(hasStrengthPowerup != true)
            {
                Debug.Log("collected Strength");
                hasStrengthPowerup = true;
                Destroy(other.gameObject);
                powerupIndicator.gameObject.SetActive(true);
                StartCoroutine(StrengthPowerupCountdownRoutine());
            }
          
        }
        if(other.CompareTag("Shooter"))
        {
            Debug.Log("Hit power");
            if(hasShooterPowerup != true)
            {
                Debug.Log("collected shooter");
                hasShooterPowerup = true;
                Destroy(other.gameObject);
                powerupIndicator.gameObject.SetActive(true);
                StartCoroutine(ShooterPowerupCountdownRoutine());
                InvokeRepeating("BulletRing",0f,2.0f);
                
            }
          
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasStrengthPowerup)
        {
            Rigidbody enemyRb =  rb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
             
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            Debug.Log("Collision with " + collision.gameObject.name + " With Powerup");

        }
        if(collision.gameObject.CompareTag("Ground"))
        {
            if(state == "InAir")
            {
                
                var obj = FindObjectsOfType<Enemy>();
                for(int i = 0;i < obj.Length-1; i++)
                {
                    Vector3 explosionPos = transform.position;
                    Rigidbody r = obj[i].GetComponent<Rigidbody>();
                    r.AddExplosionForce(600f,explosionPos,70f,5f);
                }
            }
            state = "Grounded";
            Debug.Log("Ground");
            
        }

    }
   
}

