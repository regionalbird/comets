using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ship_controller : MonoBehaviour
{
    public float speed_x = 5.0f;
    private Rigidbody2D rigidbody2D;
    public GameObject gravity_device;

    public AudioSource audioSource;
    public AudioClip shot;
    public AudioClip dam;
    public float shot_volume = 0.5f;
 
    private Vector2 dir;
    private Vector2 move_dir;

    public Vector2 mouseWorldPos;

    private int health;
    private bool is_hurt;

    //private Animation anim;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        health = 5;
        //anim = gameObject.GetComponent<Animation>();
        is_hurt = false;
    }


    void FixedUpdate()
    {
        MoveShip();
        FaceDirection();
        if (rigidbody2D.velocity != Vector2.zero)
        {
            if (!audioSource.isPlaying)
            {
                //audioSource.Play();
            }
        }
    }

    void Update()
    {
        FireDevice();
    }

    void MoveShip()
    {
        /*
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody2D.velocity = new Vector2(Time.deltaTime * speed_x, 0.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody2D.position += new Vector2(-(Time.deltaTime * speed_x), 0.0f);
        }
        
        rigidbody2D.velocity = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed_x;
        if (rigidbody2D.velocity.x >= 0.2f || rigidbody2D.velocity.y >= 0.2f)
        {
            anim.Play("idle_anim");
        }
        */
        move_dir = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed_x;
        rigidbody2D.AddForce(move_dir,ForceMode2D.Force);
    }

    void FaceDirection()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // store mouse position
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos); // gotta cast screen position to world position
        dir =  mouseWorldPos - rigidbody2D.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90.0f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FireDevice()
    {
        if (Input.GetButtonDown("Click"))
        {
            GameObject device = Instantiate(gravity_device,rigidbody2D.position + 4*dir/dir.magnitude, Quaternion.identity);
            device.tag = "Device";
            audioSource.PlayOneShot(shot, shot_volume);
            Debug.Log(Input.GetButtonDown("Click"));
        }
    }

    void OnCollisionEnter2D()
    {
        if (is_hurt == false)
        {
            is_hurt = true;
            health -= 1;
            audioSource.PlayOneShot(dam, shot_volume);
            StartCoroutine(Damage());
            if (health == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator Damage()
    {
        gameObject.GetComponent<Renderer>().enabled = false;           //show
        yield return new WaitForSeconds(0.15f);  //wait
        gameObject.GetComponent<Renderer>().enabled = true;          //hide
        yield return new WaitForSeconds(0.15f);  //wait
        gameObject.GetComponent<Renderer>().enabled = false;           //show
        yield return new WaitForSeconds(0.15f);  //wait
        gameObject.GetComponent<Renderer>().enabled = true;          //hide
        yield return new WaitForSeconds(0.15f);  //wait
        gameObject.GetComponent<Renderer>().enabled = false;          //hide
        yield return new WaitForSeconds(0.15f);  //wait
        gameObject.GetComponent<Renderer>().enabled = true;          //hide
        is_hurt = false;
    }
}
