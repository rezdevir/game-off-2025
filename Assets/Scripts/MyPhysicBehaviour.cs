using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


enum PlayerState { Air_Pressed, Surface_Pressed_Good,Surface_Pressed_Bad, none, Air, Surface,InDeepWater}
public class MyPhysicBehaviour : MonoBehaviour
{
    Rigidbody2D rg;
    Collider2D cl;
    PlayerState state=PlayerState.Air;
    InputSystem_Actions PlayerInput;
    [SerializeField] float GoDeepDist = 1f;
    [SerializeField] float GoHighDist = 1f;
    [SerializeField] float Splash_Force = 2f;
    [SerializeField] float JumpForce = 2f;

    [SerializeField] float Rotaion_Speed = 100f;
    [SerializeField] float2 RotationLimit;
    [SerializeField] float2 PlayerLimit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        PlayerInput = new InputSystem_Actions();
        PlayerInput.UI.Click.performed += PressClick;
        PlayerInput.UI.Click.canceled += CancelClick;
        rg = GetComponent<Rigidbody2D>();
        cl = GetComponent<Collider2D>();
        cl.isTrigger = true;
        PlayerInput.Enable();
    }
    void PressClick(InputAction.CallbackContext a)
    {

    }

    void CancelClick(InputAction.CallbackContext a)
    {
        rg.linearVelocity = Vector2.zero;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        MakeConstrain();
        GoDeep();
        GoUp();


    }

    void Update()
    {
        //   Jumping();
        ClickManager();
    }

    void BoardRotation(float angle)
    {
        float deg = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, Time.deltaTime * Rotaion_Speed);
        transform.rotation = Quaternion.Euler(0, 0, deg);

    }

    void ClickManager()
    {
        if (PlayerInput.UI.Click.IsPressed())
        {
            if(state==PlayerState.Air)
            {
                // rg.linearVelocityY -= 5;
                state = PlayerState.Air_Pressed;
                
            }

        }
        else
        {
             if(state==PlayerState.Air_Pressed)
            {
                state = PlayerState.Air;
            }
        }

    }


    void MakeConstrain()
    {


        float z = transform.eulerAngles.z;

        // Convert to signed angle (-180 to +180)
        if (z > 180) z -= 360;

        // Clamp rotation
        z = Mathf.Clamp(z, RotationLimit.x, RotationLimit.y);

        // Apply rotation back
        transform.rotation = Quaternion.Euler(0, 0, z);



        Vector2 pos = rg.position;

        // Clamp X position
        if (pos.x < PlayerLimit.x)
        {
            pos.x = PlayerLimit.x;
            rg.linearVelocity = new Vector2(0, rg.linearVelocity.y); // stop going outside
        }
        else if (pos.x > PlayerLimit.y)
        {
            pos.x = PlayerLimit.y;
            rg.linearVelocity = new Vector2(0, rg.linearVelocity.y);
        }

        // rg.MovePosition(pos);


    }

    Vector2 water_surfece_position;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ocean")
        {
            if (state == PlayerState.Air)
            {
                Debug.Log("On Splash");
                water_surfece_position = transform.position;
                state = PlayerState.Surface;

            }
 

        }
    }


    float surfaceAngle; // store last slope angle

    void OnCollisionStay2D(Collision2D collision)
    {
        // Check if we're colliding with the wave
        if (collision.collider.CompareTag("ocean"))
        {
            // Get first contact point
          
            ContactPoint2D contact = collision.GetContact(0);
            Debug.Log("Get Slope Stay:" + GetSlop(contact));




        }
    }

    bool GetSlop(ContactPoint2D contact)
    {
        Vector2 normal = contact.normal;

        // Convert normal to surface angle
        surfaceAngle = Mathf.Atan2(normal.x, normal.y) * Mathf.Rad2Deg;
        // Debug.Log(surfaceAngle);
        if (surfaceAngle < 0)
        {
            Debug.Log("Up Hill");
            // IsUpHill = true;
            return true;
        }
        else
        {
            // IsUpHill = false;
            Debug.Log("Down Hill");
            return false;
        }
    }

    void GoDeep()
    {
        if (state == PlayerState.Surface )
        {

            if (Mathf.Abs(Vector2.Distance(water_surfece_position, transform.position)) >= GoDeepDist)
            {
                //Reach Deepest section 
                // Now 
                Debug.Log("Reach Deepest");
                rg.linearVelocity = Vector2.zero;
                state = PlayerState.InDeepWater;
                StartCoroutine(CoolDown_Splash());
                rg.AddForceY(Splash_Force);
            }
        }
    
    }

    bool Sp_coolDown = true;
    IEnumerator CoolDown_Splash()
    {
        Sp_coolDown = false;
        yield return new WaitForSeconds(1f);
        Sp_coolDown = true;
    }
    void GoUp()
    {
        if (state == PlayerState.InDeepWater)
        {
            if (Sp_coolDown)
            {
                //Reach Highest section 
                // Now 
                state = PlayerState.Air;
                rg.linearVelocity = Vector2.zero;
                cl.isTrigger = false;
                
            }
        }
    }

    void Surffing()
    {
        //This Act Just when Pressed click
    }
    public void Jumping()
    {
        if (PlayerInput.UI.Click.IsPressed())
        {
            Debug.Log("PRESSED");
            rg.AddForceY(JumpForce);
            rg.AddForceX(JumpForce / 2);

        }
    }

}
