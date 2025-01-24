using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using ColorCrosshair.API;
using HarmonyLib;
using System.Diagnostics;

namespace ColorCrosshair
{
    [BepInPlugin("Dinorush." + MODNAME, MODNAME, "1.1.0")]
    internal sealed class Loader : BasePlugin
    {
        public const string MODNAME = "ColorCrosshair";

#if DEBUG
        private static ManualLogSource Logger;
#endif

        [Conditional("DEBUG")]
        public static void DebugLog(object data)
        {
#if DEBUG
            Logger.LogMessage(data);
#endif
        }

        public override void Load()
        {
#if DEBUG
            Logger = Log;
#endif
            Log.LogMessage("Loading " + MODNAME);
            Configuration.Init();
            ColorPatch.Init();
            ColorCrosshairAPI.Init();

            new Harmony(MODNAME).PatchAll(typeof(ColorPatch));

            Log.LogMessage("Loaded " + MODNAME);
        }
    }
}