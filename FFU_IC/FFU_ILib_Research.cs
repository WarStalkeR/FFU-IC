using Mafi;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Research;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using System.Reflection;

namespace FFU_Industrial_Lib {
    public static partial class FFU_ILib {
        /// <remarks>
        /// Modifies vehicle limit bonus of a <b>ResearchNodeProto</b>. Requires <c>integer</c> value.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Define new storage capacity as <b>int</b> variable:<br/>
        /// <c>int newVehicleCapacity = 40;</c>
        /// 
        /// <br/><br/>Apply new vehicle limit value via <b>ResearchNodeProto</b> identifier:<br/>
        /// <c>SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease5, newStorageCapacity);</c>
        /// </remarks>
        public static void SetTechVehicleCapacity(ResearchNodeProto.ID refResearchID, int newVehCap) {
            if (_pReg == null) { ModLog.Warning($"SetTechVehicleCapacity: the ProtoRegistrator is not referenced!"); return; };
            ResearchNodeProto refResearch = ResearchRef(refResearchID);
            if (refResearch == null) { ModLog.Warning($"SetTechVehicleCapacity: 'refReserach' is undefined!"); return; }
            refResearch.Units.ForEach(refUnit => {
                if (refUnit is VehicleLimitIncreaseUnlock) {
                    VehicleLimitIncreaseUnlock refUnitVehCap = (VehicleLimitIncreaseUnlock)refUnit;
                    ModLog.Info($"{refResearch.Id} Vehicle Capacity: {refUnitVehCap.LimitIncrease} -> {newVehCap}");
                    FieldInfo fieldLimit = typeof(VehicleLimitIncreaseUnlock).GetField("LimitIncrease", BindingFlags.Instance | BindingFlags.Public);
                    fieldLimit.SetValue(refUnit, newVehCap);
                }
            });
            SyncProtoMod(refResearch);
        }

        /// <remarks>
        /// Modifies title text of a <b>ResearchNodeProto</b> unlock item. Requires <c>string[]</c> array and <c>integer</c> value.<br/><br/>
        /// 
        /// <br/><br/>The <b>string[]</b> array requires these parameters: 
        /// <br/><b>locale identifier</b>, <b>singular variant</b>, <b>plural variant</b>, <b>locale description</b>. 
        /// <br/>For reference just use the original values.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Define new locale string array and argument variable:
        /// <br/><c>string newTechTitle = new string[] { "MyTechBonusTitle", "+{0} ITEM MAX", "+{0} ITEMS MAX", "max increase, all caps" };</c>
        /// <br/><c>int newItemCap = 100;</c>
        /// 
        /// <br/><br/>Apply new string values to unlock item via <b>ResearchNodeProto</b> identifier:<br/>
        /// <c>SetTechUnitTitle&lt;CorrectType&gt;(Ids.Research.VehicleCapIncrease5, newTechTitle, newItemCap);</c>
        /// <br/>Ensure that unlock item <b>type</b> (or class) is chosen correctly!
        /// </remarks>
        public static void SetTechUnitTitle<T>(ResearchNodeProto.ID refResearchID, string[] strSet, int refVal) {
            if (_pReg == null) { ModLog.Warning($"SetTechUnitTitle: the ProtoRegistrator is not referenced!"); return; };
            ResearchNodeProto refResearch = ResearchRef(refResearchID);
            if (refResearch == null) { ModLog.Warning($"SetTechUnitTitle: 'refReserach' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetTechUnitTitle: 'strSet' is undefined!"); return; }
            refResearch.Units.ForEach(refUnit => {
                if (refUnit is T) {
                    TypeInfo typeUnit = typeof(T).GetTypeInfo();
                    FieldInfo fieldTitle = typeUnit.GetDeclaredField("<Title>k__BackingField");
                    if (fieldTitle != null) {
                        ModLog.Info($"{refResearch.Id} unit title modified.");
                        LocStr1Plural techLoc = Loc.Str1Plural(strSet[0], strSet[1], strSet[2], strSet[3]);
                        LocStrFormatted techVehCapTitle = techLoc.Format(refVal.ToString(), refVal);
                        fieldTitle.SetValue(refUnit, techVehCapTitle);
                    }
                }
            });
            SyncProtoMod(refResearch);
        }

