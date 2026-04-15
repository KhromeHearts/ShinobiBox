using System;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using NeoModLoader.services;
using NeoModLoader.api;

namespace ShinobiBox
{
    public static class ShinobiActors
    {
        public const string HashiramaActorId = "hashirama_senju";
        public const string MadaraActorId = "madara_uchiha";
        public const string KuramaActorId = "kurama";

        public const string JuubiActorId = "juubi";


        public static void Init()
        {
            loadAssets();
        }

        public static void loadAssets()
        {
            if (AssetManager.actor_library.get(HashiramaActorId) == null)
            {
                ActorAsset hashirama = AssetManager.actor_library.clone(HashiramaActorId, "$mob$");
                hashirama.id = HashiramaActorId;
                hashirama.name_locale = "hashirama senju";
                hashirama.power_id = "spawn_hashirama_senju";

                hashirama.is_humanoid = true;
                hashirama.use_phenotypes = false;
                hashirama.unit_other = true;
                hashirama.has_advanced_textures = false;
                hashirama.check_flip = delegate { return true; };
                hashirama.name_template_sets = new string[] { "human_default_set" };

                hashirama.job = new string[] { "random_move" };
                hashirama.kingdom_id_wild = "rouge";
                hashirama.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                hashirama.animation_swim = ActorAnimationSequences.walk_0;
                hashirama.texture_asset = new ActorTextureSubAsset("actors/hashirama/", hashirama.has_advanced_textures);
                hashirama._cached_sprite = Resources.Load<Sprite>("GameResources/actors/hashirama/Main/walk_0");
                hashirama.texture_id = "hashirama";
                hashirama.icon = "units/hashirama";

                hashirama.name_taxonomic_kingdom = "animalia";
                hashirama.name_taxonomic_phylum = "chordata";
                hashirama.name_taxonomic_class = "mammalia";
                hashirama.name_taxonomic_order = "primates";
                hashirama.name_taxonomic_family = "hominidae";
                hashirama.name_taxonomic_genus = "homo";
                hashirama.name_taxonomic_species = "sapiens";

                hashirama.can_be_killed_by_stuff = true;
                hashirama.can_be_killed_by_life_eraser = true;
                hashirama.can_attack_buildings = true;
                hashirama.can_be_moved_by_powers = true;
                hashirama.can_be_hurt_by_powers = true;
                hashirama.can_be_inspected = true;
                hashirama.visible_on_minimap = true;
                hashirama.use_items = true;
                hashirama.take_items = true;
                hashirama.can_talk_with = false;
                hashirama.control_can_talk = false;
                hashirama.can_evolve_into_new_species = false;
                hashirama.can_have_subspecies = false;
                hashirama.disable_jump_animation = true;
                hashirama.has_soul = true;
                hashirama.has_baby_form = false;
                hashirama.render_heads_for_babies = false;
                hashirama.body_separate_part_hands = true;
                hashirama.shadow = false;

                hashirama.force_land_creature = true;
                hashirama.force_ocean_creature = false;
                hashirama.flying = false;
                hashirama.very_high_flyer = false;
                hashirama.run_to_water_when_on_fire = true;

                hashirama.can_turn_into_zombie = false;
                hashirama.can_turn_into_demon_in_age_of_chaos = false;
                hashirama.can_turn_into_ice_one = false;
                hashirama.can_turn_into_tumor = false;
                hashirama.can_turn_into_mush = false;
                hashirama.can_be_killed_by_divine_light = true;
                hashirama.ignored_by_infinity_coin = false;
                hashirama.actor_size = ActorSize.S13_Human;
                hashirama.can_edit_equipment = true;
                hashirama.inspect_home = true;

                hashirama.base_stats["lifespan"] = 125f;
                hashirama.base_stats["health"] = 1200f;
                hashirama.base_stats["damage"] = 25f;
                hashirama.base_stats["speed"] = 25f;
                hashirama.base_stats["attack_speed"] = 2f;
                hashirama.base_stats["intelligence"] = 200f;
                hashirama.base_stats["stamina"] = 600f;
                hashirama.base_stats["mana"] = 3500f;
                hashirama.base_stats["armor"] = 30f;
                hashirama.base_stats["critical_chance"] = 0.40f;
                hashirama.base_stats["knockback"] = 0.2f;
                hashirama.base_stats["accuracy"] = 2f;
                hashirama.base_stats["targets"] = 1f;
                hashirama.base_stats["scale"] = 0.15f;
                hashirama.base_stats["chakra"] = 500f;
                hashirama.base_stats["mass_2"] = 45f;

                hashirama.addTrait("immortal");
                hashirama.addTrait("fire_proof");
                hashirama.addTrait("regeneration");
                hashirama.addTrait("senju_clan");
                hashirama.addTrait("wood_release");
                hashirama.addTrait("hashi_cells");
                hashirama.addTrait("wood_sage_mode");
                hashirama.addTrait("will_of_fire");
                hashirama.addTrait("vast_chakra_reserve");
                hashirama.addTrait("fireN");
                hashirama.addTrait("earthN");
                hashirama.addTrait("waterN");
                hashirama.addTrait("lightningN");
                hashirama.addTrait("windN");
                hashirama.addTrait("god_of_shinobi");

                AssetManager.actor_library.loadTexturesAndSprites(hashirama);
            }

            if (AssetManager.actor_library.get(MadaraActorId) == null)
            {
                ActorAsset madara = AssetManager.actor_library.clone(MadaraActorId, "$mob$");
                madara.id = MadaraActorId;
                madara.name_locale = "madara uchiha";
                madara.power_id = "spawn_madara_uchiha";

                madara.is_humanoid = true;
                madara.use_phenotypes = false;
                madara.unit_other = true;
                madara.has_advanced_textures = false;
                madara.check_flip = delegate { return true; };
                madara.name_template_sets = new string[] { "human_default_set" };

                madara.job = new string[] { "random_move" };
                madara.kingdom_id_wild = "rouge";
                madara.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                madara.animation_swim = ActorAnimationSequences.walk_0;
                madara.texture_asset = new ActorTextureSubAsset("actors/madara/", madara.has_advanced_textures);
                madara._cached_sprite = Resources.Load<Sprite>("GameResources/actors/madara/Main/walk_0");
                madara.default_weapons = new string[] { "weapon_gunbai" };
                madara.texture_id = "madara";
                madara.icon = "units/madara";

                madara.name_taxonomic_kingdom = "animalia";
                madara.name_taxonomic_phylum = "chordata";
                madara.name_taxonomic_class = "mammalia";
                madara.name_taxonomic_order = "primates";
                madara.name_taxonomic_family = "hominidae";
                madara.name_taxonomic_genus = "homo";
                madara.name_taxonomic_species = "sapiens";

                madara.can_be_killed_by_stuff = true;
                madara.can_be_killed_by_life_eraser = true;
                madara.can_attack_buildings = true;
                madara.can_be_moved_by_powers = true;
                madara.can_be_hurt_by_powers = true;
                madara.can_be_inspected = true;
                madara.visible_on_minimap = true;
                madara.use_items = true;
                madara.take_items = true;
                madara.can_talk_with = false;
                madara.control_can_talk = false;
                madara.can_evolve_into_new_species = false;
                madara.can_have_subspecies = false;
                madara.disable_jump_animation = true;
                madara.has_soul = true;
                madara.has_baby_form = false;
                madara.render_heads_for_babies = false;
                madara.body_separate_part_hands = true;
                madara.shadow = false;

                madara.force_land_creature = true;
                madara.force_ocean_creature = false;
                madara.flying = false;
                madara.very_high_flyer = false;
                madara.run_to_water_when_on_fire = true;

                madara.can_turn_into_zombie = false;
                madara.can_turn_into_demon_in_age_of_chaos = false;
                madara.can_turn_into_ice_one = false;
                madara.can_turn_into_tumor = false;
                madara.can_turn_into_mush = false;
                madara.can_be_killed_by_divine_light = true;
                madara.ignored_by_infinity_coin = false;
                madara.actor_size = ActorSize.S13_Human;
                madara.can_edit_equipment = true;

                madara.base_stats["lifespan"] = 125f;
                madara.base_stats["health"] = 1000f;
                madara.base_stats["damage"] = 25f;
                madara.base_stats["speed"] = 25f;
                madara.base_stats["attack_speed"] = 2f;
                madara.base_stats["intelligence"] = 200f;
                madara.base_stats["stamina"] = 600f;
                madara.base_stats["mana"] = 3500f;
                madara.base_stats["armor"] = 30f;
                madara.base_stats["critical_chance"] = 0.40f;
                madara.base_stats["knockback"] = 0.2f;
                madara.base_stats["accuracy"] = 6f;
                madara.base_stats["targets"] = 1f;
                madara.base_stats["scale"] = 0.15f;
                madara.base_stats["chakra"] = 500f;
                madara.base_stats["mass_2"] = 40f;

                madara.addTrait("immortal");
                madara.addTrait("fire_proof");
                madara.addTrait("regeneration");
                madara.addTrait("uchiha_clan");
                madara.addTrait("madara_eternal_mangekyo");
                madara.addTrait("vast_chakra_reserve");
                madara.addTrait("fireN");
                madara.addTrait("earthN");
                madara.addTrait("waterN");
                madara.addTrait("lightningN");
                madara.addTrait("windN");
                madara.addTrait("ghost_of_uchiha");

                AssetManager.actor_library.loadTexturesAndSprites(madara);
            }

            if (AssetManager.actor_library.get(KuramaActorId) == null)
            {
                ActorAsset kurama = AssetManager.actor_library.clone(KuramaActorId, "$mob$");
                kurama.id = KuramaActorId;
                kurama.name_locale = "kurama";
                kurama.power_id = "spawn_kurama";

                kurama.is_humanoid = true;
                kurama.use_phenotypes = false;
                kurama.unit_other = true;
                kurama.has_advanced_textures = false;
                kurama.check_flip = delegate { return true; };
                kurama.name_template_sets = new string[] { "human_default_set" };

                kurama.job = new string[] { "random_move" };
                kurama.kingdom_id_wild = "rouge";
                kurama.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                kurama.animation_walk_speed = 5f;
                kurama.animation_swim = ActorAnimationSequences.walk_0;
                kurama.texture_asset = new ActorTextureSubAsset("actors/kurama/", kurama.has_advanced_textures);
                kurama._cached_sprite = Resources.Load<Sprite>("GameResources/actors/kurama/Main/walk_0");
                kurama.texture_id = "kurama";
                kurama.icon = "units/kurama";

                kurama.can_be_killed_by_stuff = true;
                kurama.can_be_killed_by_life_eraser = true;
                kurama.can_attack_buildings = true;
                kurama.can_be_moved_by_powers = true;
                kurama.can_be_hurt_by_powers = true;
                kurama.can_be_inspected = true;
                kurama.visible_on_minimap = true;
                kurama.use_items = false;
                kurama.take_items = false;
                kurama.can_talk_with = false;
                kurama.control_can_talk = false;
                kurama.can_evolve_into_new_species = false;
                kurama.can_have_subspecies = false;
                kurama.disable_jump_animation = true;
                kurama.has_soul = true;
                kurama.has_baby_form = false;
                kurama.render_heads_for_babies = false;
                kurama.body_separate_part_hands = true;
                kurama.shadow = false;

                kurama.force_land_creature = true;
                kurama.force_ocean_creature = false;
                kurama.flying = false;
                kurama.very_high_flyer = false;
                kurama.run_to_water_when_on_fire = true;

                kurama.can_turn_into_zombie = false;
                kurama.can_turn_into_demon_in_age_of_chaos = false;
                kurama.can_turn_into_ice_one = false;
                kurama.can_turn_into_tumor = false;
                kurama.can_turn_into_mush = false;
                kurama.can_be_killed_by_divine_light = true;
                kurama.ignored_by_infinity_coin = false;
                kurama.actor_size = ActorSize.S17_Dragon;
                kurama.can_edit_equipment = true;

                kurama.base_stats["lifespan"] = 999f;
                kurama.base_stats["health"] = 3000f;
                kurama.base_stats["damage"] = 123f;
                kurama.base_stats["speed"] = 25f;
                kurama.base_stats["attack_speed"] = -0.10f;
                kurama.base_stats["intelligence"] = 200f;
                kurama.base_stats["stamina"] = 600f;
                kurama.base_stats["mana"] = 3500f;
                kurama.base_stats["armor"] = 30f;
                kurama.base_stats["critical_chance"] = 0.40f;
                kurama.base_stats["knockback"] = 0.2f;
                kurama.base_stats["accuracy"] = 6f;
                kurama.base_stats["range"] = 1.2f;
                kurama.base_stats["targets"] = 5f;
                kurama.base_stats["scale"] = 0.15f;
                kurama.base_stats["chakra"] = 100000f;
                kurama.base_stats["mass_2"] = 1f;

                kurama.addTrait("immortal");
                kurama.addTrait("strong");
                kurama.addTrait("regeneration");
                kurama.addTrait("vast_chakra_reserve");
                kurama.addTrait("nine_tails_jinchuriki");

                AssetManager.actor_library.loadTexturesAndSprites(kurama);
            }

            if (AssetManager.actor_library.get(JuubiActorId) == null)
            {
                ActorAsset juubi = AssetManager.actor_library.clone(JuubiActorId, "$mob$");
                juubi.id = JuubiActorId;
                juubi.name_locale = "juubi";
                juubi.power_id = "summon_ten_tails";

                juubi.is_humanoid = true;
                juubi.use_phenotypes = false;
                juubi.unit_other = true;
                juubi.has_advanced_textures = false;
                juubi.check_flip = delegate { return true; };
                juubi.name_template_sets = new string[] { "human_default_set" };

                juubi.job = new string[] { "random_move" };
                juubi.kingdom_id_wild = "rouge";
                juubi.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                juubi.animation_walk_speed = 5f;
                juubi.animation_swim = ActorAnimationSequences.walk_0;
                juubi.texture_asset = new ActorTextureSubAsset("actors/juubi/", juubi.has_advanced_textures);
                juubi._cached_sprite = Resources.Load<Sprite>("GameResources/actors/juubi/Main/walk_0");
                juubi.texture_id = "juubi";
                juubi.icon = "units/juubi";

                juubi.can_be_killed_by_stuff = true;
                juubi.can_be_killed_by_life_eraser = true;
                juubi.can_attack_buildings = true;
                juubi.can_be_moved_by_powers = true;
                juubi.can_be_hurt_by_powers = true;
                juubi.can_be_inspected = true;
                juubi.visible_on_minimap = true;
                juubi.use_items = false;
                juubi.take_items = false;
                juubi.can_talk_with = false;
                juubi.control_can_talk = false;
                juubi.can_evolve_into_new_species = false;
                juubi.can_have_subspecies = false;
                juubi.disable_jump_animation = true;
                juubi.has_soul = true;
                juubi.has_baby_form = false;
                juubi.render_heads_for_babies = false;
                juubi.body_separate_part_hands = true;
                juubi.shadow = false;

                juubi.force_land_creature = true;
                juubi.force_ocean_creature = false;
                juubi.flying = false;
                juubi.very_high_flyer = false;
                juubi.run_to_water_when_on_fire = true;

                juubi.can_turn_into_zombie = false;
                juubi.can_turn_into_demon_in_age_of_chaos = false;
                juubi.can_turn_into_ice_one = false;
                juubi.can_turn_into_tumor = false;
                juubi.can_turn_into_mush = false;
                juubi.can_be_killed_by_divine_light = true;
                juubi.ignored_by_infinity_coin = false;
                juubi.actor_size = ActorSize.S17_Dragon;
                juubi.can_edit_equipment = true;

                juubi.base_stats["lifespan"] = 999f;
                juubi.base_stats["health"] = 4200f;
                juubi.base_stats["damage"] = 180f;
                juubi.base_stats["speed"] = 25f;
                juubi.base_stats["attack_speed"] = -35f;
                juubi.base_stats["intelligence"] = 200f;
                juubi.base_stats["stamina"] = 900f;
                juubi.base_stats["mana"] = 5000f;
                juubi.base_stats["armor"] = 45f;
                juubi.base_stats["critical_chance"] = 0.45f;
                juubi.base_stats["knockback"] = 0.25f;
                juubi.base_stats["accuracy"] = 6f;
                juubi.base_stats["range"] = 1.3f;
                juubi.base_stats["targets"] = 6f;
                juubi.base_stats["scale"] = 0.20f;
                juubi.base_stats["chakra"] = 120000f;
                juubi.base_stats["mass_2"] = 1f;

                juubi.addTrait("immortal");
                juubi.addTrait("strong");
                juubi.addTrait("regeneration");
                juubi.addTrait("vast_chakra_reserve");
                juubi.addTrait("hagoromo_chakra");
                juubi.addTrait("ten_tails_jinchuriki");

                AssetManager.actor_library.loadTexturesAndSprites(juubi);
            }
        }

    }
}
