using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeElapsed;
    private bool isTimerRunning = true;

    void Update()
    {
        if (!isTimerRunning) return;

        timeElapsed += Time.deltaTime;
        timerText.text = timeElapsed.ToString("F2");
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }
}
