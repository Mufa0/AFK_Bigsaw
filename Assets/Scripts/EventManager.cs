using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static ActiveEvent activeEvent;
   
}
public class ActiveEvent : UnityEvent<string> { };
