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
        ClickManager();
        Surffing();
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
            if (state == PlayerState.Air_Pressed)
            {
                if (GetSlop(contact))
                {
                    //UpHill sink
                    state = PlayerState.Surface_Pressed_Bad;
                }
                else
                {
                    //DownHill boost speed
                    state = PlayerState.Surface_Pressed_Good;
                }
            }

            if (state == PlayerState.Surface_Pressed_Bad)
            {

            }
            if (state == PlayerState.Surface_Pressed_Good)
            {

            }



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
                
                state = PlayerState.Air;
                rg.linearVelocity = Vector2.zero;
                cl.isTrigger = false;
                
            }
        }
    }

   
    void Surffing()
    {
        switch (state)
        {
            case PlayerState.Air_Pressed:
                PressInAir();
                break;
            case PlayerState.Air:
                NoPressAir();
                break;
            case PlayerState.Surface_Pressed_Bad:
                SurfaceBadPress();
                break;
            case PlayerState.Surface_Pressed_Good:
                SurfaceGoodPress();
                break;

        }

    }

     bool First_Time = true;
    void PressInAir()
    {
                if (First_Time)
                {
                    First_Time = false;
                    rg.linearVelocityY -= 5;
                    BoardRotation(-30);
                    cl.isTrigger = false;
                }
    }

    void NoPressAir()
    {                if (!First_Time)
                {
                    rg.linearVelocityY += 2;
                    BoardRotation(0);
                    cl.isTrigger = true;
                    First_Time = true;
                }
    }

    void SurfaceGoodPress()
    {
        Debug.Log("SurfaceGoodPress");

        rg.linearVelocity = new Vector2(10, rg.linearVelocity.y);
        state = PlayerState.Surface;
    }
    
    void SurfaceBadPress()
    {
        Debug.Log("SurfaceBadPress");
        state = PlayerState.Surface;
    }
    public void Jumping()
    {
        if (PlayerInput.UI.Click.IsPressed() && state == PlayerState.Surface)
        {
            Debug.Log("PRESSED");
            rg.AddForceY(JumpForce);
            state = PlayerState.Air;
            // rg.AddForceX(JumpForce / 2);

        }
    }

}
