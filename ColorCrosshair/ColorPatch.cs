using ColorCrosshair.API;
using HarmonyLib;
using UnityEngine;

namespace ColorCrosshair
{
    internal static class ColorPatch
    {
        public static void Init()
        {
            ColorCrosshairAPI.OnReload += RefreshDefaultColors;
        }

        public static void RefreshDefaultColors()
        {
            CircleCrosshair cc = GuiManager.CrosshairLayer.m_circleCrosshair;
            cc.SetColor(cc.m_crosshairColOrg = Configuration.defaultColor);
            cc.SetChargeUpColor(cc.m_chargeUpColOrg = Configuration.chargeColor);
        }

        [HarmonyPatch(typeof(CrosshairGuiLayer), nameof(CrosshairGuiLayer.Setup))]
        [HarmonyWrapSafe]
        [HarmonyPostfix]
        private static void SetDefaultColors(CrosshairGuiLayer __instance, Transform root, string name)
        {
            CircleCrosshair cc = __instance.m_circleCrosshair;
            cc.SetColor(cc.m_crosshairColOrg = Configuration.defaultColor);
            cc.SetChargeUpColor(cc.m_chargeUpColOrg = Configuration.chargeColor);
        }

        [HarmonyPatch(typeof(CrosshairGuiLayer), nameof(CrosshairGuiLayer.TriggerChargeUpBlink))]
        [HarmonyWrapSafe]
        [HarmonyPrefix]
        private static void SetChargeNotificationColors(CrosshairGuiLayer __instance, ref Color blinkColor)
        {
            if (blinkColor.Equals(Color.white))
                blinkColor = Configuration.chargeBlinkColor;
            else
                blinkColor = Configuration.chargeWarningColor;
        }

        [HarmonyPatch(typeof(CrosshairGuiLayer), nameof(CrosshairGuiLayer.TriggerBlink))]
        [HarmonyWrapSafe]
        [HarmonyPrefix]
        private static void SetEnemyInRangeColor(CrosshairGuiLayer __instance, ref Color blinkColor)
        {
            blinkColor = Configuration.enemyBlinkColor;
        }
    }
}