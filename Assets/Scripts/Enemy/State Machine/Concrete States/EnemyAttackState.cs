using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private Transform _playerTransform;

    private float _timer;
    private float _timeBetweenAttacks = 2f;

    private float _exitTimer; 
    private float _timeTillExit = 3f; // Time till enemy exits attack state
    private float _distanceToCountExit = 3f; // Distance to count as exit
    private float _projectileSpeed = 10f;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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

        enemy.MoveEnemy(Vector2.zero);

        if (_timer > _timeBetweenAttacks)
        {
            _timer = 0f;

            Vector2 direction = (_playerTransform.position - enemy.transform.position).normalized;

            Rigidbody2D projectile = GameObject.Instantiate(enemy.ProjectilePrefab, enemy.transform.position, Quaternion.identity);
            projectile.velocity = direction * _projectileSpeed;
        }

        if (Vector2.Distance(_playerTransform.position, enemy.transform.position) > _distanceToCountExit) //Dont use Distance check, use a trigger check
        {
            _exitTimer += Time.deltaTime;

            if (_exitTimer > _timeTillExit)
            {
                enemyStateMachine.ChangeState(enemy.FollowState);
            }
        }
        else
        {
            _exitTimer = 0f;
        }
        _timer += Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
