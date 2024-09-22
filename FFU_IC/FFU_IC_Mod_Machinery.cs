using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using System.Collections.Generic;
using FFU_Industrial_Lib;

namespace FFU_Industrial_Capacity {
    internal partial class FFU_IC_Mod_Machinery : IModData {
        // Modification Definitions
        private readonly Dictionary<string, string[]> LayoutStrings =
            new Dictionary<string, string[]>() {
        };
        private readonly Dictionary<string, Dictionary<char, char[]>> LayoutMap =
            new Dictionary<string, Dictionary<char, char[]>>() { 
        };

        public void RegisterData(ProtoRegistrator registrator) {
            // ExampleUse(FFU_IC_IDs.Recipes.CopperSmeltingArcHalf);
        }

        public void ExampleUse(ProtoRegistrator registrator, RecipeProto.ID newRecipeID) {
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

            // Machinery Modifications
            FFU_ILib.SetMachineLayout(Ids.Machines.ArcFurnace, layout["Example"], map["Example"]);

            // New Recipes Addition
            registrator.RecipeProtoBuilder.Start("Copper smelting (arc half)", newRecipeID, Ids.Machines.ArcFurnace)
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