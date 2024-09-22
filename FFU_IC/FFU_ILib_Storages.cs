using Mafi;
using System.Reflection;
using Mafi.Localization;
using Mafi.Base.Prototypes.Buildings.ThermalStorages;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Static;

namespace FFU_Industrial_Lib {
    public static partial class FFU_ILib {
        /// <remarks>
        /// Modifies storage capacity of a <b>StorageProto</b>. Requires <c>integer</c> value.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Define new storage capacity as <b>int</b> variable:<br/>
        /// <c>int newStorageCapacity = 600;</c>
        /// 
        /// <br/><br/>Apply new capacity value via <b>StorageProto</b> identifier:<br/>
        /// <c>SetStorageCapacity(Ids.Buildings.StorageUnit, newStorageCapacity);</c>
        /// </remarks>
        public static void SetStorageCapacity(StaticEntityProto.ID refStorageID, int newMaterialCap) {
            if (_pReg == null) { ModLog.Warning($"SetStorageCapacity: the ProtoRegistrator is not referenced!"); return; };
            StorageProto refStorage = StorageRef(refStorageID);
            if (refStorage == null) { ModLog.Warning($"SetStorageCapacity: can't find StorageProto reference!"); return; }
            if (newMaterialCap <= 0) { ModLog.Warning($"SetStorageCapacity: 'newMaterialCap' is invalid!"); return; }
            ModLog.Info($"{refStorage.Id} Capacity: {refStorage.Capacity} -> {newMaterialCap}");
            FieldInfo fieldStorage = typeof(StorageBaseProto).GetField("Capacity", BindingFlags.Instance | BindingFlags.Public);
            fieldStorage.SetValue(refStorage, new Quantity(newMaterialCap));
            SyncProtoMod(refStorage);
        }

        /// <remarks>
        /// Modifies thermal capacity of a <b>ThermalStorageProto</b>. Requires <c>integer</c> value.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Define new thermal capacity as <b>int</b> variable:<br/>
        /// <c>int newThermalCapacity = 15000;</c>
        /// 
        /// <br/><br/>Apply new capacity value via <b>ThermalStorageProto</b> identifier:<br/>
        /// <c>SetThermalCapacity(Ids.Buildings.ThermalStorage, newThermalCapacity);</c>
        /// </remarks>
        public static void SetThermalCapacity(StaticEntityProto.ID refThermalID, int newThermalCap) {
            if (_pReg == null) { ModLog.Warning($"SetThermalCapacity: the ProtoRegistrator is not referenced!"); return; };
            ThermalStorageProto refThermal = ThermalRef(refThermalID);
            if (refThermal == null) { ModLog.Warning($"SetThermalCapacity: can't find ThermalStorageProto reference!"); return; }
            if (newThermalCap <= 0) { ModLog.Warning($"SetThermalCapacity: 'newThermalCap' is invalid!"); return; }
            ModLog.Info($"{refThermal.Id} Capacity: {refThermal.Capacity} -> {newThermalCap}");
            FieldInfo fieldThermal = typeof(ThermalStorageProto).GetField("Capacity", BindingFlags.Instance | BindingFlags.Public);
            fieldThermal.SetValue(refThermal, new Quantity(newThermalCap));
            SyncProtoMod(refThermal);
        }

        /// <remarks>
        /// Modifies nuclear and retired waste capacity of a <b>NuclearWasteStorageProto</b>. Requires two <c>integer</c> values.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Define new nuclear and retired waste capacity as <b>int</b> variables:<br/>
        /// <c>int newNuclearCapacity = 12000;</c><br/>
        /// <c>int newRetiredCapacity = 3000;</c>
        /// 
        /// <br/><br/>Apply new capacity values via <b>NuclearWasteStorageProto</b> identifier:<br/>
        /// <c>SetNuclearCapacity(Ids.Buildings.NuclearWasteStorage, newNuclearCapacity, newRetiredCapacity);</c>
        /// </remarks>
        public static void SetNuclearCapacity(StaticEntityProto.ID refNuclearID, int newNuclearCap, int newRetiredCap) {
            if (_pReg == null) { ModLog.Warning($"SetNuclearCapacity: the ProtoRegistrator is not referenced!"); return; };
            NuclearWasteStorageProto refNuclear = NuclearRef(refNuclearID);
            if (refNuclear == null) { ModLog.Warning($"SetNuclearCapacity: can't find NuclearWasteStorageProto reference!"); return; }
            if (newNuclearCap <= 0) { ModLog.Warning($"SetNuclearCapacity: 'newNuclearCap' is invalid!"); return; }
            if (newRetiredCap <= 0) { ModLog.Warning($"SetNuclearCapacity: 'newRetiredCap' is invalid!"); return; }
            ModLog.Info($"{refNuclear.Id} Capacity: {refNuclear.Capacity}/{refNuclear.RetiredWasteCapacity} -> {newNuclearCap}/{newRetiredCap}");
            FieldInfo fieldNuclear = typeof(StorageBaseProto).GetField("Capacity", BindingFlags.Instance | BindingFlags.Public);
            FieldInfo fieldRetried = typeof(NuclearWasteStorageProto).GetField("RetiredWasteCapacity", BindingFlags.Instance | BindingFlags.Public);
            fieldNuclear.SetValue(refNuclear, new Quantity(newNuclearCap));
            fieldRetried.SetValue(refNuclear, new Quantity(newRetiredCap));
            SyncProtoMod(refNuclear);
        }

