using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Naylo
{
    public class MathProcessor
    {
        public enum ComputationType
        {
            Add,
            Sub,
            Mul,
            Div,
            Mod,
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public float Compute(float left, float right, ComputationType computationType)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            return 0;
        }
    }

    [HarmonyPatch(typeof(MathProcessor), nameof(MathProcessor.Compute))]
    internal class MathProcessorPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new(instructions);
            newInstructions.Clear();

            Label consolidate = generator.DefineLabel();
            Label add = generator.DefineLabel();
            Label sub = generator.DefineLabel();
            Label mul = generator.DefineLabel();
            Label div = generator.DefineLabel();
            Label mod = generator.DefineLabel();
            Label ret = generator.DefineLabel();

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_1).WithLabels(consolidate),
                new(OpCodes.Ldarg_2),
                new(OpCodes.Ldarg_3),
                new(OpCodes.Ldc_I4_S, (int)MathProcessor.ComputationType.Add),
                new(OpCodes.Ceq),
                new(OpCodes.Brtrue_S, add),
                new(OpCodes.Ldarg_3),
                new(OpCodes.Ldc_I4_S, (int)MathProcessor.ComputationType.Sub),
                new(OpCodes.Ceq),
                new(OpCodes.Brtrue_S, sub),
                new(OpCodes.Ldarg_3),
                new(OpCodes.Ldc_I4_S, (int)MathProcessor.ComputationType.Mul),
                new(OpCodes.Ceq),
                new(OpCodes.Brtrue_S, mul),
                new(OpCodes.Ldarg_3),
                new(OpCodes.Ldc_I4_S, (int)MathProcessor.ComputationType.Div),
                new(OpCodes.Ceq),
                new(OpCodes.Brtrue_S, div),
                new(OpCodes.Ldarg_3),
                new(OpCodes.Ldc_I4_S, (int)MathProcessor.ComputationType.Mod),
                new(OpCodes.Ceq),
                new(OpCodes.Brtrue_S, mod),
                new CodeInstruction(OpCodes.Add).WithLabels(add),
                new(OpCodes.Br_S, ret),
                new CodeInstruction(OpCodes.Sub).WithLabels(sub),
                new(OpCodes.Br_S, ret),
                new CodeInstruction(OpCodes.Mul).WithLabels(mul),
                new(OpCodes.Br_S, ret),
                new CodeInstruction(OpCodes.Div).WithLabels(div),
                new(OpCodes.Br_S, ret),
                new CodeInstruction(OpCodes.Rem).WithLabels(mod),
                new CodeInstruction(OpCodes.Ret).WithLabels(ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];
        }
    }
}
