using System.Collections;
using UnityEngine;

public class LambungArController : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject uiCanvas;

    [Header("Digestion Squeeze Settings")]
    public float animationDuration = 1.0f;
    public float squeezeAmount = 0.35f;
    public float bloatAmount = 0.3f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip tapSound;

    private Vector3 originalScale;
    private Coroutine animCoroutine;
    private Camera mainCamera;

    private void Awake()
    {
        originalScale = transform.localScale;
        mainCamera = Camera.main;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectObjectTap();
        }
    }

    public void OnTargetFound()
    {
        if (uiCanvas != null) uiCanvas.SetActive(true);
        TriggerDigestionAnimation(animationDuration);
        PlayTapSound();
    }

    public void OnTargetLost()
    {
        if (uiCanvas != null) uiCanvas.SetActive(false);
    }

    private void DetectObjectTap()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.root == transform.root)
            {
                TriggerDigestionAnimation(0.8f);
                PlayTapSound();
            }
        }
    }

    private void TriggerDigestionAnimation(float duration)
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimateDigestion(duration));
    }

    private IEnumerator AnimateDigestion(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float currentScaleY = originalScale.y;
            float currentScaleXZ = originalScale.x;

            if (t < 0.4f)
            {
                float squeezeProgress = t / 0.4f;
                float smoothSqueeze = Mathf.Sin(squeezeProgress * Mathf.PI * 0.5f);

                currentScaleXZ = originalScale.x * (1f - (smoothSqueeze * squeezeAmount));
                currentScaleY = originalScale.y * (1f + (smoothSqueeze * squeezeAmount * 0.8f));
            }
            else
            {

                float bloatProgress = (t - 0.4f) / 0.6f;
                float bounceWave = Mathf.Sin(bloatProgress * Mathf.PI * 4f) * Mathf.Exp(-3.5f * bloatProgress);

                float bloatEffect = bounceWave * bloatAmount;
                currentScaleXZ = originalScale.x * (1f + bloatEffect);
                currentScaleY = originalScale.y * (1f - (bloatEffect * 0.5f));
            }

            transform.localScale = new Vector3(currentScaleXZ, currentScaleY, currentScaleXZ);

            yield return null;
        }

        transform.localScale = originalScale;
    }

    private void PlayTapSound()
    {
        if (audioSource != null && tapSound != null)
        {
            audioSource.PlayOneShot(tapSound);
        }
    }
}