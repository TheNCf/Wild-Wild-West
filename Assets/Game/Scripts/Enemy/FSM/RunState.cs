using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
    private readonly EnemyAI _enemy;
    private readonly float _attackRange;

    private List<Transition> _transitions = new List<Transition>();

    public RunState(EnemyAI enemy, float attackRange)
    {
        _enemy = enemy;
        _attackRange = attackRange;

        _transitions.Add(new Transition(
            condition: () => _enemy.Target != null && Vector3.Distance(_enemy.transform.position, _enemy.Target.Position) <= _attackRange,
            targetState: typeof(ShootState)
        ));
    }

    public IReadOnlyList<Transition> Transitions => _transitions;

    public void Enter()
    {
        if (_enemy.Agent.isActiveAndEnabled)
            _enemy.Agent.isStopped = false;
    }

    public void Tick()
    {
        if (_enemy.Target != null)
            _enemy.Agent.SetDestination(_enemy.Target.Position);
    }

    public void Exit()
    {
        if (_enemy.Agent.isActiveAndEnabled)
            _enemy.Agent.isStopped = true;
    }
}
