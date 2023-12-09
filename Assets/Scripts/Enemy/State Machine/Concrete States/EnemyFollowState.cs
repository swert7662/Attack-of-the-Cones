using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowState : EnemyState
{
    private Transform _playerTransform;
    private float _movementSpeed = 1.75f;
    public EnemyFollowState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType TriggerType)
    {
        base.AnimationTriggerEvent(TriggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector2 moveDirection = (_playerTransform.position - enemy.transform.position).normalized;
        enemy.MoveEnemy(moveDirection * _movementSpeed);

        if (enemy.IsWithinStrikingDistance)
        {
            enemyStateMachine.ChangeState(enemy.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
