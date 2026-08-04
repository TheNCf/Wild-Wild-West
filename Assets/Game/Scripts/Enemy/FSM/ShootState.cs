using System.Collections.Generic;
using UnityEngine;

public class ShootState : IState
{
    private readonly EnemyAI _enemy;
    private readonly float _attackRange;
    private readonly Transform _shootPoint;

    public List<Transition> _transitions = new List<Transition>();

    public ShootState(EnemyAI enemy, float attackRange, Transform shootPoint)
    {
        _enemy = enemy;
        _attackRange = attackRange;
        _shootPoint = shootPoint;

        _transitions.Add(new Transition(
            condition: () => CheckTransitionToRun(),
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

    private bool CheckTransitionToRun()
    {
        if (_enemy.Target == null)
            return true;

        Ray ray = new Ray(_shootPoint.position, _enemy.Target.Position - _shootPoint.position + Vector3.up);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _attackRange) == false)
            return true;

        if (hitInfo.collider.TryGetComponent(out CharacterHealth _) == false)
            return true;

        return false;
    }
}
