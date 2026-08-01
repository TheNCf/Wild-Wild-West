using System.Collections;
using System.Collections.Generic;

public interface IState
{
    public IReadOnlyList<Transition> Transitions { get; }
    public void Enter();
    public void Tick();
    public void Exit();
}
