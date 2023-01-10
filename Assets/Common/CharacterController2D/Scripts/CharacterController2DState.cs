using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CometaPrototypes.CharacterController2D
{
    public class CharacterController2DState
    {
        // is the character colliding with something below it ?
        public bool IsCollidingBelow { get; set; }
        // is the character grounded ?
        public bool IsGrounded { get { return IsCollidingBelow; } }
        // is the character falling right now ?
        public bool IsFalling { get; set; }
        // is the character jumping right now
        public bool IsJumping { get; set; }
        // was the character grounded last frame ?
        public bool WasGroundedLastFrame { get; set; }
        // did the character just become grounded ?
        public bool JustGotGrounded { get; set; }

        public void Reset()
        {
            JustGotGrounded = false;
            IsFalling = true;
        }
    }
}