using UnityEngine;

public class PanelPanduanController : MonoBehaviour
{
    [Header("UI Panels")]
    public CanvasGroup panelMenuUtama; // Slot Panel Menu Utama
    public CanvasGroup panelPanduan;   // Slot Panel Panduan

    void Start()
    {
        // Saat game baru mulai, pastikan Panel Panduan tersembunyi
        HidePanel(panelPanduan);
    }

    // --- DIPANGGIL SAAT TOMBOL PANDUAN DI MENU UTAMA DIKLIK ---
    public void BukaPanduan()
    {
        HidePanel(panelMenuUtama);
        ShowPanel(panelPanduan);
    }

    // --- DIPANGGIL SAAT TOMBOL KEMBALI DI PANEL PANDUAN DIKLIK ---
    public void TutupPanduan()
    {
        HidePanel(panelPanduan);
        ShowPanel(panelMenuUtama);
    }

    // --- HELPER SHOW / HIDE CANVAS GROUP ---
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