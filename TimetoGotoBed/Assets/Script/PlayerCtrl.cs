using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    [Header("Player")]
    public float Player_Speed = 100.0f;
    public int Player_Jump = 60;
    public float Player_Run = 130.0f;
    public float Player_Rot = 200.0f;

    [Header("Bool")]
    bool IsRun = false;
    bool IsJump = false;
    bool IsFloor = false;
    bool IsBedClicked;
    //bool FrogClicked;
    bool IsNearBed;
    bool hasLogged = false; //�α� 1ȸ�� ����ϰ�

    [Header("Vector3")]
    Vector3 MoveDir = Vector3.zero;     //����
    Vector3 m_CacVec = Vector3.zero;    //ȸ��
    Vector3 HalfSize = Vector3.zero;    
    Vector3 m_CacCurPos = Vector3.zero; //���� ������ �������� ����

    [Header("Object")]
    public GameObject Floor;
    //public GameObject Frog;
    public GameObject Bed;
    public Camera PlayerCamera;

    [Header("Text")]
    public TMP_Text BedText;

    Rigidbody rigid;
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.8f;

    public NavMeshAgent agent;

    float h = 0f;
    float v = 0f;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //BedText.enabled = false;

        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");    //-1.0f ~ 1.0f
        v = Input.GetAxis("Vertical");      //-1.0f ~ 1.0f
        Move();
        Jump();
        Run();
        Rot();
        OnObjectPressed();
    }

    private void FixedUpdate()
    {
        Vector3 gravity = globalGravity * gravityScale * 2 * Vector3.up;
        rigid.AddForce(gravity, ForceMode.Acceleration);
    }

    void Move()     //�ȱ�
    {
        MoveDir = (Vector3.forward * v) + (Vector3.right * h);
        MoveDir.Normalize(); //���� ���ͷ� ���

        transform.Translate(MoveDir * Time.deltaTime * Player_Speed, Space.Self);
    }

    void Jump()     //�ٱ�
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsJump && IsFloor)
        {
            rigid.AddForce(Vector3.up * Player_Jump, ForceMode.Impulse);
            IsJump = false;  
        }
    }

    void Run()  //�޸���
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!IsRun) 
            {
                print("�޸���");
                IsRun = true;
                Player_Speed = Player_Run;
            }

        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (IsRun)   //LeftShift�� ������ �������� �ȴ´�
            {
                print("�ȱ�");
                IsRun = false;
                Player_Speed = 100.0f;
            }
        }

    }

    void Rot()      //ȸ��
    {
        //ī�޶� ȸ��
        if (Input.GetMouseButton(1))    //��Ŭ����
        {
            m_CacVec = PlayerCamera.transform.eulerAngles;
            m_CacVec.y += (Player_Rot * Time.deltaTime * Input.GetAxis("Mouse X"));
            m_CacVec.x += (Player_Rot * Time.deltaTime * Input.GetAxis("Mouse Y"));

            if (80.0f < m_CacVec.x && m_CacVec.x < 320.0f)
                m_CacVec.x = 340.0f;

            if (m_CacVec.x < 90.0f && 12.0f < m_CacVec.x)
                m_CacVec.x = 12.0f;

            PlayerCamera.transform.eulerAngles = m_CacVec;
            
        }
    }

    void OnObjectPressed() // ���� Ŭ��
    {
        if (Input.GetMouseButtonDown(0) && !hasLogged)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ŭ�� ��ġ �������� Raycast ����
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject; // ray�� �浹�� ��ü ������

                Debug.Log("Hit object: " + hitObject.name); // �浹�� ������Ʈ �̸� �α� ���

                if (hitObject.CompareTag("Bed"))
                {
                    IsBedClicked = true;
                    Debug.Log("Click Bed");
                    BedMove();
                    hasLogged = true;
                }
            }
        }
    }


    void BedMove()
    {
        StartCoroutine(MoveAndRotateToBed());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            IsFloor = true;
            IsJump = true;
            print("��������");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            IsFloor = false;
            IsJump = false;
            print("����");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bed"))
        {
            BedText.enabled = true;
            IsNearBed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bed"))
        {
            BedText.enabled = false;
            IsNearBed = false;
        }
    }

    IEnumerator MoveAndRotateToBed()
    {
        agent.SetDestination(Bed.transform.position);

        // ħ��� �̵� Ȯ��
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(-90f, 180f, 0f);
        float sleepduration = 2.0f;
        float timeElapsed = 0f;

        // ȸ�� ����
        while (timeElapsed < sleepduration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, timeElapsed / sleepduration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
