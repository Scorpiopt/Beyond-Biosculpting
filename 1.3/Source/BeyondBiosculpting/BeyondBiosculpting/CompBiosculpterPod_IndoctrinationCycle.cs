using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace BeyondBiosculpting
{
    public class CompProperties_BiosculpterPod_IndoctrinationCycle : CompProperties_BiosculpterPod_BaseCycle
    {
        public CompProperties_BiosculpterPod_IndoctrinationCycle()
        {
            this.compClass = typeof(CompBiosculpterPod_IndoctrinationCycle);
        }
    }
    public class CompBiosculpterPod_IndoctrinationCycle : CompBiosculpterPod_Cycle
    {
        public TraitEntry toRemove;
        public TraitEntry toAdd;
        public override void CycleCompleted(Pawn pawn)
        {
            pawn.story.traits.RemoveTrait(pawn.story.traits.allTraits.First(x => x.def == toRemove.traitDef && x.Degree == toRemove.degree));
            pawn.story.traits.GainTrait(new Trait(toAdd.traitDef, toAdd.degree));
            this.toAdd = null;
            this.toRemove = null;
            Messages.Message("BB.BiosculpterIndoctrinationCompletedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Deep.Look(ref toRemove, "toRemove");
            Scribe_Deep.Look(ref toAdd, "toAdd");
        }

        [HarmonyPatch(typeof(CompBiosculpterPod), nameof(CompBiosculpterPod.PrepareCycleJob))]
        public static class CompBiosculpterPod_PrepareCycleJob_Patch
        {
            public static void Postfix(Pawn hauler, Pawn biosculptee, CompBiosculpterPod_Cycle cycle, Job job)
            {

                if (cycle is CompBiosculpterPod_IndoctrinationCycle indoctrinationCycle)
                {
                    Find.WindowStack.Add(new Window_ChangePersonality(biosculptee, indoctrinationCycle));
                }
            }
        }
    }
}
