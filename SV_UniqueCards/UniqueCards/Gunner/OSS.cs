using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppLanguage.Lua;
using Il2CppStarVaders;
using Il2CppTMPro;
using MelonLoader;
using MelonLoader.Utils;
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

namespace SV_UniqueCards
{
    public class OSS : AModCard
    {
        #region Basic properties
        public override string DisplayName => "One Small Step";

        public override string Description =>
            "Destroy this card and two random non-<nobr><b><i><color=#5cdd3a>Junk</color></i></b></nobr> cards.\n Create one of three random 0 cost cards in your hand. ( <font=\"StarvadersGun-Regular SDF\"><size=150%><voffset=-0.11em>Sherman's E4-5</i></font></b></smallcaps></color></size></voffset>, <font=\"StarvadersGun-Regular SDF\"><size=150%><voffset=-0.11em>A-10 Warthogs</i></font></b></smallcaps></color></size></voffset>, or <font=\"StarvadersGun-Regular SDF\"><size=150%><voffset=-0.11em>AN/SEQ-3</i></font></b></smallcaps></color></size></voffset> )";
        public override Il2CppCollections.HashSet<CardTrait> Traits => new System.Collections.Generic.HashSet<CardTrait>()
        {
            CardTrait.Tactic
        }.ToILCPP();

        public override ClassName Class => ClassName.Gunner;

        public override PilotName PilotUnique => PilotName.Zeke;

        public override Rarity Rarity => Rarity.Legendary;

        public override bool IsPurged => true;
        #endregion

        #region Components and traits
        public override int ClassBaseCost => 2;

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new System.Collections.Generic.HashSet<MoreInfoWordName>()
        {
            ModContentManager.GetModMoreInfoName("Destroy")
        }.ToILCPP();

        public override Il2CppCollections.HashSet<CardName> MoreInfoCards => new System.Collections.Generic.HashSet<CardName>() 
        {
            ModContentManager.GetModCardName<OSS1>(),
            ModContentManager.GetModCardName<OSS2>(),
            ModContentManager.GetModCardName<OSS3>()
        }.ToILCPP();



        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new System.Collections.Generic.HashSet<ComponentTrait>()
        {
        }.ToILCPP();


        public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new System.Collections.Generic.HashSet<ComponentName>()
        {
            ComponentName.SwiftPlus,
            ComponentName.TacticalPlus,
            ModContentManager.GetModComponentName<OSSComponent>()

        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new System.Collections.Generic.HashSet<ComponentName>()
        {
        }.ToILCPP();

        public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => new System.Collections.Generic.HashSet<CardTrait>()
        {
            CardTrait.Random
        }.ToILCPP();
        #endregion

        #region Tasks

        

        public override Il2CppCollections.List<Selection> GetSelections(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<Selection> selections = new();

            selections.Add(new Selection(
                new DefaultSelectionCondition(),
                selectionDescriptor: SelectionDescriptor.None
            ));

            return selections;
        }

        public override Il2CppCollections.List<ATask> GetPostSelectionTaskList(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<ATask> taskList = new();

            taskList.Add(new OSS_1(cardID));

            return taskList;
        }
        #endregion
    }

    public class OSSComponent : AModComponent
    {
        public override string DisplayName => "One Big Leap";

        public override string Description => "Choose which cards to destroy. <nobr><b><i><color=#5cdd3a>Junk</color></i></b></nobr> cards can also be destroyed";

        public override ClassName Class => ClassName.Gunner;

        public override void ModifyCardModel(CardModel cardModel)
        {

            foreach (SelectionTaskGroup taskGroup in cardModel.SelectionTaskGroups)
            {
                taskGroup.PostSelectionTaskList.Clear();
                taskGroup.PostSelectionTaskList.Add(new OSS_2());
            }

            cardModel.PlayCondition = new AndCondition(
                cardModel.PlayCondition,
                new IntGreaterThanCondition(
                    new NumberOfCardsInHandNotBeingPlayedValue(),
                    new Il2CppSystem.Int32 { m_value = 2 }.BoxIl2CppObject()
            ));
        }
    }
}