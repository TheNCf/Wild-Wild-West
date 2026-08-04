using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
    private readonly EnemyAI _enemy;
    private readonly float _attackRange;
    private readonly Transform _shootPoint;

    private List<Transition> _transitions = new List<Transition>();

    public RunState(EnemyAI enemy, float attackRange, Transform shootPoint)
    {
        _enemy = enemy;
        _attackRange = attackRange;
        _shootPoint = shootPoint;

        _transitions.Add(new Transition(
            condition: () => CheckTransitionToShoot(),
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

    private bool CheckTransitionToShoot()
    {
        if (_enemy.Target == null)
            return false;

        Ray ray = new Ray(_shootPoint.position, _enemy.Target.Position - _shootPoint.position + Vector3.up);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _attackRange) == false)
            return false;

        if (hitInfo.collider.TryGetComponent(out CharacterHealth _) == false)
            return false;

        return true;
    }
}
