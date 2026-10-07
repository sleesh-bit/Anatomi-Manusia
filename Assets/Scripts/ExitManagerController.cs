using UnityEngine;

public class ExitManagerController : MonoBehaviour
{
    [Header("UI Panels")]
    public CanvasGroup panelKeluar; // Pop-up Keluar Game

    void Start()
    {
        // Sembunyikan pop-up saat awal game
        SembunyikanPopUp();
    }

    // Dipanggil saat tombol KELUAR di Menu Utama diklik
    public void TampilkanPopUpKeluar()
    {
        if (panelKeluar == null) return;
        panelKeluar.alpha = 1f;
        panelKeluar.interactable = true;
        panelKeluar.blocksRaycasts = true;
    }

    // Dipanggil saat tombol BATALKAN diklik
    public void SembunyikanPopUp()
    {
        if (panelKeluar == null) return;
        panelKeluar.alpha = 0f;
        panelKeluar.interactable = false;
        panelKeluar.blocksRaycasts = false;
    }

    // Dipanggil saat tombol MELANJUTKAN diklik (Keluar dari Aplikasi)
    public void KeluarAplikasi()
    {
        Debug.Log("Game Keluar!");
        Application.Quit(); // Perintah keluar dari APK Android / Game
    }
}