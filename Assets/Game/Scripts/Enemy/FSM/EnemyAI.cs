using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _attackRange = 10f;
    [SerializeField] private Component _targetComponent;
    [SerializeField] private Transform _shootPoint;

    private ITargetable _target;
    private IState _currentState;
    private Dictionary<Type, IState> _states;

    public NavMeshAgent Agent { get; private set; }
    public ITargetable Target => _target;

    private void OnValidate()
    {
        if (_targetComponent != null && _targetComponent is not ITargetable)
        {
            Debug.LogError($"[{nameof(EnemyAI)}] Error. {_targetComponent.name} doesn't implement ITargetable");
            _targetComponent = null;
        }
    }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

        if (_targetComponent != null)
        {
            _target = _targetComponent as ITargetable;

            if (_target == null)
                Debug.LogError($"[{nameof(EnemyAI)}] No object implementing ITargetable");
        }

        InitializeStateMachine();
    }

    private void Update()
    {
        if (_currentState == null) 
            return;

        _currentState.Tick();

        CheckTransitions();
    }

    public void ChangeState<T>() where T : IState => ChangeState(typeof(T));

    private void InitializeStateMachine()
    {
        _states = new Dictionary<Type, IState>
        {
            { typeof(RunState), new RunState(this, _attackRange, _shootPoint) },
            { typeof(ShootState), new ShootState(this, _attackRange, _shootPoint) }
        };

        ChangeState<RunState>();
    }

    private void CheckTransitions()
    {
        foreach (var transition in _currentState.Transitions)
        {
            if (transition.Condition())
            {
                ChangeState(transition.TargetState);
                break;
            }
        }
    }

    private void ChangeState(Type newStateType)
    {
        _currentState?.Exit();
        _currentState = _states[newStateType];
        _currentState.Enter();
    }
}