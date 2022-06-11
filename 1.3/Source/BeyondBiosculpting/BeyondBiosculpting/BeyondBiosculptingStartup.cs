using HarmonyLib;
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
}
