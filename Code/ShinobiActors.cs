using System;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using NeoModLoader.services;
using NeoModLoader.api;
using System.Security.Cryptography.X509Certificates;

namespace ShinobiBox
{
    public static class ShinobiActors
    {
        public const string HashiramaActorId = "hashirama_senju";
        public const string MadaraActorId = "madara_uchiha";
        public const string KuramaActorId = "kurama";
        public const string JuubiActorId = "juubi";
        public const string KakashiActorId = "kakashi_hatake";
        public const string SasukeActorId = "sasuke_uchiha";
        public const string OrochimaruActorId = "orochimaru";
        public const string NarutoActorId = "naruto_uzumaki";
        public const string SakuraActorId = "sakura_haruno";
        public const string ItachiActorId = "itachi_uchiha";
        public const string ObitoActorId = "obito_uchiha";
        public const string HagoromoActorId = "hagoromo_otsutsuki";
        public const string JinchurikiObitoActorId = "jinchuriki_obito";
        public const string JinchurikiMadaraActorId = "jinchuriki_madara";


        public static void Init()
        {
            loadAssets();
        }

        public static void loadAssets()
        {
            #region Shinobi Actors

            #region Hashirama
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

                hashirama.can_be_killed_by_stuff = true;
                hashirama.can_be_killed_by_life_eraser = true;
                hashirama.can_attack_buildings = true;
                hashirama.can_be_moved_by_powers = true;
                hashirama.can_be_hurt_by_powers = true;
                hashirama.can_be_inspected = true;
                hashirama.visible_on_minimap = true;
                hashirama.use_items = true;
                hashirama.take_items = false;
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
                hashirama.can_level_up = false;

                hashirama.base_stats["lifespan"] = 125f;
                hashirama.base_stats["health"] = 1200f;
                hashirama.base_stats["damage"] = 10f;
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

                hashirama.action_death = new WorldAction(HashiramaOnDeath);
                AssetManager.actor_library.loadTexturesAndSprites(hashirama);
            }

            #endregion

            #region Madara

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

                madara.can_be_killed_by_stuff = true;
                madara.can_be_killed_by_life_eraser = true;
                madara.can_attack_buildings = true;
                madara.can_be_moved_by_powers = true;
                madara.can_be_hurt_by_powers = true;
                madara.can_be_inspected = true;
                madara.visible_on_minimap = true;
                madara.use_items = true;
                madara.take_items = false;
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
                madara.can_level_up = false;

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
                madara.base_stats["damage"] = 10f;
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
                madara.addTrait("uchiha_clan");
                madara.addTrait("madara_eternal_mangekyo");
                madara.addTrait("vast_chakra_reserve");
                madara.addTrait("curse_of_hatred");
                madara.addTrait("fireN");
                madara.addTrait("earthN");
                madara.addTrait("waterN");
                madara.addTrait("lightningN");
                madara.addTrait("windN");
                madara.addTrait("ghost_of_uchiha");

                madara.action_death = new WorldAction(MadaraOnDeath);
                AssetManager.actor_library.loadTexturesAndSprites(madara);
            }

            #endregion

            #region Sasuke

            if (AssetManager.actor_library.get(SasukeActorId) == null)
            {
                ActorAsset sasuke = AssetManager.actor_library.clone(SasukeActorId, "$mob$");
                sasuke.id = SasukeActorId;
                sasuke.name_locale = "sasuke uchiha";
                sasuke.power_id = "spawn_sasuke_uchiha";

                sasuke.is_humanoid = true;
                sasuke.use_phenotypes = false;
                sasuke.unit_other = true;
                sasuke.has_advanced_textures = false;
                sasuke.check_flip = delegate { return true; };
                sasuke.name_template_sets = new string[] { "human_default_set" };

                sasuke.job = new string[] { "random_move" };
                sasuke.kingdom_id_wild = ShinobiKingdoms.RougeKingdomId;
                sasuke.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                sasuke.animation_swim = ActorAnimationSequences.walk_0;
                sasuke.texture_asset = new ActorTextureSubAsset("actors/sasuke/", sasuke.has_advanced_textures);
                sasuke._cached_sprite = Resources.Load<Sprite>("GameResources/actors/sasuke/Main/walk_0");
                sasuke.default_weapons = new string[] { "weapon_kusanagi" };
                sasuke.texture_id = "sasuke";
                sasuke.icon = "units/sasuke";

                sasuke.can_be_killed_by_stuff = true;
                sasuke.can_be_killed_by_life_eraser = true;
                sasuke.can_attack_buildings = true;
                sasuke.can_be_moved_by_powers = true;
                sasuke.can_be_hurt_by_powers = true;
                sasuke.can_be_inspected = true;
                sasuke.visible_on_minimap = true;
                sasuke.use_items = true;
                sasuke.take_items = false;
                sasuke.can_talk_with = false;
                sasuke.control_can_talk = false;
                sasuke.can_evolve_into_new_species = false;
                sasuke.can_have_subspecies = false;
                sasuke.disable_jump_animation = true;
                sasuke.has_soul = true;
                sasuke.has_baby_form = false;
                sasuke.render_heads_for_babies = false;
                sasuke.body_separate_part_hands = true;
                sasuke.shadow = false;
                sasuke.can_level_up = false;

                sasuke.force_land_creature = true;
                sasuke.force_ocean_creature = false;
                sasuke.flying = false;
                sasuke.very_high_flyer = false;
                sasuke.run_to_water_when_on_fire = true;

                sasuke.can_turn_into_zombie = false;
                sasuke.can_turn_into_demon_in_age_of_chaos = false;
                sasuke.can_turn_into_ice_one = false;
                sasuke.can_turn_into_tumor = false;
                sasuke.can_turn_into_mush = false;
                sasuke.can_be_killed_by_divine_light = true;
                sasuke.ignored_by_infinity_coin = false;
                sasuke.actor_size = ActorSize.S13_Human;
                sasuke.can_edit_equipment = true;

                sasuke.base_stats["lifespan"] = 125f;
                sasuke.base_stats["health"] = 1500f;
                sasuke.base_stats["damage"] = 7f;
                sasuke.base_stats["speed"] = 26f;
                sasuke.base_stats["attack_speed"] = 1.5f;
                sasuke.base_stats["intelligence"] = 180f;
                sasuke.base_stats["stamina"] = 620f;
                sasuke.base_stats["mana"] = 3600f;
                sasuke.base_stats["armor"] = 13f;
                sasuke.base_stats["critical_chance"] = 0.40f;
                sasuke.base_stats["knockback"] = 0.2f;
                sasuke.base_stats["accuracy"] = 5f;
                sasuke.base_stats["targets"] = 1f;
                sasuke.base_stats["scale"] = 0.15f;
                sasuke.base_stats["chakra"] = 500f;
                sasuke.base_stats["mass_2"] = 40f;

                sasuke.addTrait("uchiha_clan");
                sasuke.addTrait("sasuke_rinnegan");
                sasuke.addTrait("vast_chakra_reserve");
                sasuke.addTrait("fireN");
                sasuke.addTrait("lightningN");
                sasuke.addTrait("cursed_mark");
                sasuke.addTrait("curse_of_hatred");
                sasuke.addTrait("indra_chakra");
                sasuke.addTrait("chidoriJ");

                sasuke.action_death = new WorldAction(SasukeOnDeath);
                AssetManager.actor_library.loadTexturesAndSprites(sasuke);
            }

