using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject mc;
    public GameObject sc;
    public GameObject credits;

    public AudioMixer mainMixer;
    public Slider volumeSlider;

    // Add a reference to the UI Image for the icon
    public Image volumeIcon;
    // Add references for your four icon sprites
    public Sprite highVolumeIcon; // 3 waves
    public Sprite midVolumeIcon;  // 2 waves
    public Sprite lowVolumeIcon;  // 1 wave
    public Sprite mutedIcon;      // X icon

    public Slider brightnessSlider;
    public GameObject brObject;
    public Image brightnessPanel; // The full-screen black image

    private bool isPaused = false;    // To track the game's pause state

    public static Menu instance;

    void Awake()
    {
        // If an instance of this already exists and it's not this one...
        if (instance != null && instance != this)
        {
            // ...then destroy this new one. We only want one.
            Destroy(gameObject);
            return; // Stop running code on this duplicate instance.
        }

        // This is the first time an instance is being created.
        instance = this; // Set the static instance to this object.

        brObject = GameObject.FindGameObjectWithTag("Brightness");

        brightnessPanel = brObject.GetComponent<Image>();

        DontDestroyOnLoad(gameObject); // Don't destroy this object when scenes change.
    }

    // Start is called before the first frame update
    void Start()
    {
        sc.SetActive(false);

        mc.SetActive(true);
        credits.SetActive(false);

        // Load the saved volume level, or use 1 (full volume) if none is saved.
        float savedVolume = PlayerPrefs.GetFloat("MusicVolumeLevel", 1f);

        // Set the slider's value to the loaded volume.
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }

        // Apply this volume to the mixer.
        SetVolume(savedVolume);

        // Load the saved brightness level, or use 1 (full brightness) if none is saved.
        float savedBrightness = PlayerPrefs.GetFloat("BrightnessLevel", 1f);
        if (brightnessSlider != null)
        {
            brightnessSlider.value = savedBrightness;
        }
        SetBrightness(savedBrightness);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Title")
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    public void PauseGame()
    {
        isPaused = true;
        sc.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Freeze game time

    }

    public void ResumeGame()
    {
        isPaused = false;
        sc.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Unfreeze game time

        if(SceneManager.GetActiveScene().name == "Title")
        {
            mc.SetActive(true);
        }
    }
    public void Play()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void HideMenu()
    {
        mc.SetActive(false);
        sc.SetActive(true);
    }

    public void ShowMenu()
    {
        mc.SetActive(true);
        sc.SetActive(false);
    }

    public void ShowCredits()
    {
        mc.SetActive(false);
        credits.SetActive(true);
    }

    public void HideCredits()
    {
        credits.SetActive(false);
        mc.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        // The Audio Mixer uses a logarithmic scale (decibels), not a linear one (0 to 1).
        // We need to convert the linear slider value to the logarithmic decibel scale.
        // A value of 0.0001 is silent (-80 dB) and 1 is full volume (0 dB).
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);

        // Save the volume setting for the next time the game is opened.
        PlayerPrefs.SetFloat("MusicVolumeLevel", volume);

        UpdateVolumeIcon(volume);

    }

    private void UpdateVolumeIcon(float volume)
    {
        if (volumeIcon == null) return; // Exit if no icon is assigned

        // Check volume and assign the correct sprite
        // You can adjust these threshold values to your liking
        if (volume <= volumeSlider.minValue) // Muted
        {
            volumeIcon.sprite = mutedIcon;
        }
        else if (volume > volumeSlider.minValue && volume <= 0.4f) // Low
        {
            volumeIcon.sprite = lowVolumeIcon;
        }
        else if (volume > 0.4f && volume <= 0.8f) // Medium
        {
            volumeIcon.sprite = midVolumeIcon;
        }
        else // High
        {
            volumeIcon.sprite = highVolumeIcon;
        }
    }

    public void SetBrightness(float brightness)
    {
        if (brightnessPanel == null) return;

        // The slider value goes from 0 (dark) to 1 (bright).
        // The panel's alpha needs to go from 1 (opaque black) to 0 (transparent).
        // So, we invert the slider's value to get the alpha.
        float alpha = 1 - brightness;

        // Set the panel's color. The RGB values stay black (0,0,0).
        brightnessPanel.color = new Color(0, 0, 0, alpha);

        // Save the brightness setting.
        PlayerPrefs.SetFloat("BrightnessLevel", brightness);
    }
}
