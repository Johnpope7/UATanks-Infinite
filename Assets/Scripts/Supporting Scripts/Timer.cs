using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float timerDuration = 1f * 60f; //how long the timer lasts
    public float timer; //the actual timer
    [SerializeField]
    private Text firstMinute; //the double digit minute slot on the timer
    [SerializeField]
    private Text secondMinute; //the single digit minute slot on the timer
    [SerializeField]
    private Text seperatorMinute; //the divider for the timer
    [SerializeField]
    private Text firstSecond; //the double digit second slot on the timer
    [SerializeField]
    private Text secondSecond; //the single digit second slot on the timer

    #endregion

    #region Functions
    #region Builtin Functions
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance) 
        {
            ResetTimer(); //if our game manager is available it starts the timer at start
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime; //subtracts time from timer
            UpdateTimerDisplay(timer);
        }
        else 
        {
            Flash();
        }
    }
    #endregion
    #region Custom Functions
    private void ResetTimer() 
    {
        timer = timerDuration; //resets timer
    }

    private void UpdateTimerDisplay(float time) 
    {
        float minutes = Mathf.FloorToInt(time / 60); //makes sure our minutes are actually zero, rounds down
        float seconds = Mathf.FloorToInt(time % 60); //makes sure our seconds are actually zero, rounds down

        string currentTime = string.Format("{00:00}{1:00}", minutes, seconds); //formats or timer
        firstMinute.text = currentTime[0].ToString(); //sets firstMinute equal to the first character in the array
        secondMinute.text = currentTime[1].ToString(); //sets secondMinute equal to the second character in the array
        firstSecond.text = currentTime[2].ToString(); //sets firstSecond equal to the third character in the array
        secondSecond.text = currentTime[3].ToString(); //sets secondSecond equal to the fourth character in the array
    }
    private void Flash() 
    {
        if (timer != 0) 
        {
            timer = 0;
            UpdateTimerDisplay(timer);
        }
    }
    #endregion
    #endregion
}
