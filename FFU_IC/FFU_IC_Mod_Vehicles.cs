using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Localization;
using System.Collections.Generic;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Vehicles : IModData {
        // Modification Variables
        private ProtoRegistrator pReg = null;

        // Modification Definitions
        private readonly Dictionary<string, int> TruckCapacity = 
            new Dictionary<string, int>() {
            { "T1", 100 },
            { "T2", 300 },
            { "T3", 900 },
        };
        private readonly Dictionary<string, int> ExcavCapacity = 
            new Dictionary<string, int>() {
            { "T1", 25 },
            { "T2", 75 },
            { "T3", 225 },
        };
        private readonly Dictionary<string, double[]> TruckDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 2.0, 1.2, 50, 0.06, 0.09, 60, 20, 2.5, 1.25, 1.25 }},
            { "T2", new double[] { 2.5, 1.4, 50, 0.06, 0.09, 60, 20, 2.5, 1.5, 1.5 }},
            { "T3", new double[] { 1.5, 1.0, 50, 0.02, 0.06, 60, 15, 3.0, 1.5, 1.5 }},
            { "T2H", new double[] { 3.0, 1.6, 50, 0.06, 0.09, 60, 20, 2.5, 1.5, 1.5 }},
            { "T3H", new double[] { 1.8, 1.2, 50, 0.02, 0.06, 60, 15, 3.0, 1.5, 1.5 }},
        };
        private readonly Dictionary<string, double[]> ExcavDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 1.2, 0.8, 50, 0.04, 0.06, 10, 2, 2, 0, 0 }},
            { "T2", new double[] { 0.8, 0.6, 50, 0.025, 0.05, 8, 1, 2.5, 0, 0 }},
            { "T3", new double[] { 0.5, 0.4, 40, 0.015, 0.03, 4.0, 0.4, 3, 0, 0 }},
            { "T2H", new double[] { 1.0, 0.7, 50, 0.025, 0.05, 8, 1, 2.5, 0, 0 }},
            { "T3H", new double[] { 0.7, 0.5, 40, 0.015, 0.03, 4.0, 0.4, 3, 0, 0 }},
        };
        private readonly Dictionary<string, double[]> TrHarvDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 1.0, 0.7, 50, 0.04, 0.06, 8, 1.5, 2, 0, 0 }},
            { "T2", new double[] { 1.5, 1.0, 50, 0.05, 0.06, 10, 2, 2, 0, 0 }},
            { "T2H", new double[] { 1.8, 1.2, 50, 0.05, 0.06, 10, 2, 2, 0, 0 }},
        };
        private readonly Dictionary<string, double[]> TrPlantDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 1.0, 0.7, 50, 0.04, 0.06, 8, 1.5, 2, 0, 0 }},
            { "T1H", new double[] { 1.2, 0.8, 50, 0.04, 0.06, 8, 1.5, 2, 0, 0 }},
        };

        // Localization Definitions
        private readonly Dictionary<string, string[]> TruckLocStrings = 
            new Dictionary<string, string[]>() {
            { "T1", new string[] { "Heavy duty pickup truck with max capacity of {0}. It can go under transports that are at height {1} or higher.", "truck description, for instance {0}=20,{1}=2", "100" }},
            { "T2", new string[] { "Large industrial truck with max capacity of {0}. It can go under transports if they are at height {1} or higher.", "vehicle description, for instance {0}=20,{1}=2", "300" }},
            { "T3F", new string[] { "Large hauling truck with max capacity of {0}. This type can transport only liquid or gas products. It cannot go under transports.", "vehicle description, for instance {0}=150", "900" }},
            { "T3L", new string[] { "Large hauling truck with max capacity of {0}. This type can transport only loose products (coal for instance). It cannot go under transports.", "vehicle description, for instance {0}=150", "900" }},
        };
        private readonly Dictionary<string, string[]> ExcavLocStrings =
            new Dictionary<string, string[]>() {
            { "T1", new string[] { "Suitable for mining any terrain with max bucket capacity of {0}. It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=6", "25" }},
            { "T2", new string[] { "This is a serious mining machine with max bucket capacity of {0}! It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=18", "75" }},
            { "T3", new string[] { "Extremely large excavator that can mine any terrain with ease. It has bucket capacity of {0}. It cannot go under transports due to its size, use ramps to cross them.", "vehicle description, for instance {0}=60", "225" }},
        };

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            FFU_IC_Lib.ProtoReg = registrator;
            LocalizationManager.IgnoreDuplicates();

            // Truck Modifications - Capacity
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.TruckT1.Id, TruckCapacity["T1"]);
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.TruckT2.Id, TruckCapacity["T2"]);
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.TruckT3Fluid.Id, TruckCapacity["T3"]);
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.TruckT3Loose.Id, TruckCapacity["T3"]);
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.TruckT2H.Id, TruckCapacity["T2"]);
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.TruckT3FluidH.Id, TruckCapacity["T3"]);
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.TruckT3LooseH.Id, TruckCapacity["T3"]);

            // Truck Modifications - Drive Data
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TruckT1.Id, TruckDriveData["T1"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TruckT2.Id, TruckDriveData["T2"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TruckT3Fluid.Id, TruckDriveData["T3"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TruckT3Loose.Id, TruckDriveData["T3"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TruckT2H.Id, TruckDriveData["T2H"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TruckT3FluidH.Id, TruckDriveData["T3H"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TruckT3LooseH.Id, TruckDriveData["T3H"]);

            // Truck Modifications - Localization
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.TruckT1.Id, TruckLocStrings["T1"], true);
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.TruckT2.Id, TruckLocStrings["T2"], true);
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.TruckT3Fluid.Id, TruckLocStrings["T3F"]);
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.TruckT3Loose.Id, TruckLocStrings["T3L"]);
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.TruckT2H.Id, TruckLocStrings["T2"], true);
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.TruckT3FluidH.Id, TruckLocStrings["T3F"]);
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.TruckT3LooseH.Id, TruckLocStrings["T3L"]);

            // Excavator Modifications - Capacity
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.ExcavatorT1, ExcavCapacity["T1"]);
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.ExcavatorT2, ExcavCapacity["T2"]);
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.ExcavatorT3, ExcavCapacity["T3"]);
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.ExcavatorT2H, ExcavCapacity["T2"]);
            FFU_IC_Lib.SetVehicleCapacity(Ids.Vehicles.ExcavatorT3H, ExcavCapacity["T3"]);

            // Excavator Modifications - Drive Data
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.ExcavatorT1, ExcavDriveData["T1"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.ExcavatorT2, ExcavDriveData["T2"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.ExcavatorT3, ExcavDriveData["T3"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.ExcavatorT2H, ExcavDriveData["T2H"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.ExcavatorT3H, ExcavDriveData["T3H"]);

            // Excavator Modifications - Localization
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.ExcavatorT1, ExcavLocStrings["T1"]);
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.ExcavatorT2, ExcavLocStrings["T2"]);
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.ExcavatorT3, ExcavLocStrings["T3"]);
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.ExcavatorT2H, ExcavLocStrings["T2"]);
            FFU_IC_Lib.SetVehicleDescription(Ids.Vehicles.ExcavatorT3H, ExcavLocStrings["T3"]);

            // Tree Harvester Modifications - Drive Data
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TreeHarvesterT1, TrHarvDriveData["T1"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TreeHarvesterT2, TrHarvDriveData["T2"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TreeHarvesterT2H, TrHarvDriveData["T2H"]);

            // Tree Planter Modifications - Drive Data
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TreePlanterT1, TrPlantDriveData["T1"]);
            FFU_IC_Lib.SetVehicleDriveData(Ids.Vehicles.TreePlanterT1H, TrPlantDriveData["T1H"]);
        }
    }
}