using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody))]
public class SpaceShipControl : MonoBehaviour
{
    [Header("=== Ship Movement Settings ===")]
    [SerializeField] private float yawTorque = 500f;
    [SerializeField] private float pitchTorque = 1000f;
    [SerializeField] private float rollTorque = 1000f;
    [SerializeField] private float thrust = 100f;
    [SerializeField] private float upThrust = 50f;
    [SerializeField] private float strafeThrust = 50f;
    [SerializeField, Range(0.001f, 0.999f)] private float thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)] private float upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)] private float leftRightGlideReduction = 0.111f;
    float glide, verticalGlide, horizontalGlide = 0f;
    [SerializeField] private GameObject explodeParticle;
    [SerializeField] private Transform tm;
    [SerializeField] private ParticleSystem boostParticles;

    [Header("=== Boost Settings ===")]
    [SerializeField] private float maxBoostAmt = 2f;
    [SerializeField] private float boostDepricationRate = 0.25f;
    [SerializeField] private float boostRechargerate = 0.5f;
    [SerializeField] private float boostMultiplier = 5f;
    public bool boosting = false;
    public float curBoostAmt;


    Rigidbody rb;


    //Input Values
    private float thrust1D;
    private float strafe1D;
    private float upDown1D;
    private float roll1D;
    private Vector2 pitchYaw;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        curBoostAmt = maxBoostAmt;
    }


    void FixedUpdate()
    {
        HandleBoosting();
        HandleMovement();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we collided with has a specific tag, e.g., "Enemy"
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Destroy the player spaceship
            Instantiate(explodeParticle, tm.position, tm.rotation);
            SceneManager.LoadScene("Lose Scene", LoadSceneMode.Single);
        }
    }


    void HandleBoosting()
    {
    
      if (boosting && curBoostAmt >0f) 
      {
            curBoostAmt -= boostDepricationRate;
            if (!boostParticles.isPlaying)
            {
                boostParticles.Play(); // Start the particle system when boosting begins
            }
            if (curBoostAmt<=0f)
            {
                boosting=false;
            }
      }
      else
      {
         if (boostParticles.isPlaying)
         {
            boostParticles.Stop(); // Stop the particle system when not boosting
         }
         
         if (curBoostAmt < maxBoostAmt)
         {
            curBoostAmt += boostRechargerate;
         }
       }
        
    }


    void HandleMovement()
    {
        //Roll
        rb.AddRelativeTorque(Vector3.back * roll1D * rollTorque * Time.deltaTime);
        //Pitch
        rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * pitchTorque * Time.deltaTime);
        //
        rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * yawTorque * Time.deltaTime);

        //Thrust
        if (thrust1D > 0.1f || thrust1D < -0.1f)
        {
            float curThrust;

            if(boosting)
            {
                curThrust = thrust * boostMultiplier;
            }
            else
            {
                curThrust = thrust;
            }


            rb.AddRelativeForce(Vector3.forward * thrust1D * curThrust * Time.deltaTime);
            glide = thrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.forward * glide * Time.deltaTime);
            glide *= thrustGlideReduction;
        }

        //UpDown
        if (upDown1D > 0.1f || upDown1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.up * upDown1D * upThrust * Time.fixedDeltaTime);
            verticalGlide = upDown1D * upThrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.up * verticalGlide * Time.fixedDeltaTime);
            verticalGlide = upDownGlideReduction;
        }

        // STRAFING
        if (strafe1D > 0.1f || strafe1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.right * strafe1D * upThrust * Time.fixedDeltaTime);
            horizontalGlide = strafe1D * strafeThrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.right*strafe1D*upThrust * Time.fixedDeltaTime);
            horizontalGlide = leftRightGlideReduction;
        }

       

    }
   
    
    #region Input Methods
    public void OnThrust(InputAction.CallbackContext context)
    {
        thrust1D = context.ReadValue<float>();
    }

    public void OnStrafe(InputAction.CallbackContext context)
    {
        strafe1D = context.ReadValue<float>();
    }

    public void OnUpDown(InputAction.CallbackContext context)
    {
        upDown1D = context.ReadValue<float>();
    }

    public void onRoll(InputAction.CallbackContext context)
    {
        roll1D = context.ReadValue<float>();
    }

    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        boosting = context.performed;
    }

    #endregion
 }


