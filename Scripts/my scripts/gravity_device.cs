using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravity_device : MonoBehaviour
{

    Rigidbody2D device_rigidbody; // Rigidbody2D to store the device's rigidbody in
    //public static float range = 5.0f; // how far does the gravity reach? no effect outside of range
    private Vector2 device_position; // store the position
    public float speed = 10.0f;

    public GameObject ball;
    public static float range;

    private Vector2 dir;
    public Vector2 center;
    private Vector2 mouse_position;

    public float my_torque = 5.0f;

    // Start is called before the first frame update
    void Awake()
    {
        
        device_rigidbody = GetComponent<Rigidbody2D>(); // grab that rigidbody
        device_position = device_rigidbody.position; // store position based on rigidbody position
        //GameObject ball = GameObject.Find("Ball");
        orbital_gravity orbital_gravity = ball.GetComponent<orbital_gravity>();
        range = orbital_gravity.outer_range;
        GameObject ship = GameObject.Find("ship");
        ship_controller ship_controller = ship.GetComponent<ship_controller>();
        mouse_position = ship_controller.mouseWorldPos;
        dir = mouse_position - device_position;
        device_rigidbody.AddForce(speed * (dir/dir.magnitude),ForceMode2D.Impulse);
        device_rigidbody.AddTorque(-1*my_torque, ForceMode2D.Impulse);   
    }

    void Start()
    {
        /*
        device_rigidbody = GetComponent<Rigidbody2D>(); // grab that rigidbody
        device_position = device_rigidbody.position; // store position based on rigidbody position
        orbital_gravity orbital_gravity = ball.GetComponent<orbital_gravity>();
        range = orbital_gravity.outer_range;
        GameObject ship = GameObject.Find("ship");
        ship_controller ship_controller = ship.GetComponent<ship_controller>();
        mouse_position = ship_controller.mouseWorldPos;
        dir = mouse_position - device_position;
        device_rigidbody.AddForce(speed * (dir/dir.magnitude),ForceMode2D.Impulse);
        */
    }

    void FixedUpdate()
    {
        //device_rigidbody.AddForce(speed * (dir/dir.magnitude),ForceMode2D.Impulse);
    }
    
    void OnDrawGizmos()
    {
        // draw a circle around each device in the gizmos pane so u can see its range
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(device_position, range);
    }
    
    // Update is called once per frame
    void OnBecameInvisible() {
        Destroy(gameObject); // when a ball goes offscreen, kill it
    }
    
    
}
