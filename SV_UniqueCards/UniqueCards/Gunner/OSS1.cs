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

    public class OSS1 : AModCard
    {
        #region Basic properties
        public override string DisplayName => "Sherman's E4-5";

        public override string Description =>
            "Strike tiles in a large cone above you.\nDismantles itself when played";
        public override Il2CppCollections.HashSet<CardTrait> Traits => new System.Collections.Generic.HashSet<CardTrait>()
        {
            CardTrait.Attack
        }.ToILCPP();

        public override ClassName Class => ClassName.Gunner;

        public override PilotName PilotUnique => PilotName.Zeke;

        public override Rarity Rarity => Rarity.Created;

        public override bool IsToken => true;
        #endregion

        #region Components and traits
        public override int ClassBaseCost => 0;

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new System.Collections.Generic.HashSet<MoreInfoWordName>()
        {

            MoreInfoWordName.FlamethrowerImage,
            MoreInfoWordName.Deconstruct,
            MoreInfoWordName.Purge,
        }.ToILCPP();

        public override Il2CppCollections.HashSet<CardName> MoreInfoCards => new System.Collections.Generic.HashSet<CardName>()
        {
            CardName.Sparks,
            CardName.Propel,
            CardName.Vapor
        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new System.Collections.Generic.HashSet<ComponentTrait>()
        {
        }.ToILCPP();


        public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new System.Collections.Generic.HashSet<ComponentName>()
        {
            ModContentManager.GetModComponentName<OSS1Component>()

        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new System.Collections.Generic.HashSet<ComponentName>()
        {
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

            taskList.Add(new OSS1_1VFX());
            taskList.Add(new OSS1_1());
            taskList.Add(new DeconstructTask(cardID));


            return taskList;
        }
        #endregion
    }

    public class OSS1Component : AModComponent
    {
        public override string DisplayName => "Innovation";

        public override string Description => "Strike twice. Make 2 random <nobr><b><i><color=#51affb>Move</color></i></b></nobr> and 2 random <nobr><b><i><color=#5cdd3a>Junk</color></i></b></nobr> cards in your hand free this turn.";

        public override ClassName Class => ClassName.Gunner;

        public override void ModifyCardModel(CardModel cardModel)
        {
            foreach (SelectionTaskGroup taskGroup in cardModel.SelectionTaskGroups)
            {
                taskGroup.PostSelectionTaskList.Clear();
            }

            cardModel.HiddenTraits = new System.Collections.Generic.HashSet<CardTrait>(){CardTrait.Random}.ToILCPP();
        }

        public override Il2CppCollections.List<ATask> GetPostSelectionTaskList(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<ATask> taskList = new();

            taskList.Add(new OSS1_1VFX());
            taskList.Add(new OSS1_2VFX()); 
            taskList.Add(new OSS1_2());
            taskList.Add(new OSS1_2_Misc());
            taskList.Add(new DeconstructTask(cardID));


            return taskList;
        }
    }

}