using UnityEngine;
using System.Collections;

public class Player_Attack : MonoBehaviour
{
    [SerializeField] Animator atkanim;
    [SerializeField] GameObject CollisionP1ATK1;
    [SerializeField] GameObject CollisionP1ATK2;
    [SerializeField] GameObject CollisionP1ATK3;
    [SerializeField] float InputFrameATK2 = 4f;
    //[SerializeField] float InputFrameATK3 = 1f;
    [SerializeField] float StartupFrame1 = 0.2f;
    [SerializeField] float ActiveFrame1 = 2.8f;
    [SerializeField] float Endingframe1 = 1.0f;
    [SerializeField] float StartupFrame2 = 0.5f;
    [SerializeField] float ActiveFrame2 = 3.0f;
    [SerializeField] float Endingframe2 = 1.0f;
    [SerializeField] float StartupFrame3 = 0.5f;
    [SerializeField] float ActiveFrame3 = 3.0f;
    [SerializeField] float Endingframe3 = 1.0f;
    public KeyCode AttackKey;
    [SerializeField] private PlayerState State;
    //[SerializeField] PlayerMovement P1Movement;
    // (use if player atk can't move)

    bool IsAttacking;
    bool notFirstATK;
    bool SecondATK;

    float InputTime;
    float buffhit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        atkanim = GetComponent<Animator>();
        CollisionP1ATK1.SetActive(false);
        CollisionP1ATK2.SetActive(false);
        CollisionP1ATK3.SetActive(false);
        SecondATK = false;
        notFirstATK = false;
        IsAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (buffhit == 0)
        {
            State.IsBuffed = false;
        }
        //Debug.Log(InputTime);

        if (notFirstATK && !IsAttacking)
        {
            InputTime += Time.deltaTime;
        }

        if (InputTime >= InputFrameATK2)
        {
            notFirstATK = false;
            SecondATK = false;
            InputTime = 0f;
        }

        if (IsAttacking == false)
        {
            if (Input.GetKeyDown(AttackKey))
            {
                if (!notFirstATK)
                {
                    IsAttacking = true;
                    StartCoroutine(ActiveAtk1());
                }
                else if (notFirstATK && !SecondATK)
                {
                    IsAttacking = true;
                    StartCoroutine(ActiveAtk2());
                }
                else if (SecondATK)
                {
                    IsAttacking = true;
                    StartCoroutine(ActiveAtk3());
                }
            }
        }
    }

    IEnumerator ActiveAtk1()
    {
        yield return new WaitForSeconds(StartupFrame1);
        CollisionP1ATK1.SetActive(true);
        atkanim.SetBool("Attack1", true);
        //Physics2D.SyncTransforms();
        yield return new WaitForSeconds(ActiveFrame1);
        CollisionP1ATK1.SetActive(false);
        atkanim.SetBool("Attack1", false);
        yield return new WaitForSeconds(Endingframe1);
        IsAttacking = false;
        notFirstATK = true;
    }

    IEnumerator ActiveAtk2()
    {
        yield return new WaitForSeconds(StartupFrame2);
        CollisionP1ATK2.SetActive(true);
        atkanim.SetBool("Attack2", true);
        yield return new WaitForSeconds(ActiveFrame2);
        CollisionP1ATK2.SetActive(false);
        atkanim.SetBool("Attack2", false);
        yield return new WaitForSeconds(Endingframe2);
        IsAttacking = false;
        SecondATK = true;
        notFirstATK = true;
        InputTime = 0f;
    }

    IEnumerator ActiveAtk3()
    {
        yield return new WaitForSeconds(StartupFrame3);
        CollisionP1ATK3.SetActive(true);
        atkanim.SetBool("Attack3", true);
        yield return new WaitForSeconds(ActiveFrame3);
        CollisionP1ATK3.SetActive(false);
        atkanim.SetBool("Attack3", false);
        yield return new WaitForSeconds(Endingframe3);
        IsAttacking = false;
        SecondATK = false;
        notFirstATK = false;
        InputTime = 0f;
    }

    public void StopAllAtk()
    {
        StopAllCoroutines();
        CollisionP1ATK1.SetActive(false);
        CollisionP1ATK2.SetActive(false);
        CollisionP1ATK3.SetActive(false);
        atkanim.SetBool("Attack3", false);
        atkanim.SetBool("Attack2", false);
        atkanim.SetBool("Attack1", false);
    }

    public void hitted()
    {
        buffhit --;
        Debug.Log(buffhit);
        CollisionP1ATK1.SetActive(false);
        CollisionP1ATK2.SetActive(false);
        CollisionP1ATK3.SetActive(false);
    }

    public void buffed()
    {
        buffhit = 2;
        Debug.Log(buffhit);
    }
    //COMBAT - 
    //if last hit > 1s 
    //1st hit
    //hitting = true
    //(COROUTINE)
    //hitting = false
    //else if 2nd hit x hit
    //2nd hit
    //hitting = true
    //(COROUTINE)
    //hitting = false
    //else 
    //3rd hit
    //hitting = true
    //(COROUTINE)
    //hitting = false

    //hit box -
    //if hitting = true - 
    //game object = true (attack range) (collider)
    //player movement = false
}
