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
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Playables;
using static UnityEngine.Rendering.ReloadAttribute;
using Il2CppCollections = Il2CppSystem.Collections.Generic;

namespace SV_UniqueCards
{
    public class Ablation_1 : AModTask
    {
        public Ablation_1()
        {
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            int Meltdowns = taskInstance.EncounterModel.Values[EncounterValue.Heat];

            MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(taskInstance.EncounterModel.GridModel.GetPlayerCoord()).transform.position));

            yield return taskInstance.TaskEngine.ProcessTask(
                new EncounterValueOperationTask(
                    EncounterValue.Heat,
                    Il2CppStarVaders.Operation.Replace,
                    new Il2CppSystem.Int32 { m_value = 0 }.BoxIl2CppObject()
                )
            ).Cast<Il2CppSystem.Object>();

            for (int i = 0; i < Meltdowns; i++)
            {
                yield return taskInstance.TaskEngine.ProcessTask(
                    new CreateCardTask(
                        CardName: new Il2CppSystem.Int32 { m_value = (int)CardName.Meltdown }.BoxIl2CppObject(),
                        Pile: Pile.Hand,
                        isFastMode: false,
                        rarity: new()
                    )
                ).Cast<Il2CppSystem.Object>();
            }
        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject Fire = new GameObject("FireVFX");
            Fire.transform.position = spawnPosition + new UnityEngine.Vector3(0f, -4f, 1f);
            Fire.transform.localScale = new UnityEngine.Vector3(4f, 4f, 4f);
            Fire.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);

            SpriteRenderer sr = Fire.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 999;

            float secondsPerFrame = 0.05f;
            Sprite[] frames = AblationVFXManager.GetOrLoadSprites();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null && sr != null)
                {
                    sr.sprite = frames[i];
                }

                yield return new UnityEngine.WaitForSeconds(secondsPerFrame);
            }

            if (Fire != null)
            {
                UnityEngine.Object.Destroy(Fire);
            }
        }
    }

    public class Ablation_1_1 : AModTask
    {
        public Ablation_1_1()
        {
        }
        public Ablation_1_1(Il2CppSystem.Object cardID)
        {
            SetArg(ArgKey.CardID, cardID);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {

            Pile[] pilesToScan = { Pile.Hand, Pile.Draw, Pile.Discard };

            System.Collections.Generic.List<CardID> meltdownCards = pilesToScan
                .SelectMany(pile => taskInstance.EncounterModel.CardPlayModel.GetPile(pile).ToMono().Select(cardID => new { cardID, pile }))
                .Where(card => card.cardID.CardName == CardName.Meltdown)
                .OrderByDescending(card => { return taskInstance.EncounterModel.GetModelItem<CardModel>(card.cardID.ToID()).IsBurnt; })
                .ThenBy(card => System.Array.IndexOf(pilesToScan, card.pile))
                .Select(card => card.cardID)
                .ToList();

            Il2CppSystem.Object cardID = taskInstance.GetArg<Il2CppSystem.Object>(ArgKey.CardID);

            if (meltdownCards.Count > 0)
            {
                yield return taskInstance.TaskEngine.ProcessTask(
                    new MoveCardTask(meltdownCards[0].BoxIl2CppObject(), Pile.Hand, false, new Il2CppSystem.Nullable<float>(0.2f))
                ).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new PurgeCardTask(meltdownCards[0].BoxIl2CppObject())
                ).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new SkipNextTask()
                ).Cast<Il2CppSystem.Object>();
            }

            else
            {
                yield return taskInstance.TaskEngine.ProcessTask(
                    new PreviewWhiffTask(cardID)
                ).Cast<Il2CppSystem.Object>();
            }
        }
    }

    public class Ablation_1_2 : AModTask
    {
        public Ablation_1_2()
        {
        }

        public Ablation_1_2(Il2CppSystem.Object cardID)
        {
            SetArg(ArgKey.CardID, cardID);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {

            Pile[] pilesToScan = { Pile.Hand, Pile.Draw, Pile.Discard };

            System.Collections.Generic.List<CardID> junkCards = pilesToScan
                .SelectMany(pile => taskInstance.EncounterModel.CardPlayModel.GetPile(pile).ToMono().Select(cardID => new { cardID, pile }))
                .Where(card => taskInstance.EncounterModel.GetModelItem<CardModel>(card.cardID.ToID()).Traits.Contains(CardTrait.Junk))
                .OrderByDescending(card => {return taskInstance.EncounterModel.GetModelItem<CardModel>(card.cardID.ToID()).IsBurnt;})
                .ThenBy(card => taskInstance.EncounterModel.GetModelItem<CardModel>(card.cardID.ToID()).Traits.Count)
                .ThenBy(card => System.Array.IndexOf(pilesToScan, card.pile))
                .Select(card => card.cardID)
                .ToList();

            CardID cardID = taskInstance.GetArg<CardID>(ArgKey.CardID);

            if (junkCards.Count > 0)
            {
                yield return taskInstance.TaskEngine.ProcessTask(
                    new MoveCardTask(junkCards[0].BoxIl2CppObject(), Pile.Hand, false, new Il2CppSystem.Nullable<float>(0.2f))
                ).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new PurgeCardTask(junkCards[0].BoxIl2CppObject())
                ).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new SkipNextTask()
                ).Cast<Il2CppSystem.Object>();
            }

            else
            {
                yield return taskInstance.TaskEngine.ProcessTask(
                    new PreviewWhiffTask(cardID.BoxIl2CppObject())
                ).Cast<Il2CppSystem.Object>();
            }
        }
    }

    public static class AblationVFXManager
    {
        public static Sprite[] FireFrames = new Sprite[9];

        public static Sprite[] GetOrLoadSprites()
        {
            if (FireFrames[0] != null)
                return FireFrames;

            Assembly assembly = Assembly.GetExecutingAssembly();

            for (int i = 0; i < 9; i++)
            {
                string resourceName = $"SV_UniqueCards.gridfx.Gunner.Backblast_{i:D2}.png";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        byte[] fileData = new byte[stream.Length];
                        stream.Read(fileData, 0, (int)stream.Length);

                        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGBA32, false);
                        ImageConversion.LoadImage(tex, fileData);

                        tex.filterMode = FilterMode.Bilinear;
                        tex.wrapMode = TextureWrapMode.Clamp;
                        tex.anisoLevel = 1;
                        tex.mipMapBias = 0f;
                        tex.Compress(false);

                        FireFrames[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }
                }
            }
            return FireFrames;
        }
    }

}