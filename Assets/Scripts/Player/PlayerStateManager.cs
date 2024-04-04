using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Moving
    }

    public enum MoveDirection
    {
        North,
        South,
        East,
        West,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest,
        None // Use for Idle or no movement
    }
    private Vector2 movementInput;
    public Vector2 MovementInput => movementInput; 

    public PlayerState currentState = PlayerState.Idle;
    public MoveDirection currentDirection = MoveDirection.None;

    public void SetMovementInput(Vector2 input)
    {
        movementInput = input;
        if (input == Vector2.zero)
        {
            currentState = PlayerState.Idle;
        }
        else
        {
            currentState = PlayerState.Moving;
            DetermineDirection(input);
        }
    }

    private void DetermineDirection(Vector2 input)
    {
        // Threshold for considering diagonal movement.
        // A higher value makes diagonal movement require more deliberate input.
        float diagonalThreshold = 0.5f;

        // Check for diagonal input with a threshold
        bool isDiagonal;
        if (Mathf.Abs(input.x) < .1 || Mathf.Abs(input.y) < .1) 
        { 
            isDiagonal = false;
        }
        else { isDiagonal = Mathf.Abs(Mathf.Abs(input.x) - Mathf.Abs(input.y)) <= diagonalThreshold; }

        if (isDiagonal)
        {
            if (input.x > 0 && input.y > 0) currentDirection = MoveDirection.NorthEast;
            else if (input.x < 0 && input.y > 0) currentDirection = MoveDirection.NorthWest;
            else if (input.x > 0 && input.y < 0) currentDirection = MoveDirection.SouthEast;
            else if (input.x < 0 && input.y < 0) currentDirection = MoveDirection.SouthWest;
        }
        else
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                currentDirection = input.x > 0 ? MoveDirection.East : MoveDirection.West;
            }
            else
            {
                currentDirection = input.y > 0 ? MoveDirection.North : MoveDirection.South;
            }
        }
    }

}
