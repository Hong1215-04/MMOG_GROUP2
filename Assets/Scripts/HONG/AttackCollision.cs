using UnityEngine;
using System.Collections;

public class AttackCollision : MonoBehaviour
{
    [SerializeField] GameObject CollisionP1ATK;
    [SerializeField] float StartupFrame = 0.5f;
    [SerializeField] float ActiveFrame = 3.0f;
    [SerializeField] float Endingframe = 1.0f;

    bool IsAttacking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CollisionP1ATK.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAttacking == false)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                IsAttacking = true;
                StartCoroutine(ActiveAtk());
            }
        }
    }

    IEnumerator ActiveAtk()
    {
        yield return new WaitForSeconds(StartupFrame);
        CollisionP1ATK.SetActive(true);
        yield return new WaitForSeconds(ActiveFrame);
        CollisionP1ATK.SetActive(false);
        yield return new WaitForSeconds(Endingframe);
        IsAttacking = false;
    }
}
