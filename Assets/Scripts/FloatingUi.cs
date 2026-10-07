using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    [Header("Pengaturan Melambai / Rotate")]
    public float kecepatan = 2f;    
    public float sudutMaks = 10f;   

    void Update()
    {

        float zAngle = Mathf.Sin(Time.time * kecepatan) * sudutMaks;
        transform.localRotation = Quaternion.Euler(0, 0, zAngle);
    }
}