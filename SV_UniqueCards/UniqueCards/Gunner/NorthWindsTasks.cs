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
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.ReloadAttribute;
using Il2CppCollections = Il2CppSystem.Collections.Generic;

namespace SV_UniqueCards
{
    public class NorthWindsPull : AModTask
    {
        public NorthWindsPull()
        {
        }

        public NorthWindsPull(TargetValue targetValue)
        {
            SetArg(ArgKey.Coord, targetValue);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {

            var grid = taskInstance.EncounterModel.GridModel;

            var enemies = grid.GetAllEnemies(ignoreSort: false).ToMono();

            var sortedEnemies = enemies
                .Select(id => new
                {
                    EntityID = id,
                    Coord = grid.GetEntityCoord(id, includeDeathCoord: false)
                })
                .OrderBy(data => data.Coord.y)
                .ThenBy(data => data.Coord.x)
                .ToList();

            foreach (var enemy in sortedEnemies)
            {

                var targetCoords = grid.GetCoordsInDirection(enemy.Coord, Direction.S, distance: 1, inclusive: false).ToMono(); 

                if (targetCoords.Count > 0)
                {
                    var southTile = targetCoords[0];

                    if (grid.IsCoordEmpty(southTile))
                    {
                        if (!taskInstance.IsPreviewModeView)
                        {
                            MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(enemy.Coord).transform.position));
                        }

                        var pullDownTask = new PushTileEffectTask(
                            Coord: enemy.Coord.BoxIl2CppObject(),
                            Direction: new Il2CppSystem.Int32 { m_value = (int)Direction.S }.BoxIl2CppObject(),
                            Distance: new Il2CppSystem.Int32 { m_value = 1 }.BoxIl2CppObject()
                        );

                        yield return taskInstance.TaskEngine.ProcessTask(pullDownTask).Cast<Il2CppSystem.Object>();
                    }
                }
            }
        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject Winds = new GameObject("WindsVFX");
            Winds.transform.position = spawnPosition + new UnityEngine.Vector3(0f, -0.5f, 0f);
            Winds.transform.localScale = new UnityEngine.Vector3(5f, 5f, 5f);
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

    public class NorthWindsPush : AModTask
    {
        public NorthWindsPush()
        {
        }

        public NorthWindsPush(TargetValue targetValue)
        {
            SetArg(ArgKey.Coord, targetValue);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            var grid = taskInstance.EncounterModel.GridModel;

            var enemies = grid.GetAllEnemies(ignoreSort: false).ToMono();

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
                if (!taskInstance.IsPreviewModeView)
                {
                    MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(enemy.Coord).transform.position));
                }

                var pushDownTask = new PushTileEffectTask(
                    Coord: enemy.Coord.BoxIl2CppObject(),
                    Direction: new Il2CppSystem.Int32 { m_value = (int)Direction.S }.BoxIl2CppObject(),
                    Distance: new Il2CppSystem.Int32 { m_value = 1 }.BoxIl2CppObject()
                );

                yield return taskInstance.TaskEngine.ProcessTask(pushDownTask).Cast<Il2CppSystem.Object>();
            }
        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject Winds = new GameObject("WindsVFX");
            Winds.transform.position = spawnPosition + new UnityEngine.Vector3(0f, -0.5f, 0f);
            Winds.transform.localScale = new UnityEngine.Vector3(5f, 5f, 5f);
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

            foreach (var cardID in taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Hand).ToMono().ToList())
            {
                CardModel card = taskInstance.EncounterModel.GetModelItem<CardModel>(cardID.ToID());
                if (card.IsFrozen)
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

            yield return taskInstance.TaskEngine.ProcessTask(new DrawTopDrawPileTask()).Cast<Il2CppSystem.Object>();
            yield return taskInstance.TaskEngine.ProcessTask(new DrawTopDrawPileTask()).Cast<Il2CppSystem.Object>();

            System.Collections.Generic.List<Il2CppSystem.ValueTuple<Trigger, ACondition>> startConditions = new()
            {
            new (Trigger.PostTask, new IsTypeCondition<StartTurnTask>(new RunningTaskValue()))
            };

            System.Collections.Generic.List<ATask> startTasks = new()
            {
            new EncounterValueOperationTask(
                EncounterValue.MaxHeat,
                Il2CppStarVaders.Operation.Add,
                new Il2CppSystem.Int32 { m_value = 1 }.BoxIl2CppObject(),
                false
            ),
            new NorthWindsEnd()
            };

            TriggerEffect startTrigger = new TriggerEffect(startConditions.ToILCPP(), startTasks.ToILCPP(), true);

            yield return taskInstance.TaskEngine.ProcessTask(new AddTriggerEffectTask(
                startTrigger
            )).Cast<Il2CppSystem.Object>();

        }
    }

    public class NorthWindsEnd : AModTask
    {
        public NorthWindsEnd() { }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView) 
                yield break;

            System.Collections.Generic.List<Il2CppSystem.ValueTuple<Trigger, ACondition>> endConditions = new()
            {
                new(Trigger.PreTask, new IsTypeCondition<EndTurnTask>(new RunningTaskValue()))
            };

            System.Collections.Generic.List<ATask> endTasks = new()
            {
                new EncounterValueOperationTask(
                    EncounterValue.MaxHeat,
                    Il2CppStarVaders.Operation.Subtract,
                    new Il2CppSystem.Int32 { m_value = 1 }.BoxIl2CppObject(),
                    false
                )
            };

            TriggerEffect endTrigger = new TriggerEffect(endConditions.ToILCPP(), endTasks.ToILCPP(), true);

            yield return taskInstance.TaskEngine.ProcessTask(new AddTriggerEffectTask(
                endTrigger
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
                string resourceName = $"SV_UniqueCards.gridfx.Snowy_{i:D2}.png";

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