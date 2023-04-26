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
    public FMODParams globalParams = new FMODParams(true);
    //
    [SerializeField]
    [Range(0f, 2f)]
        float volumePercent = 1f;
    [SerializeField]
    [Range(0f, 2f)]
        float pitch = 1f;
    [SerializeField]
    [Range(0f, 2f)]
        private float intensity = 1f;
    [SerializeField]
    [Range(0f, 2f)]
        private float speed = 1f;
    [SerializeField]
    [Range(0f, 2f)]
        private float reverb = 1f;

    // Structs:
    public enum SFX
    {
        door_close, door_open, 
        footstep_ground, footstep_grass, footstep_gravel, footstep_wood,
        paper_crumble
    }
    public struct FMODParams
    {
        public float volumePercent;
        public float pitch;
        public float intensity;
        public float speed;
        public float reverb;
        //
        public FMODParams(bool defaultToggles = true)
        {
            volumePercent = 1f;
            pitch = 1f;
            //
            intensity = 1f;
            speed = 1f;
            reverb = 0;
        }
    }
    private Dictionary<SFX, string> soundToEventDict = new Dictionary<SFX, string>()
    {
        { SFX.door_close, "event:/door_close" },
        { SFX.door_open, "event:/door_open" },
        { SFX.footstep_ground, "event:/footstep_ground" },
        { SFX.footstep_grass, "event:/footstep_grass" },
        { SFX.footstep_gravel, "event:/footstep_gravel" },
        { SFX.footstep_wood, "event:/footstep_wood" },
        { SFX.paper_crumble, "event:/paper_crumble" },
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

    // Interface:
    public void PlaySound(SFX sound, bool useDefault = true, FMODParams soundParams = default(FMODParams))
    {
        // Plays specified sound @ player:
        PlaySound(sound, transform.position, useDefault, soundParams);
    }
    public void PlaySound(SFX sound, Vector3 position, bool useDefault = true, FMODParams soundParams = default(FMODParams))
    {
        // Plays specified sound @ location:
        if (useDefault)
        {
            PlayEmitterEvent(sound, position, ref globalParams);
        }
        else
        {
            PlayEmitterEvent(sound, position, ref soundParams);
        }
    }
    
    // Utils:
    private void PlayEmitterEvent(SFX sound, Vector3 position, ref FMODParams soundParams)
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
        eventInstance.setVolume(soundParams.volumePercent);
        eventInstance.setPitch(soundParams.pitch);
        //eventInstance.setVolume(1f);
        //eventInstance.setPitch(1f);
        UnityEngine.Debug.Log(soundParams.volumePercent);
        // Set Parameters:
        //eventInstance.setParameterByName("intensity", soundParams.intensity);
        //eventInstance.setParameterByName("speed", soundParams.speed);
        //eventInstance.setParameterByName("reverb", soundParams.reverb);
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
