using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowState : EnemyState
{

    public EnemyFollowState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType TriggerType)
    {
        base.AnimationTriggerEvent(TriggerType);

        enemy.EnemyFollowBaseInstance.DoAnimationTriggerEventLogic(TriggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        enemy.EnemyFollowBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();

        enemy.EnemyFollowBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        enemy.EnemyFollowBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        enemy.EnemyFollowBaseInstance.DoPhysicsUpdateLogic();
    }
}
