using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Events/Void Event")]
public class GameEventVoid : ScriptableObject
{
    public Action onEventRaised;

    public void EventRaised()
    {
        onEventRaised?.Invoke();
    }
}
