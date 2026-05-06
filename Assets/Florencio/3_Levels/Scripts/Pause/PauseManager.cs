using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private Canvas resolutionUI;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private RectTransform resumeButton;
    [SerializeField] private RectTransform levelSelectButton;
    [SerializeField] private RectTransform settingsButton;
    [SerializeField] private RectTransform exitButton;

    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private TextMeshProUGUI levelErrorText;
    [SerializeField] private RectTransform levelOneButton;
    [SerializeField] private RectTransform levelTwoButton;
    [SerializeField] private Image levelTwoImage;
    [SerializeField] private Image levelTwoLocked;
    [SerializeField] private RectTransform levelThreeButton;
    [SerializeField] private Image levelThreeImage;
    [SerializeField] private Image levelThreeLocked;
    [SerializeField] private RectTransform levelFourButton;
    [SerializeField] private Image levelFourImage;
    [SerializeField] private Image levelFourLocked;
    [SerializeField] private RectTransform lsBackButton;

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private RectTransform sBackButton;

    [SerializeField] private Color lockedColor;
    private Color unlockedColor = Color.white;

    private bool isPaused = false;

    private void Awake()
    {
        Time.timeScale = 1f;

        pauseMenu.SetActive(false);

        levelSelectMenu.SetActive(false);
        levelErrorText.gameObject.SetActive(false);

        settingsMenu.SetActive(false);

        UpdateLevel2Button();
        UpdateLevel3Button();
        UpdateLevel4Button();

        if (PlayerPrefs.GetInt("OpenLevelSelect", 0) == 1)
        {
            PlayerPrefs.DeleteKey("OpenLevelSelect");
            PlayerPrefs.Save();

            Time.timeScale = 0f;
            isPaused = true;

            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            levelSelectMenu.SetActive(true);
        }
    }

    private void Update()
    {
        if(Keyboard.current.pKey.wasPressedThisFrame)
        {
            Paused();
        }
        HandleClicks();
    }

    private void HandleClicks()
    {
        if(!Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if(resumeButton.gameObject.activeInHierarchy && HoveringOverButton(resumeButton, mousePosition))
        {
            Resume();
            
            return;
        }

        if(levelSelectButton.gameObject.activeInHierarchy && HoveringOverButton(levelSelectButton, mousePosition))
        {
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            levelSelectMenu.SetActive(true);
            
            return;
        }

        if(settingsButton.gameObject.activeInHierarchy && HoveringOverButton(settingsButton, mousePosition))
        {
            pauseMenu.SetActive(false);
            levelSelectMenu.SetActive(false);
            settingsMenu.SetActive(true);
            
            return;
        }

        if(exitButton.gameObject.activeInHierarchy && HoveringOverButton(exitButton, mousePosition))
        {
            isPaused = false;

            Time.timeScale = 1f;

            SceneManager.LoadScene("MainMenu");

            return;
        }

        if(levelSelectMenu.activeInHierarchy && HoveringOverButton(levelOneButton, mousePosition))
        {
            LoadScene("LevelOne");
        }

        if(levelSelectMenu.activeInHierarchy && HoveringOverButton(levelTwoButton, mousePosition))
        {
            if(LevelCompleted.IsLevel1Completed())
            {
                LoadScene("LevelTwo");
            }
            else
            {
                levelErrorText.gameObject.SetActive(true);
                levelErrorText.canvasRenderer.SetAlpha(1f);
                levelErrorText.CrossFadeAlpha(0f, 0.5f, true);
            }
        }

        if(levelSelectMenu.activeInHierarchy && HoveringOverButton(levelThreeButton, mousePosition))
        {
            if(LevelCompleted.IsLevel2Completed())
            {
                LoadScene("LevelThree");
            }
            else
            {
                levelErrorText.gameObject.SetActive(true);
                levelErrorText.canvasRenderer.SetAlpha(1f);
                levelErrorText.CrossFadeAlpha(0f, 0.5f, true);
            }
        }

        if(levelSelectMenu.activeInHierarchy && HoveringOverButton(levelFourButton, mousePosition))
        {
            if(LevelCompleted.IsLevel3Completed())
            {
                LoadScene("LevelA");
            }
            else
            {
                levelErrorText.gameObject.SetActive(true);
                levelErrorText.canvasRenderer.SetAlpha(1f);
                levelErrorText.CrossFadeAlpha(0f, 0.5f, true);
            }
        }

        if(levelSelectMenu.activeInHierarchy && HoveringOverButton(lsBackButton, mousePosition))
        {
            levelSelectMenu.SetActive(false);
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(true);

            return;
        }

        if(settingsMenu.activeInHierarchy && HoveringOverButton(sBackButton, mousePosition))
        {
            settingsMenu.SetActive(false);
            levelSelectMenu.SetActive(false);
            pauseMenu.SetActive(true);

            return;
        }
    }

    private bool HoveringOverButton(RectTransform rect, Vector2 position)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, position);
    }

    private void Paused()
    {
        if(isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;

        isPaused = true;

        pauseMenu.SetActive(true);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    private void Resume()
    {
        pauseMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);

        isPaused = false;

        Time.timeScale = 1f;
    }

    private void LoadScene(string sceneName)
    {
        isPaused = false;

        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);

        return;
    }

    private void UpdateLevel2Button()
    {
        if(LevelCompleted.IsLevel1Completed())
        {
            levelTwoImage.color = unlockedColor;
            levelTwoLocked.gameObject.SetActive(false);
        }
        else
        {
            levelTwoImage.color = lockedColor;
            levelTwoLocked.gameObject.SetActive(true);
        }
    }

    private void UpdateLevel3Button()
    {
        if(LevelCompleted.IsLevel2Completed())
        {
            levelThreeImage.color = unlockedColor;
            levelThreeLocked.gameObject.SetActive(false);
        }
        else
        {
            levelThreeImage.color = lockedColor;
            levelThreeLocked.gameObject.SetActive(true);
        }
    }

    private void UpdateLevel4Button()
    {
        if(LevelCompleted.IsLevel3Completed())
        {
            levelFourImage.color = unlockedColor;
            levelFourLocked.gameObject.SetActive(false);
        }
        else
        {
            levelFourImage.color = lockedColor;
            levelFourLocked.gameObject.SetActive(true);
        }
    }
}