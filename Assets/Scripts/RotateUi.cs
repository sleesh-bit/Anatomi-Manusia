using UnityEngine;

public class RotateUI : MonoBehaviour
{
    public float kecepatanPutar = 15f;

    void Update()
    {
        transform.Rotate(0, 0, kecepatanPutar * Time.deltaTime);
    }
}