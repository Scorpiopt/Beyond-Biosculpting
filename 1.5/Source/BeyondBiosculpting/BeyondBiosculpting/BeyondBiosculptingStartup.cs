using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace BeyondBiosculpting
{
    [StaticConstructorOnStartup]
    public static class BeyondBiosculptingStartup
    {
        static BeyondBiosculptingStartup()
        {
            new Harmony("BeyondBiosculpting.Mod").PatchAll();
        }
    }

    [HarmonyPatch(typeof(CompBiosculpterPod), "CannotUseNowPawnCycleReason", new Type[] { typeof(Pawn), typeof(Pawn), typeof(CompBiosculpterPod_Cycle), typeof(bool) })]
    public static class CompBiosculpterPod_CannotUseNowPawnCycleReason_Patch
    {
        public static void Postfix(ref string __result, Pawn hauler, Pawn biosculptee, CompBiosculpterPod_Cycle cycle, bool checkIngredients = true)
        {
            if (cycle is CompBiosculpterPod_IndoctrinationCycle && __result is null)
            {
                if (biosculptee.story?.traits?.allTraits.NullOrEmpty() ?? false)
                {
                    __result = "BB.MustHaveOneTrait".Translate();
                }
            }
        }
    }
}
