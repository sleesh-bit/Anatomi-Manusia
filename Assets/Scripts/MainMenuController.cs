using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panel UI")]
    public GameObject panelMenuUtama; // Objek yang menampung tombol-tombol depan
    public GameObject panelPanduan;
    public GameObject panelTentang;

    [Header("Pengaturan Volume & BGM")]
    public Slider volumeSlider;
    public AudioSource bgmAudioSource;

    [Header("Fitur Mute / Unmute")]
    public Image tombolMusicImage;
    public Sprite iconMusicOn;
    public Sprite iconMusicOff;

    private CanvasGroup sliderCanvasGroup;
    private bool isMuted = false;
    private float lastVolume = 1f;

    private void Start()
    {
        if (volumeSlider != null)
        {
            sliderCanvasGroup = volumeSlider.GetComponent<CanvasGroup>();
            volumeSlider.onValueChanged.AddListener(UbahVolume);
            SembunyikanSlider();
        }

        if (bgmAudioSource != null && volumeSlider != null)
        {
            volumeSlider.value = bgmAudioSource.volume;
            lastVolume = bgmAudioSource.volume;
        }
    }

    // --- FUNGSI TENTANG (PINDAH TAMPILAN FULL) ---
    public void TombolTentang()
    {
        if (panelMenuUtama != null) panelMenuUtama.SetActive(false); // Sembunyikan Menu Utama
        if (panelTentang != null) panelTentang.SetActive(true);       // Munculkan Panel Tentang
    }

    public void TutupTentang()
    {
        if (panelTentang != null) panelTentang.SetActive(false);     // Sembunyikan Panel Tentang
        if (panelMenuUtama != null) panelMenuUtama.SetActive(true);   // Munculkan Menu Utama Lagi
    }

    // --- FUNGSI AUDIO & SLIDER ---
    public void TombolToggleMute()
    {
        isMuted = !isMuted;
        if (isMuted)
        {
            if (volumeSlider != null) lastVolume = volumeSlider.value;
            SetAudioVolume(0);
            if (volumeSlider != null) volumeSlider.value = 0;
            if (tombolMusicImage != null && iconMusicOff != null) tombolMusicImage.sprite = iconMusicOff;
        }
        else
        {
            SetAudioVolume(lastVolume <= 0.05f ? 0.5f : lastVolume);
            if (volumeSlider != null) volumeSlider.value = (lastVolume <= 0.05f ? 0.5f : lastVolume);
            if (tombolMusicImage != null && iconMusicOn != null) tombolMusicImage.sprite = iconMusicOn;
        }
    }

    public void TombolToggleSliderVolume()
    {
        if (sliderCanvasGroup != null)
        {
            if (sliderCanvasGroup.alpha == 0) TampilkanSlider();
            else SembunyikanSlider();
        }
    }

    private void TampilkanSlider()
    {
        sliderCanvasGroup.alpha = 1;
        sliderCanvasGroup.interactable = true;
        sliderCanvasGroup.blocksRaycasts = true;
    }

    private void SembunyikanSlider()
    {
        sliderCanvasGroup.alpha = 0;
        sliderCanvasGroup.interactable = false;
        sliderCanvasGroup.blocksRaycasts = false;
    }

    public void UbahVolume(float nilaiVolume)
    {
        SetAudioVolume(nilaiVolume);
        if (isMuted && nilaiVolume > 0)
        {
            isMuted = false;
            if (tombolMusicImage != null && iconMusicOn != null) tombolMusicImage.sprite = iconMusicOn;
        }
    }

    private void SetAudioVolume(float vol)
    {
        AudioListener.volume = vol;
        if (bgmAudioSource != null) bgmAudioSource.volume = vol;
    }

    // --- FUNGSI LAIN ---
    public void TombolMulai(string namaSceneAR) => SceneManager.LoadScene(namaSceneAR);
    public void TombolPanduan() { if (panelPanduan != null) panelPanduan.SetActive(true); }
    public void TombolKeluar() => Application.Quit();
}