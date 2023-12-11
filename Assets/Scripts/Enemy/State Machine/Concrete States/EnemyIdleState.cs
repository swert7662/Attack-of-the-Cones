using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType TriggerType)
    {
        base.AnimationTriggerEvent(TriggerType);

        enemy.EnemyIdleBaseInstance.DoAnimationTriggerEventLogic(TriggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        enemy.EnemyIdleBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();

        enemy.EnemyIdleBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        enemy.EnemyIdleBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        enemy.EnemyIdleBaseInstance.DoPhysicsUpdateLogic();
    }
}
