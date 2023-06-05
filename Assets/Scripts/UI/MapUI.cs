using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class MapUI : MonoBehaviour, IDirectionControllable
{
    [SerializeField] UIDocument document;
    [SerializeField] string mapFilePrefix = "map_floor";
    // UI Tags
    const string k_map = "map";

    // UI References
    VisualElement m_map;

    // This data structure doesn't quite make sense since the current implementation destroys this
    // object then reinstantiates it every time a scene is loaded, but it's just the most readable
    Dictionary<int, Texture2D> floorNumToMapTexture = new Dictionary<int, Texture2D>();


    void Awake()
    {
        m_map = document.rootVisualElement.Q<VisualElement>(k_map);

        InitializeMapTextureDictionary();
        InitializeDisplayedMap();

        // NOTE: As of now, there's nothing we do with the map since
        // it's automatically assigned a graphic in the UXML, but perhaps
        // there's something we want to do to it later
    }

    private void InitializeDisplayedMap()
    {
        const string mapScenePattern = @"Assets/Scenes/([0-9]+)F/*";

        Regex mapSceneRegex = new Regex(mapScenePattern);
        string currentScenePath = SceneManager.GetActiveScene().path;
        Match mapScenePathMatcher = mapSceneRegex.Match(currentScenePath);
        if (mapScenePathMatcher.Success)
        {
            int mapFloorNum = int.Parse(mapScenePathMatcher.Groups[1].Value);
            Debug.Log($"Current scene \"{currentScenePath}\" is on floor {mapFloorNum}");

            m_map.style.backgroundImage = new StyleBackground(floorNumToMapTexture[mapFloorNum]);
        }
        else
            Debug.Log($"Didn't recognize scene with path \"{currentScenePath}\" ");
    }

    private void InitializeMapTextureDictionary()
    {
        const string mapTexturePattern = @"map_floor([0-9]+)";

        Regex mapTexturePatternRegex = new Regex(mapTexturePattern);
        Texture2D[] maps = Resources.LoadAll<Texture2D>("Maps");
        foreach (var map in maps)
        {
            Match mapTextureMatcher = mapTexturePatternRegex.Match(map.name);
            if (mapTextureMatcher.Success)
            {
                Debug.Log($"Found a map \"{mapTextureMatcher.Groups[0]}\" for floor {mapTextureMatcher.Groups[1].Value}");

                // No error checking because we don't have time
                int mapFloorNum = int.Parse(mapTextureMatcher.Groups[1].Value);

                if (floorNumToMapTexture.ContainsKey(mapFloorNum))
                    Debug.Log($"Duplicated map for floor {mapFloorNum}, not adding");
                else
                    floorNumToMapTexture[mapFloorNum] = map;
            }
            else
                Debug.Log($"Didn't recognize invalid map texture with name \"{map.name}\"...");
        }
    }

    public void MoveUp()
    {
        // Don't do anything for now
    }

    public void MoveDown()
    {
        // Don't do anything for now
    }

    public void MoveLeft()
    {
        // Don't do anything for now
    }

    public void MoveRight()
    {
        // Don't do anything for now
    }

    public void Submit()
    {
        // Don't do anything for now
    }
}
