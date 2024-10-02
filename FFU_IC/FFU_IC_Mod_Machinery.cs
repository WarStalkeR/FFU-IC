using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using System.Collections.Generic;
using FFU_Industrial_Lib;
using Mafi.Core.Entities.Static.Layout;

namespace FFU_Industrial_Capacity {
    internal partial class FFU_IC_Mod_Machinery : IModData {
        // Modification Definitions
        private readonly Dictionary<string, string[]> LayoutStrings =
            new Dictionary<string, string[]>() {
            { "Desalinator", new string[] {
                "A@>[3][3][3][3][3][3][3][3][3][3][3]>@W", 
                "   [3][3][3][3][3][3][3][4][3][3][3]   ", 
                "   [4][4][4][4][4][4][4][4][4][4][3]>@X",
                "B@>[3][3][3][3][3][3][3][3][3][3][3]>@E",
            }},
            { "WaterPumpT1", new string[] {
                "                                    [2][2]         ", 
                "-6~-5}-4}-3}-2}-1}-1}{P}{2}{2}{P}{2}[2][2][2][2]>@X", 
                "-6~-5}-4}-3}-2}-1}-1}{P}{2}{2}{P}{2}[2][2][2][2]   ",
                "                                 {1}[2][2][2][2]>@E",
            }},
            { "WaterPumpT2", new string[] {
                "                                                         {1}(2)[2][2][2]   ", 
                "-6~-5}-4}-3}-2}-1}-5}-4}-3}-2}-1}{1}{1}{1}{1}{P}{2}{2}{P}{2}(2)[2][2][2]>@X", 
                "-6~-5}-4}-3}-2}-1}-5}-4}-3}-2}-1}{1}{1}{1}{1}{P}{2}{2}{P}{2}(2)[2][2][2]   ",
                "                                                         {1}(2)[2][2][2]>@E",
            }},
        };
        private readonly Dictionary<string, Dictionary<char, char[]>> LayoutMap =
            new Dictionary<string, Dictionary<char, char[]>>() {
            { "Example", new Dictionary<char, char[]>() {
                { 'A', new char[] { 'A', 'B', 'I' }},
                { 'C', new char[] { 'C', 'D' }},
                { 'Y', new char[] { 'Y', 'Z' }},
                { 'V', new char[] { 'V', 'W', 'X' }},
            }},
        };
        private readonly Dictionary<string, EntityLayoutParams> LayoutParams =
            new Dictionary<string, EntityLayoutParams>() {
            { "WaterPumpT1", new EntityLayoutParams((LayoutTile x) => x.Constraint == LayoutTileConstraint.None || x.Constraint.HasAnyConstraints(LayoutTileConstraint.Ocean), 
                new CustomLayoutToken[4] {
                    new CustomLayoutToken("-0~", (EntityLayoutParams p, int h) => new LayoutTokenSpec(-h - 1, -h + 2, LayoutTileConstraint.Ocean)),
                    new CustomLayoutToken("-0}", (EntityLayoutParams p, int h) => new LayoutTokenSpec(-h - 1, -h + 2)),
                    new CustomLayoutToken("~~~", (EntityLayoutParams p, int h) => new LayoutTokenSpec(-13, -10, LayoutTileConstraint.Ocean)),
                    new CustomLayoutToken("{P}", delegate {
                        int? maxTerrainHeight = 0;
                        return new LayoutTokenSpec(-5, 2, LayoutTileConstraint.None, null, null, maxTerrainHeight);
                    })
                })
            },
            { "WaterPumpT2", new EntityLayoutParams((LayoutTile x) => x.Constraint == LayoutTileConstraint.None || x.Constraint.HasAnyConstraints(LayoutTileConstraint.Ocean), 
                new CustomLayoutToken[4] {
                    new CustomLayoutToken("-0~", (EntityLayoutParams p, int h) => new LayoutTokenSpec(-h - 6, -h - 3, LayoutTileConstraint.Ocean)),
                    new CustomLayoutToken("-0}", (EntityLayoutParams p, int h) => new LayoutTokenSpec(-h - 6, -h - 3)),
                    new CustomLayoutToken("~~~", (EntityLayoutParams p, int h) => new LayoutTokenSpec(-22, -19, LayoutTileConstraint.Ocean)),
                    new CustomLayoutToken("{P}", delegate
                    {
                        int? maxTerrainHeight = 0;
                        return new LayoutTokenSpec(-9, 2, LayoutTileConstraint.None, null, null, maxTerrainHeight);
                    })
                })
            },
        };

        public void RegisterData(ProtoRegistrator registrator) {
            // Rebalanced Robotic Assemblies Computing Use
            FFU_ILib.SetComputingUse(Ids.Machines.AssemblyRoboticT1, Computing.FromTFlops(2));
            FFU_ILib.SetComputingUse(Ids.Machines.AssemblyRoboticT2, Computing.FromTFlops(4));

            // Thermal Desalinator Exhaust Port
            FFU_ILib.SetMachineLayout(Ids.Machines.ThermalDesalinator, LayoutStrings["Desalinator"]);

            // Ocean Water Pumps Exhaust Port
            // FFU_ILib.SetMachineLayout(Ids.Machines.OceanWaterPumpT1, LayoutStrings["WaterPumpT1"], null, LayoutParams["WaterPumpT1"]);
            // FFU_ILib.SetMachineLayout(Ids.Machines.OceanWaterPumpLarge, LayoutStrings["WaterPumpT2"], null, LayoutParams["WaterPumpT2"]);
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