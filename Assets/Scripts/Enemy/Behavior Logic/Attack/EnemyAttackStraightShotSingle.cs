using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Straight Shot Single", menuName = "Enemy Logic/Attack Logic/Straight Shot Single")]
public class EnemyAttackStraightShotSingle : EnemyAttackSOBase
{
    [SerializeField] private Rigidbody2D ProjectilePrefab;
    [SerializeField] private float _timeBetweenAttacks = 2f;
    [SerializeField] private float _timeTillExit = 3f; // Time till enemy exits attack state
    [SerializeField] private float _distanceToCountExit = 3f; // Distance to count as exit
    [SerializeField] private float _projectileSpeed = 10f;

    private float _timer;
    private float _exitTimer;
    
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        enemy.MoveEnemy(Vector2.zero);

        if (_timer > _timeBetweenAttacks)
        {
            _timer = 0f;

            Vector2 direction = (playerTransform.position - enemy.transform.position).normalized;

            Rigidbody2D projectile = GameObject.Instantiate(ProjectilePrefab, enemy.transform.position, Quaternion.identity);
            projectile.velocity = direction * _projectileSpeed;
        }

        if (Vector2.Distance(playerTransform.position, enemy.transform.position) > _distanceToCountExit) //Dont use Distance check, use a trigger check
        {
            _exitTimer += Time.deltaTime;

            if (_exitTimer > _timeTillExit)
            {
                enemy.StateMachine.ChangeState(enemy.FollowState);
            }
        }
        else
        {
            _exitTimer = 0f;
        }
        _timer += Time.deltaTime;
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
