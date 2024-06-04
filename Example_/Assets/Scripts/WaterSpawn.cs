using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterSpawn : MonoBehaviour
{
    Rigidbody rb;

    GameObject trigger1;
    GameObject trigger2;
    GameObject trigger3;

    GameObject floor3;

    GameObject waterSpawn;

    float speed = 5.0f;
    float acceleration = 1.5f;

    private float lastLogTime = 0f; // 마지막 로그 출력 시간

    // Start is called before the first frame update
    void Start()
    {
        trigger1 = GameObject.Find("Trigger1");
        trigger2 = GameObject.Find("Trigger2");
        trigger3 = GameObject.Find("Trigger3");

        floor3 = GameObject.Find("floor3");

        waterSpawn = GameObject.Find("WaterSpawn");

        this.rb = GetComponent<Rigidbody>();

        // Application.targetFrameRate = 60;
        // QualitySettings.vSyncCount = 0;
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

        LogSpeed();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "Trigger3")
        {
            Spawn();
        }
    }


    void Spawn()
    {
        Vector3 SpawnY = floor3.transform.position;
        SpawnY.y = floor3.transform.position.y;
        waterSpawn.transform.position = SpawnY;
    }

    void LogSpeed()
    {
        if (Time.time - lastLogTime >= 1f) // 1초마다 로그 출력
        {
            Debug.Log("Time speed = " + speed);
            lastLogTime = Time.time;
        }
    }
}