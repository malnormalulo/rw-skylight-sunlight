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
			// MethodInfo roofedMethod = AccessTools.Method(typeof(RoofGrid), nameof(RoofGrid.Roofed));
			foreach (var instruction in instructions)
			{
				Log.Message(instruction.ToString());
				if (instruction.opcode == OpCodes.Callvirt
					&& ((MethodInfo)instruction.operand).DeclaringType == typeof(RoofGrid)
					&& ((MethodInfo)instruction.operand).Name == "Roofed")
				{
					Log.Message("Found Roofed method call");
					var method = AccessTools.Method(typeof(Patch_SanguophageUtility_InSunlight), nameof(Patch_SanguophageUtility_InSunlight.RoofedWithoutSkylight));
					yield return new CodeInstruction(OpCodes.Callvirt, method);
				}
				else
				{
					yield return instruction;
				}
			}
		}

		public static bool RoofedWithoutSkylight(this RoofGrid rg, IntVec3 cell)
		{
			Log.Message("Currently a passthrough");
			return rg.Roofed(cell);
		}
	}
}
