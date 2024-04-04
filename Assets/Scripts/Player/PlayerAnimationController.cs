using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerStateManager stateManager;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private List<Sprite> staticIdleSprites;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HandleAnimation();
        AdjustAnimationSpeed();
    }

    private void HandleAnimation()
    {
        switch (stateManager.currentState)
        {
            case PlayerStateManager.PlayerState.Idle:
                animator.CrossFade("Idle", 0, 0);
                PlayDirectionalIdleAnimation();
                break;
            case PlayerStateManager.PlayerState.Moving:
                if (playerStats.Speed > 9) { PlayDirectionalRunAnimation(); }
                else { PlayDirectionalWalkAnimation(); }                
                break;
        }
    }
    private void AdjustAnimationSpeed()
    {
        if (playerStats.Speed > 9)
        {
            // Ensures animation speed increases with player speed but at a controlled rate
            float animSpeed = Mathf.Clamp(playerStats.Speed / 9, 1, 2); // Example: Caps the maximum speed at 2
            animator.SetFloat("MoveSpeed", animSpeed);
        }
        else
        {
            // Default animation speed
            float animSpeed = Mathf.Clamp(playerStats.Speed / 6, 1, 2);
            animator.SetFloat("MoveSpeed", animSpeed);
        }
    }

    private void PlayDirectionalRunAnimation()
    {
        switch (stateManager.currentDirection)
        {
            case PlayerStateManager.MoveDirection.North:
                animator.CrossFade("Run North", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.South:
                animator.CrossFade("Run South", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.East:
                animator.CrossFade("Run East", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.West:
                animator.CrossFade("Run West", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.NorthEast:
                animator.CrossFade("Run NorthEast", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.NorthWest:
                animator.CrossFade("Run NorthWest", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.SouthEast:
                animator.CrossFade("Run SouthEast", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.SouthWest:
                animator.CrossFade("Run SouthWest", 0, 0);
                break;
            // Default case for None or any other undefined states
            default:
                animator.CrossFade("Idle", 0, 0); // Using 0s for instant transition
                break;
        }
    }

    private void PlayDirectionalWalkAnimation()
    {
        switch (stateManager.currentDirection)
        {
            case PlayerStateManager.MoveDirection.North:
                animator.CrossFade("Walk North", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.South:
                animator.CrossFade("Walk South", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.East:
                animator.CrossFade("Walk East", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.West:
                animator.CrossFade("Walk West", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.NorthEast:
                animator.CrossFade("Walk NorthEast", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.NorthWest:
                animator.CrossFade("Walk NorthWest", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.SouthEast:
                animator.CrossFade("Walk SouthEast", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.SouthWest:
                animator.CrossFade("Walk SouthWest", 0, 0);
                break;
            // Default case for None or any other undefined states
            default:
                animator.CrossFade("Idle", 0, 0); // Using 0s for instant transition
                break;
        }
    }

    private void PlayDirectionalIdleAnimation()
    {
        switch (stateManager.currentDirection)
        {
            case PlayerStateManager.MoveDirection.North:
                animator.CrossFade("Idle North", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.South:
                animator.CrossFade("Idle South", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.East:
                animator.CrossFade("Idle East", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.West:
                animator.CrossFade("Idle West", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.NorthEast:
                animator.CrossFade("Idle NorthEast", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.NorthWest:
                animator.CrossFade("Idle NorthWest", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.SouthEast:
                animator.CrossFade("Idle SouthEast", 0, 0);
                break;
            case PlayerStateManager.MoveDirection.SouthWest:
                animator.CrossFade("Idle SouthWest", 0, 0);
                break;
        }
    }
}
