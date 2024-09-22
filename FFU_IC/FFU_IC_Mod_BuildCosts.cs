using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using System.Collections.Generic;
using FFU_Industrial_Lib;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_BuildCosts : IModData {
        // Modification Definitions
        private readonly Dictionary<string, Dictionary<ProductProto.ID, int>> buildCostData =
            new Dictionary<string, Dictionary<ProductProto.ID, int>>() {
            { "ConveyorT1", new Dictionary<ProductProto.ID, int>() {
                { Ids.Products.ConstructionParts, 4 },
                { Ids.Products.Rubber, 2 },
            }},
            { "ConveyorT2", new Dictionary<ProductProto.ID, int>() {
                { Ids.Products.ConstructionParts2, 4 },
                { Ids.Products.Rubber, 4 },
            }},
            { "StorageT4", new Dictionary<ProductProto.ID, int>() {
                { Ids.Products.ConstructionParts4, 90 },
            }},
        };

        public void RegisterData(ProtoRegistrator registrator) {
            // Solid+Loose Belts Cost Modification
            FFU_ILib.SetBuildingCost(Ids.Transports.FlatConveyor, buildCostData["ConveyorT1"]);
            FFU_ILib.SetBuildingCost(Ids.Transports.FlatConveyorT2, buildCostData["ConveyorT2"]);
            FFU_ILib.SetBuildingCost(Ids.Transports.LooseMaterialConveyor, buildCostData["ConveyorT1"]);
            FFU_ILib.SetBuildingCost(Ids.Transports.LooseMaterialConveyorT2, buildCostData["ConveyorT2"]);

            // High-End Storages Cost Modification
            FFU_ILib.SetBuildingCost(Ids.Buildings.StorageUnitT4, buildCostData["StorageT4"]);
            FFU_ILib.SetBuildingCost(Ids.Buildings.StorageLooseT4, buildCostData["StorageT4"]);
            FFU_ILib.SetBuildingCost(Ids.Buildings.StorageFluidT4, buildCostData["StorageT4"]);
        }
    }
}