            #endregion

            #region Orochimaru

            if (AssetManager.actor_library.get(OrochimaruActorId) == null)
            {
                ActorAsset orochimaru = AssetManager.actor_library.clone(OrochimaruActorId, "$mob$");
                orochimaru.id = OrochimaruActorId;
                orochimaru.name_locale = "orochimaru";
                orochimaru.power_id = "spawn_orochimaru";

                orochimaru.is_humanoid = true;
                orochimaru.use_phenotypes = false;
                orochimaru.unit_other = true;
                orochimaru.has_advanced_textures = false;
                orochimaru.check_flip = delegate { return true; };
                orochimaru.name_template_sets = new string[] { "human_default_set" };

                orochimaru.job = new string[] { "random_move" };
                orochimaru.kingdom_id_wild = ShinobiKingdoms.RougeKingdomId;
                orochimaru.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                orochimaru.animation_swim = ActorAnimationSequences.walk_0;
                orochimaru.texture_asset = new ActorTextureSubAsset("actors/orochimaru/", orochimaru.has_advanced_textures);
                orochimaru._cached_sprite = Resources.Load<Sprite>("GameResources/actors/orochimaru/Main/walk_0");
                orochimaru.default_weapons = new string[] { "weapon_kusanagi" };
                orochimaru.texture_id = "orochimaru";
                orochimaru.icon = "units/orochimaru";

                orochimaru.can_be_killed_by_stuff = true;
                orochimaru.can_be_killed_by_life_eraser = true;
                orochimaru.can_attack_buildings = true;
                orochimaru.can_be_moved_by_powers = true;
                orochimaru.can_be_hurt_by_powers = true;
                orochimaru.can_be_inspected = true;
                orochimaru.visible_on_minimap = true;
                orochimaru.use_items = true;
                orochimaru.take_items = false;
                orochimaru.can_talk_with = false;
                orochimaru.control_can_talk = false;
                orochimaru.can_evolve_into_new_species = false;
                orochimaru.can_have_subspecies = false;
                orochimaru.disable_jump_animation = true;
                orochimaru.has_soul = true;
                orochimaru.has_baby_form = false;
                orochimaru.render_heads_for_babies = false;
                orochimaru.body_separate_part_hands = true;
                orochimaru.shadow = false;
                orochimaru.can_level_up = false;

                orochimaru.force_land_creature = true;
                orochimaru.force_ocean_creature = false;
                orochimaru.flying = false;
                orochimaru.very_high_flyer = false;
                orochimaru.run_to_water_when_on_fire = true;

                orochimaru.can_turn_into_zombie = false;
                orochimaru.can_turn_into_demon_in_age_of_chaos = false;
                orochimaru.can_turn_into_ice_one = false;
                orochimaru.can_turn_into_tumor = false;
                orochimaru.can_turn_into_mush = false;
                orochimaru.can_be_killed_by_divine_light = true;
                orochimaru.ignored_by_infinity_coin = false;
                orochimaru.actor_size = ActorSize.S13_Human;
                orochimaru.can_edit_equipment = true;

                orochimaru.base_stats["lifespan"] = 125f;
                orochimaru.base_stats["health"] = 1500f;
                orochimaru.base_stats["damage"] = 5f;
                orochimaru.base_stats["speed"] = 26f;
                orochimaru.base_stats["attack_speed"] = 1.5f;
                orochimaru.base_stats["intelligence"] = 180f;
                orochimaru.base_stats["stamina"] = 620f;
                orochimaru.base_stats["mana"] = 3600f;
                orochimaru.base_stats["armor"] = 13f;
                orochimaru.base_stats["critical_chance"] = 0.40f;
                orochimaru.base_stats["knockback"] = 0.2f;
                orochimaru.base_stats["accuracy"] = 5f;
                orochimaru.base_stats["targets"] = 1f;
                orochimaru.base_stats["scale"] = 0.15f;
                orochimaru.base_stats["chakra"] = 500f;
                orochimaru.base_stats["mass_2"] = 40f;

                orochimaru.addTrait("regeneration");
                orochimaru.addTrait("cursed_mark");
                orochimaru.addTrait("snake_sage");
                orochimaru.addTrait("vast_chakra_reserve");
                orochimaru.addTrait("wood_release");
                orochimaru.addTrait("shikotsumyaku");

                AssetManager.actor_library.loadTexturesAndSprites(orochimaru);
            }

