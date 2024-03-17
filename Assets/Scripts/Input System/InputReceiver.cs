using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputReceiver : MonoBehaviour
{
    protected IinputHandler[] inputHandlers;
    public abstract void OnInputReceived();

    private void Awake()
    {
        inputHandlers = GetComponents<IinputHandler>();
    }
}
