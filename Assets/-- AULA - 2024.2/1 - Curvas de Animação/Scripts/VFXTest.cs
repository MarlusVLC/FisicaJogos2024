using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXTest : MonoBehaviour
{
    public string subsystemName;
    public bool debugOnUpdate;
    
    private VisualEffect vfx;

    private List<string> systemNames = new();

    private void Awake()
    {
        vfx = GetComponent<VisualEffect>();
    }

    private void Update()
    {
        if (debugOnUpdate == false)
        {
            return;
        }
        DebugSystemNames();
        Debug.Log($"{subsystemName} alive count: {vfx.GetParticleSystemInfo(subsystemName).aliveCount}");
    }

    [ContextMenu("Debug System Names")]
    private void DebugSystemNames()
    {
        print("Alive particles count: " + vfx.aliveParticleCount);
        // vfx.GetOutputEventNames(systemNames);
        // Debug.Log("Showing ALL SYSTEM names for " + gameObject.name);
        // foreach (var name in systemNames)
        // {
        //     print(name);
        // }
    }
}