            #endregion

            #region Kakashi

            if (AssetManager.actor_library.get(KakashiActorId) == null)
            {
                ActorAsset kakashi = AssetManager.actor_library.clone(KakashiActorId, "$mob$");
                kakashi.id = KakashiActorId;
                kakashi.name_locale = "kakashi hatake";
                kakashi.power_id = "spawn_kakashi_hatake";

                kakashi.is_humanoid = true;
                kakashi.use_phenotypes = false;
                kakashi.unit_other = true;
                kakashi.has_advanced_textures = false;
                kakashi.check_flip = delegate { return true; };
                kakashi.name_template_sets = new string[] { "human_default_set" };

                kakashi.job = new string[] { "random_move" };
                kakashi.kingdom_id_wild = ShinobiKingdoms.KonohaKingdomId;
                kakashi.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                kakashi.animation_swim = ActorAnimationSequences.walk_0;
                kakashi.texture_asset = new ActorTextureSubAsset("actors/kakashi/", kakashi.has_advanced_textures);
                kakashi._cached_sprite = Resources.Load<Sprite>("GameResources/actors/kakashi/Main/walk_0");
                kakashi.default_weapons = new string[] { "basic_kunai" };
                kakashi.texture_id = "kakashi";
                kakashi.icon = "units/kakashi";

                kakashi.can_be_killed_by_stuff = true;
                kakashi.can_be_killed_by_life_eraser = true;
                kakashi.can_attack_buildings = true;
                kakashi.can_be_moved_by_powers = true;
                kakashi.can_be_hurt_by_powers = true;
                kakashi.can_be_inspected = true;
                kakashi.visible_on_minimap = true;
                kakashi.use_items = true;
                kakashi.take_items = false;
                kakashi.can_talk_with = false;
                kakashi.control_can_talk = false;
                kakashi.can_evolve_into_new_species = false;
                kakashi.can_have_subspecies = false;
                kakashi.disable_jump_animation = true;
                kakashi.has_soul = true;
                kakashi.has_baby_form = false;
                kakashi.render_heads_for_babies = false;
                kakashi.body_separate_part_hands = true;
                kakashi.shadow = false;
                kakashi.can_level_up = false;

                kakashi.force_land_creature = true;
                kakashi.force_ocean_creature = false;
                kakashi.flying = false;
                kakashi.very_high_flyer = false;
                kakashi.run_to_water_when_on_fire = true;

                kakashi.can_turn_into_zombie = false;
                kakashi.can_turn_into_demon_in_age_of_chaos = false;
                kakashi.can_turn_into_ice_one = false;
                kakashi.can_turn_into_tumor = false;
                kakashi.can_turn_into_mush = false;
                kakashi.can_be_killed_by_divine_light = true;
                kakashi.ignored_by_infinity_coin = false;
                kakashi.actor_size = ActorSize.S13_Human;
                kakashi.can_edit_equipment = true;

                kakashi.base_stats["lifespan"] = 125f;
                kakashi.base_stats["health"] = 1200f;
                kakashi.base_stats["damage"] = 7f;
                kakashi.base_stats["speed"] = 25f;
                kakashi.base_stats["attack_speed"] = 2.5f;
                kakashi.base_stats["intelligence"] = 170f;
                kakashi.base_stats["stamina"] = 700f;
                kakashi.base_stats["mana"] = 3700f;
                kakashi.base_stats["armor"] = 27f;
                kakashi.base_stats["critical_chance"] = 0.40f;
                kakashi.base_stats["knockback"] = 0.2f;
                kakashi.base_stats["accuracy"] = 4f;
                kakashi.base_stats["targets"] = 1f;
                kakashi.base_stats["scale"] = 0.15f;
                kakashi.base_stats["chakra"] = 520f;
                kakashi.base_stats["mass_2"] = 40f;

                kakashi.addTrait("vast_chakra_reserve");
                kakashi.addTrait("windN");
                kakashi.addTrait("lightningN");
                kakashi.addTrait("earthN");
                kakashi.addTrait("fireN");
                kakashi.addTrait("waterN");
                kakashi.addTrait("rasenganJ");
                kakashi.addTrait("shadow_clone_jutsu");
                kakashi.addTrait("rasenshuriken");
                kakashi.addTrait("chidoriJ");
                kakashi.addTrait("mangekyo_sharingan");

                AssetManager.actor_library.loadTexturesAndSprites(kakashi);
            }

