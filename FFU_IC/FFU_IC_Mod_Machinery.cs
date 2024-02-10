﻿using Mafi;
using Mafi.Base;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Ports.Io;
using Mafi.Core.Syncers;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using static Mafi.Base.Assets.Core;

namespace FFU_Industrial_Capacity {
    internal partial class FFU_IC_Mod_Machinery : IModData {
        // Modification Variables
        ProtoRegistrator protoReg = null;
        public static readonly RecipeProto.ID CopperSmeltingArcHalf = new RecipeProto.ID("CopperSmeltingArcHalf");

        // Modification Definitions
        public readonly Dictionary<string, string[]> LayoutStrings =
            new Dictionary<string, string[]>() {
            { "ArcFurnaceT1", new string[] {
                "C#>[2][6][6][6][6][6][6][6][2]>~Y",
                "D#>[2][7][7][6][6][6][6][4][2]>'X",
                "A~>[2][7][7][6][7][7][6][4][2]>'V",
                "B~>[2][7][7][6][7][7][6][4][2]>'W",
                "I~>[2][7][7][6][6][6][6][6][2]>~Z",
                "J@>[2][6][6][6][6][6][6][6][2]>@E"
            }},
        };
        public readonly Dictionary<string, Dictionary<char, char[]>> LayoutMap =
            new Dictionary<string, Dictionary<char, char[]>>() {
            { "ArcFurnaceT1", new Dictionary<char, char[]>() {
                { 'A', new char[] { 'A', 'B', 'I' }},
                { 'C', new char[] { 'C', 'D' }},
                { 'Y', new char[] { 'Y', 'Z' }},
                { 'V', new char[] { 'V', 'W', 'X' }},
            }},
        };

        // Reflection Helpers
        public MachineProto MachineRef(MachineProto.ID refID) => protoReg.PrototypesDb.Get<MachineProto>(refID).Value;
        public void SetMachineLayout(MachineProto refMachine, string[] strLayout, Dictionary<char, char[]> charMap = null) {
            EntityLayout newLayout = protoReg.LayoutParser.ParseLayoutOrThrow(strLayout);
            TypeInfo typeLayout = typeof(LayoutEntityProto).GetTypeInfo();
            FieldInfo fieldLayout = typeLayout.GetDeclaredField("<Layout>k__BackingField");
            if (fieldLayout != null) {
                fieldLayout.SetValue(refMachine, newLayout);
                FieldInfo fieldPortsIn = typeof(LayoutEntityProto).GetField("InputPorts", BindingFlags.Instance | BindingFlags.Public);
                FieldInfo fieldPortsOut = typeof(LayoutEntityProto).GetField("OutputPorts", BindingFlags.Instance | BindingFlags.Public);
                ImmutableArray<IoPortTemplate> updatedIn = refMachine.Layout.Ports.Where((IoPortTemplate pt) => pt.Type == IoPortType.Input).ToImmutableArray();
                ImmutableArray<IoPortTemplate> updatedOut = refMachine.Layout.Ports.Where((IoPortTemplate pt) => pt.Type == IoPortType.Output).ToImmutableArray();
                fieldPortsIn.SetValue(refMachine, updatedIn);
                fieldPortsOut.SetValue(refMachine, updatedOut);
                if (charMap != null) SyncMachineRecipeMap(refMachine, charMap);
            }
        }
        public void SyncMachineRecipeMap(MachineProto refMachine, Dictionary<char, char[]> charMap) {
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
            protoReg = registrator;

            // Machinery References
            MachineProto furnace = MachineRef(Ids.Machines.ArcFurnace);

            // Machinery Modifications
            SetMachineLayout(furnace, LayoutStrings["ArcFurnaceT1"], LayoutMap["ArcFurnaceT1"]);

            // New Recipes Addition
            protoReg.RecipeProtoBuilder
            .Start("Copper smelting (arc half)", CopperSmeltingArcHalf, furnace)
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