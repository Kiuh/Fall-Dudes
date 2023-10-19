using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Timer timer;

    [SerializeField]
    private StartZone startZone;

    [SerializeField]
    private PlayerCore playerCore;

    [SerializeField]
    private FinishZone finishZone;

    [SerializeField]
    private ScreensController screensController;

    [SerializeField]
    private ThirdPersonCamera playerCamera;

    [SerializeField]
    private StartLineZone startLineZone;

    private void Awake()
    {
        playerCore.OnDeath.AddListener(OnPlayerDeath);
        finishZone.OnPlayerReachFinish.AddListener(OnPlayerReachFinish);
        startLineZone.OnPlayerReachStartLine.AddListener(OnPlayerReachStartLine);
        screensController.OnPlayerPressTryAgain.AddListener(TryAgain);
        SetupPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerCamera.UnlockCursor();
        }
    }

    private void OnPlayerReachStartLine()
    {
        timer.StartTimer();
    }

    private void OnPlayerDeath()
    {
        screensController.ShowLoseScreen(timer.CurrentTime);
        LockPlayer();
        playerCore.PlayerController.PlayDeathAnimation();
    }

    private void OnPlayerReachFinish()
    {
        screensController.ShowWinScreen(timer.CurrentTime);
        LockPlayer();
        playerCore.PlayerController.PlayVictoryAnimation();
    }

    private void TryAgain()
    {
        screensController.HideScreens();
        FindObjectsOfType<FallingBlock>(true).ToList().ForEach(x => x.ResetBlock());
        SetupPlayer();
    }

    private void SetupPlayer()
    {
        timer.Reset();
        playerCamera.LockCursor();
        playerCamera.LockModelRotation = false;
        playerCore.SetHealth(playerCore.StartPlayerHealth);
        playerCore.transform.position = startZone.GetRandomPoint();
        playerCore.PlayerController.LockInteraction = false;
        playerCore.PlayerController.ReturnToNormalAnimations();
    }

    private void LockPlayer()
    {
        playerCamera.LockModelRotation = true;
        playerCore.PlayerController.LockInteraction = true;
        playerCore.PlayerController.ResetForces();
        playerCamera.UnlockCursor();
        timer.Stop();
    }
}
