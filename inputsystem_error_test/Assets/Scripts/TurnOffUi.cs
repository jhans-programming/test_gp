using UnityEngine;
using TMPro;

public class TurnOffUi : MonoBehaviour
{
    public TextMeshProUGUI tmpText;   // Assign your TMP object in Inspector
    public float disableDelay = 3f;   // Seconds before hiding

    void Start()
    {
        // Start the countdown to disable text
        StartCoroutine(DisableTMP());
    }

    private System.Collections.IEnumerator DisableTMP()
    {
        yield return new WaitForSeconds(disableDelay);

        if (tmpText != null)
            tmpText.gameObject.SetActive(false);
    }
}
