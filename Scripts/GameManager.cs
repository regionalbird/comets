using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int max_devices = 3; // limit the number of devices you can use at once
    private int number_of_devices = 0; // used to track how many devices there are
    
    public GameObject device; // where we store the device prefab
    private GameObject new_device; // used each time we instantiate a device

    public GameObject cursor; // store the cursor game object
    private GameObject this_frame; // place to store + destroy cursor each frame
    private GameObject last_frame; // place to store cursor based on this frame's position

    private static float range_of_device; // used to grab range from gravity_device

    private bool device_allowed; // tells us when we've hit the limit of devices

    private Text my_text;
    public static int devices_remaining;

    private GameObject[] devices;

    private GameObject[] winObjects;

    private static bool has_won;

    public static bool reset;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1;
        range_of_device = gravity_device.range; // grabs device range from gravity_device
        winObjects = GameObject.FindGameObjectsWithTag("ShowOnWin");
        //has_won = win_condition.won;
        hideWon();
        reset = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // store mouse position
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePos); // gotta cast screen position to world position
        RaycastHit2D hitData = Physics2D.Raycast(worldPosition, Vector2.zero, 0); // find out if there is something there already
        
        //has_won = win_condition.won;

        if (hitData.collider == null){
            if (number_of_devices < max_devices)
            {
            last_frame = this_frame; // update last_frame position before destroying
            this_frame = Instantiate(cursor, worldPosition, Quaternion.identity); // update this_frame position
            Destroy(last_frame); // destroy last_frame
            }
            
            // if allowed, on click make a device
            if (Input.GetButtonDown("Click") && AllowDevice()){
                new_device = Instantiate(device, worldPosition, Quaternion.identity);
                new_device.tag = "Device"; // tag devices so we can fetch the array of all of them in orbital_gravity
                number_of_devices += 1;
                devices_remaining = max_devices-number_of_devices;
                //my_text.text = "Devices Remaining: " + devices_remaining;
                //Debug.Log("Device Created");
            }
        }
        if (Input.GetButtonDown("Click") && hitData.collider.name == "Reset")
        {
            devices = GameObject.FindGameObjectsWithTag("Device");
            foreach (var device in devices)
            {
                Destroy(device);
            }
            number_of_devices = 0;
            devices_remaining = max_devices;
            //win_condition.current_balls = 0;
        }

        if (has_won == true)
        {
            showWon();
            if (Input.GetButtonDown("Click") && hitData.collider.name == "Reset")
            {
                //win_condition.current_balls = 0;
                has_won = false;
                reset = true;
                hideWon();
            }
        }
    }

    bool AllowDevice(){ // checks if you can make a device
        if (number_of_devices >= max_devices)
        {
            device_allowed = false;
        }
        else
        {
            device_allowed = true;
        }
        return device_allowed;
    }

    public void showWon(){
		foreach(GameObject g in winObjects){
			g.SetActive(true);
		}
	}
    
    public void hideWon(){
		foreach(GameObject g in winObjects){
			g.SetActive(false);
		}
        //Time.timeScale = 0;
	}
    
    void OnDrawGizmos()
    {
        // draws a circle around the device as you are moving the cursor so you can see its influence
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(worldPosition, range_of_device);
    }
}
