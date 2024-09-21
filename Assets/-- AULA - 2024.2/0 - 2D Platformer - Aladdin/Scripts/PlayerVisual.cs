using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private float runningInclination = 15f;
    [SerializeField] private SpriteRenderer parachuteRenderer;

    private PlayerRun playerRun;
    private PlayerGlide playerGlide;

    private void Awake()
    {
        playerRun = GetComponentInParent<PlayerRun>();
        playerGlide = GetComponentInParent<PlayerGlide>();
    }

    private void Start()
    {
        playerGlide.OnGlideToggle.AddListener(ctx => parachuteRenderer.enabled = ctx);
    }

    private void Update()
    {
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        currentRotation.z = playerRun.IsRunning ? runningInclination : 0.0f;
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
}