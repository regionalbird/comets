using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbital_gravity : MonoBehaviour
{
    private Vector2 device_position; // place to store device position
    Rigidbody2D ball_rigidbody; // place to store ball rigidbody
    private GameObject[] devices; // array of all devices updated every frame
    private Vector2 total_force; // place to store the aggregate force on each ball
    private Vector2 ball_position; // place to store ball positions

    public float ball_mass = 10.0f;
    public float device_mass = 10.0f;
    public float G = 6.67f*Mathf.Pow(10,-11); // constant of gravity
    
    // scaler for device mass
    // (would need to be huge for it to exert any gravitational force on the ball)
    public float scaler = 1*Mathf.Pow(10,8); 

    // gravity is only active inside of the range
    public float inner_ratio = 0.5f;
    public float outer_range = 6.0f;
    private float inner_range;
    

    // compresses the increase in gravity as the ball gets closer to the device
    public float compressor = 0.9f;

    private Vector2 starting_force;
    public float force_magnitude;
    private Vector2 overall_starting_force;

    private AudioSource source;
    private float xsquare;
    private int g;

    public SpriteRenderer rend;  // renderer variable
    private CircleCollider2D collid;

    //public GameObject ship;
    public static int canfire;
    
    
    void Start()
    {
        source = GetComponent<AudioSource>();
        ball_rigidbody = GetComponent<Rigidbody2D>(); // u know what it do baby
        //starting_force = new Vector2(1.0f,Mathf.Sqrt(3));
        overall_starting_force = (force_magnitude*starting_force/starting_force.magnitude);
        //ball_rigidbody.AddForce(overall_starting_force, ForceMode2D.Impulse);
        GameObject ship = GameObject.Find("ship");
        ship_controller ship_controller = ship.GetComponent<ship_controller>();
        canfire = ship_controller.can_fire;
    }

    void FixedUpdate()
    {
        inner_range = outer_range * inner_ratio;
        // populate our devices array with all devices currently instantiated (this feels clever!)
        devices = GameObject.FindGameObjectsWithTag("Device");
        ball_position = ball_rigidbody.position; // update ball position based off physics
        // apply the total force of all the devices the ball (calls calculate_force() function)
        ball_rigidbody.AddForce(calculate_force(), ForceMode2D.Impulse);
        canfire = ship_controller.can_fire;
        Debug.Log(canfire);
        if (Input.GetButtonDown("Click") && canfire == 1)
        {
            GravPulse();
        }
    }
 /*
    void Update()
    {
        ScreenWrapping();
    }
    */

    public Vector2 calculate_force(){
        total_force = new Vector2(0.0f,0.0f); // reset total_force to zero each update
        foreach (var device in devices) // iterate through the devices array
        {
            device_position = device.transform.position; // store the position of the device
            float distance = Vector2.Distance(device_position, ball_position); // distance between ball and device
            if (distance <= outer_range) // sets a radius so devices only affect within radius
            { // sets an inner range so the force caps out and doesn't shoot off to infinity
                if (distance <= inner_range){ distance = inner_range;
            }
            float distance_squared = compressor * distance * distance; // ??? not sure why we do this here
            // calculate the force applied to the ball by the device
            float force = (scaler * G * device_mass * ball_mass) / (distance_squared);
            Vector2 heading = (device_position - ball_position); // store the direction to apply the force in
            Debug.DrawLine(device_position, ball_position); // draw urself the vector
            // compute the force vector by normalizing the heading and multiplying it by the force
            Vector2 force_with_direction = (force * (heading/heading.magnitude));
            //force_with_direction.y = 0.0f;
            //if (force_with_direction.x < 0.0f) {force_with_direction.x=0.0f;}
            total_force += force_with_direction; // add force to sum of forces from all devices
            }
            else
            {total_force += new Vector2(0.0f, 0.0f);} // if the device is out of range apply no force for it
        }
        return total_force; // spits out the total force of all devices, which is applied in update
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ball")
        {
            //gameObject.SetActive(false);
            xsquare = Mathf.Pow(ball_position.x, 2);
            source.panStereo = Mathf.Sign(ball_position.x)*(xsquare/(1+xsquare));
            source.Play();
            rend = GetComponent<SpriteRenderer>(); // gets sprite renderer

            rend.enabled = false; // sets to false if hit.

            collid = GetComponent<CircleCollider2D>();

            collid.enabled = false;
            Destroy(gameObject, 2f); 
        }
        
    }
    
    void GravPulse()
    {
        ball_rigidbody.AddForce(10*calculate_force(), ForceMode2D.Impulse);
    }
    
    
    /*void OnBecameInvisible() {
        Destroy(gameObject); // when a ball goes offscreen, kill it
    }*/
    
}
