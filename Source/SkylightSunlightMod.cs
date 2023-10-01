using System.Reflection;
using Verse;
using HarmonyLib;

namespace com.malnormalulo.SkylightSunlight
{
    [StaticConstructorOnStartup]
    public class SkylightSunlightMod
    {
        static SkylightSunlightMod()
        {
            var harmony = new Harmony("com.malnormalulo.SkylightSunlight");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("[Skylight Sunlight] Loaded");
        }
    }
}
