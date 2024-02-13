using Mafi;
using Mafi.Base;
using Mafi.Base.Prototypes.Buildings.ThermalStorages;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Localization;
using System.Collections.Generic;
using System.Reflection;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Storages : IModData {
        // Modification Variables
        ProtoRegistrator pReg = null;
        public bool usePower = false;

        // Modification Definitions
        public readonly Dictionary<string, int> StorageCapacity =
            new Dictionary<string, int>() {
            { "DefaultT1", 600 },
            { "DefaultT2", 1800 },
            { "DefaultT3", 9000 },
            { "DefaultT4", 27000 },
            { "RadWaste", 12000 },
            { "RetWaste", 3000 },
            { "Thermal", 15000 },
        };

        // Localization Definitions
        public readonly Dictionary<string, string[]> StorageLocStrings =
            new Dictionary<string, string[]>() {
            { "Solid", new string[] { "StorageSolidFormattedBase__desc", "Stores up to {0} units of a solid product.", "description for storage" }},
            { "Loose", new string[] { "StorageLooseFormattedBase__desc", "Stores up to {0} units of a loose product.", "description for storage" }},
            { "Fluid", new string[] { "StorageFluidFormattedBase__desc", "Stores up to {0} units of a liquid or gas product.", "description for storage" }},
        };

        // Reflection Helpers
        public void SetStorageCapacity(StorageProto refStorage, int newMaterialCap) {
            if (refStorage == null) { ModLog.Warning($"SetStorageCapacity: 'refStorage' is undefined!"); return; }
            ModLog.Info($"{refStorage.Id} Capacity: {refStorage.Capacity} -> {newMaterialCap}");
            FieldInfo fieldStorage = typeof(StorageBaseProto).GetField("Capacity", BindingFlags.Instance | BindingFlags.Public);
            fieldStorage.SetValue(refStorage, new Quantity(newMaterialCap));
        }
        public void SetStorageCapacity(ThermalStorageProto refThermal, int newThermalCap) {
            if (refThermal == null) { ModLog.Warning($"SetStorageCapacity: 'refThermal' is undefined!"); return; }
            ModLog.Info($"{refThermal.Id} Capacity: {refThermal.Capacity} -> {newThermalCap}");
            FieldInfo fieldThermal = typeof(ThermalStorageProto).GetField("Capacity", BindingFlags.Instance | BindingFlags.Public);
            fieldThermal.SetValue(refThermal, new Quantity(newThermalCap));
        }
        public void SetStorageCapacity(NuclearWasteStorageProto refNuclear, int newNuclearCap, int newRetiredCap) {
            if (refNuclear == null) { ModLog.Warning($"SetStorageCapacity: 'refNuclear' is undefined!"); return; }
            ModLog.Info($"{refNuclear.Id} Capacity: {refNuclear.Capacity}/{refNuclear.RetiredWasteCapacity} -> {newNuclearCap}/{newRetiredCap}");
            FieldInfo fieldNuclear = typeof(StorageBaseProto).GetField("Capacity", BindingFlags.Instance | BindingFlags.Public);
            FieldInfo fieldRetried = typeof(NuclearWasteStorageProto).GetField("RetiredWasteCapacity", BindingFlags.Instance | BindingFlags.Public);
            fieldNuclear.SetValue(refNuclear, new Quantity(newNuclearCap));
            fieldRetried.SetValue(refNuclear, new Quantity(newRetiredCap));
        }
        public void SetStorageDescription(StorageProto refStorage, string[] strSet) {
            if (refStorage == null) { ModLog.Warning($"SetStorageDescription: 'refStorage' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetStorageDescription: 'strSet' is undefined!"); return; }
            LocStr1 locStr = Loc.Str1(strSet[0], strSet[1], strSet[2]);
            LocStr locDesc = LocalizationManager.CreateAlreadyLocalizedStr(refStorage.Id.Value + 
                "__desc", locStr.Format(refStorage.Capacity.ToString()).ToString() + (usePower ? " " +
                Loc.Str("StoragePowerConsumptionSuffix", "Consumes power when sending or receiving products via its ports.", 
                "appended at the end of a description to explain that a storage consumes power").ToString() : ""));
            TypeInfo typeProto = typeof(Mafi.Core.Prototypes.Proto).GetTypeInfo();
            FieldInfo fieldStrings = typeProto.GetDeclaredField("<Strings>k__BackingField");
            if (fieldStrings != null) {
                ModLog.Info($"{refStorage.Id} description modified.");
                Mafi.Core.Prototypes.Proto.Str currStr = (Mafi.Core.Prototypes.Proto.Str)fieldStrings.GetValue(refStorage);
                Mafi.Core.Prototypes.Proto.Str newStr = new Mafi.Core.Prototypes.Proto.Str(currStr.Name, locDesc);
                fieldStrings.SetValue(refStorage, newStr);
            }
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;
            usePower = pReg.DifficultyConfig.PowerSetting != 
                GameDifficultyConfig.LogisticsPowerSetting.DoNotConsume;

            // Storage References
            StorageProto refSolidT1 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageUnit);
            StorageProto refSolidT2 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageUnitT2);
            StorageProto refSolidT3 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageUnitT3);
            StorageProto refSolidT4 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageUnitT4);
            StorageProto refLooseT1 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageLoose);
            StorageProto refLooseT2 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageLooseT2);
            StorageProto refLooseT3 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageLooseT3);
            StorageProto refLooseT4 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageLooseT4);
            StorageProto refFluidT1 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageFluid);
            StorageProto refFluidT2 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageFluidT2);
            StorageProto refFluidT3 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageFluidT3);
            StorageProto refFluidT4 = FFU_IC_IDs.StorageRef(pReg, Ids.Buildings.StorageFluidT4);
            ThermalStorageProto refThermal = FFU_IC_IDs.ThermalRef(pReg, Ids.Buildings.ThermalStorage);
            NuclearWasteStorageProto refNuclear = FFU_IC_IDs.NuclearRef(pReg, Ids.Buildings.NuclearWasteStorage);

            // Solid Storage Modifications
            SetStorageCapacity(refSolidT1, StorageCapacity["DefaultT1"]);
            SetStorageCapacity(refSolidT2, StorageCapacity["DefaultT2"]);
            SetStorageCapacity(refSolidT3, StorageCapacity["DefaultT3"]);
            SetStorageCapacity(refSolidT4, StorageCapacity["DefaultT4"]);
            SetStorageDescription(refSolidT1, StorageLocStrings["Solid"]);
            SetStorageDescription(refSolidT2, StorageLocStrings["Solid"]);
            SetStorageDescription(refSolidT3, StorageLocStrings["Solid"]);
            SetStorageDescription(refSolidT4, StorageLocStrings["Solid"]);

            // Loose Storage Modifications
            SetStorageCapacity(refLooseT1, StorageCapacity["DefaultT1"]);
            SetStorageCapacity(refLooseT2, StorageCapacity["DefaultT2"]);
            SetStorageCapacity(refLooseT3, StorageCapacity["DefaultT3"]);
            SetStorageCapacity(refLooseT4, StorageCapacity["DefaultT4"]);
            SetStorageDescription(refLooseT1, StorageLocStrings["Loose"]);
            SetStorageDescription(refLooseT2, StorageLocStrings["Loose"]);
            SetStorageDescription(refLooseT3, StorageLocStrings["Loose"]);
            SetStorageDescription(refLooseT4, StorageLocStrings["Loose"]);

            // Fluid Storage Modifications
            SetStorageCapacity(refFluidT1, StorageCapacity["DefaultT1"]);
            SetStorageCapacity(refFluidT2, StorageCapacity["DefaultT2"]);
            SetStorageCapacity(refFluidT3, StorageCapacity["DefaultT3"]);
            SetStorageCapacity(refFluidT4, StorageCapacity["DefaultT4"]);
            SetStorageDescription(refFluidT1, StorageLocStrings["Fluid"]);
            SetStorageDescription(refFluidT2, StorageLocStrings["Fluid"]);
            SetStorageDescription(refFluidT3, StorageLocStrings["Fluid"]);
            SetStorageDescription(refFluidT4, StorageLocStrings["Fluid"]);

            // Thermal Storage Modifications
            SetStorageCapacity(refThermal, StorageCapacity["Thermal"]);

            // Nuclear Storage Modifications
            SetStorageCapacity(refNuclear, StorageCapacity["RadWaste"], StorageCapacity["RetWaste"]);
        }
    }
}