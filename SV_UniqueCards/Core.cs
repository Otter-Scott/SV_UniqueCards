using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppLanguage.Lua;
using Il2CppStarVaders;
using Il2CppTMPro;
using MelonLoader;
using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.ReloadAttribute;
using Il2CppCollections = Il2CppSystem.Collections.Generic;

[assembly: MelonInfo(typeof(SV_UniqueCards.Core), "SV_UniqueCards", "0.3.0", "Otter", null)]
[assembly: MelonGame("Pengonauts", "StarVaders")]

namespace SV_UniqueCards
{
    public class Core : SVMod
    {

        protected override void EarlyRegisterMod()
        {
            base.EarlyRegisterMod();
            RegisterMoreInfoPanel("Destroy", "<b><color=#FFBF00>Destroy</color></b></nobr>: Temporarily remove card for the remainder of the battle. On purge effects do not trigger when a card is destroyed.");
        }
        protected override void LateRegisterMod()
        {
            base.LateRegisterMod();

            /*PlayerCardData Card1 = new(ModContentManager.GetModCardName<NorthWinds>())
            {
                Component = ModContentManager.GetModComponentName<HailComponent>()
            };
            PlayerCardData Card2 = new(ModContentManager.GetModCardName<Ablation>())
            {
                Component = ModContentManager.GetModComponentName<CatalysedComponent>()
            };
            PlayerCardData Card3 = new(ModContentManager.GetModCardName<OSS2>())
            {
                Component = ModContentManager.GetModComponentName<OSS2Component>()
            };
            PlayerCardData Card4 = new PlayerCardData(CardName.Exhaust)
            {
                Component = ComponentName.Chilled
            };
            PlayerCardData Card5 = new PlayerCardData(CardName.Jam)
            {
                Component = ComponentName.Fiery
            };
            PlayerCardData Card6 = new PlayerCardData(CardName.Airlift)
            {
                Component = ComponentName.Fiery
            };

            var RoxyDeck = new List<PlayerCardData>
            {
                Card1,
                Card2
            };

            RegisterContentMod(new PilotModification(PilotName.Roxy)
            {
                targetPilot = PilotName.Roxy,
                startingCards = RoxyDeck.ToILCPP()
            });*/


            // https://docs.unity3d.com/ScriptReference/ImageConversion.LoadImage.html
            // https://docs.unity3d.com/ScriptReference/Texture2D.html
            // https://docs.unity3d.com/ScriptReference/Sprite.Create.html
        }

    }
}