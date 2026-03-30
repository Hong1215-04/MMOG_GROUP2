using UnityEngine;
using System.Collections;

public class Player_Attack : MonoBehaviour
{
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
    //[SerializeField] PlayerMovement P1Movement;
    // (use if player atk can't move)

    bool IsAttacking;
    bool notFirstATK;
    bool SecondATK;

    float InputTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        Debug.Log(InputTime);

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
            if (Input.GetKeyDown(KeyCode.H))
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
        yield return new WaitForSeconds(ActiveFrame1);
        CollisionP1ATK1.SetActive(false);
        yield return new WaitForSeconds(Endingframe1);
        IsAttacking = false;
        notFirstATK = true;
    }

    IEnumerator ActiveAtk2()
    {
        yield return new WaitForSeconds(StartupFrame2);
        CollisionP1ATK2.SetActive(true);
        yield return new WaitForSeconds(ActiveFrame2);
        CollisionP1ATK2.SetActive(false);
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
        yield return new WaitForSeconds(ActiveFrame3);
        CollisionP1ATK3.SetActive(false);
        yield return new WaitForSeconds(Endingframe3);
        IsAttacking = false;
        SecondATK = false;
        notFirstATK = false;
        InputTime = 0f;
    }

    public void StopAllAtk()
    {
        StopAllCoroutines();
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
