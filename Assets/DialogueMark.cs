using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMark : MonoBehaviour
{

    private void Start()
    {
        shift = initialPos;
    }

    [SerializeField] private float initialPos;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxShift;


    [SerializeField]private float shift;
    [SerializeField]private float amp;
    [SerializeField]private bool movingUp = true;
    private void FixedUpdate()
    {
        if (movingUp)
        {
            shift += maxSpeed - Mathf.Pow(shift - initialPos, 2) / amp;
        }
        else
        {
            shift -= maxSpeed - Mathf.Pow(shift - initialPos, 2) / amp;
        }

        if (shift > initialPos + maxShift)
        {
            movingUp = false;
        }
        if(shift < initialPos - maxShift)
        {
            movingUp = true;
        }

        var tr = transform;
        var pos = tr.parent.position;
        pos.y += 0.8f + shift;
        tr.position = pos;
    }
}
