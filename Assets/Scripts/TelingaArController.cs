using System.Collections;
using UnityEngine;

public class TelingaArController : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject uiCanvas;

    [Header("Wiggle & Bounce Settings")]
    public float animationDuration = 0.9f;
    public float wiggleAngle = 20f;      
    public float bounceStrength = 0.25f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip tapSound;

    private Vector3 originalScale;
    private Quaternion originalRotation;
    private Coroutine animCoroutine;
    private Camera mainCamera;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalRotation = transform.localRotation;
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
        TriggerWiggleAnimation(animationDuration);
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
                TriggerWiggleAnimation(animationDuration);
                PlayTapSound();
            }
        }
    }

    private void TriggerWiggleAnimation(float duration)
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimateWiggle(duration));
    }

    private IEnumerator AnimateWiggle(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float wave = Mathf.Sin(t * Mathf.PI * 8f) * Mathf.Exp(-3.5f * t);

            float currentAngle = wave * wiggleAngle;
            transform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, currentAngle);

            float squish = wave * bounceStrength;
            transform.localScale = new Vector3(
                originalScale.x * (1f + squish),
                originalScale.y * (1f - squish),
                originalScale.z * (1f + squish)
            );

            yield return null;
        }

        transform.localRotation = originalRotation;
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