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

    public class OSS2_1 : AModTask
    {
        public OSS2_1() 
        { 
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            int playerX = taskInstance.EncounterModel.GridModel.GetPlayerCoord().x;

            Selection selection = new(
                new AndCondition(
                    new IsTypeCondition<Coord>(new TargetValue()),
                    new IsCoordInRowOrColumnRangeCondition(
                        new TargetValue(),
                        new Il2CppSystem.Int32 { m_value = playerX }.BoxIl2CppObject(),
                        new Il2CppSystem.Int32 { m_value = playerX }.BoxIl2CppObject(),
                        false
                    )
                ),
                selectionDescriptor: SelectionDescriptor.Tile
            );

            var Task1 = new OSS2TaskUp(new TargetValue()).Convert();

            yield return taskInstance.TaskEngine.ProcessTask(
                new SelectionTask(
                    new System.Collections.Generic.List<Selection> { selection }.ToILCPP(),
                    new System.Collections.Generic.List<ATask> { Task1 }.ToILCPP(),
                    new System.Collections.Generic.List<ATask>().ToILCPP(),
                    isFullyCancellable: false
                )
            ).Cast<Il2CppSystem.Object>();
        }
    }

    public class OSS2_2 : AModTask
    {
        public OSS2_2()
        {
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            int playerX = taskInstance.EncounterModel.GridModel.GetPlayerCoord().x;

            Selection selection = new(
                new AndCondition(
                    new IsTypeCondition<Coord>(new TargetValue()),
                    new IsCoordInRowOrColumnRangeCondition(
                        new TargetValue(),
                        new Il2CppSystem.Int32 { m_value = playerX }.BoxIl2CppObject(),
                        new Il2CppSystem.Int32 { m_value = playerX }.BoxIl2CppObject(),
                        false
                    )
                ),
                selectionDescriptor: SelectionDescriptor.Tile
            );

            var Task1 = new OSS2TaskUp(new TargetValue()).Convert();
            var Task2 = new OSS2TaskDown(new TargetValue()).Convert();


            yield return taskInstance.TaskEngine.ProcessTask(
                new SelectionTask(
                    new System.Collections.Generic.List<Selection> { selection }.ToILCPP(),
                    new System.Collections.Generic.List<ATask> { Task1 }.ToILCPP(),
                    new System.Collections.Generic.List<ATask>().ToILCPP(),
                    isFullyCancellable: false
                )
            ).Cast<Il2CppSystem.Object>();

            yield return taskInstance.TaskEngine.ProcessTask(
                new SelectionTask(
                    new System.Collections.Generic.List<Selection> { selection }.ToILCPP(),
                    new System.Collections.Generic.List<ATask> { Task2 }.ToILCPP(),
                    new System.Collections.Generic.List<ATask>().ToILCPP(),
                    isFullyCancellable: false
                )
            ).Cast<Il2CppSystem.Object>();
        }
    }

    public class OSS2_2_Misc : AModTask
    {
        public OSS2_2_Misc()
        {
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            var discard = taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Discard).ToMono();

            System.Collections.Generic.List<int> TacticDiscardList = new();

            for (int i = 0; i < discard.Count; i++)
            {
                if (taskInstance.EncounterModel.GetModelItem<CardModel>(discard[i].ToID()).Traits.Contains(CardTrait.Tactic))
                {
                    TacticDiscardList.Add(i);
                };

                continue;
            }

            System.Random random = new();

            int randomIndexDiscard = random.Next(TacticDiscardList.Count);

            CardID cardIdDiscard = discard[TacticDiscardList[randomIndexDiscard]];

            yield return taskInstance.TaskEngine.ProcessTask(
                new DrawTopCardOfTraitTask(
                    CardTrait.Tactic
            )).Cast<Il2CppSystem.Object>();

            yield return taskInstance.TaskEngine.ProcessTask(
                new ReturnalSpecificTask(
                    cardIdDiscard.BoxIl2CppObject()
            )).Cast<Il2CppSystem.Object>();

        }
    }

    public class OSS2TaskUp : AModTask
    {
        public OSS2TaskUp()
        {
        }

        public OSS2TaskUp(TargetValue targetValue)
        {
            SetArg(ArgKey.Coord, targetValue);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            int gridX = taskInstance.EncounterModel.GridModel.GridShape.x;
            Coord chosenCoords = taskInstance.GetArg<Il2CppSystem.Object>(ArgKey.Coord).Unbox<Coord>();

            for (int x = 0; x < gridX; x++)
            {
                Coord targetCoord = new Coord(x, chosenCoords.y);

                Il2CppCollections.List<ATask> animTasks = new();

                animTasks.Add(new OSS2UpTaskVFX(targetCoord));

                yield return taskInstance.TaskEngine.ProcessTask(
                        new AddCoordDelayedAttackTask(
                            new ArtilleryStrikeDelayedAttack(Direction.N),
                            taskInstance.GridModel.PlayerID.Cast<Il2CppSystem.Object>(),
                            targetCoord.BoxIl2CppObject(),
                            animTasks,
                            false,
                            0.1f
                        )
                ).Cast<Il2CppSystem.Object>();
            }
        }
    }

    public class OSS2UpTaskVFX : AModTask
    {
        public OSS2UpTaskVFX()
        {
        }

        public OSS2UpTaskVFX(Coord targetCoord)
        {
            Il2CppSystem.Collections.Generic.List<Coord> list = new();
            list.Add(targetCoord);
            SetArg(ArgKey.Coord, list);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            Coord targetCoord = taskInstance.GetArg<Il2CppSystem.Collections.Generic.List<Coord>>(ArgKey.Coord)[0];

            MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(targetCoord).transform.position));
        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject OSS2 = new GameObject("OSS2VFX");
            OSS2.transform.position = spawnPosition + new UnityEngine.Vector3(0f, -8f, 0f);
            OSS2.transform.localScale = new UnityEngine.Vector3(2.5f, 2.5f, 2.5f);
            OSS2.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);

            SpriteRenderer sr = OSS2.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 999;

            float secondsPerFrame = 0.025f;
            Sprite[] frames = OSS2VFXManager.GetOrLoadSprites();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null && sr != null)
                {
                    sr.sprite = frames[i];
                }

                yield return new UnityEngine.WaitForSeconds(secondsPerFrame);
            }

            if (OSS2 != null)
            {
                UnityEngine.Object.Destroy(OSS2);
            }
        }
    }

    public class OSS2TaskDown : AModTask
    {
        public OSS2TaskDown()
        {
        }

        public OSS2TaskDown(TargetValue targetValue)
        {
            SetArg(ArgKey.Coord, targetValue);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            int gridX = taskInstance.EncounterModel.GridModel.GridShape.x;
            Coord chosenCoords = taskInstance.GetArg<Il2CppSystem.Object>(ArgKey.Coord).Unbox<Coord>();

            for (int x = 0; x < gridX; x++)
            {
                Coord targetCoord = new Coord(x, chosenCoords.y);

                Il2CppCollections.List<ATask> animTasks = new();

                animTasks.Add(new OSS2DownTaskVFX(targetCoord));

                yield return taskInstance.TaskEngine.ProcessTask(
                    new AddCoordDelayedAttackTask(
                        new ArtilleryStrikeDelayedAttack(Direction.S),
                        taskInstance.GridModel.PlayerID.Cast<Il2CppSystem.Object>(),
                        targetCoord.BoxIl2CppObject(),
                        animTasks,
                        false,
                        0.1f
                    )
                ).Cast<Il2CppSystem.Object>();
            }
        }
    }

    public class OSS2DownTaskVFX : AModTask
    {
        public OSS2DownTaskVFX()
        {
        }

        public OSS2DownTaskVFX(Coord targetCoord)
        {
            Il2CppSystem.Collections.Generic.List<Coord> list = new();
            list.Add(targetCoord);
            SetArg(ArgKey.Coord, list);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            Coord targetCoord = taskInstance.GetArg<Il2CppSystem.Collections.Generic.List<Coord>>(ArgKey.Coord)[0];

            MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(targetCoord).transform.position));
        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject OSS2 = new GameObject("OSS2VFX");
            OSS2.transform.position = spawnPosition + new UnityEngine.Vector3(0f, 8f, 0f);
            OSS2.transform.localScale = new UnityEngine.Vector3(2.5f, 2.5f, 2.5f);
            OSS2.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 180f);

            SpriteRenderer sr = OSS2.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 999;

            float secondsPerFrame = 0.025f;
            Sprite[] frames = OSS2VFXManager.GetOrLoadSprites();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null && sr != null)
                {
                    sr.sprite = frames[i];
                }

                yield return new UnityEngine.WaitForSeconds(secondsPerFrame);
            }

            if (OSS2 != null)
            {
                UnityEngine.Object.Destroy(OSS2);
            }
        }
    }

    public static class OSS2VFXManager
    {
        public static Sprite[] OSS2Frames = new Sprite[14];

        public static Sprite[] GetOrLoadSprites()
        {
            if (OSS2Frames[0] != null)
                return OSS2Frames;

            Assembly assembly = Assembly.GetExecutingAssembly();

            for (int i = 0; i < 14; i++)
            {
                string resourceName = $"SV_UniqueCards.gridfx.Gunner.OSS2_{i:D2}.png";

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

                        OSS2Frames[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }
                }
            }
            return OSS2Frames;
        }
    }

}