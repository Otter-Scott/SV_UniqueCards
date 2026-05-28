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
using static UnityEngine.Rendering.ReloadAttribute;
using Il2CppCollections = Il2CppSystem.Collections.Generic;

[assembly: MelonInfo(typeof(SV_UniqueCards.Core), "SV_UniqueCards", "0.3.0", "Otter", null)]
[assembly: MelonGame("Pengonauts", "StarVaders")]

namespace SV_UniqueCards
{
    public class Core : SVMod
    {

        protected override void LateRegisterMod()
        {

            /*PlayerCardData ablationCatalysed = new(ModContentManager.GetModCardName<Fusion>())
            {
                Component = ModContentManager.GetModComponentName<TriggerComponent>()
            };

            var RoxyDeckManifest = new List<PlayerCardData>
            {
                new PlayerCardData(CardName.ArtilleryStrike),
                new PlayerCardData(CardName.ArtilleryStrike),
                new PlayerCardData(CardName.ArtilleryStrike),
                ablationCatalysed,
                ablationCatalysed,
                ablationCatalysed,
                new (ModContentManager.GetModCardName<Ablation>()),
                new (ModContentManager.GetModCardName<Ablation>()),
                new (ModContentManager.GetModCardName<NorthWinds>()),
            };
            RegisterContentMod(new PilotModification(PilotName.Roxy)
            {
                targetPilot = PilotName.Noel,
                startingCards = RoxyDeckManifest.ToILCPP()
            });*/

        }

    }
}