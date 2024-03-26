using System.Collections;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private GameObject _electricVFX;    
    private float electricEffectDuration; // Set this to the duration of your particle effect loop

    private Coroutine _deactivationCoroutine;

    private void Start()
    {
        electricEffectDuration = _electricVFX.GetComponent<ParticleSystem>().main.duration;
    }

    public void PlayerXPGainedParticles()
    {
        Debug.Log("XP Gained Particles");
    }
    public void PlayerLevelUpParticles()
    {
        Debug.Log("Level Up Particles");
    }
    public void PlayerDeathParticles()
    {
        Debug.Log("Player Death Particles");
    }
    public void TogglePlayerElectricParticles()
    {
        if (!_electricVFX.activeSelf)
        {
            Debug.Log("Electric Particles Active: true");
            _electricVFX.SetActive(true);
        }
        else
        {
            _electricVFX.SetActive(false);
            /*
            if (_deactivationCoroutine != null)
            {
                StopCoroutine(_deactivationCoroutine);
            }
            _deactivationCoroutine = StartCoroutine(DeactivateAfterDelay(electricEffectDuration));
            */
        }
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        // Wait for the duration of one loop of the particle system effect
        yield return new WaitForSeconds(delay);

        Debug.Log("Electric Particles Active: false");
        _electricVFX.SetActive(false);
    }
}
