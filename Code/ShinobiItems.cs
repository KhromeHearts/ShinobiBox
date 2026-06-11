using System;
using System.Threading;
using NCMS;
using UnityEngine;
using ReflectionUtility;
using System.Text;
using System.Linq;
using ai;
using NeoModLoader.services;
using NeoModLoader.api;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using NCMS.Utils;
using UnityEngine.Events;
using HarmonyLib;
using System.Collections.Generic;


namespace ShinobiBox
{
    internal static class ShinobiItems
    {
        private const string GroupId_ShinobiEquipment = "shinobi_equipment";

        internal static bool EquipItem(Actor actor, string itemId)
        {
            if (actor == null || actor.equipment == null || string.IsNullOrEmpty(itemId)) return false;

            EquipmentAsset asset = AssetManager.items.get(itemId);
            if (asset == null) return false;

            Item itemAsset = World.world.items.generateItem(asset);
            if (itemAsset == null) return false;

            actor.equipment.setItem(itemAsset, actor);
            return true;
        }

        public static void Init()
        {
            loadItemGroup();
            loadCustomItems();
        }

        private static void loadItemGroup()
        {
            AssetManager.item_groups.add(new ItemGroupAsset
            {
                id = GroupId_ShinobiEquipment,
                name = "shinobi_equipment_group",
                color = "#8B0000"
            });
        }
        
