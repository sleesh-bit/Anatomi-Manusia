using UnityEngine;

public class NoseAnimation : MonoBehaviour
{
    [Header("Nose Object")]
    public Transform nose;

    [Header("Animation Settings")]
    public float pulseSpeed = 8.0f;
    public float pulseScaleAmount = 0.2f; // Pembesaran 20% dari ukuran asli
    public float animationDuration = 1.0f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    private Vector3 noseInitialScale;
    private bool isAnimating = false;
    private float timer = 0.0f;

    void Start()
    {
        if (nose != null)
        {
            noseInitialScale = nose.localScale;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void OnMouseDown()
    {
        OnTargetFound();
    }

    public void OnTargetFound()
    {
        if (nose != null && noseInitialScale == Vector3.zero)
        {
            noseInitialScale = nose.localScale;
        }

        isAnimating = true;
        timer = animationDuration;

        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void OnTargetLost()
    {
        isAnimating = false;
        ResetNose();
    }

    void Update()
    {
        if (isAnimating)
        {
            timer -= Time.deltaTime;

            if (timer > 0)
            {
                AnimateNose();
            }
            else
            {
                isAnimating = false;
                ResetNose();
            }
        }
    }

    void AnimateNose()
    {
        if (nose == null) return;

        float wave = (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) * 0.5f; // Nilai 0 sampai 1
        float scaleFactor = 1.0f + (wave * pulseScaleAmount);
        nose.localScale = noseInitialScale * scaleFactor;
    }

    void ResetNose()
    {
        if (nose != null && noseInitialScale != Vector3.zero)
        {
            nose.localScale = noseInitialScale;
        }
    }
}