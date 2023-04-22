using FMOD;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class FMODManager : MonoBehaviour
{
    // Refs:
    public static FMODManager Instance;
    private StudioEventEmitter eventEmitter;

    // Vars:
    [SerializeField] private bool printDebug;
    public FMODParams globalParams = new FMODParams();

    // Members:
    public enum SFX
    {
        door_close, door_open, footstep_ground
    }
    public struct FMODParams
    {
        public int intensity;
        public int speed;

        public FMODParams(bool defaultToggles = true)
        {
            intensity = 1;
            speed = 1;
        }
    }
    private Dictionary<SFX, string> soundToEventDict = new Dictionary<SFX, string>()
    {
        { SFX.door_close, "event:/door_close" },
        { SFX.door_open, "event:/door_open" },
        { SFX.footstep_ground, "event:/footstep_ground" },
    };

    // Functions:
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        eventEmitter = GetComponent<StudioEventEmitter>();
    }

    // Sound calling:
    public void PlaySound(SFX sound, float volumePercent = 1f, float pitch = 1f, bool isStatic = false)
    {
        // Plays specified sound @ player:
        PlaySound(sound, transform.position, pitch, volumePercent, isStatic);
    }
    public void PlaySound(SFX sound, Vector3 position, float volumePercent, float pitch, bool isStatic)
    {
        // Plays specified sound @ location:
        PlayEmitterEvent(sound, position, volumePercent, pitch, isStatic);
    }
    // Utils:
    private void PlayEmitterEvent(SFX sound, Vector3 position, float volumePercent, float pitch, bool isStatic)
    {
        // Checks:
        if (!soundToEventDict.ContainsKey(sound))
        {
            PrintDebug(sound + " not found in dictionary.");
            return;
        }
        // Grab path:
        string eventPath = soundToEventDict[sound];
        // Create instance of event:
        FMOD.Studio.EventInstance eventInstance = RuntimeManager.CreateInstance(eventPath);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        // Set pitch and volume:
        eventInstance.setVolume(volumePercent);
        eventInstance.setPitch(pitch);
        // Ignore parameters (if isStatic):
        if (isStatic)
        {
            // ?
        }
        // Play and release:
        PrintDebug(sound + " played.");
        eventInstance.start();
        eventInstance.release();
    }
    private void PrintDebug(string message)
    {
        if (printDebug)
        {
            UnityEngine.Debug.Log("FMODManager: " + message);
        }
    }
}
