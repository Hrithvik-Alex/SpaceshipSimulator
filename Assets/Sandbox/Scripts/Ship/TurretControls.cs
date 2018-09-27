using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sandbox
{
    public class TurretControls
    {
        public Vector2 aimTo;
        public bool isTriggerPulled;
        public float cooldownRemaining = 0f;
    }
}