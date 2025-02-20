
using UnityEngine;

public class DoubleClick
{
    private int _clicked;
    private float _clickTime;
    private float _clickDelay;

    public DoubleClick(float clickDelay)
    {
        _clickDelay = clickDelay;
    }
    
    public bool Click()
    {
        _clicked++;
        if (_clicked == 1) _clickTime = Time.time;
    
        if (_clicked > 1 && Time.time - _clickTime < _clickDelay)
        {
            _clicked = 0;
            _clickTime = 0;
            return true;
        }
        if (_clicked > 2 || Time.time - _clickTime > 1) _clicked = 0;
        return false;
    }   
}