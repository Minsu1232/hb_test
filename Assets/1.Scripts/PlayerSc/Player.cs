using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    [SerializeField]
    Transform character;
    [SerializeField]
    Transform cameraArm;
    [SerializeField]
    GameObject[] skillArrow;
    [SerializeField]
    Transform ShotPointer;
    [SerializeField]
    Transform target;

    Rigidbody rb;
    Animator animator;

    

    

    bool isWalk;
    bool isDie;

   public  bool isAim;

    // 스킬 쿨타임 체크용
    bool cool1;
    bool cool2;

    public PlayerState playerState;

    public int skillComand;
    private void OnEnable()
    {
       skillComand = 0;
       isDie = false;
       isWalk = true;
        
       rb = GetComponent<Rigidbody>();
       animator = GetComponent<Animator>();
    }
    void Start()
    {
    }

    void Update()
    {
        LookAround(); // 카메라 매서드
        MoveAndCamera(); // 이동 관련 매서드
        Skill(); // 스킬 관련 매서드
        ArrowShot(); // 화살 물리력 매서드
        
    }

    void MoveAndCamera()
    {

        {
            if (isWalk == true)
            {
                animator.SetBool("IsWalking", false);
                // 입력값을 vector2로 저장
                Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                // 카메라 회전에 따른 
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

                character.forward = lookForward;

                // 전진 속도
                if (Input.GetAxisRaw("Vertical") > 0.5)
                {
                    //달리기
                    if (Input.GetKey(KeyCode.LeftShift))
                    {                        
                        animator.SetBool("IsWalking", true);
                        transform.position += moveDir * Time.deltaTime * playerState.runSpeed;
                    }
                    //뛰기
                    else if(Input.GetKeyUp(KeyCode.LeftShift))
                    {                        
                        animator.SetBool("IsWalking", false);
                        transform.position += moveDir * Time.deltaTime * playerState.walkSpeed;
                    }

                }
                // 후진 속도
                transform.position += moveDir * Time.deltaTime * 4f;


                // 블렌트 트리 애니메이션 움직임의 자연스러움을 위해
                float smoothedValueX = Mathf.Lerp(animator.GetFloat("X"), moveInput.x, 5f * Time.deltaTime);
                float smoothedValueY = Mathf.Lerp(animator.GetFloat("Y"), moveInput.y, 5f * Time.deltaTime);

                // 블렌드 트리 애니메이션
                animator.SetFloat("X", smoothedValueX);
                animator.SetFloat("Y", smoothedValueY);
            }
        }




}

     void LookAround()
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector3 camAngle = cameraArm.rotation.eulerAngles;
            float x = camAngle.x - mouseDelta.y;

            // 상하 카메라 이동의 제한
            if (x < 180f)
            {
                x = Mathf.Clamp(x, -1f, 70f);

            }
            else
            {
                x = Mathf.Clamp(x, 335f, 361f);
            }


            cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        }
    
    //캐릭터 죽음 관련 메서드
     void Die()
     {
        if(isDie == true)
        {
            animator.SetBool("IsDie", true);
            Destroy(gameObject, 2f);
        }       
        
     }
    public void Skill()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            skillArrow[0].gameObject.SetActive(true);
            skillComand = 0;

        }
        if (cool1 == false &&Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(SkillCool1());
            skillArrow[1].gameObject.SetActive(true);
            skillComand = 1;
        }
        if (cool2 == false && Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(SkillCool2());
            skillArrow[2].gameObject.SetActive(true);
            skillComand = 2;
        }
          
    }
    IEnumerator SkillCool1()
    {
        cool1 = true;
        yield return new WaitForSeconds(5f);
        cool1 = false;
    }
    IEnumerator SkillCool2()
    {
        cool2 = true;
        yield return new WaitForSeconds(3f);
        cool2 = false;
    }
    void ArrowShot()
    {
        if (Input.GetMouseButtonDown(0) && isAim == true)
        {
            Vector3 directionToTarget = (target.position - ShotPointer.transform.position).normalized;
            Quaternion arrowRotation = Quaternion.Euler(90,0,0);
            GameObject arrowInstance = Instantiate(skillArrow[skillComand], ShotPointer.transform.position , arrowRotation);
            
                   
            Rigidbody arrowRb = arrowInstance.AddComponent<Rigidbody>(); // Rigidbody 컴포넌트 동적 추가
            arrowRb.useGravity = false;
            ArrowPhysics(arrowRb,directionToTarget);
            
        }

        if (isAim == false && Input.GetMouseButtonDown(1))
        {
            isAim = true;
        }
        else if (isAim == true && Input.GetMouseButtonDown(1))
        {
            isAim = false;
        }
    }
    public void ArrowPhysics(Rigidbody arrowRb, Vector3 directionToTarget)
    {
        arrowRb.AddForce(directionToTarget * 10f, ForceMode.Impulse); // Use ForceMode.Impulse for an immediate force application
    }
   
}

