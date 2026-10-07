using System.Collections;
using UnityEngine;

public class SingleHandController : MonoBehaviour
{
    [Header("Animation Settings")]
    public float animationDuration = 1.2f;
    public float elasticStrength = 0.4f;
    public float waveRotationAmount = 35f;

    [Header("Phase / Direction Settings")]
    public float startDelay = 0f;
    public bool invertDirection = false;

    private Vector3 originalScale;
    private Quaternion originalRotation;
    private Coroutine animCoroutine;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalRotation = transform.localRotation;
    }

    public void TriggerAnimation(float duration, float strength)
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimateHand(duration, strength));
    }

    private IEnumerator AnimateHand(float duration, float strength)
    {
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        float elapsedTime = 0f;
        float dir = invertDirection ? -1f : 1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

       
            float bounceWave = Mathf.Sin(t * Mathf.PI * 8f) * Mathf.Exp(-4f * t) * strength;
            transform.localScale = originalScale * (1f + bounceWave);

    
            float waveZ = Mathf.Sin(t * Mathf.PI * 6f) * Mathf.Exp(-3f * t) * waveRotationAmount * dir;
            float waveX = Mathf.Cos(t * Mathf.PI * 6f) * Mathf.Exp(-3f * t) * (waveRotationAmount * 0.4f) * dir;

            transform.localRotation = originalRotation * Quaternion.Euler(waveX, 0f, waveZ);

            yield return null;
        }

        transform.localScale = originalScale;
        transform.localRotation = originalRotation;
    }
}