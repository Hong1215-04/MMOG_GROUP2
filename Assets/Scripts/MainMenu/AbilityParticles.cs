using System.Collections;
using UnityEngine;

public class AbilityParticles : MonoBehaviour
{
    [SerializeField] GameObject particles;
    [SerializeField] PlayerAbility ability;
    [SerializeField] float delay;

    private void Start()
    {
        ability.OnUsed += ShowParticles;
        ability.OnUseCompleted += HideParticles;
    }

    void ShowParticles(bool placeholder)
    {

        particles.SetActive(true);
    }
    void HideParticles()
    {
        StartCoroutine(ShowParticlesAfterDelay());
    }

    IEnumerator ShowParticlesAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        particles.SetActive(false);
    }

    private void OnDestroy()
    {
        ability.OnUsed -= ShowParticles;
        ability.OnUseCompleted -= HideParticles;
    }
}
