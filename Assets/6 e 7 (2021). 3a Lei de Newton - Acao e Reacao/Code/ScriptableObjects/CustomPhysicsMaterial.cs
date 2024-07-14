using UnityEngine;

[CreateAssetMenu(fileName = "Custom Physics Material", menuName = "ScriptableObjects/Physics/Material", order = 1)]
public class CustomPhysicsMaterial : ScriptableObject
{
    [Min(0)] public float Bounciness = 0.45f;
    [Min(0)] public float Friction = 0.01f;

    public static float GetResultantBounciness(CustomPhysicsMaterial a, CustomPhysicsMaterial b)
    {
        return (a.Bounciness + b.Bounciness) / 2;
    }
    
    public static float GetResultantFriction(CustomPhysicsMaterial a, CustomPhysicsMaterial b)
    {
        return (a.Friction + b.Friction) / 2;
    }
}