using UnityEngine;

public class LegAnimation : MonoBehaviour
{
    [Header("Leg Objects")]
    public Transform leftLeg;
    public Transform rightLeg;

    [Header("Animation Settings")]
    public float walkSpeed = 6.0f;
    public float swingAngle = 15.0f;
    public float stepHeight = 0.03f; // Diperkecil biar gak lompat jauh
    public float animationDuration = 5.0f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    private Vector3 leftInitialPos;
    private Vector3 rightInitialPos;
    private Quaternion leftInitialRot;
    private Quaternion rightInitialRot;

    private bool isAnimating = false;
    private float timer = 0.0f;
    private bool isInitialized = false;

    void Start()
    {
        InitPositions();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void InitPositions()
    {
        if (isInitialized) return;

        if (leftLeg != null)
        {
            leftInitialPos = leftLeg.localPosition;
            leftInitialRot = leftLeg.localRotation;
        }

        if (rightLeg != null)
        {
            rightInitialPos = rightLeg.localPosition;
            rightInitialRot = rightLeg.localRotation;
        }

        isInitialized = true;
    }

    void OnMouseDown()
    {
        OnTargetFound();
    }

    public void OnTargetFound()
    {
        InitPositions();

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
        ResetLegs();
    }

    void Update()
    {
        if (isAnimating)
        {
            timer -= Time.deltaTime;

            if (timer > 0)
            {
                AnimateLegs();
            }
            else
            {
                isAnimating = false;
                ResetLegs();
            }
        }
    }

    void AnimateLegs()
    {
        if (leftLeg == null || rightLeg == null) return;

        float leftWave = Mathf.Sin(Time.time * walkSpeed);
        float rightWave = Mathf.Sin((Time.time * walkSpeed) + Mathf.PI);

        float leftRotX = leftWave * swingAngle;
        float rightRotX = rightWave * swingAngle;

        leftLeg.localRotation = leftInitialRot * Quaternion.Euler(leftRotX, 0f, 0f);
        rightLeg.localRotation = rightInitialRot * Quaternion.Euler(rightRotX, 0f, 0f);

        float leftLiftY = Mathf.Max(0f, leftWave) * stepHeight;
        float rightLiftY = Mathf.Max(0f, rightWave) * stepHeight;

        leftLeg.localPosition = leftInitialPos + new Vector3(0f, leftLiftY, 0f);
        rightLeg.localPosition = rightInitialPos + new Vector3(0f, rightLiftY, 0f);
    }

    void ResetLegs()
    {
        if (leftLeg != null && isInitialized)
        {
            leftLeg.localPosition = leftInitialPos;
            leftLeg.localRotation = leftInitialRot;
        }

        if (rightLeg != null && isInitialized)
        {
            rightLeg.localPosition = rightInitialPos;
            rightLeg.localRotation = rightInitialRot;
        }
    }
}