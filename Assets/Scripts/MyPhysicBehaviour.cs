using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyPhysicBehaviour : MonoBehaviour
{
    Rigidbody2D rg;
    Collider2D cl;

    InputSystem_Actions PlayerInput;
    [SerializeField] float GoDeepDist = 1f;
    [SerializeField] float GoHighDist = 1f;
    [SerializeField] float Splash_Force = 2f;
    [SerializeField] float JumpForce=2f;
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
          Jumping();
    }

    void MakeConstrain()
    {
        // transform.eulerAngles = new Vector3(0, 0,
        // Mathf.Clamp(transform.eulerAngles.z, RotationLimit.x, RotationLimit.y));

        float z = transform.eulerAngles.z;

        // Convert to signed angle (-180 to +180)
        if (z > 180) z -= 360;

        // Clamp rotation
        z = Mathf.Clamp(z, RotationLimit.x, RotationLimit.y);

        // Apply rotation back
        transform.rotation = Quaternion.Euler(0, 0, z);
        // Vector2 pos = rg.position;
        // pos.x = Mathf.Clamp(pos.x, PlayerLimit.x, PlayerLimit.y);
        // rg.MovePosition(pos);
        // transform.position=new Vector3(0, 0,
        // Mathf.Clamp(transform.position.x, PlayerLimit.x, PlayerLimit.y));


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


    bool touch_surface = false;
    bool reach_deppest = false;
    bool reach_highest = false;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ocean")
        {
            if (!touch_surface)
            {
                Debug.Log("On Splash");
                water_surfece_position = transform.position;
                touch_surface = true;

            }

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "ocean")
        {
            if (!touch_surface)
            {
                Debug.Log("Float on Water");
                touch_surface = false;
                reach_deppest = false;
                reach_highest = false;

            }

        }
    }

    void GoDeep()
    {
        if (touch_surface && !reach_deppest)
        {
            if (Mathf.Abs(Vector2.Distance(water_surfece_position, transform.position)) >= GoDeepDist)
            {
                //Reach Deepest section 
                // Now 
                Debug.Log("Reach Deepest");
                rg.linearVelocity = Vector2.zero;
                reach_deppest = true;
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
        if (reach_deppest && !reach_highest)
        {
            if (Sp_coolDown)
            {
                //Reach Deepest section 
                // Now 
                Debug.Log("Reach Highest");
                rg.linearVelocity = Vector2.zero;
                cl.isTrigger = false;
                reach_highest = true;




            }
        }
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
