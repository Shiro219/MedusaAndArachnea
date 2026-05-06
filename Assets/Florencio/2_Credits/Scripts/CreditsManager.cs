// Done
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI thanks;
    
    [SerializeField] private RectTransform exitButton;
    [SerializeField] private GameObject exitMenu;
    
    [SerializeField] private RectTransform toMenu;
    [SerializeField] private RectTransform quitGame;

    private Animator ani;

    private float timer = 0f;

    private bool creditsFinished = false;
    private bool exitButtonActive = false;

    AnimatorStateInfo stateInfo;

    private void Awake()
    {
        creditsText.gameObject.SetActive(true);

        thanks.gameObject.SetActive(false);

        exitButton.gameObject.SetActive(false);
        exitMenu.SetActive(false);

        ani = creditsText.GetComponent<Animator>();
    }

    private void Update()
    {
        HandleClicks();

        if (Keyboard.current.spaceKey.isPressed)
        {
            ani.speed = 4f;
        }
        else
        {
            ani.speed = 1f;
        }

        if (creditsFinished)
        {
            if (!exitButtonActive)
            {
                timer += Time.deltaTime;

                if (timer >= 2f)
                {
                    exitButton.gameObject.SetActive(true);
                    exitButtonActive = true;
                }
            }
            return;
        }

        stateInfo = ani.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Credits_Scroll") && stateInfo.normalizedTime >= 1f)
        {
            thanks.gameObject.SetActive(true);
            creditsFinished = true;
        }
    }

    private void HandleClicks()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if(!Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        if(Hovering(exitButton, mousePos))
        {
            creditsText.gameObject.SetActive(false);
            
            thanks.gameObject.SetActive(false);
            
            exitButton.gameObject.SetActive(false);
            exitMenu.SetActive(true);
            
            return;
        }
        else if(Hovering(toMenu, mousePos))
        {
            SceneManager.LoadScene("MainMenu");
            
            exitMenu.SetActive(false);

            return;
        }
        else if(Hovering(quitGame, mousePos))
        {
            Application.Quit();

            Debug.Log("Quitting Game.");

            return;
        }
    }

    private bool Hovering(RectTransform rect, Vector2 pos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, pos);
    }
}