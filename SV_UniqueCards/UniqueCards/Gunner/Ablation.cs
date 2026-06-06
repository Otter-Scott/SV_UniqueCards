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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.ReloadAttribute;
using Il2CppCollections = Il2CppSystem.Collections.Generic;

namespace SV_UniqueCards
{
    public class Ablation : AModCard
    {
        #region Basic properties
        public override string DisplayName => "Ablation";

        public override string Description =>
            "Reset your <nobr><sprite=\"TextIcons\" name=\"Heat\"> <b><color=#FFBF00>Heat</color></b></nobr>. For each <nobr><sprite=\"TextIcons\" name=\"Heat\"> <b><color=#FFBF00>Heat</color></b></nobr> removed this way, add a <font=\"StarvadersGun-Regular SDF\"><size=150%><voffset=-0.11em>Meltdown</i></font></b></smallcaps></color></size></voffset> to your hand.\nIf unplayed, purge this card and ALL <nobr><b><i><color=#fd9756>Burnt</color></i></b></nobr> or <nobr><b><i><color=#5cdd3a>Junk</color></i></b></nobr> cards in your hand.";
        public override Il2CppCollections.HashSet<CardTrait> Traits => new System.Collections.Generic.HashSet<CardTrait>()
        {
            CardTrait.Tactic
        }.ToILCPP();

        public override ClassName Class => ClassName.Gunner;

        public override PilotName PilotUnique => PilotName.Roxy;

        public override Rarity Rarity => Rarity.Legendary;
        #endregion

        #region Components and traits
        public override int ClassBaseCost => 0;

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new System.Collections.Generic.HashSet<MoreInfoWordName>()
        {
            MoreInfoWordName.Heat,
            MoreInfoWordName.Burnt,
            MoreInfoWordName.Junk
,
        }.ToILCPP();

        public override Il2CppCollections.HashSet<CardName> MoreInfoCards => new System.Collections.Generic.HashSet<CardName>() 
        { 
            CardName.Meltdown 
        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new System.Collections.Generic.HashSet<ComponentTrait>()
        {
            ComponentTrait.Basic,
            ComponentTrait.SelectionLess
        }.ToILCPP();


        public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new System.Collections.Generic.HashSet<ComponentName>()
        {
            ModContentManager.GetModComponentName<CatalysedComponent>()

        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new System.Collections.Generic.HashSet<ComponentName>()
        {
            ComponentName.Chilled,
            ComponentName.Broken,
            ComponentName.Echo,
            ComponentName.Refreshed,
            ComponentName.Risky,
            ComponentName.Boosted
        }.ToILCPP();

        public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => new System.Collections.Generic.HashSet<CardTrait>().ToILCPP();
        #endregion

        #region Tasks

        public override Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue cardID)
        {
            List<Il2CppSystem.ValueTuple<Trigger, ACondition>> triggerConditions = new()
            {
                new (Trigger.PreTask, new AndCondition(
                    new IsTypeCondition<EndTurnTask>(new RunningTaskValue()),
                    new CardInHandButNotBeingPlayedCondition(cardID)
                ))
            };

            List<ATask> triggerTasks = new()
            {
                new AblationPurgeTask(cardID)
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


            taskList.Add(new Ablation_1());


            return taskList;
        }
        #endregion
    }

    public class CatalysedComponent : AModComponent
    {
        public override string DisplayName => "Catalysed";

        public override string Description => "While this card is in your hand, burning a card purges a <font=\"StarvadersGun-Regular SDF\"><size=150%><voffset=-0.11em>Meltdown</i></font></b></smallcaps></color></size></voffset> instead.";

        public override ClassName Class => ClassName.Gunner;

        public override Il2CppSystem.Collections.Generic.List<ATask> GetOnCreateTaskList(OnCreateIDValue cardID)
        {
            return base.GetOnCreateTaskList(cardID);
        }
        public override Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue cardID)
        {
            List<Il2CppSystem.ValueTuple<Trigger, ACondition>> triggerConditions = new()
            {
                new (Trigger.PreSkipTask, new AndCondition(
                    new IsTypeCondition<SetCardBurntTask>(new RunningTaskValue()),
                    new CardInHandButNotBeingPlayedCondition(cardID),
                    new OrCondition(
                        new CardNameInPileCondition(CardName.Meltdown , Pile.Hand),
                        new CardNameInPileCondition(CardName.Meltdown , Pile.Discard),
                        new CardNameInPileCondition(CardName.Meltdown , Pile.Draw)),
                    new NotCondition(new CardNameInPileCondition(CardName.Fuel , Pile.Hand))
                ))
            };

            List<ATask> triggerTasks = new()
            {
                new AblationMeldownTask(),
                new SkipNextTask(),
            };

            return new List<TriggerEffect>()
            {
                new TriggerEffect(triggerConditions.ToILCPP(), triggerTasks.ToILCPP())

            }.ToILCPP();
        }
    }
}