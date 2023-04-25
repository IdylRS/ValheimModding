using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace JotunnModStub
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class JotunnModStub : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jotunn.jotunnmodstub";
        public const string PluginName = "JotunnModStub";
        public const string PluginVersion = "0.0.1";
        
        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private Harmony harmony;

        private void Awake()
        {
            // PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;
            harmony = new Harmony(Info.Metadata.GUID);
            harmony.PatchAll();
        }

        private void Update()
        {
            
        }

        private void AddClonedItems()
        {
            // Create and add a custom item based on SwordBlackmetal
            ItemConfig evilSwordConfig = new ItemConfig();
            evilSwordConfig.Name = "$item_evilsword";
            evilSwordConfig.Description = "$item_evilsword_desc";
            evilSwordConfig.CraftingStation = "piece_workbench";
            evilSwordConfig.AddRequirement(new RequirementConfig("Stone", 1));
            evilSwordConfig.AddRequirement(new RequirementConfig("Wood", 1));

            CustomItem evilSword = new CustomItem("EvilSword", "SwordBlackmetal", evilSwordConfig);
            ItemManager.Instance.AddItem(evilSword);

            // You want that to run only once, Jotunn has the item cached for the game session
            PrefabManager.OnVanillaPrefabsAvailable -= AddClonedItems;
        }
    }

    [HarmonyPatch(typeof(Character), nameof(Character.ApplyDamage))]
    static class Damage_Patch
    {
        static void Prefix(Character __instance, ref HitData hit)
        {
            if (__instance.Equals(Player.m_localPlayer))
            {
                Logger.LogDebug("Took damage: " + hit.GetTotalDamage());
            }
        }
    }
}