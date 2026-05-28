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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.ReloadAttribute;
using Il2CppCollections = Il2CppSystem.Collections.Generic;

namespace SV_UniqueCards
{
    public class FusionMain : AModTask
    {
        public FusionMain() 
        { 
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView) yield break;

            if (taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Hand).ToMono().Count > 1)
            {
                var selection1 = new Selection(
                    new AndCondition(
                        new IsTypeCondition<CardID>(new TargetValue()),
                        new CardInHandButNotBeingPlayedCondition(new TargetValue())
                    ),
                    selectionDescriptor: SelectionDescriptor.CardToPurge
                );

                var Task1 = new FusionCheck(new TargetValue()).Convert();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new SelectionTask(
                        Selections: new System.Collections.Generic.List<Selection> { selection1 }.ToILCPP(),
                        TaskList: new System.Collections.Generic.List<ATask> { Task1 }.ToILCPP(),
                        PreSelectionPreviewTaskList: new System.Collections.Generic.List<ATask>().ToILCPP(),
                        isFullyCancellable: false
                    )
                ).Cast<Il2CppSystem.Object>();

                var selection2 = new Selection(
                    new IsTypeCondition<Coord>(new TargetValue()),
                    selectionDescriptor: SelectionDescriptor.Tile
                );

                var Task2 = new FusionStrike(new TargetValue()).Convert();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new SelectionTask(
                        Selections: new System.Collections.Generic.List<Selection> { selection2 }.ToILCPP(),
                        TaskList: new System.Collections.Generic.List<ATask> { Task2 }.ToILCPP(),
                        PreSelectionPreviewTaskList: new System.Collections.Generic.List<ATask>().ToILCPP(),
                        isFullyCancellable: false
                    )
                ).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new CreateCardTask(
                        CardName: new Il2CppSystem.Int32 { m_value = (int)ModContentManager.GetModCardName<Fusion>() }.BoxIl2CppObject(),
                        Pile: Pile.Discard,
                        rarity: new()
                    )
                ).Cast<Il2CppSystem.Object>();
            }
        }
    }

    public class FusionMainComp : AModTask
    {
        public FusionMainComp() 
        { 
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView) yield break;

            if (taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Hand).ToMono().Count > 2)
            {
                var selection1 = new Selection(
                    new AndCondition(
                        new IsTypeCondition<CardID>(new TargetValue()),
                        new CardInHandButNotBeingPlayedCondition(new TargetValue())
                    ),
                    selectionDescriptor: SelectionDescriptor.CardToPurge
                );

                var Task1 = new FusionCheck(new TargetValue()).Convert();

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

                var selection2 = new Selection(
                    new IsTypeCondition<Coord>(new TargetValue()),
                    selectionDescriptor: SelectionDescriptor.Tile
                );

                var Task2 = new FusionStrike(new TargetValue()).Convert();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new SelectionTask(
                        Selections: new System.Collections.Generic.List<Selection> { selection2 }.ToILCPP(),
                        TaskList: new System.Collections.Generic.List<ATask> { Task2 }.ToILCPP(),
                        PreSelectionPreviewTaskList: new System.Collections.Generic.List<ATask>().ToILCPP(),
                        isFullyCancellable: false
                    )
                ).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new CreateCardTask(
                        CardName: new Il2CppSystem.Int32 { m_value = (int)ModContentManager.GetModCardName<Fusion>() }.BoxIl2CppObject(),
                        Pile: Pile.Discard,
                        rarity: new()
                    )
                ).Cast<Il2CppSystem.Object>();
            }
        }
    }

    public class FusionCheck : AModTask
    {
        public FusionCheck() 
        { 
        }

        public FusionCheck(TargetValue targetValue)
        {
            SetArg(ArgKey.CardID, targetValue);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            Il2CppSystem.Object selectedCard = taskInstance.GetArg<Il2CppSystem.Object>(ArgKey.CardID);

            var cardModel = taskInstance.EncounterModel.GetModelItem<CardModel>(selectedCard.Unbox<CardID>().ToID());

            if (cardModel.CardName == ModContentManager.GetModCardName<Fusion>())
            {
                yield return taskInstance.TaskEngine.ProcessTask(
                    new QuarkTask(selectedCard)
                ).Cast<Il2CppSystem.Object>();

                yield return taskInstance.TaskEngine.ProcessTask(
                    new CreateCardTask(
                        CardName: new Il2CppSystem.Int32 { m_value = (int)ModContentManager.GetModCardName<Fusion>() }.BoxIl2CppObject(),
                        Pile: Pile.Discard,
                        rarity: new()
                    )
                ).Cast<Il2CppSystem.Object>();
            }
                yield return taskInstance.TaskEngine.ProcessTask(
                    new MoveCardTask(
                        CardID: selectedCard,
                        Pile: Pile.Discard,
                        speed: new Il2CppSystem.Nullable<float>()
                    )
                ).Cast<Il2CppSystem.Object>();
        }
    }

    public class FusionStrike : AModTask
    {
        public FusionStrike()
        {
        }

        public FusionStrike(TargetValue targetValue)
        {
            SetArg(ArgKey.Coord, targetValue);
        }

        public override System.Collections.IEnumerator Execute(ATask taskInstance)
        {
            if (taskInstance.IsPreviewModeView)
                yield break;

            var chosenCoords = taskInstance.GetArg<Il2CppSystem.Object>(ArgKey.Coord).Unbox<Coord>();

            MelonLoader.MelonCoroutines.Start(PlayStandaloneVFX(taskInstance.GridView.GetTileView(chosenCoords).transform.position));

            Coord[] strikeCoords = new Coord[]
            {
            new Coord(chosenCoords.x, chosenCoords.y + 1),
            new Coord(chosenCoords.x, chosenCoords.y - 1),
            new Coord(chosenCoords.x - 1, chosenCoords.y),
            new Coord(chosenCoords.x + 1, chosenCoords.y)
            };

            foreach (Coord target in strikeCoords)
            {
                if (taskInstance.EncounterModel.GridModel.IsCoordInGridRange(target))
                    {
                    yield return taskInstance.TaskEngine.ProcessTask(
                        new StrikeTileEffectTask(
                            target.BoxIl2CppObject(),
                            CardName.Jam,
                            type: GridFX.Strike
                        )
                    ).Cast<Il2CppSystem.Object>();
                }
            }

            

        }

        private static System.Collections.IEnumerator PlayStandaloneVFX(UnityEngine.Vector3 spawnPosition)
        {
            GameObject Fusion = new GameObject("FusionVFX");
            Fusion.transform.position = spawnPosition + new UnityEngine.Vector3(0f, 0f, 0f);
            Fusion.transform.localScale = new UnityEngine.Vector3(8f, 8f, 8f);
            Fusion.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);

            SpriteRenderer sr = Fusion.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 999;

            float secondsPerFrame = 0.05f;
            Sprite[] frames = FusionVFXManager.GetOrLoadSprites();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null && sr != null)
                {
                    sr.sprite = frames[i];
                }

                yield return new UnityEngine.WaitForSeconds(secondsPerFrame);
            }

            if (Fusion != null)
            {
                UnityEngine.Object.Destroy(Fusion);
            }
        }
    }

    public static class FusionVFXManager
    {
        public static Sprite[] FusionFrames = new Sprite[10];

        public static Sprite[] GetOrLoadSprites()
        {
            if (FusionFrames[0] != null)
                return FusionFrames;

            Assembly assembly = Assembly.GetExecutingAssembly();

            for (int i = 0; i < 10; i++)
            {
                string resourceName = $"Please_work_SV.gridfx.Fusion_{i:D2}.png";

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

                        FusionFrames[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }
                }
            }
            return FusionFrames;
        }
    }
}