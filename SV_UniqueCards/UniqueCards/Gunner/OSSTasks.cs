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
    public class OSSPurge : AModTask
    {
        public OSSPurge() 
        { 
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            var hand = taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Hand).ToMono();
            var validCardsList = new System.Collections.Generic.List<int>();

            for (int i = 0; i < hand.Count; i++)
            {
                var cardModel = taskInstance.EncounterModel.GetModelItem<CardModel>(hand[i].ToID());

                if (cardModel.Traits.Contains(CardTrait.Junk) || taskInstance.EncounterModel.CardPlayModel.CardsBeingPlayed.Contains(hand[i]))
                {
                    continue;
                };

                validCardsList.Add(i);
            }

            int validCount = validCardsList.Count;

            if (validCount >= 2)
            {
                System.Random rnd = new System.Random();


                int rndIndex1 = rnd.Next(validCount);
                int rndIndex2;
                do
                {
                    rndIndex2 = rnd.Next(validCount);
                } while (rndIndex1 == rndIndex2);

                int handIndex1 = validCardsList[rndIndex1];
                int handIndex2 = validCardsList[rndIndex2];

                var cardId1 = hand[handIndex1];
                var cardId2 = hand[handIndex2];

                yield return taskInstance.TaskEngine.ProcessTask(
                    new PurgeCardTask(cardId1.BoxIl2CppObject())
                ).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new PurgeCardTask(cardId2.BoxIl2CppObject())
                ).Cast<Il2CppSystem.Object>();

                int rngValue = rnd.Next(3);
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

    public class OSS2Task : AModTask
    {
        public OSS2Task() { }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;
        }
    }
}