            #endregion

            #region Itachi

            if (AssetManager.actor_library.get(ItachiActorId) == null)
            {
                ActorAsset itachi = AssetManager.actor_library.clone(ItachiActorId, "$mob$");
                itachi.id = ItachiActorId;
                itachi.name_locale = "itachi uchiha";
                itachi.power_id = "spawn_itachi_uchiha";

                itachi.is_humanoid = true;
                itachi.use_phenotypes = false;
                itachi.unit_other = true;
                itachi.has_advanced_textures = false;
                itachi.check_flip = delegate { return true; };
                itachi.name_template_sets = new string[] { "human_default_set" };

                itachi.job = new string[] { "random_move" };
                itachi.kingdom_id_wild = ShinobiKingdoms.RougeKingdomId;
                itachi.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                itachi.animation_swim = ActorAnimationSequences.walk_0;
                itachi.texture_asset = new ActorTextureSubAsset("actors/itachi/", itachi.has_advanced_textures);
                itachi._cached_sprite = Resources.Load<Sprite>("GameResources/actors/itachi/Main/walk_0");
                itachi.default_weapons = new string[] { "basic_kunai" };
                itachi.texture_id = "itachi";
                itachi.icon = "units/itachi";

                itachi.can_be_killed_by_stuff = true;
                itachi.can_be_killed_by_life_eraser = true;
                itachi.can_attack_buildings = true;
                itachi.can_be_moved_by_powers = true;
                itachi.can_be_hurt_by_powers = true;
                itachi.can_be_inspected = true;
                itachi.visible_on_minimap = true;
                itachi.use_items = true;
                itachi.take_items = false;
                itachi.can_talk_with = false;
                itachi.control_can_talk = false;
                itachi.can_evolve_into_new_species = false;
                itachi.can_have_subspecies = false;
                itachi.disable_jump_animation = true;
                itachi.has_soul = true;
                itachi.has_baby_form = false;
                itachi.render_heads_for_babies = false;
                itachi.body_separate_part_hands = true;
                itachi.shadow = false;
                itachi.can_level_up = false;

                itachi.force_land_creature = true;
                itachi.force_ocean_creature = false;
                itachi.flying = false;
                itachi.very_high_flyer = false;
                itachi.run_to_water_when_on_fire = true;

                itachi.can_turn_into_zombie = false;
                itachi.can_turn_into_demon_in_age_of_chaos = false;
                itachi.can_turn_into_ice_one = false;
                itachi.can_turn_into_tumor = false;
                itachi.can_turn_into_mush = false;
                itachi.can_be_killed_by_divine_light = true;
                itachi.ignored_by_infinity_coin = false;
                itachi.actor_size = ActorSize.S13_Human;
                itachi.can_edit_equipment = true;

                itachi.base_stats["lifespan"] = 125f;
                itachi.base_stats["health"] = 1500f;
                itachi.base_stats["damage"] = 8f;
                itachi.base_stats["speed"] = 26f;
                itachi.base_stats["attack_speed"] = 1.5f;
                itachi.base_stats["intelligence"] = 180f;
                itachi.base_stats["stamina"] = 620f;
                itachi.base_stats["mana"] = 3600f;
                itachi.base_stats["armor"] = 13f;
                itachi.base_stats["critical_chance"] = 0.40f;
                itachi.base_stats["knockback"] = 0.2f;
                itachi.base_stats["accuracy"] = 5f;
                itachi.base_stats["targets"] = 1f;
                itachi.base_stats["scale"] = 0.15f;
                itachi.base_stats["chakra"] = 500f;
                itachi.base_stats["mass_2"] = 40f;

                itachi.addTrait("regeneration");
                itachi.addTrait("uchiha_clan");
                itachi.addTrait("mangekyo_sharingan");
                itachi.addTrait("vast_chakra_reserve");
                itachi.addTrait("curse_of_hatred");
                itachi.addTrait("rank_anbu");

                AssetManager.actor_library.loadTexturesAndSprites(itachi);
            }

            #endregion

            #region Naruto

