using UnityEngine;

namespace Sandbox
{
    struct DetectedObject
    {
        GravitySignature signature;
        SpaceMaterial material;
        Vector2 position;
        // velocity is 0 if only GWI data can be used
        Vector2 velocity;
        bool kepler;
    }
}