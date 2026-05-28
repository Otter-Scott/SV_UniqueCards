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
    public class Fusion : AModCard
    {
        #region Basic properties
        public override string DisplayName => "Fusion";

        public override string Description =>
            "Discard a card and add a <font=\"StarvadersGun-Regular SDF\"><size=150%><voffset=-0.11em>Fusion</i></font></b></smallcaps></color></size></voffset> to your discard. Every Fusion discarded triggers <font=\"StarvadersGun-Regular SDF\"><size=150%><voffset=-0.11em>Quark</i></font></b></smallcaps></color></size></voffset>'s effect and adds another Fusion.\nStrike all tiles adjacent to a tile.";
        public override Il2CppCollections.HashSet<CardTrait> Traits => new System.Collections.Generic.HashSet<CardTrait>()
        {
            CardTrait.Attack,
            CardTrait.Tactic
        }.ToILCPP();

        public override ClassName Class => ClassName.Gunner;

        public override PilotName PilotUnique => PilotName.Min;

        public override Rarity Rarity => Rarity.Legendary;

        public override int ClassBaseCost => 1;

        #endregion

        #region Components and traits

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new System.Collections.Generic.HashSet<MoreInfoWordName>()
        {
            MoreInfoWordName.Burnt
,
        }.ToILCPP();

        public override Il2CppCollections.HashSet<CardName> MoreInfoCards => new System.Collections.Generic.HashSet<CardName>() 
        { 
            CardName.Quark
        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new System.Collections.Generic.HashSet<ComponentTrait>()
        {
            ComponentTrait.Basic,
        }.ToILCPP();


        public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new System.Collections.Generic.HashSet<ComponentName>()
        {
            ModContentManager.GetModComponentName<TriggerComponent>()

        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new System.Collections.Generic.HashSet<ComponentName>()
        {
            ComponentName.Fiery
        }.ToILCPP();

        public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => new System.Collections.Generic.HashSet<CardTrait>().ToILCPP();
        #endregion

        #region Tasks

        public override Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue cardID)
        {
            return new List<TriggerEffect>()
            {

            }.ToILCPP();
        }

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

            taskList.Add(new FusionMain());

            return taskList;
        }
        #endregion
    }

    public class TriggerComponent : AModComponent
    {
        public override string DisplayName => "Trigger";

        public override string Description => "Discard an additional card. Stays in your hand when played.";

        public override ClassName Class => ClassName.Gunner;

        public override void ModifyCardModel(CardModel cardModel)
        {
            foreach (SelectionTaskGroup taskGroup in cardModel.SelectionTaskGroups)
            {
                taskGroup.PostSelectionTaskList.Clear();
                taskGroup.PostSelectionTaskList.Add(new FusionMainComp());
            }

            cardModel.Destination = Pile.Hand;
        }
    }
}