            if (AssetManager.actor_library.get(NarutoActorId) == null)
            {
                ActorAsset naruto = AssetManager.actor_library.clone(NarutoActorId, "$mob$");
                naruto.id = NarutoActorId;
                naruto.name_locale = "naruto uzumaki";
                naruto.power_id = "spawn_naruto_uzumaki";

                naruto.is_humanoid = true;
                naruto.use_phenotypes = false;
                naruto.unit_other = true;
                naruto.has_advanced_textures = false;
                naruto.check_flip = delegate { return true; };
                naruto.name_template_sets = new string[] { "human_default_set" };

                naruto.job = new string[] { "random_move" };
                naruto.kingdom_id_wild = ShinobiKingdoms.KonohaKingdomId;
                naruto.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                naruto.animation_swim = ActorAnimationSequences.walk_0;
                naruto.texture_asset = new ActorTextureSubAsset("actors/naruto/", naruto.has_advanced_textures);
                naruto._cached_sprite = Resources.Load<Sprite>("GameResources/actors/naruto/Main/walk_0");
                naruto.texture_id = "naruto";
                naruto.icon = "units/naruto";

                naruto.can_be_killed_by_stuff = true;
                naruto.can_be_killed_by_life_eraser = true;
                naruto.can_attack_buildings = true;
                naruto.can_be_moved_by_powers = true;
                naruto.can_be_hurt_by_powers = true;
                naruto.can_be_inspected = true;
                naruto.visible_on_minimap = true;
                naruto.use_items = true;
                naruto.take_items = false;
                naruto.can_talk_with = false;
                naruto.control_can_talk = false;
                naruto.can_evolve_into_new_species = false;
                naruto.can_have_subspecies = false;
                naruto.disable_jump_animation = true;
                naruto.has_soul = true;
                naruto.has_baby_form = false;
                naruto.render_heads_for_babies = false;
                naruto.body_separate_part_hands = true;
                naruto.shadow = false;
                naruto.can_level_up = false;

                naruto.force_land_creature = true;
                naruto.force_ocean_creature = false;
                naruto.flying = false;
                naruto.very_high_flyer = false;
                naruto.run_to_water_when_on_fire = true;

                naruto.can_turn_into_zombie = false;
                naruto.can_turn_into_demon_in_age_of_chaos = false;
                naruto.can_turn_into_ice_one = false;
                naruto.can_turn_into_tumor = false;
                naruto.can_turn_into_mush = false;
                naruto.can_be_killed_by_divine_light = true;
                naruto.ignored_by_infinity_coin = false;
                naruto.actor_size = ActorSize.S13_Human;
                naruto.can_edit_equipment = true;

                naruto.base_stats["lifespan"] = 125f;
                naruto.base_stats["health"] = 1200f;
                naruto.base_stats["damage"] = 15f;
                naruto.base_stats["speed"] = 25f;
                naruto.base_stats["attack_speed"] = 2.5f;
                naruto.base_stats["intelligence"] = 170f;
                naruto.base_stats["stamina"] = 700f;
                naruto.base_stats["mana"] = 3700f;
                naruto.base_stats["armor"] = 27f;
                naruto.base_stats["critical_chance"] = 0.40f;
                naruto.base_stats["knockback"] = 0.2f;
                naruto.base_stats["accuracy"] = 4f;
                naruto.base_stats["targets"] = 1f;
                naruto.base_stats["scale"] = 0.15f;
                naruto.base_stats["chakra"] = 520f;
                naruto.base_stats["mass_2"] = 40f;

                naruto.addTrait("regeneration");
                naruto.addTrait("uzumaki_clan");
                naruto.addTrait("will_of_fire");
                naruto.addTrait("vast_chakra_reserve");
                naruto.addTrait("windN");
                naruto.addTrait("rasenganJ");
                naruto.addTrait("shadow_clone_jutsu");
                naruto.addTrait("rasenshuriken");
                naruto.addTrait("nine_tails_jinchuriki");

                naruto.action_death = new WorldAction(NarutoOnDeath);
                AssetManager.actor_library.loadTexturesAndSprites(naruto);
            }

            #endregion

            #region Obito

            if (AssetManager.actor_library.get(ObitoActorId) == null)
            {
                ActorAsset obito = AssetManager.actor_library.clone(ObitoActorId, "$mob$");
                obito.id = ObitoActorId;
                obito.name_locale = "obito uchiha";
                obito.power_id = "spawn_obito_uchiha";

                obito.is_humanoid = true;
                obito.use_phenotypes = false;
                obito.unit_other = true;
                obito.has_advanced_textures = false;
                obito.check_flip = delegate { return true; };
                obito.name_template_sets = new string[] { "human_default_set" };

                obito.job = new string[] { "random_move" };
                obito.kingdom_id_wild = ShinobiKingdoms.RougeKingdomId;
                obito.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                obito.animation_swim = ActorAnimationSequences.walk_0;
                obito.texture_asset = new ActorTextureSubAsset("actors/obito/", obito.has_advanced_textures);
                obito._cached_sprite = Resources.Load<Sprite>("GameResources/actors/obito/Main/walk_0");
                obito.texture_id = "obito";
                obito.icon = "units/obito";

                obito.can_be_killed_by_stuff = true;
                obito.can_be_killed_by_life_eraser = true;
                obito.can_attack_buildings = true;
                obito.can_be_moved_by_powers = true;
                obito.can_be_hurt_by_powers = true;
                obito.can_be_inspected = true;
                obito.visible_on_minimap = true;
                obito.use_items = true;
                obito.take_items = false;
                obito.can_talk_with = false;
                obito.control_can_talk = false;
                obito.can_evolve_into_new_species = false;
                obito.can_have_subspecies = false;
                obito.disable_jump_animation = true;
                obito.has_soul = true;
                obito.has_baby_form = false;
                obito.render_heads_for_babies = false;
                obito.body_separate_part_hands = true;
                obito.shadow = false;
                obito.can_level_up = false;

                obito.force_land_creature = true;
                obito.force_ocean_creature = false;
                obito.flying = false;
                obito.very_high_flyer = false;
                obito.run_to_water_when_on_fire = true;

                obito.can_turn_into_zombie = false;
                obito.can_turn_into_demon_in_age_of_chaos = false;
                obito.can_turn_into_ice_one = false;
                obito.can_turn_into_tumor = false;
                obito.can_turn_into_mush = false;
                obito.can_be_killed_by_divine_light = true;
                obito.ignored_by_infinity_coin = false;
                obito.actor_size = ActorSize.S13_Human;
                obito.can_edit_equipment = true;

                obito.base_stats["lifespan"] = 125f;
                obito.base_stats["health"] = 1500f;
                obito.base_stats["damage"] = 28f;
                obito.base_stats["speed"] = 26f;
                obito.base_stats["attack_speed"] = 1.5f;
                obito.base_stats["intelligence"] = 180f;
                obito.base_stats["stamina"] = 620f;
                obito.base_stats["mana"] = 3600f;
                obito.base_stats["armor"] = 13f;
                obito.base_stats["critical_chance"] = 0.40f;
                obito.base_stats["knockback"] = 0.2f;
                obito.base_stats["accuracy"] = 5f;
                obito.base_stats["targets"] = 1f;
                obito.base_stats["scale"] = 0.15f;
                obito.base_stats["chakra"] = 500f;
                obito.base_stats["mass_2"] = 40f;

                obito.addTrait("ten_tails_jinchuriki");
                obito.addTrait("uchiha_clan");
                obito.addTrait("rinnegan");
                obito.addTrait("vast_chakra_reserve");
                obito.addTrait("fireN");
                obito.addTrait("yinyang_release");
                obito.addTrait("wood_release");
                obito.addTrait("mangekyo_sharingan");

                AssetManager.actor_library.loadTexturesAndSprites(obito);
            }

