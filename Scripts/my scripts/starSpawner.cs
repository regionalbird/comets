using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starSpawner : MonoBehaviour
{
    public Camera mainCam; // get the cam
    private float sceneWidth;
    private float sceneHeight;
    private float right;
    private float left;
    private float top;
    private float bottom;

    private int count;
    private float spawnx;
    private float spawny;
    private float spawnsize;

    public GameObject star;
    
    // Start is called before the first frame update
    void Start()
    {
        sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect; // 
        sceneHeight = mainCam.orthographicSize * 2;
        right = sceneWidth / 2;
        left = right * -1;
        top = sceneHeight / 2;
        bottom = top * -1;

        SpawnStars();
    }

    // Update is called once per frame
    void SpawnStars()
    {
        count = Random.Range(15,30);
        for (int i = 0; i < count; i++)
        {
            spawnx = Random.Range(left, right);
            spawny = Random.Range(top,bottom);
            spawnsize = Random.Range(0.2f,1.2f);
            GameObject s = Instantiate(star, new Vector2(spawnx, spawny), Quaternion.identity);
            s.transform.localScale = new Vector2(spawnsize, spawnsize);
            s.transform.Rotate(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        }
    }
}
