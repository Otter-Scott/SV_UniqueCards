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
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.ReloadAttribute;
using Il2CppCollections = Il2CppSystem.Collections.Generic;

namespace SV_UniqueCards
{
    public class OSS1_1 : AModTask
    {
        public OSS1_1()
        {
        }


        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            Coord player = taskInstance.EncounterModel.GridModel.GetPlayerCoord();

            Coord[] strikeTiles =
            [
            new Coord(player.x, player.y + 1),
            new Coord(player.x - 1, player.y + 2),
            new Coord(player.x, player.y + 2),
            new Coord(player.x + 1, player.y + 2),
            new Coord(player.x - 2, player.y + 3),
            new Coord(player.x - 1, player.y + 3),
            new Coord(player.x, player.y + 3),
            new Coord(player.x + 1, player.y + 3),
            new Coord(player.x + 2, player.y + 3),
            new Coord(player.x - 3, player.y + 4),
            new Coord(player.x - 2, player.y + 4),
            new Coord(player.x - 1, player.y + 4),
            new Coord(player.x, player.y + 4),
            new Coord(player.x + 1, player.y + 4),
            new Coord(player.x + 2, player.y + 4),
            new Coord(player.x + 3, player.y + 4),
            ];

            if (taskInstance.IsPreviewModeView)
            {
                foreach (Coord tile in strikeTiles)
                {
                    if (taskInstance.EncounterModel.GridModel.IsCoordInGridRange(tile))
                    {
                            yield return taskInstance.TaskEngine.ProcessTask(
                                new StrikeTileEffectTask(
                                    tile.BoxIl2CppObject()
                            )).Cast<Il2CppSystem.Object>();
                    }
                }

                yield break;
            }

