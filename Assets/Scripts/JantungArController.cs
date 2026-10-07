using System.Collections;
using UnityEngine;

public class JantungArController : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject uiCanvas;

    [Header("Heartbeat Settings")]
    public float pulseDuration = 0.8f;    
    public float pulseStrength = 0.25f; 
    public int beatCount = 2;             

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
        TriggerHeartbeat(pulseDuration, beatCount);
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
                TriggerHeartbeat(0.6f, 2);
                PlayTapSound();
            }
        }
    }

    private void TriggerHeartbeat(float duration, int beats)
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimateHeartbeat(duration, beats));
    }

    private IEnumerator AnimateHeartbeat(float duration, int beats)
    {
        float timePerBeat = duration / beats;

        for (int i = 0; i < beats; i++)
        {
            float elapsedTime = 0f;

            while (elapsedTime < timePerBeat)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / timePerBeat;
                
                float pulse = Mathf.Sin(t * Mathf.PI) * pulseStrength;

                transform.localScale = originalScale * (1f + pulse);

                yield return null;
            }
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