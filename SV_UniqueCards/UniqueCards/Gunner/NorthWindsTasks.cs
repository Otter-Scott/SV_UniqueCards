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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Experimental.TerrainAPI.TerrainUtility.TerrainMap;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.ReloadAttribute;
using Il2CppCollections = Il2CppSystem.Collections.Generic;

namespace SV_UniqueCards
{
    public class NorthWinds_1 : AModTask
    {
        public NorthWinds_1()
        {
        }

        public NorthWinds_1(TargetValue targetValue)
        {
            SetArg(ArgKey.Coord, targetValue);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {

            var enemies = taskInstance.EncounterModel.GridModel.GetAllEnemies(ignoreSort: false).ToMono();

            var sortedEnemies = enemies
                .Select(id => new{Coord = taskInstance.EncounterModel.GridModel.GetEntityCoord(id, includeDeathCoord: false)})
                .OrderBy(data => data.Coord.y)
                .ThenBy(data => data.Coord.x)
                .ToList();

            foreach (var enemy in sortedEnemies)
            {

                Coord southTile = new Coord(enemy.Coord.x, enemy.Coord.y - 1);

                if (taskInstance.EncounterModel.GridModel.IsCoordInGridRange(southTile) && taskInstance.EncounterModel.GridModel.IsCoordEmpty(southTile))
                    {
                        if (!taskInstance.IsPreviewModeView)
                        {
                            MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(enemy.Coord).transform.position));
                        }

                        yield return taskInstance.TaskEngine.ProcessTask(
                            new PushTileEffectTask(
                                Coord: enemy.Coord.BoxIl2CppObject(),
                                Direction: new Il2CppSystem.Int32 { m_value = (int)Direction.S }.BoxIl2CppObject(),
                                Distance: new Il2CppSystem.Int32 { m_value = 1 }.BoxIl2CppObject()
                        )).Cast<Il2CppSystem.Object>();
                }
            }
        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject Winds = new GameObject("WindsVFX");
            Winds.transform.position = spawnPosition + new UnityEngine.Vector3(0f, -0.5f, 0f);
            Winds.transform.localScale = new UnityEngine.Vector3(6f, 6f, 6f);
            Winds.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);

            SpriteRenderer sr = Winds.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 999;

