using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterControllerParameters
{
    [Header("Gravity")]
    [Tooltip("The force to apply vertically at all times")]
    public float Gravity = -30f;
    [Tooltip("a multiplier applied to the character's gravity when going down")]
    public float FallMultiplier = 1f;
    [Tooltip("a multiplier applied to the character's gravity when going up")]
    public float AscentMultiplier = 1f;

    [Header("Speed")]

    [Tooltip("Maximum velocity for the character, to prevent it from too fast")]
    public Vector2 MaxVelocity = new Vector2(100f, 100f);
    [Tooltip("Speed factor on the ground")]
    public float SpeedAccelerationOnGround = 20f;
    /// general speed factor
    [Tooltip("general speed factor")]
    public float SpeedFactor = 1;
}
