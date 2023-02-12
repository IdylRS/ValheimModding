using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
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
            PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;
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
        private static string url = "http://192.168.0.197/resist";
        
        static void Prefix(Character __instance, ref HitData hit)
        {
            if (__instance.Equals(Player.m_localPlayer))
            {
                float damage = hit.GetTotalDamage();
                double val = Math.Min(damage*10, 1500);

                if(Player.m_localPlayer.GetHealth() - damage <= 0f)
                {
                    val = 1500;
                }

                SendShock(val);
            }
        }

        static async void SendShock(double val)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync(url + "?val=" + val);
            }
        }
    }
}