            #endregion

            #region Hagoromo

            if (AssetManager.actor_library.get(HagoromoActorId) == null)
            {
                ActorAsset hagoromo = AssetManager.actor_library.clone(HagoromoActorId, "$mob$");
                hagoromo.id = HagoromoActorId;
                hagoromo.name_locale = "hagoromo otsutsuki";
                hagoromo.power_id = "spawn_hagoromo_otsutsuki";

                hagoromo.is_humanoid = true;
                hagoromo.use_phenotypes = false;
                hagoromo.unit_other = true;
                hagoromo.has_advanced_textures = false;
                hagoromo.check_flip = delegate { return true; };
                hagoromo.name_template_sets = new string[] { "human_default_set" };

                hagoromo.job = new string[] { "random_move" };
                hagoromo.kingdom_id_wild = ShinobiKingdoms.KonohaKingdomId;
                hagoromo.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                hagoromo.animation_swim = ActorAnimationSequences.walk_0;
                hagoromo.texture_asset = new ActorTextureSubAsset("actors/hagoromo/", hagoromo.has_advanced_textures);
                hagoromo._cached_sprite = Resources.Load<Sprite>("GameResources/actors/hagoromo/Main/walk_0");
                hagoromo.texture_id = "hagoromo";
                hagoromo.icon = "units/hagoromo";

                hagoromo.can_be_killed_by_stuff = true;
                hagoromo.can_be_killed_by_life_eraser = true;
                hagoromo.can_attack_buildings = true;
                hagoromo.can_be_moved_by_powers = true;
                hagoromo.can_be_hurt_by_powers = true;
                hagoromo.can_be_inspected = true;
                hagoromo.visible_on_minimap = true;
                hagoromo.use_items = true;
                hagoromo.take_items = false;
                hagoromo.can_talk_with = false;
                hagoromo.control_can_talk = false;
                hagoromo.can_evolve_into_new_species = false;
                hagoromo.can_have_subspecies = false;
                hagoromo.disable_jump_animation = true;
                hagoromo.has_soul = true;
                hagoromo.has_baby_form = false;
                hagoromo.render_heads_for_babies = false;
                hagoromo.body_separate_part_hands = true;
                hagoromo.shadow = false;
                hagoromo.can_level_up = false;

                hagoromo.force_land_creature = true;
                hagoromo.force_ocean_creature = false;
                hagoromo.flying = false;
                hagoromo.very_high_flyer = false;
                hagoromo.run_to_water_when_on_fire = true;

                hagoromo.can_turn_into_zombie = false;
                hagoromo.can_turn_into_demon_in_age_of_chaos = false;
                hagoromo.can_turn_into_ice_one = false;
                hagoromo.can_turn_into_tumor = false;
                hagoromo.can_turn_into_mush = false;
                hagoromo.can_be_killed_by_divine_light = true;
                hagoromo.ignored_by_infinity_coin = false;
                hagoromo.actor_size = ActorSize.S13_Human;
                hagoromo.can_edit_equipment = true;

                hagoromo.base_stats["lifespan"] = 999f;
                hagoromo.base_stats["health"] = 10000f;
                hagoromo.base_stats["damage"] = 28f;
                hagoromo.base_stats["speed"] = 50f;
                hagoromo.base_stats["attack_speed"] = 2.5f;
                hagoromo.base_stats["intelligence"] = 1000f;
                hagoromo.base_stats["stamina"] = 10000f;
                hagoromo.base_stats["mana"] = 10000f;
                hagoromo.base_stats["armor"] = 27f;
                hagoromo.base_stats["critical_chance"] = 0.90f;
                hagoromo.base_stats["knockback"] = 0.2f;
                hagoromo.base_stats["accuracy"] = 4f;
                hagoromo.base_stats["targets"] = 1f;
                hagoromo.base_stats["scale"] = 0.20f;
                hagoromo.base_stats["chakra"] = 10000f;
                hagoromo.base_stats["mass_2"] = 40f;

                hagoromo.addTrait("regeneration");
                hagoromo.addTrait("otsutsuki_clan");
                hagoromo.addTrait("rinnegan");
                hagoromo.addTrait("vast_chakra_reserve");
                hagoromo.addTrait("hagoromo_chakra");
                hagoromo.addTrait("windN");
                hagoromo.addTrait("earthN");
                hagoromo.addTrait("fireN");
                hagoromo.addTrait("waterN");
                hagoromo.addTrait("lightningN");
                hagoromo.addTrait("ten_tails_jinchuriki");

                hagoromo.action_death = new WorldAction(HagoromoOnDeath);
                AssetManager.actor_library.loadTexturesAndSprites(hagoromo);
            }

