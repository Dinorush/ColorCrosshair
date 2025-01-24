using System.IO;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using ColorCrosshair.API;

namespace ColorCrosshair
{
    internal static class Configuration
    {
        public static Color defaultColor = new(.65f, .65f, .65f, 1f);
        public static Color chargeColor = new(.85f, .85f, .85f, .7f);
        public static Color chargeBlinkColor = Color.white;
        public static Color chargeWarningColor = ColorExt.Red(0.6f);
        public static Color enemyBlinkColor = Color.white;

        public static Color hitmarkerColor = new(1f, 0f, 0f, .408f);
        public static Color critHitmarkerColor = new(1f, 0.212f, 0f, .314f);
        public static Color killHitmarkerColor = new(1f, .678f, 0.051f, 1f);
        public static Color armorHitmarkerColor = new(1f, 1f, 1f, .22f);

        private static ConfigFile configFile;

        internal static void Init()
        {
            configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, Loader.MODNAME + ".cfg"), saveOnInit: true);
            BindAll(configFile);
            ColorCrosshairAPI.OnReload += OnFileChanged;
        }

        private static void OnFileChanged()
        {
            string section = "General Settings";
            configFile.Reload();
            SetField(ref defaultColor, (string)configFile[section, "Default Color"].BoxedValue);
            SetField(ref chargeColor, (string)configFile[section, "Chargeup Color"].BoxedValue);
            SetField(ref chargeBlinkColor, (string)configFile[section, "Charge Finished Color"].BoxedValue);
            SetField(ref chargeWarningColor, (string)configFile[section, "Charge Warning Color"].BoxedValue);
            SetField(ref enemyBlinkColor, (string)configFile[section, "Enemy In Range Color"].BoxedValue);

            section = "Hitmarker Settings";
            SetField(ref hitmarkerColor, (string)configFile[section, "Hitmarker Color"].BoxedValue);
            SetField(ref critHitmarkerColor, (string)configFile[section, "Crit Hitmarker Color"].BoxedValue);
            SetField(ref killHitmarkerColor, (string)configFile[section, "Kill Hitmarker Color"].BoxedValue);
            SetField(ref armorHitmarkerColor, (string)configFile[section, "Armor Hitmarker Color"].BoxedValue);
        }

        private static void SetField(ref Color field, string val)
        {
            if (!val.StartsWith('#'))
                val = '#' + val;

            if (ColorUtility.TryParseHtmlString(val, out var color))
                field = color;
        }

        private static void BindAll(ConfigFile config)
        {
            string section = "General Settings";
            BindField(ref defaultColor, config, section, "Default Color", "The default color of the reticle.\nVanilla default: #7d7d7d99");
            BindField(ref chargeColor, config, section, "Chargeup Color", "The color for melee weapon charging progress.\nVanilla default: #BEBEBEAD");
            BindField(ref chargeBlinkColor, config, section, "Charge Finished Color", "The color that briefly blinks when melee weapon charging finishes.\nVanilla default: #FFFFFFFF");
            BindField(ref chargeWarningColor, config, section, "Charge Warning Color", "The color that briefly blinks when the melee weapon charge is about to expire.\nVanilla default: #FF000099");
            BindField(ref enemyBlinkColor, config, section, "Enemy In Range Color", "The color that briefly blinks when looking at an enemy in melee range while holding a melee weapon.\nVanilla default: #FFFFFFFF");

            section = "Hitmarker Settings";
            BindField(ref hitmarkerColor, config, section, "Hitmarker Color", "The color for normal hitmarkers.\nVanilla default: #FF000068");
            BindField(ref critHitmarkerColor, config, section, "Crit Hitmarker Color", "The color for weakspot or back damage hitmarkers.\nVanilla default: #FF360050");
            BindField(ref killHitmarkerColor, config, section, "Kill Hitmarker Color", "The color for kill hitmarkers.\nVanilla default: #FFAD0DFF");
            BindField(ref armorHitmarkerColor, config, section, "Armor Hitmarker Color", "The color for armor damage hitmarkers.\nVanilla default: #FFFFFF46");
        }

        private static void BindField(ref Color field, ConfigFile config, string section, string key, string description)
        {
            string val = config.Bind(section, key, ColorUtility.ToHtmlStringRGBA(field), description).Value;
            SetField(ref field, val);
        }
    }
}
