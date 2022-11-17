using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using static HarmonyLib.AccessTools;

namespace Naylo
{
    public class HUDManager
    {
        public static void Init()
        {
        }
    }

    [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.Init))]
    internal class HUDManagerPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new(instructions);
            newInstructions.Clear();

            LocalBuilder stringBuilder = generator.DeclareLocal(typeof(StringBuilder));

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                new(OpCodes.Newobj, GetDeclaredConstructors(typeof(StringBuilder), null)[0]),
                new(OpCodes.Stloc_S, stringBuilder.LocalIndex),
                new(OpCodes.Ldc_I4_S, (int)ConsoleColor.Red),
                new(OpCodes.Call, Method(typeof(ConsoleExtensions), nameof(ConsoleExtensions.ClearConsole))),
                new(OpCodes.Ldloc_S, stringBuilder.LocalIndex),
                new(OpCodes.Ldstr, "\nOperations"),
                new(OpCodes.Callvirt, Method(typeof(StringBuilder), nameof(StringBuilder.AppendLine), new[] { typeof(string) })),
                new(OpCodes.Pop),
                new(OpCodes.Ldloc_S, stringBuilder.LocalIndex),
                new(OpCodes.Ldstr, "[0] Add"),
                new(OpCodes.Callvirt, Method(typeof(StringBuilder), nameof(StringBuilder.AppendLine), new[] { typeof(string) })),
                new(OpCodes.Pop),
                new(OpCodes.Ldloc_S, stringBuilder.LocalIndex),
                new(OpCodes.Ldstr, "[1] Sub"),
                new(OpCodes.Callvirt, Method(typeof(StringBuilder), nameof(StringBuilder.AppendLine), new[] { typeof(string) })),
                new(OpCodes.Pop),
                new(OpCodes.Ldloc_S, stringBuilder.LocalIndex),
                new(OpCodes.Ldstr, "[2] Mul"),
                new(OpCodes.Callvirt, Method(typeof(StringBuilder), nameof(StringBuilder.AppendLine), new[] { typeof(string) })),
                new(OpCodes.Pop),
                new(OpCodes.Ldloc_S, stringBuilder.LocalIndex),
                new(OpCodes.Ldstr, "[3] Div"),
                new(OpCodes.Callvirt, Method(typeof(StringBuilder), nameof(StringBuilder.AppendLine), new[] { typeof(string) })),
                new(OpCodes.Pop),
                new(OpCodes.Ldloc_S, stringBuilder.LocalIndex),
                new(OpCodes.Ldstr, "[4] Mod"),
                new(OpCodes.Callvirt, Method(typeof(StringBuilder), nameof(StringBuilder.AppendLine), new[] { typeof(string) })),
                new(OpCodes.Pop),
                new(OpCodes.Ldloc_S, stringBuilder.LocalIndex),
                new(OpCodes.Callvirt, Method(typeof(StringBuilder), nameof(StringBuilder.ToString), new Type[] { })),
                new(OpCodes.Call, Method(typeof(Console), nameof(Console.WriteLine), new[] { typeof(string) })),
                new(OpCodes.Ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];
        }
    }
}
