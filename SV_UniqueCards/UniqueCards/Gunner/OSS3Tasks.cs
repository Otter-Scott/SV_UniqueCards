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

    public class OSS3_1 : AModTask
    {
        public OSS3_1()
        {
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            Coord player = taskInstance.EncounterModel.GridModel.GetPlayerCoord();
            int gridY = taskInstance.EncounterModel.GridModel.GridShape.y;

            Coord[] strikeColumns =
            [
            new Coord(player.x - 1, player.y),
            new Coord(player.x + 1, player.y)
            ];

            if (taskInstance.IsPreviewModeView)
            {
                foreach (Coord column in strikeColumns)
                {
                    if (taskInstance.EncounterModel.GridModel.IsCoordInGridRange(column))
                    {
                        for (int y = 0; y < gridY; y++)
                        {
                            Coord targetCoord = new Coord(column.x, y);
                                yield return taskInstance.TaskEngine.ProcessTask(
                                new StrikeTileEffectTask(
                                    targetCoord.BoxIl2CppObject()
                                )).Cast<Il2CppSystem.Object>();

                        }
                    }
                }

                yield break;
            }

            

            foreach (Coord column in strikeColumns)
            {
                if (taskInstance.EncounterModel.GridModel.IsCoordInGridRange(column))
                {
                    for (int y = 0; y < gridY; y++)
                    {
                        Coord targetCoord = new Coord(column.x, y);
                        MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(targetCoord).transform.position));
                    }

                    for (int y = 0; y < gridY; y++)
                    {
                        Coord targetCoord = new Coord(column.x, y);
                        if (!taskInstance.EncounterModel.GridModel.IsCoordEmpty(targetCoord))
                        {
                            yield return taskInstance.TaskEngine.ProcessTask(
                            new StrikeTileEffectTask(
                                targetCoord.BoxIl2CppObject()
                            )).Cast<Il2CppSystem.Object>();
                        }

                    }
                }
            }
        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject OSS3 = new GameObject("OSS3VFX");
            OSS3.transform.position = spawnPosition + new UnityEngine.Vector3(0f, 0f, 0f);
            OSS3.transform.localScale = new UnityEngine.Vector3(2.5f, 20f, 2.5f);
            OSS3.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);

            SpriteRenderer sr = OSS3.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 999;

            float secondsPerFrame = 0.03f;
            Sprite[] frames = OSS3VFXManager.GetOrLoadSprites();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null && sr != null)
                {
                    sr.sprite = frames[i];
                }

                yield return new UnityEngine.WaitForSeconds(secondsPerFrame);
            }

            if (OSS3 != null)
            {
                UnityEngine.Object.Destroy(OSS3);
            }
        }
    }

    public class OSS3_2 : AModTask
    {
        public OSS3_2()
        {
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            

            Coord player = taskInstance.EncounterModel.GridModel.GetPlayerCoord();
            int gridY = taskInstance.EncounterModel.GridModel.GridShape.y;

            Coord[] strikeColumns =
            [
            new Coord(player.x - 2, player.y),
            new Coord(player.x - 1, player.y),
            new Coord(player.x + 1, player.y),
            new Coord(player.x + 2, player.y)
            ];

            if (taskInstance.IsPreviewModeView)
            {
                foreach (Coord column in strikeColumns)
                {
                    if (taskInstance.EncounterModel.GridModel.IsCoordInGridRange(column))
                    {
                        for (int y = 0; y < gridY; y++)
                        {
                            Coord targetCoord = new Coord(column.x, y);
                            yield return taskInstance.TaskEngine.ProcessTask(
                            new StrikeTileEffectTask(
                                targetCoord.BoxIl2CppObject()
                            )).Cast<Il2CppSystem.Object>();

                        }
                    }
                }

                yield break;
            }

            foreach (Coord column in strikeColumns)
            {
                if (taskInstance.EncounterModel.GridModel.IsCoordInGridRange(column))
                {
                    for (int y = 0; y < gridY; y++)
                    {
                        Coord targetCoord = new Coord(column.x, y);

                        if (!taskInstance.EncounterModel.GridModel.IsCoordEmpty(targetCoord))
                        {
                            MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(targetCoord).transform.position));
                        }
                    }

                    for (int y = 0; y < gridY; y++)
                    {
                        Coord targetCoord = new Coord(column.x, y);
                        if (!taskInstance.EncounterModel.GridModel.IsCoordEmpty(targetCoord))
                        {
                            yield return taskInstance.TaskEngine.ProcessTask(
                            new StrikeTileEffectTask(
                                targetCoord.BoxIl2CppObject()
                            )).Cast<Il2CppSystem.Object>();
                        }

                    }
                }
            }
        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject OSS3 = new GameObject("OSS3VFX");
            OSS3.transform.position = spawnPosition + new UnityEngine.Vector3(0f, 0f, 0f);
            OSS3.transform.localScale = new UnityEngine.Vector3(2.5f, 20f, 2.5f);
            OSS3.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);

            SpriteRenderer sr = OSS3.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 999;

            float secondsPerFrame = 0.03f;
            Sprite[] frames = OSS3VFXManager.GetOrLoadSprites();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null && sr != null)
                {
                    sr.sprite = frames[i];
                }

                yield return new UnityEngine.WaitForSeconds(secondsPerFrame);
            }

            if (OSS3 != null)
            {
                UnityEngine.Object.Destroy(OSS3);
            }
        }
    }

    public class OSS3_2_Misc : AModTask
    {
        public OSS3_2_Misc()
        {
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            var hand = taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Hand).ToMono();

            System.Collections.Generic.List<int> attackList = new();

            for (int i = 0; i < hand.Count; i++)
            {
                if (taskInstance.EncounterModel.GetModelItem<CardModel>(hand[i].ToID()).Traits.Contains(CardTrait.Attack))
                {
                    attackList.Add(i);
                };

                continue;
            }

            System.Random random = new();

            int randomIndexDiscard = random.Next(attackList.Count);

            CardID cardId = hand[attackList[randomIndexDiscard]];

            yield return taskInstance.TaskEngine.ProcessTask(
                new GiveCardRepeatTask(
                    cardId.BoxIl2CppObject(),
                    amount: new Il2CppSystem.Int32 { m_value = 2 }.BoxIl2CppObject()
            )).Cast<Il2CppSystem.Object>();
        }
    }

    public static class OSS3VFXManager
    {
        public static Sprite[] OSS3Frames = new Sprite[12];

        public static Sprite[] GetOrLoadSprites()
        {
            if (OSS3Frames[0] != null)
                return OSS3Frames;

            Assembly assembly = Assembly.GetExecutingAssembly();

            for (int i = 0; i < 12; i++)
            {
                string resourceName = $"SV_UniqueCards.gridfx.Gunner.OSS3_{i:D2}.png";

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

                        OSS3Frames[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }
                }
            }
            return OSS3Frames;
        }
    }
}