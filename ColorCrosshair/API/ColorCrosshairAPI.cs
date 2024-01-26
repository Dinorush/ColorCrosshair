using BepInEx;
using GTFO.API.Utilities;
using System;
using UnityEngine;

namespace ColorCrosshair.API
{
    public static class ColorCrosshairAPI
    {
        public static event Action? OnReload;
        public static Color DefaultColor { get { return Configuration.defaultColor; } }
        public static Color ChargeColor { get { return Configuration.chargeColor; } }
        public static Color ChargeBlinkColor { get { return Configuration.chargeBlinkColor; } }
        public static Color ChargeWarningColor { get { return Configuration.chargeWarningColor; } }
        public static Color EnemyBlinkColor { get { return Configuration.enemyBlinkColor; } }

        internal static void Init()
        {
            LiveEditListener listener = LiveEdit.CreateListener(Paths.ConfigPath, Loader.MODNAME + ".cfg", false);
            listener.FileChanged += RunOnFileChanged;
        }

        private static void RunOnFileChanged(LiveEditEventArgs _)
        {
            OnReload?.Invoke();
        }
    }
}
