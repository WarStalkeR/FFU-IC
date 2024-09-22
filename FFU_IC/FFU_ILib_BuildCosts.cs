using Mafi;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core;
using System.Collections.Generic;

namespace FFU_Industrial_Lib {
    public static partial class FFU_ILib {
        public static void SetBuildingCost(EntityProto.ID refEntityID, Dictionary<ProductProto.ID, int> costData) {
            if (_pReg == null) { ModLog.Warning($"SetBuildingCost: the ProtoRegistrator is not referenced!"); return; };
            EntityProto refEntity = EntityRef(refEntityID);
            if (refEntity == null) { ModLog.Warning($"SetBuildingCost: 'refEntity' is undefined!"); return; }
            if (costData == null) { ModLog.Warning($"SetBuildingCost: 'costData' is undefined!"); return; }
            if (costData.Count > 4) { ModLog.Warning($"SetBuildingCost: 'costData' beyond 4 not supported!"); return; }
            string buildCostLog = $"{refEntity.Id} Build Cost:";
            foreach (var oldProd in refEntity.Costs.Price.Products)
                buildCostLog += $" {oldProd.Product.Id.ToString().Replace("Product_", "")}*{oldProd.Quantity.Value}";
            buildCostLog += " ->";
            foreach (var newProd in costData)
                buildCostLog += $" {newProd.Key.ToString().Replace("Product_", "")}*{newProd.Value}";
            ModLog.Info($"{buildCostLog}");
            List<ProductQuantity> costList = new List<ProductQuantity>();
            foreach (var newProd in costData)
                costList.Add(new ProductQuantity(ProductRef(newProd.Key), newProd.Value.Quantity()));
            AssetValue newBuildCosts = costData.Count switch {
                1 => new AssetValue(costList[0]),
                2 => new AssetValue(costList[0], costList[1]),
                3 => new AssetValue(costList[0], costList[1], costList[2]),
                4 => new AssetValue(costList[0], costList[1], costList[2], costList[3]),
                _ => new AssetValue(),
            };
            EntityCosts newCosts = new EntityCosts(newBuildCosts, refEntity.Costs.DefaultPriority,
                refEntity.Costs.Workers, refEntity.Costs.Maintenance, refEntity.Costs.IsQuickBuildDisabled);
            refEntity.Costs = newCosts;
        }
    }
}