using RimWorld;
using Verse;

namespace BeyondBiosculpting
{
    public class CompProperties_BiosculpterPod_BioRegenerationCycle : CompProperties_BiosculpterPod_BaseCycle
    {
        public CompProperties_BiosculpterPod_BioRegenerationCycle()
        {
            this.compClass = typeof(CompBiosculpterPod_BioRegenerationCycle);
        }
    }
    public class CompBiosculpterPod_BioRegenerationCycle : CompBiosculpterPod_Cycle
    {
        public override void CycleCompleted(Pawn pawn)
        {
            BioUtility.ClearAllAddictionsAndPoisons(pawn);
            BioUtility.ClearAllBadDiseasesAndRestoreParts(pawn);
            Messages.Message("BB.BiosculpterBioRegenerationCompletedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
        }
    }
}
