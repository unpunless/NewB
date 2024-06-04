using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float h = 0.0f;
    float v = 0.0f;

    float moveSpeed = 10.0f;
    Vector3 moveDir = Vector3.zero;

    public float rotSpeed = 300.0f;
    Vector3 m_CacVec = Vector3.zero;

    void Update()
    {
        //-- 이동구현
        h = Input.GetAxis("Horizontal");    //-1.0f ~ 1.0f
        v = Input.GetAxis("Vertical");      //-1.0f ~ 1.0f

        //전후좌우 디동 방향 벡터 계산
        moveDir = (Vector3.forward * v) + (Vector3.right * h);
        moveDir.Normalize();    //단위 벡터로 계산

        //Transtlate(이동방향 * Time.deltaTime * 속도, 기준좌표);
        transform.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);
        // Space.Self 생략시 기본 값은 로컬 좌표이다.
        //-- 이동구현

        //-- 카메라 회전 구현
        if (Input.GetMouseButton(1) == true)
        {   //마우스 오른쪽 버튼 누르고 있는 동안
            m_CacVec = transform.eulerAngles;
            m_CacVec.y += (rotSpeed * Time.deltaTime * Input.GetAxis("Mouse X"));
            m_CacVec.x += (rotSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"));

            //if (120.0f < m_CacVec.x && m_CacVec.x < 340.0f)
            //    m_CacVec.x = 340.0f;

            //if (m_CacVec.x < 90.0f && 12.0f < m_CacVec.x)
            //    m_CacVec.x = 12.0f;

            transform.eulerAngles = m_CacVec;
        }
        //-- 카메라 회전 구현
 
    }

    public bool IsMove()
    {
        if (h == 0.0f && v == 0.0f) { return false; }

        return true;
    }
}
