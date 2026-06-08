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

    public class OSS3 : AModCard
    {
        #region Basic properties
        public override string DisplayName => "AN/SEQ-3";

        public override string Description =>
            "Strike all tiles in your adjacent columns.\n<b><color=#FFBF00>Unrepeatable</color></b></nobr>: Dismantles itself when played.";
        public override Il2CppCollections.HashSet<CardTrait> Traits => new System.Collections.Generic.HashSet<CardTrait>()
        {
            CardTrait.Attack
        }.ToILCPP();

        public override ClassName Class => ClassName.Gunner;

        public override PilotName PilotUnique => PilotName.Zeke;

        public override Rarity Rarity => Rarity.Created;

        public override bool IsToken => true;

        public override Pile Destination => Pile.Hand;

        public override int ClassBaseCost => 0;
        #endregion

        #region Components and traits

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new System.Collections.Generic.HashSet<MoreInfoWordName>()
        {
            ModContentManager.GetModMoreInfoName("Unrepeatable"),
            MoreInfoWordName.Deconstruct
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
            ModContentManager.GetModComponentName<OSS3Component>()

        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new System.Collections.Generic.HashSet<ComponentName>()
        {
        }.ToILCPP();

        public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => new System.Collections.Generic.HashSet<CardTrait>().ToILCPP();
        #endregion

        #region Tasks

        public override Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue cardID)
        {
            List<Il2CppSystem.ValueTuple<Trigger, ACondition>> triggerConditions = new()
            {
                new (Trigger.PostTask, new AndCondition(
                    new IsTypeCondition<PlayCardEndTask>(new RunningTaskValue()),
                    new EqualsCondition(new CurrentCardIDValue(), cardID)
                ))
            };

            List<ATask> triggerTasks = new()
            {
                new DeconstructTask(cardID)
            };

            return new List<TriggerEffect>()
            {
                new TriggerEffect(triggerConditions.ToILCPP(), triggerTasks.ToILCPP())

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

            taskList.Add(new OSS3_1());

            return taskList;
        }
        #endregion
    }

    public class OSS3Component : AModComponent
    {
        public override string DisplayName => "Innovation";

        public override string Description => "You laser extends 1 tile further in both directions. A random <nobr><b><i><color=#fd554e>Attack</color></i></b></nobr> in your hand gets +2 <nobr><b><color=#FFBF00>Repeat</color></b></nobr> when played.";

        public override ClassName Class => ClassName.Gunner;

        public override void ModifyCardModel(CardModel cardModel)
        {
            foreach (SelectionTaskGroup taskGroup in cardModel.SelectionTaskGroups)
            {
                taskGroup.PostSelectionTaskList.Clear();
            }

            cardModel.HiddenTraits.Add(CardTrait.Random);

            cardModel.Traits.Add(CardTrait.Tactic);

        }

        public override Il2CppCollections.List<ATask> GetPostSelectionTaskList(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<ATask> taskList = new();

            taskList.Add(new OSS3_2());
            taskList.Add(new OSS3_2_Misc());


            return taskList;
        }
    }
}