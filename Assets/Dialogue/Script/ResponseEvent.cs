using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ResponseEvent
{
    [HideInInspector] public string name;
    [SerializeField] private UnityEvent onPickResponse;

    public UnityEvent getOnPickResponse => onPickResponse;
}
