using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static int curSpawnPoint = 0;
    public static int portalPosition5F = 0;
    public static bool playDoorCloseSoundAtNextScene = false;

    private static bool _glassBroke = false;
    public static bool GlassBroke
    {
        get
        {
            if (!_glassBroke && SaveSystem.Data != null)
            {
                _glassBroke = SaveSystem.Data.glassBroke;
            }
            return _glassBroke;
        }
        set
        {
            _glassBroke = value;
        }
    }
}
