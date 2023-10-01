using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Dubs_Skylight;
using HarmonyLib;
using RimWorld;
using Verse;

namespace com.malnormalulo.SkylightSunlight
{
	[HarmonyPatch(typeof(SanguophageUtility), "InSunlight")]
	internal static class Patch_SanguophageUtility_InSunlight
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			foreach (var instruction in instructions)
			{
				if (instruction.opcode == OpCodes.Callvirt
					&& ((MethodInfo)instruction.operand).DeclaringType == typeof(RoofGrid)
					&& ((MethodInfo)instruction.operand).Name == "Roofed")
				{
					var method = AccessTools.Method(
						typeof(Patch_SanguophageUtility_InSunlight),
						nameof(Patch_SanguophageUtility_InSunlight.RoofedWithoutSkylight)
					);
					yield return new CodeInstruction(OpCodes.Ldarg_1);
					yield return new CodeInstruction(OpCodes.Callvirt, method);
				}
				else
				{
					yield return instruction;
				}
			}
		}

		public static bool RoofedWithoutSkylight(this RoofGrid rg, IntVec3 cell, Map map)
		{
			var skylightComp = MapComp_Skylights.LightComps[map.uniqueID];
			var isSkylight = skylightComp.SkylightGrid[map.cellIndices.CellToIndex(cell)];
			return rg.Roofed(cell) && !isSkylight;
		}
	}
}
