using UnityEngine;
using System.Collections;

public class P2Attack : MonoBehaviour
{
    [SerializeField] GameObject CollisionP2ATK1;
    [SerializeField] GameObject CollisionP2ATK2;
    [SerializeField] GameObject CollisionP2ATK3;
    [SerializeField] float StartupFrame = 0.5f;
    [SerializeField] float ActiveFrame = 3.0f;
    [SerializeField] float Endingframe = 1.0f;
    //[SerializeField] PlayerMovement2 P2Movement;

    bool IsAttacking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CollisionP2ATK1.SetActive(false);
        CollisionP2ATK2.SetActive(false);
        CollisionP2ATK3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAttacking == false)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                IsAttacking = true;
                StartCoroutine(ActiveAtk());
            }
        }
    }

    public void hitted()
    {
        CollisionP2ATK1.SetActive(false);
        CollisionP2ATK2.SetActive(false);
        CollisionP2ATK3.SetActive(false);
    }

    IEnumerator ActiveAtk()
    {
        yield return new WaitForSeconds(StartupFrame);
        CollisionP2ATK1.SetActive(true);
        yield return new WaitForSeconds(ActiveFrame);
        CollisionP2ATK1.SetActive(false);
        yield return new WaitForSeconds(Endingframe);
        IsAttacking = false;
    }


}
