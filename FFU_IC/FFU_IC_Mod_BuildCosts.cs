using Mafi;
using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System.Collections.Generic;
using System.Reflection;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_BuildCosts : IModData {
        // Modification Variables
        ProtoRegistrator pReg = null;

        // Modification Definitions
        public readonly Dictionary<string, Dictionary<ProductProto.ID, int>> buildCostData =
            new Dictionary<string, Dictionary<ProductProto.ID, int>>() {
            { "ConveyorT1", new Dictionary<ProductProto.ID, int>() {
                { Ids.Products.ConstructionParts, 4 },
                { Ids.Products.Rubber, 2 },
            }},
            { "ConveyorT2", new Dictionary<ProductProto.ID, int>() {
                { Ids.Products.ConstructionParts2, 4 },
                { Ids.Products.Rubber, 4 },
            }},
        };

        // Reflection Helpers
        public ProductProto PdRef(ProductProto.ID refID) => FFU_IC_IDs.ProductRef(pReg, refID);
        public TransportProto TrnRef(EntityProto.ID refID) => FFU_IC_IDs.TransportRef(pReg, refID);
        public void SetBuildingCost(TransportProto refTransport, Dictionary<ProductProto.ID, int> costData) {
            if (refTransport == null) { ModLog.Warning($"SetBuildingCost: 'refTransport' is undefined!"); return; }
            if (costData == null) { ModLog.Warning($"SetBuildingCost: 'costData' is undefined!"); return; }
            if (costData.Count > 4) { ModLog.Warning($"SetBuildingCost: 'costData' beyond 4 not supported!"); return; }
            string buildCostLog = $"{refTransport.Id} Build Cost:";
            foreach (var oldProd in refTransport.Costs.Price.Products)
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
            EntityCosts newCosts = new EntityCosts(newBuildCosts, refTransport.Costs.DefaultPriority, 
                refTransport.Costs.Workers, refTransport.Costs.Maintenance, refTransport.Costs.IsQuickBuildDisabled);
            TypeInfo typeCosts = typeof(EntityProto).GetTypeInfo();
            FieldInfo fieldCosts = typeCosts.GetDeclaredField("<Costs>k__BackingField");
            if (fieldCosts != null) {
                fieldCosts.SetValue(refTransport, newCosts);
            }
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;

            // Transports References
            TransportProto solidT1 = TrnRef(Ids.Transports.FlatConveyor);
            TransportProto solidT2 = TrnRef(Ids.Transports.FlatConveyorT2);
            TransportProto looseT1 = TrnRef(Ids.Transports.LooseMaterialConveyor);
            TransportProto looseT2 = TrnRef(Ids.Transports.LooseMaterialConveyorT2);

            // Transports Modification
            SetBuildingCost(solidT1, buildCostData["ConveyorT1"]);
            SetBuildingCost(solidT2, buildCostData["ConveyorT2"]);
            SetBuildingCost(looseT1, buildCostData["ConveyorT1"]);
            SetBuildingCost(looseT2, buildCostData["ConveyorT2"]);
        }
    }
}