        /// <remarks>
        /// Modifies description of a <b>ResearchNodeProto</b>. For reference use description values in example below.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Activate the <b>LocalizationManager</b> override to avoid exceptions:<br/>
        /// <c>LocalizationManager.IgnoreDuplicates();</c>
        /// 
        /// <br/><br/>Define new vehicle localization as <b>string[]</b> array (using relevant strings):
        /// <br/><c>string[] researchString = new string[] { "Unlocks something and increases by {0}.", "{0}=25" };</c>
        /// <br/><c>int researchValue = 42;</c>
        /// 
        /// <br/><br/>Apply new description strings via <b>ResearchNodeProto</b> identifier:<br/>
        /// <c>SetTechDescription(Ids.Research.VehicleCapIncrease5, researchString, researchValue);</c>
        /// </remarks>
        public static void SetTechDescription(ResearchNodeProto.ID refResearchID, string[] strSet, int refVal) {
            if (_pReg == null) { ModLog.Warning($"SetTechDescription: the ProtoRegistrator is not referenced!"); return; };
            ResearchNodeProto refResearch = ResearchRef(refResearchID);
            if (refResearch == null) { ModLog.Warning($"SetTechDescription: 'refReserach' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetTechDescription: 'strSet' is undefined!"); return; }
            LocStr1 locStr = Loc.Str1(refResearch.Id.Value + "__desc", strSet[0], strSet[1]);
            LocStr locDesc = LocalizationManager.CreateAlreadyLocalizedStr(refResearch.Id.Value + "_formatted", locStr.Format(refVal.ToString()).Value);
            TypeInfo typeProto = typeof(Mafi.Core.Prototypes.Proto).GetTypeInfo();
            FieldInfo fieldStrings = typeProto.GetDeclaredField("<Strings>k__BackingField");
            if (fieldStrings != null) {
                ModLog.Info($"{refResearch.Id} description modified.");
                Mafi.Core.Prototypes.Proto.Str currStr = (Mafi.Core.Prototypes.Proto.Str)fieldStrings.GetValue(refResearch);
                Mafi.Core.Prototypes.Proto.Str newStr = new Mafi.Core.Prototypes.Proto.Str(currStr.Name, locDesc);
                fieldStrings.SetValue(refResearch, newStr);
                FieldInfo fieldDesc = typeof(ResearchNodeProto).GetField("ResolvedDescription", BindingFlags.Instance | BindingFlags.Public);
                fieldDesc.SetValue(refResearch, refResearch.Strings.DescShort);
                SyncProtoMod(refResearch);
            }
        }

        /// <remarks>
        /// Adds recipe as unlockable to the <b>ResearchNodeProto</b>. Optionally allows to define new index of added recipe.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Add existing recipe via <b>ResearchNodeProto</b>, <b>MachineProto</b> and <b>RecipeProto</b> identifiers:<br/>
        /// <c>AddTechRecipe(Ids.Research.ArcFurnace2, Ids.Machines.ArcFurnace2, NewRecipeIdentifier);</c>
        /// </remarks>
        public static void AddTechRecipe(ResearchNodeProto.ID refResearchID, MachineProto.ID refMachineID, RecipeProto.ID refNewUnitID, bool hideInUI = false, int index = -1) {
            if (_pReg == null) { ModLog.Warning($"AddTechRecipe: the ProtoRegistrator is not referenced!"); return; };
            ResearchNodeProto refResearch = ResearchRef(refResearchID);
            MachineProto refMachine = MachineRef(refMachineID);
            RecipeProto refNewUnit = RecipeRef(refNewUnitID);
            if (refResearch == null) { ModLog.Warning($"AddTechRecipe: 'refReserach' is undefined!"); return; }
            if (refMachine == null) { ModLog.Warning($"AddTechRecipe: 'refMachine' is undefined!"); return; }
            if (refNewUnit == null) { ModLog.Warning($"AddTechRecipe: 'refNewUnit' is undefined!"); return; }
            ModLog.Info($"Added new unit {refNewUnit.Id} to research {refResearch.Id}.");
            Set<IUnlockNodeUnit> newUnitList = new Set<IUnlockNodeUnit>(0, null);
            int idx = 0;
            bool wasAdded = false;
            refResearch.Units.ForEach(refUnit => {
                if (index >= 0 && idx == index) {
                    newUnitList.AddAndAssertNew(new RecipeUnlock(refNewUnit, refMachine, hideInUI));
                    wasAdded = true;
                }
                newUnitList.Add(refUnit);
                idx++;
            });
            if (index < 0 || !wasAdded) newUnitList.AddAndAssertNew(new RecipeUnlock(refNewUnit, refMachine, hideInUI));
            FieldInfo fieldUnits = typeof(ResearchNodeProto).GetField("Units", BindingFlags.Instance | BindingFlags.Public);
            fieldUnits.SetValue(refResearch, newUnitList.ToImmutableArray());
            SyncProtoMod(refResearch);
        }

