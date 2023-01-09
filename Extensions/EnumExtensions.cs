using System;
using System.Collections.Generic;
using System.Linq;
using CriticalCommonLib.Enums;
using CriticalCommonLib.Models;

namespace CriticalCommonLib.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<TEnum> GetFlags<TEnum>(this TEnum enumValue)
            where TEnum : Enum
        {
            return EnumUtil.GetFlags<TEnum>().Where(ev => enumValue.HasFlag(ev));
        }
        
        public static string FormattedName(this CharacterSex characterSex)
        {
            switch (characterSex)
            {
                case CharacterSex.Both:
                    return "皆可";
                case CharacterSex.Either:
                    return "任一"; 
                case CharacterSex.Female:
                    return "女性"; 
                case CharacterSex.Male:
                    return "男性"; 
                case CharacterSex.FemaleOnly:
                    return "只有女性"; 
                case CharacterSex.MaleOnly:
                    return "只有男性"; 
                case CharacterSex.NotApplicable:
                    return "N/A"; 
            }

            return "未知";
        }
        public static string FormattedName(this CharacterRace characterRace)
        {
            switch (characterRace)
            {
                case CharacterRace.Any:
                    return "任意种族";
                case CharacterRace.Hyur:
                    return "人族"; 
                case CharacterRace.Elezen:
                    return "精灵族"; 
                case CharacterRace.Lalafell:
                    return "拉拉菲尔族"; 
                case CharacterRace.Miqote:
                    return "猫魅族"; 
                case CharacterRace.Roegadyn:
                    return "鲁加族"; 
                case CharacterRace.Viera:
                    return "维埃拉族"; 
                case CharacterRace.AuRa:
                    return "敖龙族"; 
                case CharacterRace.None:
                    return "无"; 
            }

            return "N/A";
        }

        public static List<InventoryType> GetTypes(this InventoryCategory category)
        {
            switch (category)
            {
                case InventoryCategory.CharacterBags:
                    return new List<InventoryType>()
                        {InventoryType.Bag0, InventoryType.Bag1, InventoryType.Bag2, InventoryType.Bag3};
                case InventoryCategory.RetainerBags:
                    return new List<InventoryType>()
                        {InventoryType.RetainerBag0, InventoryType.RetainerBag1, InventoryType.RetainerBag2, InventoryType.RetainerBag2, InventoryType.RetainerBag3, InventoryType.RetainerBag4, InventoryType.RetainerBag5, InventoryType.RetainerBag6};
                case InventoryCategory.Armoire:
                    return new List<InventoryType>()
                        {InventoryType.Armoire};
                case InventoryCategory.Crystals:
                    return new List<InventoryType>()
                        {InventoryType.Crystal,InventoryType.RetainerCrystal};
                case InventoryCategory.Currency:
                    return new List<InventoryType>()
                        {InventoryType.Currency,InventoryType.Currency};
                case InventoryCategory.CharacterEquipped:
                    return new List<InventoryType>()
                        {InventoryType.GearSet0};
                case InventoryCategory.CharacterArmoryChest:
                    return new List<InventoryType>()
                        {InventoryType.ArmoryBody, InventoryType.ArmoryEar , InventoryType.ArmoryFeet , InventoryType.ArmoryHand , InventoryType.ArmoryHead , InventoryType.ArmoryLegs , InventoryType.ArmoryLegs , InventoryType.ArmoryMain , InventoryType.ArmoryNeck , InventoryType.ArmoryOff , InventoryType.ArmoryRing , InventoryType.ArmoryWaist , InventoryType.ArmoryWrist};
                case InventoryCategory.GlamourChest:
                    return new List<InventoryType>()
                        {InventoryType.GlamourChest};
                case InventoryCategory.RetainerEquipped:
                    return new List<InventoryType>()
                        {InventoryType.RetainerEquippedGear};
                case InventoryCategory.RetainerMarket:
                    return new List<InventoryType>()
                        {InventoryType.RetainerMarket};
                case InventoryCategory.CharacterSaddleBags:
                    return new List<InventoryType>()
                        {InventoryType.SaddleBag0,InventoryType.SaddleBag1};
                case InventoryCategory.CharacterPremiumSaddleBags:
                    return new List<InventoryType>()
                        {InventoryType.PremiumSaddleBag0,InventoryType.PremiumSaddleBag1};
                case InventoryCategory.FreeCompanyBags:
                    return new List<InventoryType>()
                        {InventoryType.FreeCompanyBag0,InventoryType.FreeCompanyBag1,InventoryType.FreeCompanyBag2,InventoryType.FreeCompanyBag3,InventoryType.FreeCompanyBag4,InventoryType.FreeCompanyBag5,InventoryType.FreeCompanyBag6,InventoryType.FreeCompanyBag7,InventoryType.FreeCompanyBag8,InventoryType.FreeCompanyBag9,InventoryType.FreeCompanyBag10};
            }

            return new List<InventoryType>();
        }

        public static bool IsRetainerCategory(this InventoryCategory category)
        {
            return category is InventoryCategory.RetainerBags or InventoryCategory.RetainerEquipped or InventoryCategory
                .RetainerMarket or InventoryCategory.Crystals or InventoryCategory.Currency;
        }

        public static bool IsCharacterCategory(this InventoryCategory category)
        {
            return category != InventoryCategory.RetainerBags && category != InventoryCategory.RetainerEquipped && category !=
                InventoryCategory.RetainerMarket;
        }
        
        public static InventoryCategory ToInventoryCategory(this InventoryType type)
        {
            switch (type)
            {
                case InventoryType.Armoire:
                    return InventoryCategory.Armoire;
                case InventoryType.Bag0 :
                    return InventoryCategory.CharacterBags;
                case InventoryType.Bag1 :
                    return InventoryCategory.CharacterBags;
                case InventoryType.Bag2 :
                    return InventoryCategory.CharacterBags;
                case InventoryType.Bag3 :
                    return InventoryCategory.CharacterBags;
                case InventoryType.ArmoryBody :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmoryEar :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmoryFeet :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmoryHand :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmoryHead :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmoryLegs :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmoryMain :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmoryNeck :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmoryOff :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmoryRing :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmoryWaist :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmoryWrist :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.ArmorySoulCrystal :
                    return InventoryCategory.CharacterArmoryChest;
                case InventoryType.RetainerMarket :
                    return InventoryCategory.RetainerMarket;
                case InventoryType.RetainerEquippedGear :
                    return InventoryCategory.RetainerEquipped;
                case InventoryType.GlamourChest :
                    return InventoryCategory.GlamourChest;
                case InventoryType.RetainerBag0 :
                    return InventoryCategory.RetainerBags;
                case InventoryType.RetainerBag1 :
                    return InventoryCategory.RetainerBags;
                case InventoryType.RetainerBag2 :
                    return InventoryCategory.RetainerBags;
                case InventoryType.RetainerBag3 :
                    return InventoryCategory.RetainerBags;
                case InventoryType.RetainerBag4 :
                    return InventoryCategory.RetainerBags;
                case InventoryType.RetainerBag5 :
                    return InventoryCategory.RetainerBags;
                case InventoryType.SaddleBag0 :
                    return InventoryCategory.CharacterSaddleBags;
                case InventoryType.SaddleBag1 :
                    return InventoryCategory.CharacterSaddleBags;
                case InventoryType.PremiumSaddleBag0 :
                    return InventoryCategory.CharacterPremiumSaddleBags;
                case InventoryType.PremiumSaddleBag1 :
                    return InventoryCategory.CharacterPremiumSaddleBags;
                case InventoryType.FreeCompanyBag0 :
                    return InventoryCategory.FreeCompanyBags;
                case InventoryType.FreeCompanyBag1 :
                    return InventoryCategory.FreeCompanyBags;
                case InventoryType.FreeCompanyBag2 :
                    return InventoryCategory.FreeCompanyBags;
                case InventoryType.FreeCompanyBag3 :
                    return InventoryCategory.FreeCompanyBags;
                case InventoryType.FreeCompanyBag4 :
                    return InventoryCategory.FreeCompanyBags;
                case InventoryType.FreeCompanyBag5 :
                    return InventoryCategory.FreeCompanyBags;
                case InventoryType.FreeCompanyBag6 :
                    return InventoryCategory.FreeCompanyBags;
                case InventoryType.FreeCompanyBag7 :
                    return InventoryCategory.FreeCompanyBags;
                case InventoryType.FreeCompanyBag8 :
                    return InventoryCategory.FreeCompanyBags;
                case InventoryType.FreeCompanyBag9 :
                    return InventoryCategory.FreeCompanyBags;
                case InventoryType.FreeCompanyBag10 :
                    return InventoryCategory.FreeCompanyBags;
                case InventoryType.FreeCompanyCrystal :
                    return InventoryCategory.Crystals;
                case InventoryType.RetainerGil :
                    return InventoryCategory.Currency;
                case InventoryType.Currency :
                    return InventoryCategory.Currency;
                case InventoryType.FreeCompanyGil :
                    return InventoryCategory.Currency;
                case InventoryType.Crystal :
                    return InventoryCategory.Crystals;
                case InventoryType.RetainerCrystal :
                    return InventoryCategory.Crystals;
            }
            return InventoryCategory.Other;
        }
        public static string FormattedName(this InventoryCategory category)
        {
            switch (category)
            {
                case InventoryCategory.CharacterBags:
                    return "背包";
                case InventoryCategory.CharacterSaddleBags:
                    return "陆行鸟鞍囊";
                case InventoryCategory.CharacterPremiumSaddleBags:
                    return "付费陆行鸟鞍囊";
                case InventoryCategory.FreeCompanyBags:
                    return "部队箱";
                case InventoryCategory.CharacterArmoryChest:
                    return "兵装库";
                case InventoryCategory.GlamourChest:
                    return "投影台";
                case InventoryCategory.CharacterEquipped:
                    return "已装备";
                case InventoryCategory.Armoire:
                    return "收藏柜";
                case InventoryCategory.RetainerBags:
                    return "雇员背包";
                case InventoryCategory.RetainerMarket:
                    return "出售中";
                case InventoryCategory.Currency:
                    return "货币";
                case InventoryCategory.Crystals:
                    return "水晶";
                case InventoryCategory.RetainerEquipped:
                    return "雇员已装备";
            }

            return category.ToString();
        }
        public static string FormattedName(this InventoryCategory? category)
        {
            if (category.HasValue)
            {
                return FormattedName(category.Value);
            }

            return "未知";
        }
    }
}