            #endregion

            #region Sakura

            if (AssetManager.actor_library.get(SakuraActorId) == null)
            {
                ActorAsset sakura = AssetManager.actor_library.clone(SakuraActorId, "$mob$");
                sakura.id = SakuraActorId;
                sakura.name_locale = "sakura haruno";
                sakura.power_id = "spawn_sakura_haruno";

                sakura.is_humanoid = true;
                sakura.use_phenotypes = false;
                sakura.unit_other = true;
                sakura.has_advanced_textures = false;
                sakura.check_flip = delegate { return true; };
                sakura.name_template_sets = new string[] { "human_default_set" };

                sakura.job = new string[] { "random_move" };
                sakura.kingdom_id_wild = ShinobiKingdoms.KonohaKingdomId;
                sakura.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                sakura.animation_swim = ActorAnimationSequences.walk_0;
                sakura.texture_asset = new ActorTextureSubAsset("actors/sakura/", sakura.has_advanced_textures);
                sakura._cached_sprite = Resources.Load<Sprite>("GameResources/actors/sakura/Main/walk_0");
                sakura.texture_id = "sakura";
                sakura.icon = "units/sakura";

                sakura.can_be_killed_by_stuff = true;
                sakura.can_be_killed_by_life_eraser = true;
                sakura.can_attack_buildings = true;
                sakura.can_be_moved_by_powers = true;
                sakura.can_be_hurt_by_powers = true;
                sakura.can_be_inspected = true;
                sakura.visible_on_minimap = true;
                sakura.use_items = true;
                sakura.take_items = false;
                sakura.can_talk_with = false;
                sakura.control_can_talk = false;
                sakura.can_evolve_into_new_species = false;
                sakura.can_have_subspecies = false;
                sakura.disable_jump_animation = true;
                sakura.has_soul = true;
                sakura.has_baby_form = false;
                sakura.render_heads_for_babies = false;
                sakura.body_separate_part_hands = true;
                sakura.shadow = false;
                sakura.can_level_up = false;

                sakura.force_land_creature = true;
                sakura.force_ocean_creature = false;
                sakura.flying = false;
                sakura.very_high_flyer = false;
                sakura.run_to_water_when_on_fire = true;

                sakura.can_turn_into_zombie = false;
                sakura.can_turn_into_demon_in_age_of_chaos = false;
                sakura.can_turn_into_ice_one = false;
                sakura.can_turn_into_tumor = false;
                sakura.can_turn_into_mush = false;
                sakura.can_be_killed_by_divine_light = true;
                sakura.ignored_by_infinity_coin = false;
                sakura.actor_size = ActorSize.S13_Human;
                sakura.can_edit_equipment = true;

                sakura.base_stats["lifespan"] = 125f;
                sakura.base_stats["health"] = 950f;
                sakura.base_stats["damage"] = 7f;
                sakura.base_stats["speed"] = 24f;
                sakura.base_stats["attack_speed"] = 2.2f;
                sakura.base_stats["intelligence"] = 150f;
                sakura.base_stats["stamina"] = 560f;
                sakura.base_stats["mana"] = 3000f;
                sakura.base_stats["armor"] = 24f;
                sakura.base_stats["critical_chance"] = 0.30f;
                sakura.base_stats["knockback"] = 0.2f;
                sakura.base_stats["accuracy"] = 4f;
                sakura.base_stats["targets"] = 1f;
                sakura.base_stats["scale"] = 0.15f;
                sakura.base_stats["chakra"] = 350f;
                sakura.base_stats["mass_2"] = 36f;

                sakura.addTrait("slug_sage_mode");
                sakura.addTrait("regeneration");
                sakura.addTrait("will_of_fire");
                sakura.addTrait("vast_chakra_reserve");
                sakura.addTrait("earthN");

                sakura.action_death = new WorldAction(SakuraOnDeath);
                AssetManager.actor_library.loadTexturesAndSprites(sakura);
            }

            #endregion

            #endregion

            #region Tailed Beasts

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
                kurama.kingdom_id_wild = ShinobiKingdoms.TailedBeastsKingdomId;
                kurama.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                kurama.animation_walk_speed = 5f;
                kurama.animation_speed_based_on_walk_speed = false;
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
                kurama.can_level_up = false;

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
                kurama.base_stats["range"] = 80f;
                kurama.base_stats["targets"] = 5f;
                kurama.base_stats["scale"] = 0.15f;
                kurama.base_stats["chakra"] = 100000f;
                kurama.base_stats["mass_2"] = 1f;

