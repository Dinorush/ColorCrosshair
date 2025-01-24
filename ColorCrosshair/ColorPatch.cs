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
            var layer = GuiManager.CrosshairLayer;
            CircleCrosshair cc = layer.m_circleCrosshair;
            cc.SetColor(cc.m_crosshairColOrg = Configuration.defaultColor);
            cc.SetChargeUpColor(cc.m_chargeUpColOrg = Configuration.chargeColor);

            float opacity = CellSettingsManager.SettingsData.HUD.Player_HitIndicatorOpacity.Value;
            foreach (var hit in layer.m_hitIndicators)
            {
                hit.m_hitColor = Configuration.hitmarkerColor;
                hit.UpdateColorsWithAlphaMul(opacity);
            }

            foreach (var crit in layer.m_weakspotHitIndicators)
            {
                crit.m_hitColor = Configuration.critHitmarkerColor;
                crit.m_deathColor = Configuration.killHitmarkerColor;
                crit.UpdateColorsWithAlphaMul(opacity);
            }

            foreach (var armor in layer.m_noDamageHitIndicators)
            {
                armor.m_hitColor = Configuration.armorHitmarkerColor;
                armor.UpdateColorsWithAlphaMul(opacity);
            }
        }

        [HarmonyPatch(typeof(CrosshairGuiLayer), nameof(CrosshairGuiLayer.Setup))]
        [HarmonyWrapSafe]
        [HarmonyPostfix]
        private static void SetDefaultColors(CrosshairGuiLayer __instance)
        {
            RefreshDefaultColors();
        }

        [HarmonyPatch(typeof(CrosshairGuiLayer), nameof(CrosshairGuiLayer.TriggerChargeUpBlink))]
        [HarmonyWrapSafe]
        [HarmonyPrefix]
        private static void SetChargeNotificationColors(ref Color blinkColor)
        {
            if (blinkColor.Equals(Color.white))
                blinkColor = Configuration.chargeBlinkColor;
            else
                blinkColor = Configuration.chargeWarningColor;
        }

        [HarmonyPatch(typeof(CrosshairGuiLayer), nameof(CrosshairGuiLayer.TriggerBlink))]
        [HarmonyWrapSafe]
        [HarmonyPrefix]
        private static void SetEnemyInRangeColor(ref Color blinkColor)
        {
            blinkColor = Configuration.enemyBlinkColor;
        }
    }
}