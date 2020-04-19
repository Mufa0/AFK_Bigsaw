using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayScreenManager : MonoBehaviour
{


    public Text timerText;
    private float timer;

    public void Init()
    {
        if(EventManager.playScreenEnabled == null)
        {
            EventManager.playScreenEnabled = new PlayScreenEvent();
        }

        
    }

    public void Start()
    {
        timer = 0f;
    }

    public void Update()
    {

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;

        var ss = System.Convert.ToInt32(timer % 60).ToString("00");
        var mm = (System.Math.Floor(timer / 60)).ToString("00");
        timerText.text = mm + ":" + ss;

    }

    public void startNewGame()
    {
        GameObject puzzle = this.GetComponentInChildren<PuzzleController>().gameObject;
        GameObject puzzleParent = puzzle.transform.parent.gameObject;
        if (puzzle != null)
        {
            DestroyImmediate(puzzle);
            Instantiate(GameController.newPuzzlePrefab).transform.SetParent(puzzleParent.transform);
        }
    }

    private void OnEnable()
    {
        if (EventManager.playScreenEnabled == null)
        {
            EventManager.playScreenEnabled = new PlayScreenEvent();
        }
        EventManager.playScreenEnabled.Invoke(true);
    }


    private void OnDisable()
    {
        if (EventManager.playScreenEnabled == null)
        {
            EventManager.playScreenEnabled = new PlayScreenEvent();
        }
        EventManager.playScreenEnabled.Invoke(false);
    }
}

