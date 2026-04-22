using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Mainmenu : MonoBehaviour
{
    private const string MusicVolumePrefKey = "MusicVolume";
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private PlayerHealth playerHealth;
    [Header("Audio")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSfx;
    [SerializeField] private AudioClip trainWhistleClip;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        float savedVolume = PlayerPrefs.GetFloat(MusicVolumePrefKey, 1f);
        ApplyMusicVolume(savedVolume);

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.SetValueWithoutNotify(savedVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    private void OnDestroy()
    {
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.RemoveListener(SetVolume);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(playerHealth != null && playerHealth.dead == true) return;
            
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
        }
    }

    public void Play()
    {
        PlayButtonClickSfx();
        PlayTrainWhistleSfx();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        PlayButtonClickSfx();
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        PlayButtonClickSfx();
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        PlayButtonClickSfx();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume()
    {
        PlayButtonClickSfx();
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
    }

    public void OpenOptions()
    {
        PlayButtonClickSfx();
        anim.SetTrigger("Movee");
        optionsPanel.SetActive(true);
        Cursor.visible = true;
    }

    public void CloseOptions()
    {
        PlayButtonClickSfx();
        optionsPanel.SetActive(false);
        Cursor.visible = false;
    }

    public void SetVolume(float volume)
    {
        ApplyMusicVolume(volume);
        PlayerPrefs.SetFloat(MusicVolumePrefKey, volume);
        PlayerPrefs.Save();
    }

    private void ApplyMusicVolume(float volume)
    {
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.SetVolume(volume);
            return;
        }

        AudioListener.volume = Mathf.Clamp01(volume);
    }

    private void PlayButtonClickSfx()
    {
        if (buttonClickSfx == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSfx);
            return;
        }

        AudioSource.PlayClipAtPoint(buttonClickSfx, Vector3.zero);
    }

    private void PlayTrainWhistleSfx()
    {
        if (trainWhistleClip == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(trainWhistleClip);
            return;
        }

        AudioSource.PlayClipAtPoint(trainWhistleClip, Vector3.zero);
    }
}
