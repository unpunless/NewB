using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterColl : MonoBehaviour
{
    Rigidbody rb;

    GameObject Trigger1;
    GameObject Trigger2;
    GameObject Trigger3;

    float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Trigger1 = GameObject.Find("Trigger1");
        Trigger2 = GameObject.Find("Trigger2");
        Trigger3 = GameObject.Find("Trigger3");
    
        this.rb = GetComponent<Rigidbody>();

        //Application.targetFrameRate = 60;
        //QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        FloorUp();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "Trigger1" || coll.gameObject.name == "Trigger3")
        {
            speed = 6.5f;
        }
        else if (coll.gameObject.name == "Trigger2")
            speed = 3.5f;
    }

    void FloorUp()
    {
        Vector3 v3 = transform.position;
        v3.y += speed * Time.deltaTime;

        transform.position = v3;

        Log();
    }

    void Log()
    {
        float Log = 0f;
        if (Time.time - Log >= 1f)
        {
            Debug.Log("Time speed = " + speed);
            Log = Time.time;
        }
    }
}
