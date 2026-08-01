using System.Collections.Generic;
using UnityEngine;

public class ShootState : IState
{
    private readonly EnemyAI _enemy;
    private readonly float _attackRange;

    public List<Transition> _transitions = new List<Transition>();

    public ShootState(EnemyAI enemy, float attackRange)
    {
        _enemy = enemy;
        _attackRange = attackRange;

        _transitions.Add(new Transition(
            condition: () => _enemy.Target == null || Vector3.Distance(_enemy.transform.position, _enemy.Target.Position) > _attackRange,
            targetState: typeof(RunState)
        ));
    }

    public IReadOnlyList<Transition> Transitions => _transitions;

    public void Enter()
    {
        _enemy.Agent.isStopped = true;
    }

    public void Tick()
    {
        if (_enemy.Target == null) return;

        Vector3 direction = (_enemy.Target.Position - _enemy.transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
    }

    public void Exit() { }
}
