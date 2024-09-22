using Mafi;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Ports.Io;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FFU_Industrial_Lib {
    public static partial class FFU_ILib {
        public static void SetMachineLayout(MachineProto.ID refMachineID, string[] strLayout, Dictionary<char, char[]> charMap = null, EntityLayoutParams layoutParams = null) {
            if (_pReg == null) { ModLog.Warning($"SetMachineLayout: the ProtoRegistrator is not referenced!"); return; };
            MachineProto refMachine = MachineRef(refMachineID);
            if (refMachine == null) { ModLog.Warning($"SetMachineLayout: 'refMachine' is undefined!"); return; }
            if (strLayout == null) { ModLog.Warning($"SetMachineLayout: 'strLayout' is undefined!"); return; }
            EntityLayout newLayout = layoutParams != null ?
                _pReg.LayoutParser.ParseLayoutOrThrow(layoutParams, strLayout) :
                _pReg.LayoutParser.ParseLayoutOrThrow(strLayout);
            TypeInfo typeLayout = typeof(LayoutEntityProto).GetTypeInfo();
            FieldInfo fieldLayout = typeLayout.GetDeclaredField("<Layout>k__BackingField");
            if (fieldLayout != null) {
                ModLog.Info($"{refMachine.Id} layout modified.");
                fieldLayout.SetValue(refMachine, newLayout);
                FieldInfo fieldPortsIn = typeof(LayoutEntityProto).GetField("InputPorts", BindingFlags.Instance | BindingFlags.Public);
                FieldInfo fieldPortsOut = typeof(LayoutEntityProto).GetField("OutputPorts", BindingFlags.Instance | BindingFlags.Public);
                ImmutableArray<IoPortTemplate> updatedIn = refMachine.Layout.Ports.Where((IoPortTemplate pt) => pt.Type == IoPortType.Input).ToImmutableArray();
                ImmutableArray<IoPortTemplate> updatedOut = refMachine.Layout.Ports.Where((IoPortTemplate pt) => pt.Type == IoPortType.Output).ToImmutableArray();
                fieldPortsIn.SetValue(refMachine, updatedIn);
                fieldPortsOut.SetValue(refMachine, updatedOut);
                if (charMap != null) SyncMachineRecipeMap(refMachine, charMap);
                SyncProtoMod(refMachine);
            }
        }

        private static void SyncMachineRecipeMap(MachineProto refMachine, Dictionary<char, char[]> charMap) {
            ModLog.Info($"{refMachine.Id} port map modified.");
            foreach (RecipeProto refRecipe in refMachine.Recipes) {
                foreach (RecipeInput refInput in refRecipe.AllInputs) {
                    char[] inMap = null;
                    foreach (IoPortTemplate portIn in refInput.Ports) {
                        if (charMap.ContainsKey(portIn.Name)) {
                            inMap = charMap[portIn.Name];
                            break;
                        }
                    }
                    if (inMap != null) {
                        ImmutableArray<IoPortTemplate> syncIn = refMachine.InputPorts.Where((IoPortTemplate pt) => inMap.Contains(pt.Name)).ToImmutableArray();
                        FieldInfo fieldInputIn = typeof(RecipeProduct).GetField("Ports", BindingFlags.Instance | BindingFlags.Public);
                        fieldInputIn.SetValue(refInput, syncIn);
                    }
                }
                foreach (RecipeOutput refOutput in refRecipe.AllOutputs) {
                    char[] outMap = null;
                    foreach (IoPortTemplate portOut in refOutput.Ports) {
                        if (charMap.ContainsKey(portOut.Name)) {
                            outMap = charMap[portOut.Name];
                            break;
                        }
                    }
                    if (outMap != null) {
                        ImmutableArray<IoPortTemplate> syncOut = refMachine.OutputPorts.Where((IoPortTemplate pt) => outMap.Contains(pt.Name)).ToImmutableArray();
                        FieldInfo fieldOutputOut = typeof(RecipeProduct).GetField("Ports", BindingFlags.Instance | BindingFlags.Public);
                        fieldOutputOut.SetValue(refOutput, syncOut);
                    }
                }
            }
        }
    }
}