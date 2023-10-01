using System.Collections.Generic;
using System.Reflection;
using OpCodes = System.Reflection.Emit.OpCodes;

using AccessTools = HarmonyLib.AccessTools;
using CodeInstruction = HarmonyLib.CodeInstruction;
using HarmonyPatch = HarmonyLib.HarmonyPatch;

using MapComp_Skylights = Dubs_Skylight.MapComp_Skylights;

namespace com.malnormalulo.SkylightSunlight
{
	[HarmonyPatch(typeof(RimWorld.SanguophageUtility), "InSunlight")]
	internal static class Patch_SanguophageUtility_InSunlight
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			foreach (var instruction in instructions)
			{
				// Intercept and replace the call to Verse.RoofGrid.Roofed
				if (instruction.opcode == OpCodes.Callvirt
					&& ((MethodInfo)instruction.operand).DeclaringType == typeof(Verse.RoofGrid)
					&& ((MethodInfo)instruction.operand).Name == "Roofed")
				{
					var method = AccessTools.Method(
						typeof(Patch_SanguophageUtility_InSunlight),
						nameof(Patch_SanguophageUtility_InSunlight.RoofedWithoutSkylight)
					);
					// The RoofGrid and the IntVec3 are already on the evaluation stack.
					// Put the argument at index 1 (the Map) on the evaluation stack.
					yield return new CodeInstruction(OpCodes.Ldarg_1);
					// Call RoofedWithoutSky with all three values on the evaluation stack.
					yield return new CodeInstruction(OpCodes.Callvirt, method);
				}
				else
				{
					yield return instruction;
				}
			}
		}

		public static bool RoofedWithoutSkylight(this Verse.RoofGrid rg, Verse.IntVec3 cell, Verse.Map map)
		{
			var skylightComp = MapComp_Skylights.LightComps[map.uniqueID];
			var isSkylight = skylightComp.SkylightGrid[map.cellIndices.CellToIndex(cell)];
			return rg.Roofed(cell) && !isSkylight;
		}
	}
}
