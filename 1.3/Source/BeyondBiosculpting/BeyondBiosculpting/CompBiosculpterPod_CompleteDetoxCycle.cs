using RimWorld;
using Verse;

namespace BeyondBiosculpting
{
    public class CompProperties_BiosculpterPod_CompleteDetoxCycle : CompProperties_BiosculpterPod_BaseCycle
    {
        public CompProperties_BiosculpterPod_CompleteDetoxCycle()
        {
            this.compClass = typeof(CompBiosculpterPod_CompleteDetoxCycle);
        }
    }
    public class CompBiosculpterPod_CompleteDetoxCycle : CompBiosculpterPod_Cycle
    {
        public override void CycleCompleted(Pawn pawn)
        {
            BioUtility.ClearAllAddictionsAndPoisons(pawn);
            Messages.Message("BB.BiosculpterDetoxCompletedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
        }
    }
}
