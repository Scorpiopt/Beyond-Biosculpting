using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace BeyondBiosculpting
{
    public class CompProperties_BiosculpterPod_ImplantConversionCycle : CompProperties_BiosculpterPod_BaseCycle
	{
        public string implantName;
        public string cycleCompletedMessageKey;
        public CompProperties_BiosculpterPod_ImplantConversionCycle()
        {
            this.compClass = typeof(CompBiosculpterPod_ImplantConversionCycle);
        }
    }
	public class CompBiosculpterPod_ImplantConversionCycle : CompBiosculpterPod_Cycle
    {
        public new CompProperties_BiosculpterPod_ImplantConversionCycle Props => base.props as CompProperties_BiosculpterPod_ImplantConversionCycle;
        public override void CycleCompleted(Pawn pawn)
		{
            Dictionary<BodyPartRecord, List<HediffDef>> hediffsToAdd = new Dictionary<BodyPartRecord, List<HediffDef>>();
	    string bionicmodnames = (Props.implantName == "bionic") ? "synthetic" : "NothingToSeeHere";
            foreach (var implant in DefDatabase<RecipeDef>.AllDefs.Where(x => x.addsHediff != null && (x.addsHediff.defName.ToLower().Contains(Props.implantName) || x.addsHediff.defName.ToLower().Contains(bionicmodnames)) && !x.addsHediff.defName.ToLower().Contains("animal")))
            {
                if (implant.appliedOnFixedBodyParts != null)
                {
                    foreach (var partDef in implant.appliedOnFixedBodyParts)
                    {
                        foreach (var part in pawn.RaceProps.body.GetPartsWithDef(partDef))
                        {
                            if (!pawn.health.hediffSet.PartIsMissing(part))
                            {
                                var newHediff = HediffMaker.MakeHediff(implant.addsHediff, pawn);
                                pawn.health.AddHediff(newHediff, part);
                                if (newHediff is Hediff_Level withLevel)
                                {
                                    withLevel.SetLevelTo((int)withLevel.def.maxSeverity);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var newHediff = HediffMaker.MakeHediff(implant.addsHediff, pawn);
                    pawn.health.AddHediff(newHediff);
                    if (newHediff is Hediff_Level withLevel)
                    {
                        withLevel.SetLevelTo((int)withLevel.def.maxSeverity);
                    }
                }
            }
            Messages.Message(Props.cycleCompletedMessageKey.Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
		}
	}
}
