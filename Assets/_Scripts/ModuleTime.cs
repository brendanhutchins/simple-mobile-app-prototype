using System.Collections;
using System.Collections.Generic;
using _Scripts.Firebase;
using TMPro;
using UnityEngine;

public class ModuleTime : MonoBehaviour
{
    private float startingTime;
    //[SerializeField] private TextMeshProUGUI timeDisplay;
    private float moduleTime;

    private void OnEnable()
    {
        startingTime = Time.time;
        StartCoroutine(TrackTime());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator TrackTime()
    {
        moduleTime = Time.time - startingTime;
        //timeDisplay.text = moduleTime.ToString();
        yield return new WaitForSeconds(0.05f);
        yield return TrackTime();
    }

    public void SubmitTime(int moduleNum)
    {
        // Firebase event here
        // Submit module time
        PlayerPrefs.SetFloat("module" + moduleNum + "Time", moduleTime);
        FirebaseManager.Instance.SaveSessionMetrics();
    }
}
