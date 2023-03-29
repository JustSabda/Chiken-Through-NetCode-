using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public int durationInSeconds;
    [SerializeField] int remainingTime;

    [SerializeField] TMP_Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        durationInSeconds = remainingTime;
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while(durationInSeconds >= 0)
        {
            timerText.text = $"{durationInSeconds / 60:D2} : {durationInSeconds % 60:D2}";
            durationInSeconds--;
            if (durationInSeconds <= 0) durationInSeconds = 0;
            yield return new WaitForSeconds(1);
        }
        EndTimer();
    }

    void EndTimer()
    {
        Debug.Log("Time's Up");
    }
}
