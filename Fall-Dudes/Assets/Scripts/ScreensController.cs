using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreensController : MonoBehaviour
{
    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private TMP_Text winScreenTime;

    [SerializeField]
    private Button winScreenButtonTryAgain;

    [SerializeField]
    private GameObject loseScreen;

    [SerializeField]
    private TMP_Text loseScreenTime;

    [SerializeField]
    private Button loseScreenButtonTryAgain;

    public UnityEvent OnPlayerPressTryAgain;

    private void Awake()
    {
        winScreenButtonTryAgain.onClick.AddListener(TryAgain);
        loseScreenButtonTryAgain.onClick.AddListener(TryAgain);
    }

    public void HideScreens()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    public void ShowWinScreen(float time)
    {
        winScreen.SetActive(true);
        winScreenTime.text = $"Time: {time:0.##}";
    }

    public void ShowLoseScreen(float time)
    {
        loseScreen.SetActive(true);
        loseScreenTime.text = $"Time: {time:0.##}";
    }

    public void TryAgain()
    {
        OnPlayerPressTryAgain?.Invoke();
    }
}
