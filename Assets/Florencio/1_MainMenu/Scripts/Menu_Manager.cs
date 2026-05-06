using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Manager : MonoBehaviour
{
    [SerializeField] private GameObject tutorial1;
    [SerializeField] private GameObject tutorial2;
    [SerializeField] private GameObject tutorial3;
    [SerializeField] private GameObject tutorial4;
    [SerializeField] private GameObject tutorial5;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private RectTransform startButton;
    [SerializeField] private RectTransform selectLevelButton;
    [SerializeField] private RectTransform settingsButton;
    [SerializeField] private RectTransform creditsButton;
    [SerializeField] private RectTransform quitButton;

    [SerializeField] private GameObject selectLevelMenu;
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
    [SerializeField] private RectTransform slmBackButton;

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private RectTransform smBackButton;

    [SerializeField] private GameObject quitMenu;
    [SerializeField] private RectTransform yesButton;
    [SerializeField] private RectTransform noButton;

    private int tutorialStep = 0;

    [SerializeField] private Color locked;
    private Color unlocked = Color.white;

    public static Menu_Manager instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        bool tutorialDone = PlayerPrefs.GetInt("TutorialDone", 0) == 1;

        if(tutorialDone)
        {
            tutorial1.SetActive(false);
            tutorial2.SetActive(false);
            tutorial3.SetActive(false);
            tutorial4.SetActive(false);
            tutorial5.SetActive(false);
            mainMenu.SetActive(true);
        }
        else
        {
            tutorial1.SetActive(true);
            tutorial2.SetActive(false);
            tutorial3.SetActive(false);
            tutorial4.SetActive(false);
            tutorial5.SetActive(false);
            mainMenu.SetActive(false);
        }
        
        selectLevelMenu.SetActive(false);
        levelErrorText.gameObject.SetActive(false);
        
        settingsMenu.SetActive(false);
        
        quitMenu.SetActive(false);

        UpdateLevel2Button();
        UpdateLevel3Button();
        UpdateLevel4Button();
        
        if(PlayerPrefs.GetInt("OpenLevelSelect", 0) == 1)
        {
            PlayerPrefs.DeleteKey("OpenLevelSelect");
            PlayerPrefs.Save();

            mainMenu.SetActive(false);
            selectLevelMenu.SetActive(true);
        }
    }

    private void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Tutorial();
        }
        
        HandleClicks();
    }

    private void HandleClicks()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if(!Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        if(mainMenu.activeInHierarchy && HoveringOnButton(startButton, mousePos))
        {
            SceneManager.LoadScene("LevelOne");

            return;
        }
        else if(mainMenu.activeInHierarchy && HoveringOnButton(selectLevelButton, mousePos))
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            quitMenu.SetActive(false);
            selectLevelMenu.SetActive(true);

            return;
        }
        else if(mainMenu.activeInHierarchy && HoveringOnButton(settingsButton, mousePos))
        {
            mainMenu.SetActive(false);
            selectLevelMenu.SetActive(false);
            quitMenu.SetActive(false);
            settingsMenu.SetActive(true);

            return;
        }
        else if(mainMenu.activeInHierarchy && HoveringOnButton(creditsButton, mousePos))
        {
            SceneManager.LoadScene("Credits");

            return;
        }
        else if(mainMenu.activeInHierarchy && HoveringOnButton(quitButton, mousePos))
        {
            mainMenu.SetActive(false);
            selectLevelMenu.SetActive(false);
            settingsMenu.SetActive(false);
            quitMenu.SetActive(true);

            return;
        }
        else if(selectLevelMenu.activeInHierarchy && HoveringOnButton(levelOneButton, mousePos))
        {
            selectLevelMenu.SetActive(false);

            SceneManager.LoadScene("LevelOne");

            return;
        }
        else if(selectLevelMenu.activeInHierarchy && HoveringOnButton(levelTwoButton, mousePos))
        {
            if(LevelCompleted.IsLevel1Completed())
            {
                selectLevelMenu.SetActive(false);

                SceneManager.LoadScene("LevelTwo");

                return;
            }
            else
            {
                levelErrorText.gameObject.SetActive(true);
                levelErrorText.canvasRenderer.SetAlpha(1f);
                levelErrorText.CrossFadeAlpha(0f, 1f, false);
            }
        }
        else if(selectLevelMenu.activeInHierarchy && HoveringOnButton(levelThreeButton, mousePos))
        {
            if(LevelCompleted.IsLevel2Completed())
            {
                selectLevelMenu.SetActive(false);

                SceneManager.LoadScene("LevelThree");

                return;
            }
            else
            {
                levelErrorText.gameObject.SetActive(true);
                levelErrorText.canvasRenderer.SetAlpha(1f);
                levelErrorText.CrossFadeAlpha(0f, 1f, false);
            }
        }
        else if(selectLevelMenu.activeInHierarchy && HoveringOnButton(levelFourButton, mousePos))
        {
            if(LevelCompleted.IsLevel3Completed())
            {
                selectLevelMenu.SetActive(false);

                SceneManager.LoadScene("LevelA");

                return;
            }
            else
            {
                levelErrorText.gameObject.SetActive(true);
                levelErrorText.canvasRenderer.SetAlpha(1f);
                levelErrorText.CrossFadeAlpha(0f, 1f, false);
            }
        }
        else if(selectLevelMenu.activeInHierarchy && HoveringOnButton(slmBackButton, mousePos))
        {
            selectLevelMenu.SetActive(false);
            mainMenu.SetActive(true);

            return;
        }
        else if(settingsMenu.activeInHierarchy && HoveringOnButton(smBackButton, mousePos))
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);

            return;
        }
        else if(quitMenu.activeInHierarchy && HoveringOnButton(yesButton, mousePos))
        {
            Application.Quit();
            
            Debug.Log("Quitting Game.");
            
            return;
        }
        else if(quitMenu.activeInHierarchy && HoveringOnButton(noButton, mousePos))
        {
            quitMenu.SetActive(false);
            mainMenu.SetActive(true);

            return;
        }
    }

    private bool HoveringOnButton(RectTransform rect, Vector2 pos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, pos);
    }

    private void UpdateLevel2Button()
    {
        if(LevelCompleted.IsLevel1Completed())
        {
            levelTwoImage.color = unlocked;
            levelTwoLocked.gameObject.SetActive(false);
        }
        else
        {
            levelTwoImage.color = locked;
            levelTwoLocked.gameObject.SetActive(true);
        }
    }

    private void UpdateLevel3Button()
    {
        if(LevelCompleted.IsLevel2Completed())
        {
            levelThreeImage.color = unlocked;
            levelThreeLocked.gameObject.SetActive(false);
        }
        else
        {
            levelThreeImage.color = locked;
            levelThreeLocked.gameObject.SetActive(true);
        }
    }

    private void UpdateLevel4Button()
    {
        if(LevelCompleted.IsLevel3Completed())
        {
            levelFourImage.color = unlocked;
            levelFourLocked.gameObject.SetActive(false);
        }
        else
        {
            levelFourImage.color = locked;
            levelFourLocked.gameObject.SetActive(true);
        }
    }

    private void Tutorial()
    {
        tutorial1.SetActive(false);
        tutorial2.SetActive(false);
        tutorial3.SetActive(false);
        tutorial4.SetActive(false);
        tutorial5.SetActive(false);
        mainMenu.SetActive(false);

        tutorialStep++;

        if(tutorialStep == 1)
        {
            tutorial2.SetActive(true);
        }
        else if(tutorialStep == 2)
        {
            tutorial3.SetActive(true);
        }
        else if(tutorialStep == 3)
        {
            tutorial4.SetActive(true);
        }
        else if (tutorialStep == 4)
        {
            tutorial5.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("TutorialDone", 1);
            PlayerPrefs.Save();

            mainMenu.SetActive(true);
        }
    }
}