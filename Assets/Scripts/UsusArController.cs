using System.Collections;
using UnityEngine;

public class UsusArController : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject uiCanvas;

    [Header("Squish & Unfold Settings")]
    public float animationDuration = 1.0f;
    [Range(0.1f, 0.8f)]
    public float squishAmount = 0.4f; 
    public float twistAngle = 30f;     

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
        TriggerUsusAnimation(animationDuration);
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
                TriggerUsusAnimation(0.7f);
                PlayTapSound();
            }
        }
    }

    private void TriggerUsusAnimation(float duration)
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimateKusutToNormal(duration));
    }

    private IEnumerator AnimateKusutToNormal(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

  
            float squishProgress = Mathf.Sin(t * Mathf.PI);
            float currentSquish = squishProgress * squishAmount;


            float bounceBack = Mathf.Sin(t * Mathf.PI * 6f) * Mathf.Exp(-4f * t) * 0.2f;

            float scaleXZ = 1f - currentSquish + bounceBack;
            float scaleY = 1f - (currentSquish * 0.7f) + bounceBack;

            transform.localScale = new Vector3(
                originalScale.x * scaleXZ,
                originalScale.y * scaleY,
                originalScale.z * scaleXZ
            );

         
            float currentTwist = Mathf.Sin(t * Mathf.PI * 2f) * twistAngle * (1f - t);
            transform.localRotation = originalRotation * Quaternion.Euler(0f, currentTwist, 0f);

            yield return null;
        }

        transform.localScale = originalScale;
        transform.localRotation = originalRotation;
    }

    private void PlayTapSound()
    {
        if (audioSource != null && tapSound != null)
        {
            audioSource.PlayOneShot(tapSound);
        }
    }
}