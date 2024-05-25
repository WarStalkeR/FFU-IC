using Mafi;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System.Collections.Generic;
using System.Reflection;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_BuildCosts : IModData {
        // Modification Variables
        private ProtoRegistrator pReg = null;

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

        // Reference Helpers
        private ProductProto PdRef(ProductProto.ID refID) => FFU_IC_IDs.ProductRef(pReg, refID);
        private TransportProto TrnRef(EntityProto.ID refID) => FFU_IC_IDs.TransportRef(pReg, refID);
        private StorageProto StRef(Mafi.Core.Entities.Static.StaticEntityProto.ID refID) => FFU_IC_IDs.StorageRef(pReg, refID);

        // Reflection Helpers
        public void SetBuildingCost(EntityProto refEntity, Dictionary<ProductProto.ID, int> costData) {
            if (refEntity == null) { ModLog.Warning($"SetBuildingCost: 'refEntity' is undefined!"); return; }
            if (costData == null) { ModLog.Warning($"SetBuildingCost: 'costData' is undefined!"); return; }
            if (costData.Count > 4) { ModLog.Warning($"SetBuildingCost: 'costData' beyond 4 not supported!"); return; }
            string buildCostLog = $"{refEntity.Id} Build Cost:";
            foreach (var oldProd in refEntity.Costs.Price.Products)
                buildCostLog += $" {oldProd.Product.Id.ToString().Replace("Product_","")}*{oldProd.Quantity.Value}";
            buildCostLog += " ->";
            foreach (var newProd in costData)
                buildCostLog += $" {newProd.Key.ToString().Replace("Product_", "")}*{newProd.Value}";
            ModLog.Info($"{buildCostLog}");
            List<ProductQuantity> costList = new List<ProductQuantity>();
            foreach (var newProd in costData)
                costList.Add(new ProductQuantity(PdRef(newProd.Key), newProd.Value.Quantity()));
            AssetValue newBuildCosts = costData.Count switch {
                1 => new AssetValue(costList[0]),
                2 => new AssetValue(costList[0], costList[1]),
                3 => new AssetValue(costList[0], costList[1], costList[2]),
                4 => new AssetValue(costList[0], costList[1], costList[2], costList[3]),
                _ => new AssetValue(),
            };
            EntityCosts newCosts = new EntityCosts(newBuildCosts, refEntity.Costs.DefaultPriority, 
                refEntity.Costs.Workers, refEntity.Costs.Maintenance, refEntity.Costs.IsQuickBuildDisabled);
            TypeInfo typeCosts = typeof(EntityProto).GetTypeInfo();
            FieldInfo fieldCosts = typeCosts.GetDeclaredField("<Costs>k__BackingField");
            if (fieldCosts != null) {
                fieldCosts.SetValue(refEntity, newCosts);
            }
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;

            // Solid+Loose Belt References
            TransportProto solidT1 = TrnRef(Ids.Transports.FlatConveyor);
            TransportProto solidT2 = TrnRef(Ids.Transports.FlatConveyorT2);
            TransportProto looseT1 = TrnRef(Ids.Transports.LooseMaterialConveyor);
            TransportProto looseT2 = TrnRef(Ids.Transports.LooseMaterialConveyorT2);

            // High-End Storage References
            StorageProto refSolidT4 = StRef(Ids.Buildings.StorageUnitT4);
            StorageProto refLooseT4 = StRef(Ids.Buildings.StorageLooseT4);
            StorageProto refFluidT4 = StRef(Ids.Buildings.StorageFluidT4);

            // Solid+Loose Belts Cost Modification
            SetBuildingCost(solidT1, buildCostData["ConveyorT1"]);
            SetBuildingCost(solidT2, buildCostData["ConveyorT2"]);
            SetBuildingCost(looseT1, buildCostData["ConveyorT1"]);
            SetBuildingCost(looseT2, buildCostData["ConveyorT2"]);

            // High-End Storages Cost Modification
            SetBuildingCost(refSolidT4, buildCostData["StorageT4"]);
            SetBuildingCost(refLooseT4, buildCostData["StorageT4"]);
            SetBuildingCost(refFluidT4, buildCostData["StorageT4"]);
        }
    }
}