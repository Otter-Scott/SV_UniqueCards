using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppLanguage.Lua;
using Il2CppStarVaders;
using Il2CppSystem.Collections.Generic;
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
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using static UnityEngine.Rendering.ReloadAttribute;
using Il2CppCollections = Il2CppSystem.Collections.Generic;

namespace SV_UniqueCards
{
    public class OSS_1 : AModTask
    {
        public OSS_1() 
        { 
        }

        public OSS_1(Il2CppSystem.Object cardID)
        {
            SetArg(ArgKey.CardID, cardID);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {

            var hand = taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Hand).ToMono();
            System.Collections.Generic.List<int> validCardsList = new();

            for (int i = 0; i < hand.Count; i++)
            {

                if (taskInstance.EncounterModel.GetModelItem<CardModel>(hand[i].ToID()).Traits.Contains(CardTrait.Junk) || taskInstance.EncounterModel.CardPlayModel.CardsBeingPlayed.Contains(hand[i]))
                {
                    continue;
                };

                validCardsList.Add(i);
            }


            if (taskInstance.IsPreviewModeView)
            {
                if (validCardsList.Count < 2)
                {
                    Il2CppSystem.Object cardID = taskInstance.GetArg<Il2CppSystem.Object>(ArgKey.CardID);

                    yield return taskInstance.TaskEngine.ProcessTask(
                        new PreviewWhiffTask(cardID)
                    ).Cast<Il2CppSystem.Object>();
                }
            }

            if (validCardsList.Count >= 2)
            {
                System.Random random = new();


                int randomIndex1 = random.Next(validCardsList.Count);
                int randomIndex2;
                do
                {
                    randomIndex2 = random.Next(validCardsList.Count);
                } while (randomIndex1 == randomIndex2);

                CardID cardId1 = hand[validCardsList[randomIndex1]];
                CardID cardId2 = hand[validCardsList[randomIndex2]];

                yield return taskInstance.TaskEngine.ProcessTask(
                    new MoveCardTask(
                        cardId1.BoxIl2CppObject(), 
                        Pile.Purged,
                        true,
                        new Il2CppSystem.Nullable<float>(0.2f)
                    )).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new MoveCardTask(
                        cardId2.BoxIl2CppObject(),
                        Pile.Purged,
                        true,
                        new Il2CppSystem.Nullable<float>(0.2f)
                    )).Cast<Il2CppSystem.Object>();

                int rngValue = random.Next(3);
                CardName cardToGenerate = rngValue switch
                {
                    0 => ModContentManager.GetModCardName<OSS1>(),
                    1 => ModContentManager.GetModCardName<OSS2>(),
                    _ => ModContentManager.GetModCardName<OSS3>()
                };

                yield return taskInstance.TaskEngine.ProcessTask(
                    new CreateCardTask(
                        new Il2CppSystem.Int32 { m_value = (int)cardToGenerate }.BoxIl2CppObject(),
                        Pile: Pile.Hand,
                        rarity: new()
                    )
                ).Cast<Il2CppSystem.Object>();
            }
        }
    }

    public class OSS_2 : AModTask
    {
        public OSS_2()
        {
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            var hand = taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Hand).ToMono();

            if (hand.Count >= 3)
            {
                Selection selection1 = new(
                    new AndCondition(
                        new IsTypeCondition<CardID>(new TargetValue()),
                        new CardInHandButNotBeingPlayedCondition(new TargetValue())
                    ),
                    selectionDescriptor: SelectionDescriptor.CardToPurge
                );

                var Task1 = new MoveCardTask(
                    new TargetValue(),
                    Pile.Purged,
                    true,
                    new Il2CppSystem.Nullable<float>(0.2f)
                );

                yield return taskInstance.TaskEngine.ProcessTask(
                    new SelectionTask(
                        Selections: new System.Collections.Generic.List<Selection> { selection1 }.ToILCPP(),
                        TaskList: new System.Collections.Generic.List<ATask> { Task1 }.ToILCPP(),
                        PreSelectionPreviewTaskList: new System.Collections.Generic.List<ATask>().ToILCPP(),
                        isFullyCancellable: false
                    )
                ).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new SelectionTask(
                        Selections: new System.Collections.Generic.List<Selection> { selection1 }.ToILCPP(),
                        TaskList: new System.Collections.Generic.List<ATask> { Task1 }.ToILCPP(),
                        PreSelectionPreviewTaskList: new System.Collections.Generic.List<ATask>().ToILCPP(),
                        isFullyCancellable: false
                    )
                ).Cast<Il2CppSystem.Object>();

                System.Random random = new();
                int rngValue = random.Next(3);
                CardName cardToGenerate = rngValue switch
                {
                    0 => ModContentManager.GetModCardName<OSS1>(),
                    1 => ModContentManager.GetModCardName<OSS2>(),
                    _ => ModContentManager.GetModCardName<OSS3>()
                };

                yield return taskInstance.TaskEngine.ProcessTask(
                    new CreateCardTask(
                        new Il2CppSystem.Int32 { m_value = (int)cardToGenerate }.BoxIl2CppObject(),
                        Pile: Pile.Hand,
                        rarity: new()
                    )
                ).Cast<Il2CppSystem.Object>();
            }
        }
    }
}