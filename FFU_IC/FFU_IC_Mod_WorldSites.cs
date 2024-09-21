using Mafi.Base;
using Mafi.Core.Mods;
using System.Collections.Generic;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_WorldSites : IModData {
        // Modification Variables
        private ProtoRegistrator pReg = null;

        // Modification Definitions
        private readonly Dictionary<string, double[]> MineProdData =
            new Dictionary<string, double[]>() {
            { "Oil", new double[] { 25, 20.0, 0.05, 20, 2 }},
            { "Water", new double[] { 15, 10.0, 0.05, 20, 2 }},
            { "Rock", new double[] { 30, 20.0, 0.05, 20, 2 }},
            { "Coal", new double[] { 25, 20.0, 0.05, 20, 2 }},
            { "Sulfur", new double[] { 15, 20.0, 0.05, 20, 2 }},
            { "Limestone", new double[] { 15, 20.0, 0.05, 20, 2 }},
            { "Quartz", new double[] { 20, 20.0, 0.05, 20, 2 }},
            { "Uranium", new double[] { 10, 20.0, 0.05, 20, 2 }},
        };

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            FFU_IC_Lib.ProtoReg = registrator;

            // World Mine Modifications
            FFU_IC_Lib.SetMineProduction(Ids.World.OilRigCost1, MineProdData["Oil"]);
            FFU_IC_Lib.SetMineProduction(Ids.World.OilRigCost2, MineProdData["Oil"]);
            FFU_IC_Lib.SetMineProduction(Ids.World.OilRigCost3, MineProdData["Oil"]);
            FFU_IC_Lib.SetMineProduction(Ids.World.WaterWell, MineProdData["Water"]);
            FFU_IC_Lib.SetMineProduction(Ids.World.RockMine, MineProdData["Rock"]);
            FFU_IC_Lib.SetMineProduction(Ids.World.CoalMine, MineProdData["Coal"]);
            FFU_IC_Lib.SetMineProduction(Ids.World.SulfurMine, MineProdData["Sulfur"]);
            FFU_IC_Lib.SetMineProduction(Ids.World.LimestoneMine, MineProdData["Limestone"]);
            FFU_IC_Lib.SetMineProduction(Ids.World.QuartzMine, MineProdData["Quartz"]);
            FFU_IC_Lib.SetMineProduction(Ids.World.UraniumMine, MineProdData["Uranium"]);
        }
    }
}