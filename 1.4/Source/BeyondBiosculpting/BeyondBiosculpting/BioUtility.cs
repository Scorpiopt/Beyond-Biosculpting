using RimWorld;
using Verse;

namespace BeyondBiosculpting
{
    public static class BioUtility
    {
        public static void ClearAllBadDiseasesAndRestoreParts(Pawn pawn)
        {
            var hediffs = pawn.health.hediffSet.hediffs;
            for (var i = hediffs.Count - 1; i >= 0; i--)
            {
                var hediff = hediffs[i];
                if (hediff is Hediff_MissingPart hediff_MissingPart)
                {
                    var part = hediff_MissingPart.Part;
                    pawn.health.RemoveHediff(hediff);
                    pawn.health.RestorePart(part);
                }
                else if (hediff.def.isBad)
                {
                    pawn.health.RemoveHediff(hediff);
                }
            }
        }
        public static void ClearAllAddictionsAndPoisons(Pawn pawn)
        {
            var hediffs = pawn.health.hediffSet.hediffs;
            for (var i = hediffs.Count - 1; i >= 0; i--)
            {
                var hediff = hediffs[i];
                if (hediff is Hediff_Addiction hediff_Addiction)
                {
                    TryRemoveHediffDef(pawn, hediff_Addiction.Chemical.toleranceHediff);
                    TryRemoveHediff(pawn, hediff_Addiction);
                }
                else if (hediff is Hediff_High)
                {
                    TryRemoveHediff(pawn, hediff);
                }
            }

            TryRemoveHediffDef(pawn, HediffDefOf.FoodPoisoning);
            TryRemoveHediffDef(pawn, HediffDefOf.ToxicBuildup);
            TryRemoveHediffDef(pawn, HediffDefOf.BiosculptingSickness);
            TryRemoveHediffDef(pawn, HediffDefOf.CryptosleepSickness);
            TryRemoveHediffDef(pawn, HediffDefOf.ResurrectionSickness);

        }

        public static void TryRemoveHediffDef(Pawn pawn, HediffDef hediffDef)
        {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
            if (hediff != null)
            {
                pawn.health.RemoveHediff(hediff);
            }
        }

        public static void TryRemoveHediff(Pawn pawn, Hediff hediff)
        {
            if (pawn.health.hediffSet.hediffs.Contains(hediff))
            {
                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}
