using AudioImportLib;
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

            int heatInSink = taskInstance.EncounterModel.Values[EncounterValue.Heat];

            MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(taskInstance.EncounterModel.GridModel.GetPlayerCoord()).transform.position));

            yield return taskInstance.TaskEngine.ProcessTask(
                new EncounterValueOperationTask(
                    EncounterValue.Heat,
                    Il2CppStarVaders.Operation.Replace,
                    new Il2CppSystem.Int32 { m_value = 0 }.BoxIl2CppObject()
                )
            ).Cast<Il2CppSystem.Object>();

            for (int i = 0; i < heatInSink; i++)
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



    public class AblationPurgeTask : AModTask
    {
        public AblationPurgeTask()
        {
        }
        public AblationPurgeTask(Il2CppSystem.Object cardID)
        {
            SetArg(ArgKey.CardID, cardID);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            foreach (CardID cardID in taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Hand).ToMono().ToList())
            {
                CardModel card = taskInstance.EncounterModel.GetModelItem<CardModel>(cardID.ToID());

                if (card.Traits.Contains(CardTrait.Junk) || card.IsBurnt)
                {
                    yield return taskInstance.TaskEngine.ProcessTask(
                        new PurgeCardTask(cardID.BoxIl2CppObject())
                    ).Cast<Il2CppSystem.Object>();
                }
            }

        }
    }

    public class AblationMeldownTask : AModTask
    {
        public AblationMeldownTask()
        {
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {

            Il2CppSystem.Object primaryCardID = taskInstance.GetArg<Il2CppSystem.Object>(ArgKey.CardID);

            if (primaryCardID != null)
            {
                yield return taskInstance.TaskEngine.ProcessTask(
                    new PurgeCardTask(primaryCardID)
                ).Cast<Il2CppSystem.Object>();
            }

            Pile[] pilesToScan = { Pile.Hand, Pile.Draw, Pile.Discard };

            System.Collections.Generic.List<CardID> meltdownCards = pilesToScan
                .SelectMany(pile => taskInstance.EncounterModel.CardPlayModel.GetPile(pile).ToMono())
                .Where(cardID => cardID.CardName == CardName.Meltdown)
                .OrderByDescending(cardID => {return taskInstance.EncounterModel.GetModelItem<CardModel>(cardID.ToID()).IsBurnt;})
                .ToList();

            yield return taskInstance.TaskEngine.ProcessTask(
                new PurgeCardTask(meltdownCards[0].BoxIl2CppObject())
            ).Cast<Il2CppSystem.Object>();

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