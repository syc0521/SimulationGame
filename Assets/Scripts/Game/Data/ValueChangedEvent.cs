using System;

public class ValueChangedEvent<T>
{
    private Delegate _evt;
    private T _value;

    public ValueChangedEvent(Action<T> evt, T value = default)
    {
        _evt = evt;
        _value = value;
    }

    public T Value
    {
        set
        {
            if (_value.Equals(value)) return;
            _value = value;
            _evt?.DynamicInvoke(value);
        }
        get => _value;
    }

    public void SetEvent(Action<T> action)
    {
        _evt = action;
    }

}