            foreach (Coord tile in strikeTiles)
            {
                if (taskInstance.EncounterModel.GridModel.IsCoordInGridRange(tile))
                {
                    if (!taskInstance.EncounterModel.GridModel.IsCoordEmpty(tile))
                    {
                        yield return taskInstance.TaskEngine.ProcessTask(
                            new StrikeTileEffectTask(
                                tile.BoxIl2CppObject()
                        )).Cast<Il2CppSystem.Object>();
                    }
                }
            }
            
        }
    }

    public class OSS1_1VFX : AModTask
    {
        public OSS1_1VFX()
        {
        }


        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            Coord player = taskInstance.EncounterModel.GridModel.GetPlayerCoord();


            MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(player).transform.position));

        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject OSS1 = new GameObject("OSS1VFX");
            OSS1.transform.position = spawnPosition + new UnityEngine.Vector3(0f, 22f, 0f);
            OSS1.transform.localScale = new UnityEngine.Vector3(12f, 12f, 12f);
            OSS1.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);

            SpriteRenderer sr = OSS1.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 999;

            float secondsPerFrame = 0.04f;
            Sprite[] frames = OSS1VFXManager.GetOrLoadSprites();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null && sr != null)
                {
                    sr.sprite = frames[i];
                }

                yield return new UnityEngine.WaitForSeconds(secondsPerFrame);
            }

            if (OSS1 != null)
            {
                UnityEngine.Object.Destroy(OSS1);
            }
        }
    }

    public class OSS1_2 : AModTask
    {
        public OSS1_2()
        {
        }


        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            Coord player = taskInstance.EncounterModel.GridModel.GetPlayerCoord();

            Coord[] strikeTiles =
            [
            new Coord(player.x, player.y + 1),
            new Coord(player.x - 1, player.y + 2),
            new Coord(player.x, player.y + 2),
            new Coord(player.x + 1, player.y + 2),
            new Coord(player.x - 2, player.y + 3),
            new Coord(player.x - 1, player.y + 3),
            new Coord(player.x, player.y + 3),
            new Coord(player.x + 1, player.y + 3),
            new Coord(player.x + 2, player.y + 3),
            new Coord(player.x - 3, player.y + 4),
            new Coord(player.x - 2, player.y + 4),
            new Coord(player.x - 1, player.y + 4),
            new Coord(player.x, player.y + 4),
            new Coord(player.x + 1, player.y + 4),
            new Coord(player.x + 2, player.y + 4),
            new Coord(player.x + 3, player.y + 4),
            ];

            if (taskInstance.IsPreviewModeView)
            {
                foreach (Coord tile in strikeTiles)
                {
                    if (taskInstance.EncounterModel.GridModel.IsCoordInGridRange(tile))
                    {
                        yield return taskInstance.TaskEngine.ProcessTask(
                            new StrikeTileEffectTask(
                                tile.BoxIl2CppObject()
                        )).Cast<Il2CppSystem.Object>();
                    }
                }

                yield break;
            }

            foreach (Coord tile in strikeTiles)
            {
                if (taskInstance.EncounterModel.GridModel.IsCoordInGridRange(tile))
                {
                    if (!taskInstance.EncounterModel.GridModel.IsCoordEmpty(tile))
                    {
                        yield return taskInstance.TaskEngine.ProcessTask(
                            new StrikeTileEffectTask(
                                tile.BoxIl2CppObject()
                        )).Cast<Il2CppSystem.Object>();

                        yield return taskInstance.TaskEngine.ProcessTask(
                            new StrikeTileEffectTask(
                                tile.BoxIl2CppObject()
                        )).Cast<Il2CppSystem.Object>();
                    }
                }
            }

        }
    }

    public class OSS1_2_Misc : AModTask
    {
        public OSS1_2_Misc()
        {
        }


        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {

            if (taskInstance.IsPreviewModeView)
                yield break;

            var hand = taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Hand).ToMono();
            System.Collections.Generic.List<int> MoveList = new();
            System.Collections.Generic.List<int> JunkList = new();

            for (int i = 0; i < hand.Count; i++)
            {
                if (taskInstance.EncounterModel.GetModelItem<CardModel>(hand[i].ToID()).Traits.Contains(CardTrait.Junk) && !taskInstance.EncounterModel.GetModelItem<CardModel>(hand[i].ToID()).IsFree)
                {
                    JunkList.Add(i);
                };

                if (taskInstance.EncounterModel.GetModelItem<CardModel>(hand[i].ToID()).Traits.Contains(CardTrait.Move) && !taskInstance.EncounterModel.GetModelItem<CardModel>(hand[i].ToID()).IsFree)
                {
                    MoveList.Add(i);
                };

                continue;
            }

            System.Random random = new();

            if (JunkList.Count > 1)
            {
                int randomIndex1 = random.Next(JunkList.Count);
                int randomIndex2;
                do
                {
                    randomIndex2 = random.Next(JunkList.Count);
                } while (randomIndex1 == randomIndex2);

                CardID cardId1 = hand[JunkList[randomIndex1]];
                CardID cardId2 = hand[JunkList[randomIndex2]];

                yield return taskInstance.TaskEngine.ProcessTask(
                    new MakeCardFreeThisTurnTask(
                        cardId1.BoxIl2CppObject()
                )).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new MakeCardFreeThisTurnTask(
                        cardId2.BoxIl2CppObject()
                )).Cast<Il2CppSystem.Object>();

            }

            else if (JunkList.Count == 1)
            {

                CardID cardId1 = hand[JunkList[0]];

                yield return taskInstance.TaskEngine.ProcessTask(
                    new MakeCardFreeThisTurnTask(
                        cardId1.BoxIl2CppObject()
                )).Cast<Il2CppSystem.Object>();
            }

            if (MoveList.Count > 1)
            {

                int randomIndex1 = random.Next(MoveList.Count);
                int randomIndex2;
                do
                {
                    randomIndex2 = random.Next(MoveList.Count);
                } while (randomIndex1 == randomIndex2);

                CardID cardId1 = hand[MoveList[randomIndex1]];
                CardID cardId2 = hand[MoveList[randomIndex2]];

                yield return taskInstance.TaskEngine.ProcessTask(
                    new MakeCardFreeThisTurnTask(
                        cardId1.BoxIl2CppObject()
                )).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new MakeCardFreeThisTurnTask(
                        cardId2.BoxIl2CppObject()
                )).Cast<Il2CppSystem.Object>();

            }

            else if (MoveList.Count == 1)
            {
                CardID cardId1 = hand[MoveList[0]];

                yield return taskInstance.TaskEngine.ProcessTask(
                    new MakeCardFreeThisTurnTask(
                        cardId1.BoxIl2CppObject()
                )).Cast<Il2CppSystem.Object>();
            }
        }
    }

    public class OSS1_2VFX : AModTask
    {
        public OSS1_2VFX()
        {
        }


        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            Coord player = taskInstance.EncounterModel.GridModel.GetPlayerCoord();


            MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(player).transform.position));

        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject OSS1 = new GameObject("OSS1VFX");
            OSS1.transform.position = spawnPosition + new UnityEngine.Vector3(0f, 22f, 0f);
            OSS1.transform.localScale = new UnityEngine.Vector3(12f, 12f, 12f);
            OSS1.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);

            SpriteRenderer sr = OSS1.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 999;
            sr.flipX = true;

            float secondsPerFrame = 0.04f;
            Sprite[] frames = OSS1VFXManager.GetOrLoadSprites();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null && sr != null)
                {
                    sr.sprite = frames[i];
                }

                yield return new UnityEngine.WaitForSeconds(secondsPerFrame);
            }

            if (OSS1 != null)
            {
                UnityEngine.Object.Destroy(OSS1);
            }
        }

    }

    public static class OSS1VFXManager
    {
        public static Sprite[] OSS1Frames = new Sprite[19];

        public static Sprite[] GetOrLoadSprites()
        {
            if (OSS1Frames[0] != null)
                return OSS1Frames;

            Assembly assembly = Assembly.GetExecutingAssembly();

            for (int i = 0; i < 19; i++)
            {
                string resourceName = $"SV_UniqueCards.gridfx.Gunner.OSS1_{i:D2}.png";

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

                        OSS1Frames[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }
                }
            }
            return OSS1Frames;
        }
    }

}