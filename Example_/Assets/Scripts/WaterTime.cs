using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTime : MonoBehaviour
{
    float speed = 5.0f;
    float acceleration = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;
        //QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        FloorUp();
    }

    void FloorUp()
    {
        speed += acceleration * Time.deltaTime; // 속도를 증가시켜 가속도 적용
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
