using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;
using System.Linq;

namespace Naylo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Harmony HarmonyInstance = new("nayloil.1337");
            HarmonyInstance.PatchAll();

            Launch();
        }

        public static void Launch()
        {
        }
    }

    [HarmonyPatch(typeof(Program), nameof(Program.Launch))]
    internal static class ProgramPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new(instructions);
            newInstructions.Clear();

            Label ret = generator.DefineLabel();
            Label lpHd = generator.DefineLabel();
            Label iter = generator.DefineLabel();
            Label leftMark = generator.DefineLabel();
            Label rightMark = generator.DefineLabel();
            Label optionLpHd = generator.DefineLabel();

            LocalBuilder processor = generator.DeclareLocal(typeof(MathProcessor));
            LocalBuilder computationType = generator.DeclareLocal(typeof(int));
            LocalBuilder option = generator.DeclareLocal(typeof(int));
            LocalBuilder left = generator.DeclareLocal(typeof(float));
            LocalBuilder right = generator.DeclareLocal(typeof(float));
            LocalBuilder result = generator.DeclareLocal(typeof(float));
            LocalBuilder consoleKey = generator.DeclareLocal(typeof(ConsoleKeyInfo));

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                new(OpCodes.Ldc_I4_S, (int)ConsoleColor.Red),
                new(OpCodes.Call, Method(typeof(ConsoleExtensions), nameof(ConsoleExtensions.ClearConsole))),
                new(OpCodes.Call, Method(typeof(HUDManager), nameof(HUDManager.Init))),
                new(OpCodes.Newobj, GetDeclaredConstructors(typeof(MathProcessor))[0]),
                new(OpCodes.Stloc_S, processor.LocalIndex),
                new(OpCodes.Ldc_I4_0),
                new(OpCodes.Dup),
                new(OpCodes.Dup),
                new(OpCodes.Dup),
                new(OpCodes.Dup),
                new(OpCodes.Stloc_S, left.LocalIndex),
                new(OpCodes.Stloc_S, right.LocalIndex),
                new(OpCodes.Stloc_S, computationType.LocalIndex),
                new(OpCodes.Stloc_S, option.LocalIndex),
                new(OpCodes.Stloc_S, result.LocalIndex),
                new(OpCodes.Br_S, iter),
                new CodeInstruction(OpCodes.Nop).WithLabels(lpHd), 
                new CodeInstruction(OpCodes.Nop).WithLabels(optionLpHd),
                new(OpCodes.Ldstr, "Option: "),
                new(OpCodes.Call, Method(typeof(ConsoleExtensions), nameof(ConsoleExtensions.AskForInput))),
                new (OpCodes.Ldloca_S, option.LocalIndex),
                new(OpCodes.Call, GetDeclaredMethods(typeof(int)).FirstOrDefault(m => m.Name == "TryParse" && m.GetParameters().Count() < 3)),
                new(OpCodes.Brfalse_S, optionLpHd),
                new(OpCodes.Ldloc_S, option.LocalIndex),
                new(OpCodes.Ldc_I4_4),
                new(OpCodes.Bgt_S, optionLpHd),
                new(OpCodes.Ldc_I4_0),
                new(OpCodes.Ldloc_S, option.LocalIndex),
                new(OpCodes.Bgt_S, optionLpHd),
                new(OpCodes.Ldc_I4_S, (int)ConsoleColor.Red),
                new(OpCodes.Call, Method(typeof(ConsoleExtensions), nameof(ConsoleExtensions.ClearConsole))),
                new(OpCodes.Ldloc_S, option.LocalIndex),
                new(OpCodes.Stloc_S, computationType.LocalIndex),
                new CodeInstruction(OpCodes.Ldstr, "Left: ").WithLabels(leftMark),
                new(OpCodes.Call, Method(typeof(ConsoleExtensions), nameof(ConsoleExtensions.AskForInput))),
                new(OpCodes.Ldloca_S, left.LocalIndex),
                new(OpCodes.Call, GetDeclaredMethods(typeof(float)).FirstOrDefault(m => m.Name == "TryParse" && m.GetParameters().Count() < 3)),
                new(OpCodes.Brfalse_S, leftMark),
                new CodeInstruction(OpCodes.Ldstr, "Right: ").WithLabels(rightMark),
                new(OpCodes.Call, Method(typeof(ConsoleExtensions), nameof(ConsoleExtensions.AskForInput))),
                new(OpCodes.Ldloca_S, right.LocalIndex),
                new(OpCodes.Call, GetDeclaredMethods(typeof(float)).FirstOrDefault(m => m.Name == "TryParse" && m.GetParameters().Count() < 3)),
                new(OpCodes.Brfalse_S, rightMark),
                new(OpCodes.Ldc_I4_S, (int)ConsoleColor.Yellow),
                new(OpCodes.Call, PropertySetter(typeof(Console), nameof(Console.ForegroundColor))),
                new(OpCodes.Ldloc_S, processor.LocalIndex),
                new(OpCodes.Ldloc_S, left.LocalIndex),
                new(OpCodes.Ldloc_S, right.LocalIndex),
                new(OpCodes.Ldloc_S, computationType.LocalIndex),
                new(OpCodes.Callvirt, Method(typeof(MathProcessor), nameof(MathProcessor.Compute))),
                new(OpCodes.Stloc_S, result.LocalIndex),
                new(OpCodes.Ldstr, "Result: "),
                new(OpCodes.Ldloca_S, result.LocalIndex),
                new(OpCodes.Callvirt, Method(typeof(float), nameof(float.ToString), new Type[] { })),
                new(OpCodes.Call, Method(typeof(string), nameof(string.Concat), new[] { typeof(string), typeof(object) })),
                new(OpCodes.Call, Method(typeof(Console), nameof(Console.WriteLine), new[] { typeof(string) })),
                new(OpCodes.Ldc_I4_S, (int)ConsoleColor.Red),
                new(OpCodes.Call, PropertySetter(typeof(Console), nameof(Console.ForegroundColor))),
                new(OpCodes.Ldstr, "[INFO] Press ENTER to continue; otherwise, press any key to exit."),
                new(OpCodes.Call, Method(typeof(Console), nameof(Console.WriteLine), new[] { typeof(string) })),
                new(OpCodes.Call, Method(typeof(Console), nameof(Console.ReadKey))),
                new(OpCodes.Stloc_S, consoleKey.LocalIndex),
                new(OpCodes.Ldloca_S, consoleKey.LocalIndex),
                new(OpCodes.Callvirt, PropertyGetter(typeof(ConsoleKeyInfo), nameof(ConsoleKeyInfo.Key))),
                new(OpCodes.Ldc_I4_S, (int)ConsoleKey.Enter),
                new(OpCodes.Ceq),
                new(OpCodes.Brfalse_S, ret),
                new CodeInstruction(OpCodes.Br_S, lpHd).WithLabels(iter),
                new CodeInstruction(OpCodes.Ret).WithLabels(ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];
        }
    }
}
