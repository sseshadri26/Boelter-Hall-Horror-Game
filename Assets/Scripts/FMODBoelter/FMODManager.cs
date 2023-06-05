using FMOD;
using FMOD.Studio;
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
    private FMODParams bgmParams = new FMODParams(true);
    public int mainBGMID = -1;
    //
    //[SerializeField]
    //[Range(0f, 2f)]
    //    float volumePercent = 1f;
    //[SerializeField]
    //[Range(0f, 2f)]
    //    float pitch = 1f;
    //[SerializeField]
    //[Range(0f, 2f)]
    //    private float intensity = 1f;
    //[SerializeField]
    //[Range(0f, 2f)]
    //    private float speed = 1f;
    //[SerializeField]
    //[Range(0f, 2f)]
    //    private float reverb = 1f;

    // Structs:
    public enum SFX
    {
        door_close, door_open, 
        footstep_ground, footstep_ground2, footstep_grass, footstep_gravel, footstep_wood,
        paper_crumble,
        music_title, music_opening, music_hallway,
        menu_scroll, menu_click,
        spooky_1, spooky_2, spooky_3
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
        { SFX.footstep_ground2, "event:/footstep_ground 2" },
        { SFX.music_title, "event:/music_title" },
        { SFX.music_opening, "event:/music_opening" },
        { SFX.music_hallway, "event:/music_hallway" },
        { SFX.menu_scroll, "event:/menu_scroll" },
        { SFX.menu_click, "event:/menu_click" },
        { SFX.spooky_1, "event:/spooky_1" },
        { SFX.spooky_2, "event:/spooky_2" },
        { SFX.spooky_3, "event:/spooky_3" },
    };

    // BGM Storage:
    private Dictionary<int, FMODParams> BGMStorageDict = new Dictionary<int, FMODParams>();
    private int currentID = -1;

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
        EventInstance eventInstance = SetupEmitterEvent(sound);
        if (useDefault)
        {
            ModifyEmitterEvent(ref eventInstance, ref globalParams, position);
        }
        else
        {
            ModifyEmitterEvent(ref eventInstance, ref soundParams, position);
        }
        // Play and release:
        PrintDebug(sound + " played.");
        eventInstance.start();
        eventInstance.release();
    }
    public int StartBGM(SFX sound, bool useDefault = true, FMODParams soundParams = default(FMODParams))
    {
        currentID++;
        if (useDefault)
        {
            BGMStorageDict.Add(currentID, globalParams);
            StartCoroutine(BGMInstance(sound, currentID, true));
        }
        else
        {
            BGMStorageDict.Add(currentID, soundParams);
            StartCoroutine(BGMInstance(sound, currentID, false));
        }
        return currentID;
    }
    public void ModifyParams(int ID, ref FMODParams soundParams)
    {
        BGMStorageDict[ID] = soundParams;
    }
    public void ChangeMainBGM(SFX sound, float volumePercent = 1f)
    {
        if (mainBGMID == -1)
        {
            bgmParams.volumePercent = volumePercent;
            mainBGMID = StartBGM(sound, false, bgmParams);
            return;
        }
        StopBGM(mainBGMID);
        bgmParams.volumePercent = volumePercent;
        mainBGMID = StartBGM(sound, false, bgmParams);
    }
    
    // Utils:
    private EventInstance SetupEmitterEvent(SFX sound)
    {
        // Checks:
        if (!soundToEventDict.ContainsKey(sound))
        {
            PrintDebug(sound + " not found in dictionary.");
            //return;
        }
        // Grab path:
        string eventPath = soundToEventDict[sound];
        // Create instance of event:
        FMOD.Studio.EventInstance eventInstance = RuntimeManager.CreateInstance(eventPath);
        return eventInstance;
    }
    private void ModifyEmitterEvent(ref EventInstance eventInstance, ref FMODParams soundParams, Vector3 position)
    {
        // Modify position:
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        // Set pitch and volume:
        eventInstance.setVolume(soundParams.volumePercent);
        eventInstance.setPitch(soundParams.pitch);
        // Set Parameters:
        eventInstance.setParameterByName("intensity", soundParams.intensity);
        eventInstance.setParameterByName("speed", soundParams.speed);
        eventInstance.setParameterByName("reverb", soundParams.reverb);
    }
    private IEnumerator BGMInstance(SFX sound, int instanceID, bool useDefault)
    {
        // Vars:
        WaitForSeconds delay = new WaitForSeconds(0.2f);
        // Setup:
        FMODParams myParams;
        if (useDefault)
        {
            myParams = globalParams;
        }
        else
        {
            myParams = BGMStorageDict[instanceID];
        }
        EventInstance eventInstance = SetupEmitterEvent(sound);
        ModifyEmitterEvent(ref eventInstance, ref myParams, transform.position);
        eventInstance.start();
        // Loops:
        while (BGMStorageDict.ContainsKey(instanceID))
        {
            if (useDefault)
            {
                myParams = globalParams;
            }
            else
            {
                myParams = BGMStorageDict[instanceID];
                UnityEngine.Debug.Log(myParams.pitch);
            }
            ModifyEmitterEvent(ref eventInstance, ref myParams, transform.position);
            yield return delay;
        }
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    private void StopBGM(int instanceID)
    {
        BGMStorageDict.Remove(instanceID);
    }
    private void PrintDebug(string message)
    {
        if (printDebug)
        {
            UnityEngine.Debug.Log("FMODManager: " + message);
        }
    }
}
