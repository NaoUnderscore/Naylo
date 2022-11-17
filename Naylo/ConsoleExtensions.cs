using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace Naylo
{
    public static class ConsoleExtensions
    {
        public static string Signature => @$"
██╗██╗          ██████╗ █████╗ ██╗      ██████╗██╗   ██╗██╗      █████╗ ████████╗ ██████╗ ██████╗ 
██║██║         ██╔════╝██╔══██╗██║     ██╔════╝██║   ██║██║     ██╔══██╗╚══██╔══╝██╔═══██╗██╔══██╗
██║██║         ██║     ███████║██║     ██║     ██║   ██║██║     ███████║   ██║   ██║   ██║██████╔╝
██║██║         ██║     ██╔══██║██║     ██║     ██║   ██║██║     ██╔══██║   ██║   ██║   ██║██╔══██╗
██║███████╗    ╚██████╗██║  ██║███████╗╚██████╗╚██████╔╝███████╗██║  ██║   ██║   ╚██████╔╝██║  ██║
╚═╝╚══════╝     ╚═════╝╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝ v{Assembly.GetExecutingAssembly().GetName().Version}";

        public static void ClearConsole(ConsoleColor color = ConsoleColor.Red)
        {
        }

        public static string AskForInput(string message)
        {
            return null;
        }
    }

    [HarmonyPatch(typeof(ConsoleExtensions))]
    public static class ConsoleExtensionsPatches
    {
        [HarmonyPatch(nameof(ConsoleExtensions.ClearConsole))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ClearConsole__Body(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new(instructions);
            newInstructions.Clear();

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                new(OpCodes.Call, Method(typeof(Console), nameof(Console.Clear))),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, PropertySetter(typeof(Console), nameof(Console.ForegroundColor))),
                new(OpCodes.Call, PropertyGetter(typeof(ConsoleExtensions), nameof(ConsoleExtensions.Signature))),
                new(OpCodes.Call, Method(typeof(Console), nameof(Console.WriteLine), new[] { typeof(string) })),
                new(OpCodes.Call, Method(typeof(Console), nameof(Console.WriteLine), new Type[] { })),
                new(OpCodes.Ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];
        }

        [HarmonyPatch(nameof(ConsoleExtensions.AskForInput))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AskForInput__Body(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new(instructions);
            newInstructions.Clear();

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, Method(typeof(Console), nameof(Console.Write), new[] { typeof(string) })),
                new(OpCodes.Call, Method(typeof(Console), nameof(Console.ReadLine))),
                new(OpCodes.Ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];
        }

    }
}
