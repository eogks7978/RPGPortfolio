using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 움직일 수 있는 객체에 상속
/// </summary>
public interface IMovable
{
    float MoveSpeed { get; }
    bool IsFacingLeft { get; }

    void CheckForLeftOrRightFacing(Vector2 velocity);
}
