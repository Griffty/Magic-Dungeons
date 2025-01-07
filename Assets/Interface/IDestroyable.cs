
using System;
using UnityEngine;

public interface IDestroyable
{
    public void At0Hp();
    public static void HasHealthHandler(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out HealthHandler h))
        {
            throw new Exception(gameObject.name + " does not contain heath Handler");
        }
    }
}
