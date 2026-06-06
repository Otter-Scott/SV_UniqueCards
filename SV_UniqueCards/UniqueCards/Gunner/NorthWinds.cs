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
    public class NorthWinds : AModCard
    {
        #region Basic properties
        public override string DisplayName => "North Winds";

        public override string Description =>
            "Pull all invaders down 1 tile. Make all cards in your hand <nobr><b><i><color=#916fd7>Frozen</color></i></b></nobr>, and immediately end your turn.\nUnused <nobr><sprite=\"TextIcons\" name=\"Heat\"> <b><color=#FFBF00>Heat</color></b></nobr> sink is added to your <nobr><sprite=\"TextIcons\" name=\"Heat\"> <b><color=#FFBF00>Heat</color></b></nobr> sink next turn.";
        public override Il2CppCollections.HashSet<CardTrait> Traits => new System.Collections.Generic.HashSet<CardTrait>()
        {
            CardTrait.Tactic
        }.ToILCPP();

        public override ClassName Class => ClassName.Gunner;

        public override PilotName PilotUnique => PilotName.Noel;

        public override Rarity Rarity => Rarity.Legendary;
        #endregion

        #region Components and traits
        public override int ClassBaseCost => 0;

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new System.Collections.Generic.HashSet<MoreInfoWordName>()
        {
            MoreInfoWordName.Pull,
            MoreInfoWordName.Frozen,
            MoreInfoWordName.Heat
,
        }.ToILCPP();

        public override Il2CppCollections.HashSet<CardName> MoreInfoCards => new System.Collections.Generic.HashSet<CardName>() {  }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new System.Collections.Generic.HashSet<ComponentTrait>()
        {
            ComponentTrait.Basic,
            ComponentTrait.SelectionLess
        }.ToILCPP();


        public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new System.Collections.Generic.HashSet<ComponentName>()
        {
            ModContentManager.GetModComponentName<HailComponent>()
        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new System.Collections.Generic.HashSet<ComponentName>()
        {
            ComponentName.Chilled,
            ComponentName.Broken,
            ComponentName.Echo,
            ComponentName.Risky,
            ComponentName.Boosted
        }.ToILCPP();

        public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => new System.Collections.Generic.HashSet<CardTrait>().ToILCPP();

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

            taskList.Add(new NorthWinds_1());
            taskList.Add(new NorthWindsMisc());
            taskList.Add(new NorthWindsFreeze());

            return taskList;
        }
        #endregion
    }

    public class HailComponent : AModComponent
    {
        public override string DisplayName => "Hail";

        public override string Description => "Invaders are pushed instead.";

        public override ClassName Class => ClassName.Gunner;


        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new System.Collections.Generic.HashSet<MoreInfoWordName>()
        {
                MoreInfoWordName.Push,
                MoreInfoWordName.Frozen
        }.ToILCPP();

        public override void ModifyCardModel(CardModel cardModel)
        {
            cardModel.MoreInfoWordNames.Clear();
            cardModel.MoreInfoWordNames.Add(MoreInfoWordName.Push);
            cardModel.MoreInfoWordNames.Add(MoreInfoWordName.Frozen);
            cardModel.MoreInfoWordNames.Add(MoreInfoWordName.Heat);

            foreach (SelectionTaskGroup taskGroup in cardModel.SelectionTaskGroups)
            {
                taskGroup.PostSelectionTaskList.Clear();
                taskGroup.PostSelectionTaskList.Add(new NorthWinds_2());
                taskGroup.PostSelectionTaskList.Add(new NorthWindsMisc());
                taskGroup.PostSelectionTaskList.Add(new NorthWindsFreeze());
            }

            cardModel.HeatCost = 1;
        }
    }
}