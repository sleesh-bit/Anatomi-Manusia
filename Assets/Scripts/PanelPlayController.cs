using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID
using UnityEngine.Android; // Library khusus permission kamera Android
#endif

public class PanelPlayController : MonoBehaviour
{
    [Header("UI Panels")]
    public CanvasGroup panelMenuUtama; // Slot Panel Menu Utama
    public CanvasGroup panelPlay;      // Slot Panel Play (yang ada bingkainya)

    [Header("AR Camera")]
    public GameObject arCamera;        // Slot ARCamera Vuforia

    void Start()
    {
        // Kondisi awal saat game dibuka: Kamera & Panel Play dimatikan dulu
        if (arCamera != null) arCamera.SetActive(false);
        HidePanel(panelPlay);
    }

    // --- DIPANGGIL SAAT TOMBOL PLAY DIKLIK ---
    public void BukaMenuPlay()
    {
        // 1. Cek & Minta Izin Kamera (Otomatis skip kalau udah diizinkan sebelumnya)
        CekIzinKamera();

        // 2. Sembunyikan Menu Utama, Tampilkan Panel Play
        HidePanel(panelMenuUtama);
        ShowPanel(panelPlay);

        // 3. Nyalakan Kamera AR Vuforia
        if (arCamera != null)
        {
            arCamera.SetActive(true);
        }
    }

    // --- DIPANGGIL SAAT TOMBOL KEMBALI DI PANEL PLAY DIKLIK ---
    public void KembaliKeMenuUtama()
    {
        // 1. Matikan Kamera AR (Biar hemat baterai & RAM HP)
        if (arCamera != null)
        {
            arCamera.SetActive(false);
        }

        // 2. Sembunyikan Panel Play, Tampilkan Kembali Menu Utama
        HidePanel(panelPlay);
        ShowPanel(panelMenuUtama);
    }

    // --- LOGIKA PERMISSION KAMERA ANDROID ---
    private void CekIzinKamera()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif
    }

    // --- HELPER UNTUK SHOW / HIDE PANEL ---
    private void ShowPanel(CanvasGroup cg)
    {
        if (cg == null) return;
        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    private void HidePanel(CanvasGroup cg)
    {
        if (cg == null) return;
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}