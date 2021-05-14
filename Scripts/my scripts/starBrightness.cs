using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starBrightness : MonoBehaviour
{
    //private Light lt;
    public UnityEngine.Experimental.Rendering.Universal.Light2D lt;
    private AudioSource source;
    public AudioClip[] chord;
    private AudioClip note;

    private float period; // period of dimming and sound swell

    // variable for storing the light swell values
    private float blowermin = 0.01f;
    private float blowermax = 0.2f;
    private float buppermin = 0.3f;
    private float buppermax = 0.5f;
    private float blower;
    private float bupper;

    // variables for storing the sound swell values
    private float slowermin = 0.05f;
    private float slowermax = 0.2f;
    private float suppermin = 0.3f;
    private float suppermax = 0.5f;
    private float slower;
    private float supper;

    private float t;
    private int n;
    private float xsquare;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        lt = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        blower = Random.Range(blowermin, blowermax);
        bupper = Random.Range(buppermin, buppermax);
        slower = Random.Range(slowermin, slowermax);
        supper = Random.Range(suppermin, suppermax);
        period = Random.Range(0.8f, 3.0f);
        t=0.0f;
        n = Random.Range(0,chord.Length);
        note = chord[n];
        source.clip = note;
        xsquare = Mathf.Pow(transform.position.x, 2);
        source.panStereo = Mathf.Sign(transform.position.x)*(xsquare/(1+xsquare));
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Dim();
        Noise();
        /*
        lt.intensity = Mathf.Lerp(lower, upper, t);
        t += Time.deltaTime;
        if (t > period)
        {
            float temp = upper;
            upper = lower;
            lower = temp;
            t = 0.0f;
        }
        */
    }

    void Dim()
    {
        lt.intensity = Mathf.Lerp(blower, bupper, t);
        t += Time.deltaTime;
        if (t > period)
        {
            float btemp = bupper;
            bupper = blower;
            blower = btemp;
            t = 0.0f;
        }
    }

    void Noise()
    {
        source.volume = Mathf.Lerp(slower, supper, t);
        t += Time.deltaTime;
        if (t > period)
        {
            float stemp = supper;
            supper = slower;
            slower = stemp;
            t = 0.0f;
        }
    }
}
