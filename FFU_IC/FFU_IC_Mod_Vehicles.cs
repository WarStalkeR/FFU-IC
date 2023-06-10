using Mafi.Core.Mods;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Vehicles : IModData {
        //Excavators Capacity
        public const int excavCapT1 = 25;
        public const int excavCapT2 = 100;
        public const int excavCapT3 = 300;
        //Trucks Capacity
        public const int truckCapT1 = 100;
        public const int truckCapT2 = 300;
        public const int truckCapT3 = 900;
        //Excavators Performance
        public const double excavFwdSpeedT1 = 1.2;
        public const double excavFwdSpeedT2 = 0.8;
        public const double excavFwdSpeedT3 = 0.5;
        public const double excavBckSpeedT1 = 0.8;
        public const double excavBckSpeedT2 = 0.6;
        public const double excavBckSpeedT3 = 0.4;
        //Trucks Performance
        public const double truckFwdSpeedT1 = 2.0;
        public const double truckFwdSpeedT2 = 2.5;
        public const double truckFwdSpeedT3 = 1.5;
        public const double truckBckSpeedT1 = 1.2;
        public const double truckBckSpeedT2 = 1.4;
        public const double truckBckSpeedT3 = 1.0;
        public void RegisterData(ProtoRegistrator registrator) {
            Register_VehiclesTRC(registrator); //Mafi.Base.Prototypes.Vehicles.TrucksData
            Register_VehiclesEXC(registrator); //Mafi.Base.Prototypes.Vehicles.ExcavatorsData
        }
    }
}