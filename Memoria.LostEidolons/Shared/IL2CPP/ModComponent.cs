using System;
using BepInEx.Logging;
using Memoria.LostEidolons.Configuration;
using Memoria.LostEidolons.Core;
using UnityEngine;
using Exception = System.Exception;
using Logger = BepInEx.Logging.Logger;

namespace Memoria.LostEidolons.IL2CPP;

public sealed class ModComponent : MonoBehaviour
{
    public static ModComponent Instance;
    public static ManualLogSource Log;

    [field: NonSerialized] public ModConfiguration Config;
    [field: NonSerialized] public GameSpeedControl SpeedControl;

    public ModComponent(IntPtr ptr) : base(ptr)
    {
    }
    
    private Boolean _isDisabled;

    public void Awake()
    {
        Log = Logger.CreateLogSource("Memoria IL2CPP");
        Log.LogMessage($"[{nameof(ModComponent)}].{nameof(Awake)}(): Begin...");
        try
        {
            Instance = this;
    
            Config = new ModConfiguration();
            SpeedControl = new GameSpeedControl();
    
            Log.LogMessage($"[{nameof(ModComponent)}].{nameof(Awake)}(): Processed successfully.");
        }
        catch (Exception ex)
        {
            _isDisabled = true;
            Log.LogError($"[{nameof(ModComponent)}].{nameof(Awake)}(): {ex}");
            throw;
        }
    }
    
    public void OnDestroy()
    {
        Log.LogInfo($"[{nameof(ModComponent)}].{nameof(OnDestroy)}()");
    }
    
    private void FixedUpdate()
    {
        try
        {
            if (_isDisabled)
                return;
        }
        catch (Exception ex)
        {
            _isDisabled = true;
            Log.LogError($"[{nameof(ModComponent)}].{nameof(FixedUpdate)}(): {ex}");
        }
    }
    
    private void Update()
    {
        try
        {
            if (_isDisabled)
                return;
        }
        catch (Exception ex)
        {
            _isDisabled = true;
            Log.LogError($"[{nameof(ModComponent)}].{nameof(Update)}(): {ex}");
        }
    }
    
    private void LateUpdate()
    {
        try
        {
            if (_isDisabled)
                return;
    
            SpeedControl.TryUpdate();
        }
        catch (Exception ex)
        {
            _isDisabled = true;
            Log.LogError($"[{nameof(ModComponent)}].{nameof(LateUpdate)}(): {ex}");
        }
    }
}