                kurama.addTrait("immortal");
                kurama.addTrait("strong");
                kurama.addTrait("regeneration");
                kurama.addTrait("vast_chakra_reserve");
                kurama.addTrait("nine_tails_jinchuriki");

                kurama.action_death = new WorldAction(KuramaOnDeath);
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
                juubi.kingdom_id_wild = ShinobiKingdoms.TailedBeastsKingdomId;
                juubi.animation_walk = new string[] { "walk_0", "walk_1", "walk_2", "walk_3" };
                juubi.animation_walk_speed = 5f;
                juubi.animation_speed_based_on_walk_speed = false;
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
                juubi.can_level_up = false;

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
                juubi.base_stats["range"] = 80f;
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

                juubi.action_death = new WorldAction(JuubiOnDeath);
                AssetManager.actor_library.loadTexturesAndSprites(juubi);
            }

            #endregion
        }

        #region On Death Actions
        public static bool SasukeOnDeath(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            Actor sasuke = pTarget.a;
            BaseSimObject attackerObject = sasuke.attackedBy;
            if (attackerObject == null || attackerObject.a == null || !attackerObject.a.isAlive()) return false;

            Actor attacker = attackerObject.a;
            if (attacker == sasuke) return false;

            if (!attacker.hasTrait("sasuke_rinnegan"))
            {
                attacker.addTrait("sasuke_rinnegan");
            }
            else
            {
                ShinobiItems.EquipItem(attacker, "weapon_kusanagi");
            }

            return true;
        }

        public static bool ItachiOnDeath(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            return true;
        }

        public static bool NarutoOnDeath(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            return true;
        }

        public static bool SakuraOnDeath(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            return true;
        }

        public static bool HagoromoOnDeath(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            for (int i = 0; i < 4; i++)
            {
                ActionLibrary.castTornado(null, pTarget, null);
            }
            MapBox.spawnLightningMedium(pTarget.current_tile, 0.15f);
            Earthquake.startQuake(pTarget.a.current_tile, EarthquakeType.SmallDisaster);
            MapBox.spawnLightningBig(pTarget.current_tile, 0.45f);

            return true;
        }

        public static bool HashiramaOnDeath(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || pTarget.a.data == null) return false;

            Actor hashirama = pTarget.a;
            BaseSimObject attackerObject = hashirama.attackedBy;
            if (attackerObject == null || attackerObject.a == null || !attackerObject.a.isAlive()) return false;

            Actor attacker = attackerObject.a;
            if (attacker == hashirama) return false;

            if (!attacker.hasTrait("hashi_cells") && Randy.randomChance(0.7f))
            {
                attacker.addTrait("hashi_cells");
            }
            else
            {
                ShinobiItems.EquipItem(attacker, "armor_warring_states");
            }

            return true;
        }

        public static bool MadaraOnDeath(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || pTarget.a.data == null) return false;

            Actor madara = pTarget.a;
            BaseSimObject attackerObject = madara.attackedBy;
            if (attackerObject == null || attackerObject.a == null || !attackerObject.a.isAlive()) return false;

            Actor attacker = attackerObject.a;
            if (attacker == madara) return false;

            if (!attacker.hasTrait("madara_eternal_mangekyo") || !attacker.hasTrait("eternal_mangekyo"))
            {
                attacker.addTrait("madara_eternal_mangekyo");
            }
            else
            {
                ShinobiItems.EquipItem(attacker, "weapon_gunbai");
                ShinobiItems.EquipItem(attacker, "armor_warring_states");
            }

            return true;
        }

        public static bool KuramaOnDeath(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || pTarget.a.data == null) return false;

            Actor kurama = pTarget.a;
            BaseSimObject attackerObject = kurama.attackedBy;
            if (attackerObject == null || attackerObject.a == null || !attackerObject.a.isAlive()) return false;

            Actor attacker = attackerObject.a;
            if (attacker == kurama) return false;

            if (!attacker.hasTrait("nine_tails_jinchuriki"))
            {
                attacker.addTrait("nine_tails_jinchuriki");
            }

            attacker.restoreHealth(attacker.getMaxHealth());
            ShinobiWorldLogs.AddWorldLog("log_kurama_sealed", "worldlog_kurama_sealed", "ui/icons/nine_tails", attacker);

            return true;
        }

        public static bool JuubiOnDeath(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || pTarget.a.data == null) return false;

            Actor juubi = pTarget.a;
            BaseSimObject attackerObject = juubi.attackedBy;
            if (attackerObject == null || attackerObject.a == null || !attackerObject.a.isAlive()) return false;

            Actor attacker = attackerObject.a;
            if (attacker == juubi) return false;

            if (!attacker.hasTrait("ten_tails_jinchuriki"))
            {
                attacker.addTrait("ten_tails_jinchuriki");
            }

            attacker.restoreHealth(attacker.getMaxHealth());
            ShinobiWorldLogs.AddWorldLog("log_juubi_sealed", "worldlog_juubi_sealed", "ui/icons/ten_tails", attacker);

            return true;
        }
        #endregion

    }
}
