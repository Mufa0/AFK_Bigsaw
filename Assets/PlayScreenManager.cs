using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScreenManager : MonoBehaviour
{

    private void Start()
    {
        if(EventManager.playScreenEnabled == null)
        {
            EventManager.playScreenEnabled = new PlayScreenEvent();
        }
    }

    private void OnEnable()
    {
        EventManager.playScreenEnabled.Invoke(true);
    }

    private void OnDisable()
    {
        EventManager.playScreenEnabled.Invoke(false);
    }
}

