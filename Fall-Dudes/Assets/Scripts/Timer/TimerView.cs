using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviour
{
    [SerializeField]
    public Timer Timer;

    [SerializeField]
    private TMP_Text timerLabel;

    private void Update()
    {
        timerLabel.text = $"Time: {Timer.CurrentTime:0.##}";
    }
}
