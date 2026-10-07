using UnityEngine;

public class PulseUI : MonoBehaviour
{
    [Header("Pengaturan Membesar-Mengecil")]
    public float kecepatan = 1f;    // Seberapa cepat kembang-kempisnya
    public float skalaTambahan = 0.02f; // Seberapa besar membesarnya (0.08 = 8% lebih besar)

    private Vector3 skalaAwal;

    void Start()
    {
        // Simpan ukuran asli objek di awal
        skalaAwal = transform.localScale;
    }

    void Update()
    {
        // Hitung perubahan skala menggunakan rumus gelombang halus (Sin)
        float faktorSkala = 1f + Mathf.Sin(Time.time * kecepatan) * skalaTambahan;

        // Terapkan skala baru
        transform.localScale = skalaAwal * faktorSkala;
    }
}