using System.Collections;
using UnityEngine;

public class OtakArController : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject uiCanvas;

    [Header("Jelly Bounce Settings")]
    public float bounceDuration = 0.8f;
    public float bounceElasticity = 0.5f;

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
        TriggerJellyBounce(bounceDuration, bounceElasticity);
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
                TriggerJellyBounce(0.5f, 0.4f);
                PlayTapSound();
            }
        }
    }

    private void TriggerJellyBounce(float duration, float strength)
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimateJelly(duration, strength));
    }

    private IEnumerator AnimateJelly(float duration, float strength)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;


            float wave = Mathf.Sin(t * Mathf.PI * 10f) * Mathf.Exp(-5f * t) * strength;

            float scaleY = 1f + wave;
            float scaleXZ = 1f - (wave * 0.5f);

            transform.localScale = new Vector3(
                originalScale.x * scaleXZ,
                originalScale.y * scaleY,
                originalScale.z * scaleXZ
            );

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