        private static void loadCustomItems()
        {
            #region Hidan's Scythe
            EquipmentAsset scythe = AssetManager.items.clone("weapon_triple_scythe", "$weapon");
            scythe.id = "weapon_triple_scythe";
            scythe.translation_key = scythe.id;
            scythe.material = "basic";
            scythe.equipment_subtype = "scythe";
            scythe.group_id = GroupId_ShinobiEquipment;
            scythe.quality = Rarity.R3_Legendary;
            scythe.is_pool_weapon = false;
            scythe.unlock(true);

            scythe.name_templates = new List<string>();
            scythe.equipment_value = 2500;
            scythe.special_effect_interval = 0.4f;
            scythe.equipment_type = EquipmentType.Weapon;
            scythe.attack_type = WeaponType.Range;
            scythe.name_class = "item_class_weapon";
            scythe.setCost(1, "adamantite", 12);

            scythe.base_stats = new BaseStats();
            scythe.base_stats.set("damage", 20f);
            scythe.base_stats.set("range", 2f);
            scythe.base_stats.set("critical_chance", 0.50f);
            
            scythe.path_icon = "ui/items/triple_scythe";
            scythe.path_gameplay_sprite = "weapons/weapon_triple_scythe"; 
            assignGameplaySpritesIfExists(scythe, scythe.id);
            
            AssetManager.items.list.AddItem(scythe);
            #endregion

            #region Kusanagi
            EquipmentAsset kusanagi = AssetManager.items.clone("weapon_kusanagi", "$weapon");
            kusanagi.id = "weapon_kusanagi";
            kusanagi.translation_key = kusanagi.id;
            kusanagi.material = "basic";
            kusanagi.equipment_subtype = "sword";
            kusanagi.group_id = GroupId_ShinobiEquipment;
            kusanagi.quality = Rarity.R3_Legendary;
            kusanagi.is_pool_weapon = false;
            kusanagi.unlock(true);
            kusanagi.equipment_type = EquipmentType.Weapon;
            kusanagi.name_templates = new List<string>();
            kusanagi.attack_type = WeaponType.Melee;
            kusanagi.name_class = "item_class_weapon";
            kusanagi.equipment_value = 12000;

            kusanagi.base_stats = new BaseStats();
            kusanagi.base_stats.set("damage", 52f);
            kusanagi.base_stats.set("attack_speed", 8f);
            kusanagi.base_stats.set("critical_chance", 0.20f);

            kusanagi.path_icon = "ui/items/kusanagi";
            kusanagi.path_gameplay_sprite = "weapons/weapon_kusanagi";
            assignGameplaySpritesIfExists(kusanagi, kusanagi.id);

            AssetManager.items.list.AddItem(kusanagi);
            #endregion

            #region Basic Kunai
            EquipmentAsset basicKunai = AssetManager.items.clone("weapon_basic_kunai", "$range");
            basicKunai.id = "weapon_basic_kunai";
            basicKunai.translation_key = basicKunai.id;
            basicKunai.material = "basic";
            basicKunai.projectile = "projectile_basic_kunai";
            basicKunai.equipment_subtype = "kunai";
            basicKunai.group_id = GroupId_ShinobiEquipment;
            basicKunai.quality = Rarity.R1_Rare;
            basicKunai.is_pool_weapon = false;
            basicKunai.unlock(true);

            basicKunai.base_stats = new BaseStats();
            basicKunai.base_stats.set("projectiles", 1f);
            basicKunai.base_stats.set("damage", 10f);
            basicKunai.base_stats.set("range", 6f);
            basicKunai.base_stats.set("attack_speed", 2f);
            basicKunai.base_stats.set("accuracy", 0.10f);
            basicKunai.path_slash_animation = "effects/slashes/slash_bow";

            basicKunai.equipment_value = 100;
            basicKunai.equipment_type = EquipmentType.Weapon;
            basicKunai.name_templates = new List<string>();
            basicKunai.name_class = "item_class_weapon";
            basicKunai.attack_type = WeaponType.Range;
            basicKunai.setCost(5, "common_metals", 2);
            basicKunai.minimum_city_storage_resource_1 = 1;

            basicKunai.path_icon = "ui/items/basic_kunai";
            basicKunai.path_gameplay_sprite = "weapons/weapon_basic_kunai";
            assignGameplaySpritesIfExists(basicKunai, basicKunai.id);

            AssetManager.items.list.AddItem(basicKunai);
            #endregion

            #region Gunbai
            EquipmentAsset gunbai = AssetManager.items.clone("weapon_gunbai", "$weapon");
            gunbai.id = "weapon_gunbai";
            gunbai.translation_key = gunbai.id;
            gunbai.material = "basic";
            gunbai.equipment_subtype = "hammer";
            gunbai.group_id = GroupId_ShinobiEquipment;
            gunbai.quality = Rarity.R3_Legendary;
            gunbai.is_pool_weapon = false;
            gunbai.unlock(true);

            gunbai.base_stats = new BaseStats();
            gunbai.base_stats.set("damage", 24f);
            gunbai.base_stats.set("knockback", 1.2f);
            gunbai.base_stats.set("armor", 6f);

            gunbai.equipment_value = 3000;
            gunbai.equipment_type = EquipmentType.Weapon;
            gunbai.name_templates = new List<string>();
            gunbai.name_class = "item_class_weapon";
            gunbai.attack_type = WeaponType.Melee;
            gunbai.setCost(1, "adamantite", 5, "iron", 25);
            gunbai.minimum_city_storage_resource_1 = 1;

            gunbai.path_icon = "ui/items/gunbai";
            gunbai.path_gameplay_sprite = "weapons/weapon_gunbai";
            assignGameplaySpritesIfExists(gunbai, gunbai.id);

            AssetManager.items.list.AddItem(gunbai);
            #endregion

            #region Paper Bomb Kunai
            EquipmentAsset paperBombKunai = AssetManager.items.clone("weapon_bomb_kunai", "$range");
            paperBombKunai.id = "weapon_bomb_kunai";
            paperBombKunai.translation_key = paperBombKunai.id;
            paperBombKunai.material = "basic";
            paperBombKunai.projectile = "projectile_paper_bomb_kunai";
            paperBombKunai.equipment_subtype = "kunai";
            paperBombKunai.group_id = GroupId_ShinobiEquipment;
            paperBombKunai.quality = Rarity.R2_Epic;
            paperBombKunai.is_pool_weapon = false;
            paperBombKunai.unlock(true);

            paperBombKunai.base_stats = new BaseStats();
            paperBombKunai.base_stats.set("projectiles", 1f);
            paperBombKunai.base_stats.set("damage", 12f);
            paperBombKunai.base_stats.set("range", 8f);
            paperBombKunai.base_stats.set("attack_speed", 1.5f);
            paperBombKunai.path_slash_animation = "effects/slashes/slash_bow";

            paperBombKunai.equipment_value = 1000;
            paperBombKunai.equipment_type = EquipmentType.Weapon;
            paperBombKunai.name_templates = new List<string>();
            paperBombKunai.name_class = "item_class_weapon";
            paperBombKunai.attack_type = WeaponType.Range;
            paperBombKunai.setCost(1, "common_metals", 1, "dragon_scales", 1);
            paperBombKunai.minimum_city_storage_resource_1 = 1;

            paperBombKunai.path_icon = "ui/items/bomb_kunai";
            paperBombKunai.path_gameplay_sprite = "weapons/weapon_bomb_kunai";
            assignGameplaySpritesIfExists(paperBombKunai, paperBombKunai.id);

            AssetManager.items.list.AddItem(paperBombKunai);
            #endregion

            #region Executioner's Blade
            EquipmentAsset executioner = AssetManager.items.clone("weapon_executioner_blade", "$weapon");
            executioner.id = "weapon_executioner_blade";
            executioner.translation_key = executioner.id;
            executioner.material = "basic";
            executioner.equipment_subtype = "greatsword";
            executioner.group_id = GroupId_ShinobiEquipment;
            executioner.quality = Rarity.R3_Legendary;
            executioner.is_pool_weapon = false;
            executioner.unlock(true);

            executioner.base_stats = new BaseStats();
            executioner.base_stats.set("damage", 24f);
            executioner.base_stats.set("range", 1.4f);
            executioner.base_stats.set("attack_speed", -5f);
            executioner.base_stats.set("knockback", 1.3f);

            executioner.equipment_value = 2500;
            executioner.equipment_type = EquipmentType.Weapon;
            executioner.name_templates = new List<string>();
            executioner.name_class = "item_class_weapon";
            executioner.attack_type = WeaponType.Melee;
            executioner.setCost(5, "mythril", 10, "iron", 5);
            executioner.minimum_city_storage_resource_1 = 1;

            executioner.path_icon = "ui/items/executioner_blade";
            executioner.path_gameplay_sprite = "weapons/weapon_executioner_blade";
            assignGameplaySpritesIfExists(executioner, executioner.id);

            AssetManager.items.list.AddItem(executioner);
            #endregion

            #region Hagoromo Staff
            EquipmentAsset hagoromoStaff = AssetManager.items.clone("weapon_hagoromo_staff", "$weapon");
            hagoromoStaff.id = "weapon_hagoromo_staff";
            hagoromoStaff.translation_key = hagoromoStaff.id;
            hagoromoStaff.material = "basic";
            hagoromoStaff.equipment_subtype = "staff";
            hagoromoStaff.group_id = GroupId_ShinobiEquipment;
            hagoromoStaff.quality = Rarity.R3_Legendary;
            hagoromoStaff.is_pool_weapon = false;
            hagoromoStaff.unlock(true);

            hagoromoStaff.base_stats = new BaseStats();
            hagoromoStaff.base_stats.set("damage", 30f);
            hagoromoStaff.base_stats.set("multiplier_damage", 0.30f);
            hagoromoStaff.base_stats.set("range", 1.5f);
            hagoromoStaff.base_stats.set("critical_chance", 0.28f);
            hagoromoStaff.base_stats.set("knockback", 1.5f);

            hagoromoStaff.equipment_value = 1000000;
            hagoromoStaff.equipment_type = EquipmentType.Weapon;
            hagoromoStaff.name_templates = new List<string>();
            hagoromoStaff.name_class = "item_class_weapon";
            hagoromoStaff.attack_type = WeaponType.Melee;
            hagoromoStaff.minimum_city_storage_resource_1 = 1;

            hagoromoStaff.path_icon = "ui/items/hagoromo_staff";
            hagoromoStaff.path_gameplay_sprite = "weapons/weapon_hagoromo_staff";
            assignGameplaySpritesIfExists(hagoromoStaff, hagoromoStaff.id);

            AssetManager.items.list.AddItem(hagoromoStaff);
            #endregion

            #region Yin Staff
            EquipmentAsset yinStaff = AssetManager.items.clone("weapon_yin_staff", "$weapon");
            yinStaff.id = "weapon_yin_staff";
            yinStaff.translation_key = yinStaff.id;
            yinStaff.material = "basic";
            yinStaff.equipment_subtype = "staff";
            yinStaff.group_id = GroupId_ShinobiEquipment;
            yinStaff.quality = Rarity.R3_Legendary;
            yinStaff.is_pool_weapon = false;
            yinStaff.unlock(true);

            yinStaff.base_stats = new BaseStats();
            yinStaff.base_stats.set("damage", 30f);
            yinStaff.base_stats.set("range", 1.5f);
            yinStaff.base_stats.set("critical_chance", 0.15f);

            yinStaff.equipment_value = 500000;
            yinStaff.equipment_type = EquipmentType.Weapon;
            yinStaff.name_templates = new List<string>();
            yinStaff.name_class = "item_class_weapon";
            yinStaff.attack_type = WeaponType.Melee;
            yinStaff.minimum_city_storage_resource_1 = 1;

            yinStaff.path_icon = "ui/items/yin_staff";
            yinStaff.path_gameplay_sprite = "weapons/weapon_yin_staff";
            assignGameplaySpritesIfExists(yinStaff, yinStaff.id);

            AssetManager.items.list.AddItem(yinStaff);
            #endregion

            #region Yang Staff
            EquipmentAsset yangStaff = AssetManager.items.clone("weapon_yang_staff", "$weapon");
            yangStaff.id = "weapon_yang_staff";
            yangStaff.translation_key = yangStaff.id;
            yangStaff.material = "basic";
            yangStaff.equipment_subtype = "staff";
            yangStaff.group_id = GroupId_ShinobiEquipment;
            yangStaff.quality = Rarity.R3_Legendary;
            yangStaff.is_pool_weapon = false;
            yangStaff.unlock(true);

            yangStaff.base_stats = new BaseStats();
            yangStaff.base_stats.set("damage", 30f);
            yangStaff.base_stats.set("range", 1.5f);
            yangStaff.base_stats.set("knockback", 1.5f);

            yangStaff.equipment_value = 500000;
            yangStaff.equipment_type = EquipmentType.Weapon;
            yangStaff.name_templates = new List<string>();
            yangStaff.name_class = "item_class_weapon";
            yangStaff.attack_type = WeaponType.Melee;
            yangStaff.minimum_city_storage_resource_1 = 1;

            yangStaff.path_icon = "ui/items/yang_staff";
            yangStaff.path_gameplay_sprite = "weapons/weapon_yang_staff";
            assignGameplaySpritesIfExists(yangStaff, yangStaff.id);

            AssetManager.items.list.AddItem(yangStaff);
            #endregion

            #region Flying Thunder God Kunai (Item)
            EquipmentAsset ftgItem = AssetManager.items.clone("weapon_ftg_kunai", "$range");
            ftgItem.id = "weapon_ftg_kunai";
            ftgItem.translation_key = ftgItem.id;
            ftgItem.material = "basic";
            ftgItem.projectile = "projectile_ftg_kunai";
            ftgItem.equipment_subtype = "kunai";
            ftgItem.group_id = GroupId_ShinobiEquipment;
            ftgItem.quality = Rarity.R3_Legendary;
            ftgItem.is_pool_weapon = false;
            ftgItem.unlock(true);
            ftgItem.durability = 100;

            ftgItem.base_stats = new BaseStats();
            ftgItem.base_stats.set("projectiles", 1f);
            ftgItem.base_stats.set("damage", 8f);
            ftgItem.base_stats.set("speed", 10f);
            ftgItem.base_stats.set("range", 9f);
            ftgItem.base_stats.set("attack_speed", 3f);
            ftgItem.path_slash_animation = "effects/slashes/slash_bow";


            ftgItem.equipment_value = 8000;
            ftgItem.equipment_type = EquipmentType.Weapon;
            ftgItem.name_templates = new List<string>();
            ftgItem.name_class = "item_class_weapon";
            ftgItem.attack_type = WeaponType.Range;
            ftgItem.setCost(1, "adamantite", 1, "mythril", 25);
            ftgItem.minimum_city_storage_resource_1 = 1;

            ftgItem.path_icon = "ui/items/ftg";
            ftgItem.path_gameplay_sprite = "weapons/weapon_ftg_kunai";
            assignGameplaySpritesIfExists(ftgItem, ftgItem.id);

            AssetManager.items.list.AddItem(ftgItem);
            #endregion

            #region Shinobi Flak Jacket
            EquipmentAsset flakJacket = AssetManager.items.clone("armor_shinobi_flak_jacket", "$armor");
            flakJacket.id = "armor_shinobi_flak_jacket";
            flakJacket.translation_key = flakJacket.id;
            flakJacket.material = "basic";
            flakJacket.group_id = GroupId_ShinobiEquipment;
            flakJacket.name_templates = new List<string>();
            flakJacket.equipment_type = EquipmentType.Armor;
            flakJacket.name_class = "item_class_armor";
            flakJacket.quality = Rarity.R1_Rare;
            flakJacket.equipment_value = 350;
            flakJacket.unlock(true);

            flakJacket.base_stats = new BaseStats();
            flakJacket.base_stats.set("armor", 10f);
            flakJacket.base_stats.set("health", 100f);
            flakJacket.base_stats.set("accuracy", 0.05f);

            flakJacket.setCost(3, "leather", 1, "common_metals", 3);
            flakJacket.minimum_city_storage_resource_1 = 1;

            flakJacket.path_icon = "ui/items/WIP";

            AssetManager.items.list.AddItem(flakJacket);
            #endregion

            #region Warring States Armor
            EquipmentAsset warringArmor = AssetManager.items.clone("armor_warring_states", "$armor");
            warringArmor.id = "armor_warring_states";
            warringArmor.translation_key = warringArmor.id;
            warringArmor.material = "basic";
            warringArmor.group_id = GroupId_ShinobiEquipment;
            warringArmor.name_templates = new List<string>();
            warringArmor.equipment_type = EquipmentType.Armor;
            warringArmor.name_class = "item_class_armor";
            warringArmor.quality = Rarity.R3_Legendary;
            warringArmor.equipment_value = 1500;
            warringArmor.unlock(true);

            warringArmor.base_stats = new BaseStats();
            warringArmor.base_stats.set("armor", 22f);
            warringArmor.base_stats.set("health", 200f);

            warringArmor.minimum_city_storage_resource_1 = 1;
            warringArmor.setCost(3, "gold", 3, "iron", 5);

            warringArmor.path_icon = "ui/items/WIP";

            AssetManager.items.list.AddItem(warringArmor);
            #endregion

            #region Madara's Six Paths Chakra Cloak
            EquipmentAsset madaraSixPathsCloak = AssetManager.items.clone("armor_madara_sixpaths_cloak", "$armor");
            madaraSixPathsCloak.id = "armor_madara_sixpaths_cloak";
            madaraSixPathsCloak.translation_key = madaraSixPathsCloak.id;
            madaraSixPathsCloak.material = "basic";
            madaraSixPathsCloak.group_id = GroupId_ShinobiEquipment;
            madaraSixPathsCloak.name_templates = new List<string>();
            madaraSixPathsCloak.equipment_type = EquipmentType.Armor;
            madaraSixPathsCloak.name_class = "item_class_armor";
            madaraSixPathsCloak.quality = Rarity.R3_Legendary;
            madaraSixPathsCloak.equipment_value = 20000;
            madaraSixPathsCloak.unlock(true);

            madaraSixPathsCloak.base_stats = new BaseStats();
            madaraSixPathsCloak.base_stats.set("armor", 26f);
            madaraSixPathsCloak.base_stats.set("health", 320f);
            madaraSixPathsCloak.base_stats.set("speed", 8f);
            madaraSixPathsCloak.base_stats.set("damage", 10f);

            madaraSixPathsCloak.setCost(3000, "mythril", 35, "adamantite", 10);
            madaraSixPathsCloak.minimum_city_storage_resource_1 = 1;

            madaraSixPathsCloak.path_icon = "ui/items/madara_sixpaths_cloak";

            AssetManager.items.list.AddItem(madaraSixPathsCloak);
            #endregion
        }

        private static void assignGameplaySpritesIfExists(EquipmentAsset item, string id)
        {
            Sprite[] sprites = getWeaponSprites(id);
            if (sprites.Length > 0)
            {
                item.gameplay_sprites = sprites;
            }
        }


        public static Sprite[] getWeaponSprites(string id)
        {
            var sprite = Resources.Load<Sprite>("weapons/" + id);
            if (sprite != null)
                return new Sprite[] { sprite };
            else
            {
                return Array.Empty<Sprite>();
            }
        }

    }
}
