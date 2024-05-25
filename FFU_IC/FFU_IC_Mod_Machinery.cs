using Mafi;
using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Ports.Io;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FFU_Industrial_Capacity {
    internal partial class FFU_IC_Mod_Machinery : IModData {
        // Modification Variables
        private ProtoRegistrator pReg = null;

        // Modification Definitions
        private readonly Dictionary<string, string[]> LayoutStrings =
            new Dictionary<string, string[]>() {
        };
        private readonly Dictionary<string, Dictionary<char, char[]>> LayoutMap =
            new Dictionary<string, Dictionary<char, char[]>>() { 
        };

        // Reference Helpers
        private MachineProto McRef(MachineProto.ID refID) => FFU_IC_IDs.MachineRef(pReg, refID);

        // Reflection Helpers
        public void SetMachineLayout(MachineProto refMachine, string[] strLayout, Dictionary<char, char[]> charMap = null) {
            if (refMachine == null) { ModLog.Warning($"SetMachineLayout: 'refMachine' is undefined!"); return; }
            if (strLayout == null) { ModLog.Warning($"SetMachineLayout: 'strLayout' is undefined!"); return; }
            EntityLayout newLayout = pReg.LayoutParser.ParseLayoutOrThrow(strLayout);
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
                FFU_IC_IDs.SyncProtoMod(refMachine);
            }
        }
        public void SyncMachineRecipeMap(MachineProto refMachine, Dictionary<char, char[]> charMap) {
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

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;

            // ExampleUse(FFU_IC_IDs.Recipes.CopperSmeltingArcHalf);
        }
        public void ExampleUse(RecipeProto.ID newRecipeID) {
            // Machinery Variables
            Dictionary<string, string[]> layout =
                new Dictionary<string, string[]>() {
                { "Example", new string[] {
                    "C#>[2][6][6][6][6][6][6][6][2]>~Y",
                    "D#>[2][7][7][6][6][6][6][4][2]>'X",
                    "A~>[2][7][7][6][7][7][6][4][2]>'V",
                    "B~>[2][7][7][6][7][7][6][4][2]>'W",
                    "I~>[2][7][7][6][6][6][6][6][2]>~Z",
                    "J@>[2][6][6][6][6][6][6][6][2]>@E",
                }},
            };
            Dictionary<string, Dictionary<char, char[]>> map =
                new Dictionary<string, Dictionary<char, char[]>>() {
                { "Example", new Dictionary<char, char[]>() {
                    { 'A', new char[] { 'A', 'B', 'I' }},
                    { 'C', new char[] { 'C', 'D' }},
                    { 'Y', new char[] { 'Y', 'Z' }},
                    { 'V', new char[] { 'V', 'W', 'X' }},
                }},
            };

            // Machinery References
            MachineProto furnace = McRef(Ids.Machines.ArcFurnace);

            // Machinery Modifications
            SetMachineLayout(furnace, layout["Example"], map["Example"]);

            // New Recipes Addition
            pReg.RecipeProtoBuilder
            .Start("Copper smelting (arc half)", newRecipeID, furnace)
            .AddInput(12, Ids.Products.CopperOreCrushed, "*", false)
            .AddInput(2, Ids.Products.Sand, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .AddInput(1, Ids.Products.Water, "J", false)
            .SetDuration(30.Seconds())
            .AddOutput(16, Ids.Products.MoltenCopper, "*", false, false)
            .AddOutput(6, Ids.Products.Slag, "*", false, false)
            .AddOutput(6, Ids.Products.Exhaust, "E", false, false)
            .BuildAndAdd();
        }
    }
}