            float secondsPerFrame = 0.05f;
            Sprite[] frames = NorthWindsVFXManager.GetOrLoadSprites();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null && sr != null)
                {
                    sr.sprite = frames[i];
                }

                yield return new UnityEngine.WaitForSeconds(secondsPerFrame);
            }

            if (Winds != null)
            {
                UnityEngine.Object.Destroy(Winds);
            }
        }
    }

    public class NorthWinds_2 : AModTask
    {
        public NorthWinds_2()
        {
        }

        public NorthWinds_2(TargetValue targetValue)
        {
            SetArg(ArgKey.Coord, targetValue);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {

            var enemies = taskInstance.EncounterModel.GridModel.GetAllEnemies(ignoreSort: false).ToMono();

            var sortedEnemies = enemies
                .Select(id => new
                {
                    EntityID = id,
                    Coord = taskInstance.EncounterModel.GridModel.GetEntityCoord(id, includeDeathCoord: false)
                })
                .OrderByDescending(data => data.Coord.y)
                .ThenBy(data => data.Coord.x)
                .ToList();

            foreach (var enemy in sortedEnemies)
            {
                if (!taskInstance.EncounterModel.GridModel.IsCoordEmpty(enemy.Coord))
                {
                    if (!taskInstance.IsPreviewModeView)
                    {
                        MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(enemy.Coord).transform.position));
                    }

                    yield return taskInstance.TaskEngine.ProcessTask(
                    new PushTileEffectTask(
                        Coord: enemy.Coord.BoxIl2CppObject(),
                        Direction: new Il2CppSystem.Int32 { m_value = (int)Direction.S }.BoxIl2CppObject(),
                        Distance: new Il2CppSystem.Int32 { m_value = 1 }.BoxIl2CppObject()
                    )).Cast<Il2CppSystem.Object>();
                }
            }
        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject Winds = new GameObject("WindsVFX");
            Winds.transform.position = spawnPosition + new UnityEngine.Vector3(0f, -0.5f, 0f);
            Winds.transform.localScale = new UnityEngine.Vector3(6f, 6f, 6f);
            Winds.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);

            SpriteRenderer sr = Winds.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 999;

            float secondsPerFrame = 0.05f;
            Sprite[] frames = NorthWindsVFXManager.GetOrLoadSprites();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null && sr != null)
                {
                    sr.sprite = frames[i];
                }

                yield return new UnityEngine.WaitForSeconds(secondsPerFrame);
            }

            if (Winds != null)
            {
                UnityEngine.Object.Destroy(Winds);
            }
        }
    }

    public class NorthWindsFreeze : AModTask
    {
        public NorthWindsFreeze()
        {
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            foreach (CardID cardID in taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Hand).ToMono().ToList())
            {
                CardModel card = taskInstance.EncounterModel.GetModelItem<CardModel>(cardID.ToID());
                if (!card.IsFrozen)
                {
                    yield return taskInstance.TaskEngine.ProcessTask(
                        new SetCardFrozenTask(cardID.BoxIl2CppObject(), true, true)
                    ).Cast<Il2CppSystem.Object>();
                }
            }


            yield return taskInstance.TaskEngine.ProcessTask(
                new EndTurnTask()
            ).Cast<Il2CppSystem.Object>();
        }
    }

    public class NorthWindsMisc : AModTask
    {
        public NorthWindsMisc()
        {
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            System.Collections.Generic.List<Il2CppSystem.ValueTuple<Trigger, ACondition>> startConditions = new()
            {
            new (Trigger.PostTask, new IsTypeCondition<StartTurnTask>(new RunningTaskValue()))
            };

            int heatToAdd = System.Math.Max(0, taskInstance.EncounterModel.Values[EncounterValue.MaxHeat] - taskInstance.EncounterModel.Values[EncounterValue.Heat]);

            System.Collections.Generic.List<ATask> startTasks = new()
            {
            new EncounterValueOperationTask(
                EncounterValue.MaxHeat,
                Il2CppStarVaders.Operation.Add,
                new Il2CppSystem.Int32 { m_value = heatToAdd }.BoxIl2CppObject(),
                false
            ),
            new NorthWindsEnd(heatToAdd)
            };

            yield return taskInstance.TaskEngine.ProcessTask(new AddTriggerEffectTask(
                new TriggerEffect(startConditions.ToILCPP(), startTasks.ToILCPP(), true)
            )).Cast<Il2CppSystem.Object>();

        }
    }

    public class NorthWindsEnd : AModTask
    {
        public NorthWindsEnd() 
        { 
        }

        public NorthWindsEnd(int heatToAdd)
        {
            Il2CppSystem.Collections.Generic.List<int> List = new();
            List.Add(heatToAdd);
            SetArg(ArgKey.Value, List);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView) 
                yield break;

            int heatToAdd = taskInstance.GetArg<Il2CppSystem.Collections.Generic.List<int>>(ArgKey.Value)[0];

            System.Collections.Generic.List<Il2CppSystem.ValueTuple<Trigger, ACondition>> endConditions = new()
            {
                new(Trigger.PreTask, new IsTypeCondition<EndTurnTask>(new RunningTaskValue()))
            };

            System.Collections.Generic.List<ATask> endTasks = new()
            {
                new EncounterValueOperationTask(
                    EncounterValue.MaxHeat,
                    Il2CppStarVaders.Operation.Subtract,
                    new Il2CppSystem.Int32 { m_value = heatToAdd }.BoxIl2CppObject(),
                    false
                )
            };

            yield return taskInstance.TaskEngine.ProcessTask(new AddTriggerEffectTask(
                new TriggerEffect(endConditions.ToILCPP(), endTasks.ToILCPP(), true)
            )).Cast<Il2CppSystem.Object>();

        }
    }

    public static class NorthWindsVFXManager
    {
        public static Sprite[] WindFrames = new Sprite[9];

        public static Sprite[] GetOrLoadSprites()
        {
            if (WindFrames[0] != null)
                return WindFrames;

            Assembly assembly = Assembly.GetExecutingAssembly();

            for (int i = 0; i < 9; i++)
            {
                string resourceName = $"SV_UniqueCards.gridfx.Gunner.Snowy_{i:D2}.png";

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

                        WindFrames[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }
                }
            }
            return WindFrames;
        }
    }

}