        /// <remarks>
        /// Removes recipe as unlockable from the <b>ResearchNodeProto</b>. Has no additional specific parameters.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Remove existing recipe via <b>ResearchNodeProto</b> and <b>RecipeProto</b> identifiers:<br/>
        /// <c>RemoveTechRecipe(Ids.Research.ArcFurnace2, RecipeIdentifierToRemove);</c>
        /// </remarks>
        public static void RemoveTechRecipe(ResearchNodeProto.ID refResearchID, RecipeProto.ID refOldUnitID, bool hideInUI = false) {
            if (_pReg == null) { ModLog.Warning($"RemoveTechRecipe: the ProtoRegistrator is not referenced!"); return; };
            ResearchNodeProto refResearch = ResearchRef(refResearchID);
            RecipeProto refOldUnit = RecipeRef(refOldUnitID);
            if (refResearch == null) { ModLog.Warning($"RemoveTechRecipe: 'refReserach' is undefined!"); return; }
            if (refOldUnit == null) { ModLog.Warning($"RemoveTechRecipe: 'refOldUnit' is undefined!"); return; }
            ModLog.Info($"Removed existing unit {refOldUnit.Id} from research {refResearch.Id}.");
            Set<IUnlockNodeUnit> newUnitList = new Set<IUnlockNodeUnit>(0, null);
            refResearch.Units.ForEach(refUnit => {
                if (!(refUnit is RecipeUnlock refRecipeUnlock) ||
                refRecipeUnlock.Proto.Id != refOldUnit.Id) {
                    newUnitList.Add(refUnit);
                }
            });
            FieldInfo fieldUnits = typeof(ResearchNodeProto).GetField("Units", BindingFlags.Instance | BindingFlags.Public);
            fieldUnits.SetValue(refResearch, newUnitList.ToImmutableArray());
            SyncProtoMod(refResearch);
        }

        /// <remarks>
        /// Make all unlockable recipes visible for a <b>ResearchNodeProto</b>, if they are initially hidden.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Make all recipes visible via <b>ResearchNodeProto</b> identifier:<br/>
        /// <c>UnhideRecipes(Ids.Research.ArcFurnace2);</c>
        /// </remarks>
        public static void UnhideRecipes(ResearchNodeProto.ID refResearchID) {
            if (_pReg == null) { ModLog.Warning($"UnhideRecipes: the ProtoRegistrator is not referenced!"); return; };
            ResearchNodeProto refResearch = ResearchRef(refResearchID);
            if (refResearch == null) { ModLog.Warning($"UnhideRecipes: 'refReserach' is undefined!"); return; }
            refResearch.Units.ForEach(refUnit => {
                RecipeUnlock refRecipeUnlock = refUnit as RecipeUnlock;
                if (refRecipeUnlock != null && refRecipeUnlock.m_hideInUi) {
                    ModLog.Info($"Making recipe {refRecipeUnlock.Proto.Id} visible in the {refResearch.Id} research.");
                    FieldInfo fieldHideInUi = typeof(ProtoUnlock).GetField("m_hideInUi", BindingFlags.Instance | BindingFlags.NonPublic);
                    fieldHideInUi.SetValue(refRecipeUnlock, false);
                }
            });
            SyncProtoMod(refResearch);
        }

        /// <remarks>
        /// Modifies 2D position of the <b>ResearchNodeProto</b> in the UI. Requires use of the <c>Vector2i</c> parameter.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Modify the position in the UI via <b>ResearchNodeProto</b> identifier:<br/>
        /// <c>SetTechPosition(Ids.Research.ArcFurnace2, new Vector2i(115,25));</c>
        /// </remarks>
        public static void SetTechPosition(ResearchNodeProto.ID refResearchID, Vector2i nodePos) {
            if (_pReg == null) { ModLog.Warning($"SetTechPosition: the ProtoRegistrator is not referenced!"); return; };
            Option<ResearchNodeProto> optResearch = ResearchRef(refResearchID);
            if (optResearch.IsNone) {
                ModLog.Warning($"Failed to set position of research node: {refResearchID}");
                return;
            }
            optResearch.Value.GridPosition = nodePos;
        }

        /// <remarks>
        /// Makes the <b>ResearchNodeProto</b> dependent on the other <b>ResearchNodeProto</b>. Everything else is automatic.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Modify the unlock dependency via <b>ResearchNodeProto</b> identifier:<br/>
        /// <c>SetTechParent(Ids.Research.AdvancedSmelting, Ids.Research.ArcFurnace2);</c>
        /// </remarks>
        public static void SetTechParent(ResearchNodeProto.ID refParentID, ResearchNodeProto.ID refChildID) {
            if (_pReg == null) { ModLog.Warning($"SetTechParent: the ProtoRegistrator is not referenced!"); return; };
            Option<ResearchNodeProto> optParent = ResearchRef(refParentID);
            Option<ResearchNodeProto> optChild = ResearchRef(refChildID);
            if (optParent.IsNone || optChild.IsNone) {
                ModLog.Warning($"Failed to set research relationship: {refChildID} => {refParentID}");
                return;
            }
            optChild.Value.AddParent(optParent.Value);
        }
    }
}