        /// <remarks>
        /// Modifies food module capacity of a <b>SettlementFoodModuleProto</b>. Requires <c>integer</c> value.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Define new food module capacity as <b>int</b> variable:<br/>
        /// <c>int newFoodModuleCapacity = 2500;</c>
        /// 
        /// <br/><br/>Apply new capacity value via <b>SettlementFoodModuleProto</b> identifier:<br/>
        /// <c>SetFoodModuleCapacity(Ids.Buildings.SettlementFoodModule, newFoodModuleCapacity);</c>
        /// </remarks>
        public static void SetFoodModuleCapacity(StaticEntityProto.ID refMarketID, int newFoodCap) {
            if (_pReg == null) { ModLog.Warning($"SetFoodModuleCapacity: the ProtoRegistrator is not referenced!"); return; };
            SettlementFoodModuleProto refMarket = MarketRef(refMarketID);
            if (refMarket == null) { ModLog.Warning($"SetFoodModuleCapacity: can't find SettlementFoodModuleProto reference!"); return; }
            if (newFoodCap <= 0) { ModLog.Warning($"SetFoodModuleCapacity: 'newFoodCap' is invalid!"); return; }
            ModLog.Info($"{refMarket.Id} Capacity: {refMarket.CapacityPerBuffer} -> {newFoodCap}");
            FieldInfo fieldMarket = typeof(SettlementFoodModuleProto).GetField("CapacityPerBuffer", BindingFlags.Instance | BindingFlags.Public);
            fieldMarket.SetValue(refMarket, new Quantity(newFoodCap));
            SyncProtoMod(refMarket);
        }

        /// <remarks>
        /// Modifies description of a <b>StorageProto</b>. For reference use description values in example below.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Activate the <b>LocalizationManager</b> override to avoid exceptions:<br/>
        /// <c>LocalizationManager.IgnoreDuplicates();</c>
        /// 
        /// <br/><br/>Define new vehicle localization as <b>string[]</b> array (using relevant strings):<br/>
        /// <c>string[] storageLocString = new string[] { "StorageSolidFormattedBase__desc", "Stores up to {0} units of a solid product.", "description for storage" };</c><br/>
        /// 
        /// <br/><br/>Apply new description strings via <b>DrivingEntityProto</b> identifier:<br/>
        /// <c>SetStorageDescription(refVehicle, storageLocString);</c>
        /// </remarks>
        public static void SetStorageDescription(StaticEntityProto.ID refStorageID, string[] strSet) {
            if (_pReg == null) { ModLog.Warning($"SetStorageDescription: the ProtoRegistrator is not referenced!"); return; };
            StorageProto refStorage = StorageRef(refStorageID);
            if (refStorage == null) { ModLog.Warning($"SetStorageDescription: can't find StorageProto reference!"); return; }
            if (strSet == null) { ModLog.Warning($"SetStorageDescription: 'strSet' is undefined!"); return; }
            if (strSet.Length != 3) { ModLog.Warning($"SetStorageDescription: 'strSet' count is incorrect!"); return; }
            LocStr1 locStr = Loc.Str1(strSet[0], strSet[1], strSet[2]);
            LocStr locDesc = LocalizationManager.CreateAlreadyLocalizedStr(refStorage.Id.Value +
                "__desc", locStr.Format(refStorage.Capacity.ToString()).ToString());
            TypeInfo typeProto = typeof(Mafi.Core.Prototypes.Proto).GetTypeInfo();
            FieldInfo fieldStrings = typeProto.GetDeclaredField("<Strings>k__BackingField");
            if (fieldStrings != null) {
                ModLog.Info($"{refStorage.Id} description modified.");
                Mafi.Core.Prototypes.Proto.Str currStr = (Mafi.Core.Prototypes.Proto.Str)fieldStrings.GetValue(refStorage);
                Mafi.Core.Prototypes.Proto.Str newStr = new Mafi.Core.Prototypes.Proto.Str(currStr.Name, locDesc);
                fieldStrings.SetValue(refStorage, newStr);
                SyncProtoMod(refStorage);
            }
        }
    }
}