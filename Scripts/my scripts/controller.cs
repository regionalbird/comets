using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    public GameObject ball; // store the asteroid game object
    private Rigidbody2D ball_rigidbody; // its rigidbody too
    public Camera mainCam; // get the cam
    private float time = 0.0f; // just initiate the time
    public float interpolationPeriod = 1.5f; // how frequently the balls are instantiated
    private float ball_size;   // scaling them down so they are smaller and more mouse-like
    private int xory; // randomly selects a side to spawn an asteroid
    private Vector2 spawnpoint; // store the place to spawn
    private Vector2 spawnforce; // store the force
    private float torque;
    private float sceneWidth;
    private float sceneHeight;
    private float right;
    private float left;
    private float top;
    private float bottom;

    private float real_interpolation;
    private List<float> interp_periods;
    private int k;
    private int p;
    private int q;
    private float t;

    private AudioSource source;
    public AudioClip[] tones;
    private AudioClip tone;

    private int m;
    private float xsquare;
    private float size_square;

    //public float forceLowerLimit;
    public float forceUpperLimit;
    public float sizeLowerLimit;
    public float sizeUpperLimit;
    // Start is called before the first frame update
    void Start()
    {
        //mainCam = Camera.main; // grab! that! cam!
        source = GetComponent<AudioSource>();
        sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect; // 
        sceneHeight = mainCam.orthographicSize * 2;
        right = sceneWidth / 2;
        left = right * -1;
        top = sceneHeight / 2;
        bottom = top * -1;
        real_interpolation = interpolationPeriod * 20;
        interp_periods = new List<float>();
        for (int i=0; i < 25; i++)
        {
            interp_periods.Add(i*interpolationPeriod);
        }
        
        //SpawnRoid();
    }

    void FixedUpdate()
    {
        time += Time.deltaTime; // update time
        t += Time.deltaTime;
        if (time >= real_interpolation)
        { // make a ball after every interpolationPeriod
            time = time - real_interpolation;
            SpawnRoid();
        }
    }

    void SpawnRoid()
    {
        
        xory = Random.Range(0,4); // choose which side to spawn it on
        //Debug.Log(xory);
        
        if (xory == 0) // top
        {
            spawnpoint = new Vector2(Random.Range(left,right),top);
            spawnforce = new Vector2(Random.Range(-1*forceUpperLimit,forceUpperLimit),Random.Range(-1*forceUpperLimit,-1*forceUpperLimit/2.0f));
        }
        if (xory == 1) // right
        {
            spawnpoint = new Vector2(right,Random.Range(bottom,top));
            spawnforce = new Vector2(Random.Range(-1*forceUpperLimit,-1*forceUpperLimit/2.0f),Random.Range(-1*forceUpperLimit,forceUpperLimit));
        }
        if (xory == 2) // bottom
        {
            spawnpoint = new Vector2(Random.Range(left,right),bottom);
            spawnforce = new Vector2(Random.Range(-1*forceUpperLimit,forceUpperLimit),Random.Range(forceUpperLimit/2.0f,forceUpperLimit));
        }
        if (xory == 3) // left
        {
            spawnpoint = new Vector2(left,Random.Range(bottom,top));
            spawnforce = new Vector2(Random.Range(forceUpperLimit/2.0f,forceUpperLimit),Random.Range(-1*forceUpperLimit,forceUpperLimit));
        }
        ball_size = Random.Range(sizeLowerLimit, sizeUpperLimit);
        torque = Random.Range(0.1f, 10.0f);
        GameObject b = Instantiate(ball, spawnpoint, Quaternion.identity); 
        b.transform.localScale = new Vector2(ball_size,ball_size); // scale the ball down
        ball_rigidbody = b.GetComponent<Rigidbody2D>();
        ball_rigidbody.mass = ball_size;
        ball_rigidbody.AddForce(spawnforce, ForceMode2D.Impulse);
        ball_rigidbody.AddTorque(torque, ForceMode2D.Impulse);

        m = Random.Range(0,tones.Length);
        tone = tones[m];
        source.clip = tone;
        xsquare = Mathf.Pow(spawnpoint.x, 2);
        source.panStereo = Mathf.Sign(spawnpoint.x)*(xsquare/(1+xsquare));
        size_square = Mathf.Pow(ball_size,2);
        source.volume = (size_square/(1+size_square))/5.0f;
        source.Play();
        NewInterp();
        
        //Debug.Log(real_interpolation);
    }

    void NewInterp()
    {
        if (t <= 20.0f)
        {
            p = 20;
            q = 25;
        }
        if (t <= 40.0f)
        {
            p = 15;
            q = 20;
        }
        if (t <= 60.0f)
        {
            p = 10;
            q = 15;
        }
        if (t <= 90.0f)
        {
            p = 5;
            q = 10;
        }
        else
        {
            p = 0;
            q = 5;
        }
        k = Random.Range(p, q);
        real_interpolation = interp_periods[k];
        Debug.Log(source.clip);
    }
}
