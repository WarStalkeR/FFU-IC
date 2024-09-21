using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Localization;
using System.Collections.Generic;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Storages : IModData {
        // Modification Variables
        private ProtoRegistrator pReg = null;

        // Modification Definitions
        private readonly Dictionary<string, int> StorageCapacity =
            new Dictionary<string, int>() {
            { "DefaultT1", 600 },
            { "DefaultT2", 1800 },
            { "DefaultT3", 9000 },
            { "DefaultT4", 27000 },
            { "RadWaste", 12000 },
            { "RetWaste", 3000 },
            { "Thermal", 15000 },
            { "Foodstuff", 2500 },
        };

        // Localization Definitions
        private readonly Dictionary<string, string[]> StorageLocStrings =
            new Dictionary<string, string[]>() {
            { "Solid", new string[] { "StorageSolidFormattedBase__desc", "Stores up to {0} units of a solid product.", "description for storage" }},
            { "Loose", new string[] { "StorageLooseFormattedBase__desc", "Stores up to {0} units of a loose product.", "description for storage" }},
            { "Fluid", new string[] { "StorageFluidFormattedBase__desc", "Stores up to {0} units of a liquid or gas product.", "description for storage" }},
        };

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            FFU_IC_Lib.ProtoReg = registrator;
            LocalizationManager.IgnoreDuplicates();

            // Solid Storage Modifications
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageUnit, StorageCapacity["DefaultT1"]);
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageUnitT2, StorageCapacity["DefaultT2"]);
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageUnitT3, StorageCapacity["DefaultT3"]);
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageUnitT4, StorageCapacity["DefaultT4"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageUnit, StorageLocStrings["Solid"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageUnitT2, StorageLocStrings["Solid"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageUnitT3, StorageLocStrings["Solid"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageUnitT4, StorageLocStrings["Solid"]);

            // Loose Storage Modifications
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageLoose, StorageCapacity["DefaultT1"]);
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageLooseT2, StorageCapacity["DefaultT2"]);
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageLooseT3, StorageCapacity["DefaultT3"]);
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageLooseT4, StorageCapacity["DefaultT4"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageLoose, StorageLocStrings["Loose"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageLooseT2, StorageLocStrings["Loose"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageLooseT3, StorageLocStrings["Loose"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageLooseT4, StorageLocStrings["Loose"]);

            // Fluid Storage Modifications
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageFluid, StorageCapacity["DefaultT1"]);
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageFluidT2, StorageCapacity["DefaultT2"]);
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageFluidT3, StorageCapacity["DefaultT3"]);
            FFU_IC_Lib.SetStorageCapacity(Ids.Buildings.StorageFluidT4, StorageCapacity["DefaultT4"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageFluid, StorageLocStrings["Fluid"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageFluidT2, StorageLocStrings["Fluid"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageFluidT3, StorageLocStrings["Fluid"]);
            FFU_IC_Lib.SetStorageDescription(Ids.Buildings.StorageFluidT4, StorageLocStrings["Fluid"]);

            // Thermal Storage Modifications
            FFU_IC_Lib.SetThermalCapacity(Ids.Buildings.ThermalStorage, StorageCapacity["Thermal"]);

            // Nuclear Storage Modifications
            FFU_IC_Lib.SetNuclearCapacity(Ids.Buildings.NuclearWasteStorage, StorageCapacity["RadWaste"], StorageCapacity["RetWaste"]);

            // Settlement Market Modifications
            FFU_IC_Lib.SetFoodModuleCapacity(Ids.Buildings.SettlementFoodModule, StorageCapacity["Foodstuff"]);
            FFU_IC_Lib.SetFoodModuleCapacity(Ids.Buildings.SettlementFoodModuleT2, StorageCapacity["Foodstuff"]);
        }
    }
}