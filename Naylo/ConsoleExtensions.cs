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
     ███▄    █  ▄▄▄     ▓██   ██▓ ██▓     ▒█████      ▄████▄   ██▓     ██▀███  
     ██ ▀█   █ ▒████▄    ▒██  ██▒▓██▒    ▒██▒  ██▒   ▒██▀ ▀█  ▓██▒    ▓██ ▒ ██▒
    ▓██  ▀█ ██▒▒██  ▀█▄   ▒██ ██░▒██░    ▒██░  ██▒   ▒▓█    ▄ ▒██░    ▓██ ░▄█ ▒
    ▓██▒  ▐▌██▒░██▄▄▄▄██  ░ ▐██▓░▒██░    ▒██   ██░   ▒▓▓▄ ▄██▒▒██░    ▒██▀▀█▄  
    ▒██░   ▓██░ ▓█   ▓██▒ ░ ██▒▓░░██████▒░ ████▓▒░   ▒ ▓███▀ ░░██████▒░██▓ ▒██▒
    ░ ▒░   ▒ ▒  ▒▒   ▓▒█░  ██▒▒▒ ░ ▒░▓  ░░ ▒░▒░▒░    ░ ░▒ ▒  ░░ ▒░▓  ░░ ▒▓ ░▒▓░
    ░ ░░   ░ ▒░  ▒   ▒▒ ░▓██ ░▒░ ░ ░ ▒  ░  ░ ▒ ▒░      ░  ▒   ░ ░ ▒  ░  ░▒ ░ ▒░
    ░   ░ ░   ░   ▒   ▒ ▒ ░░    ░ ░   ░ ░ ░ ▒     ░          ░ ░     ░░   ░ 
            ░       ░  ░░ ░         ░  ░    ░ ░     ░ ░          ░  ░   ░     
                        ░ ░                         ░ [v{Assembly.GetExecutingAssembly().GetName().Version}]";

#pragma warning disable IDE0060 // Remove unused parameter
        public static void ClearConsole(ConsoleColor color = ConsoleColor.Red)
        {
        }

        public static string AskForInput(string message, bool readKey = false)
#pragma warning restore IDE0060 // Remove unused parameter
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

            Label readKey = generator.DefineLabel();
            Label lpHd = generator.DefineLabel();

            LocalBuilder __ck_inf = generator.DeclareLocal(typeof(ConsoleKeyInfo));
            LocalBuilder __char = generator.DeclareLocal(typeof(char));

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                new(OpCodes.Ldloca_S, __ck_inf.LocalIndex),
                new(OpCodes.Initobj, typeof(ConsoleKeyInfo)),
                new(OpCodes.Ldloca_S, __char.LocalIndex),
                new(OpCodes.Initobj, typeof(char)),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, Method(typeof(Console), nameof(Console.Write), new[] { typeof(string) })),
                new(OpCodes.Ldarg_1),
                new(OpCodes.Brfalse_S, readKey),
                new CodeInstruction(OpCodes.Call, Method(typeof(Console), nameof(Console.ReadKey))).WithLabels(lpHd),
                new(OpCodes.Stloc_S, __ck_inf.LocalIndex),
                new(OpCodes.Ldloca_S, __ck_inf.LocalIndex),
                new(OpCodes.Callvirt, PropertyGetter(typeof(ConsoleKeyInfo), nameof(ConsoleKeyInfo.KeyChar))),
                new(OpCodes.Stloc_S, __char.LocalIndex),
                new(OpCodes.Ldloc_S, __char.LocalIndex),
                new(OpCodes.Call, Method(typeof(char), nameof(char.IsDigit), new[] { typeof(char) })),
                new(OpCodes.Brfalse_S, lpHd),
                new(OpCodes.Ldloca_S, __char.LocalIndex),
                new(OpCodes.Callvirt, Method(typeof(char), nameof(char.ToString), new Type[] { })),
                new(OpCodes.Ret),
                new CodeInstruction(OpCodes.Nop).WithLabels(readKey),
                new CodeInstruction(OpCodes.Call, Method(typeof(Console), nameof(Console.ReadLine))),
                new CodeInstruction(OpCodes.Ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];
        }

    }
}
