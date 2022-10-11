using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace BeyondBiosculpting
{
    [HotSwappableAttribute]
    public class Window_ChangePersonality : Window
    {
        public Pawn pawn;
        public CompBiosculpterPod_IndoctrinationCycle comp;
        public List<TraitEntry> traitsToSet = new List<TraitEntry>();
        public List<TraitEntry> alltraits = new List<TraitEntry>();
        public override Vector2 InitialSize => new Vector2(210, 170);
        public Window_ChangePersonality(Pawn pawn, CompBiosculpterPod_IndoctrinationCycle comp)
        {
            this.pawn = pawn;
            this.comp = comp;
            traitsToSet = new List<TraitEntry>();
            alltraits = new List<TraitEntry>();
            foreach (var trait in this.pawn.story.traits.allTraits)
            {
                traitsToSet.Add(new TraitEntry
                {
                    traitDef = trait.def,
                    degree = trait.Degree,
                });
            }
            foreach (var trait in DefDatabase<TraitDef>.AllDefs)
            {
                for (var i = 0; i < trait.degreeDatas.Count; i++)
                {
                    alltraits.Add(new TraitEntry
                    {
                        traitDef = trait,
                        degree = trait.degreeDatas[i].degree,
                    });
                }
            }
        }

        private Vector2 firstColumnPos;
        TraitEntry toRemove = null;
        TraitEntry toAdd = null;
        public override void DoWindowContents(Rect inRect)
        {
            firstColumnPos.x = 0;
            firstColumnPos.y = 0;
            var traitsTitle = new Rect(firstColumnPos.x, firstColumnPos.y, inRect.width, 32);
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.UpperCenter;
            Widgets.Label(traitsTitle, "BB.TraitReplacer".Translate());
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            firstColumnPos.y += 40;

            DoButton(ref firstColumnPos, toRemove?.GetLabel(pawn) ?? "BB.SelectTraitToRemove".Translate(), delegate
            {
                Find.WindowStack.Add(new Window_SelectItem<TraitEntry>(traitsToSet, delegate (TraitEntry selected)
                {
                    toRemove = selected;
                }, (TraitEntry x) => 0, delegate (TraitEntry x)
                {
                    return x.GetLabel(pawn);
                }));
            });
            DoButton(ref firstColumnPos, toAdd?.GetLabel(pawn) ?? "BB.SelectTraitToAdd".Translate(), delegate
            {
                Find.WindowStack.Add(new Window_SelectItem<TraitEntry>(alltraits.Where(x =>
                !traitsToSet.Except(toRemove).Any(y => x.traitDef == y.traitDef || x.traitDef.ConflictsWith(y.traitDef)) && x != toRemove).ToList(), delegate (TraitEntry selected)
                {
                    toAdd = selected;
                }, (TraitEntry x) => 0, delegate (TraitEntry x)
                {
                    return x.GetLabel(pawn);
                }));
            });

            var startBrainwash = new Rect(inRect.x, inRect.height - 32, 175, 32);
            bool active = toAdd != null && toRemove != null;
            GUI.color = active ? Color.white : Color.grey;
            if (Widgets.ButtonText(startBrainwash, "BB.StartIndoctrination".Translate(), active: active))
            {
                this.comp.toAdd = toAdd;
                this.comp.toRemove = toRemove;
                this.Close();
            }
            GUI.color = Color.white;
        }

        private static Rect DoButton(ref Vector2 pos, string label, Action action)
        {
            var buttonRect = new Rect(pos.x, pos.y, 175, 24);
            pos.y += 24;
            if (Widgets.ButtonText(buttonRect, label))
            {
                UI.UnfocusCurrentControl();
                action();
            }
            return buttonRect;
        }
    }
}
