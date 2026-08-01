using System;

public class Transition
{
    public Func<bool> Condition { get; }
    public Type TargetState { get; }

    public Transition(Func<bool> condition, Type targetState)
    {
        Condition = condition;
        TargetState = targetState;
    }
}