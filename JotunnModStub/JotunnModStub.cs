using BepInEx;
using BepInEx.Configuration;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
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

        private string url = "192.168.0.206/resist";
        private ButtonConfig ShockButton;

        private void Awake()
        {
            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("ModStub has landed");

            PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;
            AddInputs();

            Localization.AddTranslation("English", new Dictionary<string, string>
            {
                {"item_evilsword", "Sword of Cum"}, {"item_evilsword_desc", "Bringing the cum"},
                {"shock_message", "i hate you and ill cum on you"},
            });

            Localization.AddTranslation("English", new Dictionary<string, string>
            {
                {"item_evilsword", "Sword of Darkness"}, {"item_evilsword_desc", "Bringing the light"},
                {"evilsword_shwing", "Woooosh"}, {"evilsword_scroll", "*scroll*"},
                {"evilsword_effectname", "Evil"}, {"evilsword_effectstart", "You feel evil"},
                {"evilsword_effectstop", "You feel nice again"}
            });
        }

        private void Update()
        {
            if(ZInput.instance != null)
            {
                if (ZInput.GetButton(ShockButton.Name) && MessageHud.instance.m_msgQeue.Count == 0)
                {
                    MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "oh fuck ive cummed my pantaloons :(");
                }
            }
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

        private void AddInputs()
        {
            ShockButton = new ButtonConfig
            {
                Name = "JotunnExampleMod_RaiseSkill",
                Key = KeyCode.RightControl,
                ActiveInGUI = true,    // Enable this button in vanilla GUI (e.g. the console)
                ActiveInCustomGUI = true  // Enable this button in custom GUI
            };
            InputManager.Instance.AddButton(PluginGUID, ShockButton);
        }
    }
}