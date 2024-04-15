using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAnimation : MonoBehaviour
{
    public GameObject firstCamera; // ���� ī�޶�
    public GameObject secondCamera; // ���� ī�޶�

    public GameObject bow; // ����� Ȱ�� ��ġ
    public GameObject bowString; // ����� Ȱ�� �� ��ġ
    public GameObject bowShotpotion; // ���̹� ����� bowString�� �θ� �� ������Ʈ
    public GameObject[] arrow; // ����� ȭ�� ����
   
    Animator animator;
    Player player;

    public bool isCharging = false;
    public bool isShot;

    Vector3 bowStringOriginOffset; // bowString�� �ʱ� ��ġ�� ��� ���� ����

    int skillComand;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        bowStringOriginOffset = new Vector3(0f, 0.156f, 0f);

        isCharging = false;

        player = GetComponent<Player>();
    }

    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        Aiming(); // ���� �� ��� �ִϸ��̼�
        ArrowShot(); // ȭ�� �߻� �ִϸ��̼�
        player.Skill();
       
      

    }

    private void ArrowShot()
    {
        // ȭ�� �߻�
        if (Input.GetMouseButtonDown(0) && isCharging == true)
        {
            if (secondCamera.activeSelf)
            {
                secondCamera.SetActive(false);

            }
            StopCoroutine(StringDelay());
            arrow[player.skillComand].SetActive(false);
            animator.SetBool("IsShot", true);
            animator.SetBool("IsShoted", true);

            isCharging = false;
            animator.SetBool("IsCharging", false);
            // �ִϸ��̼� ����   IsShot, true
            //                   IsCharging, false

            bowString.transform.parent = bow.transform; // �ڽ��� ���� ��ġ�� �̵�
            bowString.transform.localPosition = bowStringOriginOffset;



        }
    }

    private void Aiming()
    {
        // ���콺 ��Ŭ�� ù��° Ŭ���� ���̹�
        if (Input.GetMouseButtonDown(1) && isCharging == false)
        {
            animator.SetBool("IsShoted", false);
            if (firstCamera.activeSelf)
            {
                secondCamera.SetActive(true);                              
            }
            
            animator.SetBool("IsCharging", true);
            isCharging = true;
            StartCoroutine(StringDelay());

        }
        // ���콺 ��Ŭ�� �ι�° Ŭ���� ���̹� ���
        else if (Input.GetMouseButtonDown(1) && isCharging == true)
        {
            if (secondCamera.activeSelf)
            {
                secondCamera.SetActive(false);

            }
            StopCoroutine(StringDelay()); // �ڷ�ƾ ���� �� ����
            arrow[player.skillComand].SetActive(false);
            animator.SetBool("IsCharging", false);
            isCharging = false;
            bowString.transform.parent = bow.transform; // �ڽ��� ���� ��ġ�� �̵�
            bowString.transform.localPosition = bowStringOriginOffset;
            arrow[player.skillComand].SetActive(false);

        }
    }

    // bowString�� ��ġ�� �ڿ��������� ���� ������ 
    IEnumerator StringDelay()
    {
        if (player.skillComand >= 0)
        {
            yield return new WaitForSeconds(0.5f);
            arrow[player.skillComand].SetActive(true);
            yield return new WaitForSeconds(0.7f);

            bowString.transform.parent = bowShotpotion.transform; // bowString�� �θ� ������ �հ������� �̵�
            bowString.transform.localPosition = Vector3.zero;
        }
      
      
    }
}
