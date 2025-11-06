using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltEvents;

public class UltEventConverter : MonoBehaviour
{
    [SerializeField] UltEvent ultEvent;

    public void CallEvent()
    {
        ultEvent.Invoke();
    }
}
