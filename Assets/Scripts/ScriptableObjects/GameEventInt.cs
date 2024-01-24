using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Pass Int Event")]
public class GameEventInt : ScriptableObject
{
    public Action<int> onEventRaised;

    public void EventRaised(int playerSteps)
    {
        onEventRaised?.Invoke(playerSteps);
    }
}
