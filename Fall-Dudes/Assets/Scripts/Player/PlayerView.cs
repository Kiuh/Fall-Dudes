using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private PlayerCore playerCore;

    [SerializeField]
    private TMP_Text healthLabel;

    private void Update()
    {
        healthLabel.text = $"Health: {playerCore.Health:0.##}";
    }
}
