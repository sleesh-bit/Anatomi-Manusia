using UnityEngine;

public class TanganArController : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject uiCanvas;

    [Header("Child Hands Reference")]
    public SingleHandController leftHandScript;
    public SingleHandController rightHandScript;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip tapSound;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
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
        TriggerBothHands(1.2f, 0.4f);
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
                TriggerBothHands(0.6f, 0.3f);
                PlayTapSound();
            }
        }
    }

    private void TriggerBothHands(float duration, float strength)
    {
        if (leftHandScript != null) leftHandScript.TriggerAnimation(duration, strength);
        if (rightHandScript != null) rightHandScript.TriggerAnimation(duration, strength);
    }

    private void PlayTapSound()
    {
        if (audioSource != null && tapSound != null)
        {
            audioSource.PlayOneShot(tapSound);
        }
    }
}