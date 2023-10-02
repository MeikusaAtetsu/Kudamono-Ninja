using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    bool run = false;
    bool showTimeLeft = true;
    bool timeEnd = false;

    float startTime = 0.0f;
    float curTime = 0.0f;
    string curStrTime = string.Empty;
    bool pause = false;

    public float timeAvailable = 30f; // 30 seconds
    float showTime = 0;

    public Text guiTimer;
	public GameObject finishedUI;

    void Start()
    {
        RunTimer();
    }

    public void RunTimer()
    {
        MouseControl.Instance.points = 0;
        Time.timeScale = 1;
        run = true;
        startTime = Time.time;
    }

    public void PauseTimer(bool b)
    {
        pause = b;
    }

    public void EndTimer()
    {
        Time.timeScale = 0;
        run = false;
    }
	
	void Update () {

        if (pause)
        {
            startTime = startTime + Time.deltaTime;
            return;
        }

        if (run)
        {
            curTime = Time.time - startTime;
        }

        if (showTimeLeft)
        {
            showTime = timeAvailable - curTime;
            if (showTime <= 0)
            {
                timeEnd = true;
                showTime = 0;

                //弹出UI界面，告诉用户本轮游戏结束。
                //暂停/停止游戏
				finishedUI.SetActive(true);
                EndTimer();
            }
        }

        int minutes = (int) (showTime / 60);
        int seconds = (int) (showTime % 60);
        int fraction = (int) ((showTime * 100) % 100);

        curStrTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
        guiTimer.text = "Time: " + curStrTime;
	
	}
}
