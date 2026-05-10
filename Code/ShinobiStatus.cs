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
    public static class ShinobiStatus
    {
        public static void Init()
        {
            loadStatusEffects();
        }

        private static void loadStatusEffects()
        {
            Material material = LibraryMaterials.instance.dict["mat_world_object_lit"];
            NineTailsJinchuriki(material);

            #region Will of Fire
            // Will of fire
            StatusAsset willPower = new StatusAsset();
            willPower.id = "power_up";
            willPower.animated = false;
            willPower.path_icon = "ui/icons/will_of_fire";
            willPower.duration = 15f;
            willPower.locale_id = "status_title_will_of_power";
            willPower.base_stats = new BaseStats();
            willPower.base_stats.set("multiplier_health", 0.05f);
            willPower.base_stats.set("multiplier_damage", 0.05f);
            willPower.base_stats.set("multiplier_speed", 0.05f);
            willPower.base_stats.set("multiplier_attack_speed", 0.05f);
            willPower.base_stats.set("armor", 5f);
            willPower.locale_description = "status_description_will_of_power";

            AssetManager.status.add(willPower);
            #endregion

            #region Kurama Sage Mode Effect
            // Kurama Sage Mode
            StatusAsset kuramaSage = new StatusAsset();
            kuramaSage.id = "kurama_sage_mode";
            kuramaSage.base_stats = new BaseStats();
            kuramaSage.base_stats.set("damage", 250f);
            kuramaSage.base_stats.set("multiplier_damage", 0.05f);
            kuramaSage.base_stats.set("health", 1000);
            kuramaSage.base_stats.set("multiplier_health", 0.05f);
            kuramaSage.base_stats.set("armor", 20f);
            kuramaSage.base_stats.set("lifespan", 20f);
            kuramaSage.base_stats.set("multiplier_lifespan", 0.10f);
            kuramaSage.base_stats.set("speed", 25f);
            kuramaSage.base_stats.set("attack_speed", 15f);
            kuramaSage.base_stats.set("multiplier_speed", 0.05f);
            kuramaSage.base_stats.set("critical_chance", 0.50f);
            kuramaSage.base_stats.set("critical_damage_multiplier", 0.50f);
            kuramaSage.base_stats.set("experience", 200f);

            kuramaSage.path_icon = "ui/icons/kurama_sage";
            kuramaSage.locale_id = "status_title_kurama_sage";
            kuramaSage.locale_description = "status_description_kurama_sage";
            kuramaSage.duration = 60f;

            AssetManager.status.add(kuramaSage);
            

            #endregion

            #region Gentle Fist Technique (Armor Break)
            // Armor Break
            StatusAsset armorBreak = new StatusAsset();
            armorBreak.id = "chakra_break";
            armorBreak.path_icon = "ui/icons/chakra_exhaustion";
            armorBreak.base_stats = new BaseStats();
            armorBreak.base_stats.set("armor", -25f);
            armorBreak.duration = 10f;
            armorBreak.locale_id = "status_title_chakra_armor_break";
            armorBreak.locale_description = "status_description_chakra_armor_break";

            AssetManager.status.add(armorBreak);
            #endregion

            #region Rasengan and Chidori Statuses
            StatusAsset rasengan = new StatusAsset();
            rasengan.id = "status_rasengan";
            rasengan.path_icon = "ui/icons/jutsuA";
            rasengan.duration = 5f;
            rasengan.base_stats = new BaseStats();
            rasengan.base_stats.set("speed", 10f);
            rasengan.base_stats.set("multiplier_damage", 0.30f);
            rasengan.base_stats.set("knockback", 2f);
            rasengan.base_stats.set
            ("knockback", 1.8f);

            rasengan.animated = true;
            rasengan.texture = "fx_rasengan";
            rasengan.material = material;
            rasengan.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{rasengan.texture}", false);

            rasengan.need_visual_render = true;
            rasengan.removed_on_damage = false;
            rasengan.use_parent_rotation = true;
            rasengan.is_animated_in_pause = false;
            rasengan.can_be_flipped = true;
            rasengan.scale = 0.35f;

            rasengan.locale_id = "status_title_rasengan";
            rasengan.locale_description = "status_description_rasengan";
            //rasengan.action_attack_target = new AttackAction(RasenganAA);
            AssetManager.status.add(rasengan);

            StatusAsset chidori = new StatusAsset();
            chidori.id = "status_chidori";
            chidori.path_icon = "ui/icons/jutsuA";
            chidori.duration = 4f;
            chidori.base_stats = new BaseStats();
            chidori.base_stats.set("speed", 8f);
            chidori.base_stats.set("multiplier_speed", 0.08f);

            chidori.animated = true;
            chidori.texture = "fx_chidori";
            chidori.material = material;
            chidori.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{chidori.texture}", false);

            chidori.need_visual_render = true;
            chidori.removed_on_damage = false;
            chidori.use_parent_rotation = true;
            chidori.is_animated_in_pause = false;
            chidori.can_be_flipped = true;

            chidori.locale_id = "status_title_chidori";
            chidori.locale_description = "status_description_chidori";
            //chidori.action_attack_target = new AttackAction(ChidoriAA);
            AssetManager.status.add(chidori);
            #endregion

            #region Shadow Clone Status
            StatusAsset shadowClone = new StatusAsset();
            shadowClone.id = "shadow_clone";
            shadowClone.duration = 20f;
            shadowClone.animated = false;
            shadowClone.path_icon = "ui/icons/jutsuB";
            shadowClone.base_stats = new BaseStats();
            shadowClone.base_stats.set("multiplier_health", 0.10f);
            shadowClone.base_stats.set("multiplier_damage", 0.10f);

            shadowClone.action_get_hit = new GetHitAction(ShadowCloneGetHit);
            shadowClone.action_finish = (WorldAction)Delegate.Combine(shadowClone.action_finish, new WorldAction(ShadowCloneFinish));

            shadowClone.locale_id = "status_title_shadow_clone";
            shadowClone.locale_description = "status_description_shadow_clone";
            AssetManager.status.add(shadowClone);

            #endregion


            #region Rinne-Cooldown
            // Rinnegan Cooldown
            StatusAsset rinneCooldown = new StatusAsset();
            rinneCooldown.id = "rinne_cooldown";
            rinneCooldown.duration = 120f;
            rinneCooldown.animated = false;
            rinneCooldown.path_icon = "ui/icons/rinnegan";
            rinneCooldown.base_stats = new BaseStats();
            rinneCooldown.locale_id = "status_title_rinne_cooldown";
            rinneCooldown.locale_description = "status_description_rinne_cooldown";
            AssetManager.status.add(rinneCooldown);

            #endregion

            #region Rinnegan Controlled
            StatusAsset rinneganControlled = new StatusAsset();
            rinneganControlled.id = "status_rinnegan_controlled";
            rinneganControlled.duration = 120f;
            rinneganControlled.animated = false;
            rinneganControlled.path_icon = "ui/icons/rinnegan";
            rinneganControlled.base_stats = new BaseStats();
            rinneganControlled.base_stats.set("multiplier_health", 0.05f);
            rinneganControlled.base_stats.set("multiplier_damage", 0.05f);
            rinneganControlled.locale_id = "status_title_rinnegan_controlled";
            rinneganControlled.locale_description = "status_description_rinnegan_controlled";
            AssetManager.status.add(rinneganControlled);

            StatusAsset madaraTamed = new StatusAsset();
            madaraTamed.id = "status_madara_tamed";
            madaraTamed.duration = 15f;
            madaraTamed.animated = false;
            madaraTamed.path_icon = "ui/icons/madara_ems";
            madaraTamed.base_stats = new BaseStats();
            madaraTamed.locale_id = "status_title_madara_tamed";
            madaraTamed.locale_description = "status_description_madara_tamed";
            madaraTamed.action_finish = (WorldAction)Delegate.Combine(madaraTamed.action_finish, new WorldAction(JutsuLibrary.RestoreKingdom));
            AssetManager.status.add(madaraTamed);

            #endregion

            #region Chakra Exhaustion
            // Chakra Exhaustion
            StatusAsset exhaustion = new StatusAsset();
            exhaustion.id = "chakra_exhaustion";
            exhaustion.base_stats = new BaseStats();
            exhaustion.base_stats.set("multiplier_speed", -0.30f);
            exhaustion.base_stats.set("multiplier_damage", -0.20f);
            exhaustion.base_stats.set("multiplier_attack_speed", -0.40f);
            exhaustion.base_stats.set("armor", -10f);
            exhaustion.duration = 20f;
            exhaustion.path_icon = "ui/icons/chakra_exhaustion";
            exhaustion.animated = false;

            exhaustion.locale_id = "status_title_chakra_exhaustion";
            exhaustion.locale_description = "status_description_chakra_exhaustion";

            exhaustion.action_on_receive = (WorldAction)Delegate.Combine(exhaustion.action_on_receive, new WorldAction(action_freeze));
            AssetManager.status.add(exhaustion);
            #endregion

            #region Genjutsu Effect
            // Genjutsu 1: Daze
            StatusAsset genjutsu1 = new StatusAsset();
            genjutsu1.id = "genjutsu_1";
            genjutsu1.base_stats = new BaseStats();
            genjutsu1.base_stats.set("multiplier_speed", -0.20f);
            genjutsu1.base_stats.set("armor", -5f);
            genjutsu1.duration = 10f;
            genjutsu1.path_icon = "ui/icons/sharingan_1t";

            genjutsu1.texture = "fx_sharingan";
            genjutsu1.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{genjutsu1.texture}", false);
            genjutsu1.animated = true;
            genjutsu1.need_visual_render = true;
            genjutsu1.is_animated_in_pause = false;
            genjutsu1.can_be_flipped = true;
            genjutsu1.removed_on_damage = false;
            genjutsu1.scale = 1.0f;
            genjutsu1.render_priority = 5;
            genjutsu1.material = material;

            genjutsu1.locale_id = "status_title_genjutsu_1";
            genjutsu1.locale_description = "status_description_genjutsu_1";

            AssetManager.status.add(genjutsu1);


            // Genjutsu 2: Illusion
            StatusAsset genjutsu2 = new StatusAsset();
            genjutsu2.id = "genjutsu_2";
            genjutsu2.base_stats = new BaseStats();
            genjutsu2.base_stats.set("multiplier_speed", -0.50f);
            genjutsu2.base_stats.set("armor", -10f);
            genjutsu2.duration = 8f;
            genjutsu2.path_icon = "ui/icons/sharingan_2t";

            genjutsu2.texture = "fx_sharingan";
            genjutsu2.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{genjutsu2.texture}", false);
            genjutsu2.animated = true;
            genjutsu2.need_visual_render = true;
            genjutsu2.is_animated_in_pause = false;
            genjutsu2.can_be_flipped = true;
            genjutsu2.removed_on_damage = false;
            genjutsu2.scale = 1.0f;
            genjutsu2.render_priority = 5;
            genjutsu2.material = material;

            genjutsu2.locale_id = "status_title_genjutsu_2";
            genjutsu2.locale_description = "status_description_genjutsu_2";

            AssetManager.status.add(genjutsu2);


            // Genjutsu 3: Paralysis
            StatusAsset genjutsu3 = new StatusAsset();
            genjutsu3.id = "genjutsu_3";
            genjutsu3.base_stats = new BaseStats();
            genjutsu3.base_stats.set("multiplier_speed", -1.0f);
            genjutsu3.duration = 10f;
            genjutsu3.path_icon = "ui/icons/sharingan_3t";

            genjutsu3.texture = "fx_sharingan";
            genjutsu3.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{genjutsu3.texture}", false);
            genjutsu3.animated = true;
            genjutsu3.need_visual_render = true;
            genjutsu3.is_animated_in_pause = false;
            genjutsu3.can_be_flipped = true;
            genjutsu3.removed_on_damage = false;
            genjutsu3.scale = 1.0f;
            genjutsu3.render_priority = 5;
            genjutsu3.material = material;

            genjutsu3.locale_id = "status_title_genjutsu_3";
            genjutsu3.locale_description = "status_description_genjutsu_3";

            genjutsu3.action_on_receive = (WorldAction)Delegate.Combine(genjutsu3.action_on_receive, new WorldAction(action_freeze));
            AssetManager.status.add(genjutsu3);
            #endregion

            #region Sharingan 1 - 3 Tomoe
            StatusAsset sharingan1 = new StatusAsset();
            sharingan1.id = "status_sharingan_1t";
            sharingan1.base_stats = new BaseStats();
            sharingan1.base_stats.set("damage", 31f);
            sharingan1.base_stats.set("multiplier_damage", 0.02f);
            sharingan1.base_stats.set("health", 48f);
            sharingan1.base_stats.set("intelligence", 10f);
            sharingan1.base_stats.set("speed", 5f);
            sharingan1.base_stats.set("critical_chance", 0.05f);
            sharingan1.base_stats.set("accuracy", 0.5f);
            sharingan1.base_stats.set("chakra", 32f);
            sharingan1.duration = 30f;
            sharingan1.path_icon = "ui/icons/sharingan_1t";
            sharingan1.locale_id = "status_title_sharingan_1t";
            sharingan1.locale_description = "status_description_sharingan_1t";
            AssetManager.status.add(sharingan1);

            StatusAsset sharingan2 = new StatusAsset();
            sharingan2.id = "status_sharingan_2t";
            sharingan2.base_stats = new BaseStats();
            sharingan2.base_stats.set("damage", 65f);
            sharingan2.base_stats.set("multiplier_damage", 0.07f);
            sharingan2.base_stats.set("health", 98f);
            sharingan2.base_stats.set("multiplier_health", 0.05f);
            sharingan2.base_stats.set("intelligence", 20f);
            sharingan2.base_stats.set("speed", 7f);
            sharingan2.base_stats.set("critical_chance", 0.10f);
            sharingan2.base_stats.set("critical_damage_multiplier", 0.05f);
            sharingan2.base_stats.set("accuracy", 1f);
            sharingan2.base_stats.set("chakra", 64f);
            sharingan2.duration = 30f;
            sharingan2.path_icon = "ui/icons/sharingan_2t";
            sharingan2.locale_id = "status_title_sharingan_2t";
            sharingan2.locale_description = "status_description_sharingan_2t";
            AssetManager.status.add(sharingan2);

            StatusAsset sharingan3 = new StatusAsset();
            sharingan3.id = "status_sharingan_3t";
            sharingan3.base_stats = new BaseStats();
            sharingan3.base_stats.set("damage", 91f);
            sharingan3.base_stats.set("multiplier_damage", 0.10f);
            sharingan3.base_stats.set("health", 136f);
            sharingan3.base_stats.set("multiplier_health", 0.10f);
            sharingan3.base_stats.set("intelligence", 40f);
            sharingan3.base_stats.set("speed", 10f);
            sharingan3.base_stats.set("critical_chance", 0.15f);
            sharingan3.base_stats.set("critical_damage_multiplier", 0.10f);
            sharingan3.base_stats.set("accuracy", 1.5f);
            sharingan3.base_stats.set("chakra", 96f);
            sharingan3.duration = 30f;
            sharingan3.path_icon = "ui/icons/sharingan_3t";
            sharingan3.locale_id = "status_title_sharingan_3t";
            sharingan3.locale_description = "status_description_sharingan_3t";
            AssetManager.status.add(sharingan3);
            #endregion

            #region Amaterasu Effect
            // Amaterasu
            StatusAsset amaterasu = new StatusAsset();
            amaterasu.id = "status_amaterasu";
            amaterasu.base_stats = new BaseStats();
            amaterasu.base_stats.set("multiplier_speed", -0.90f);
            amaterasu.base_stats.set("armor", -50f);
            amaterasu.base_stats.set("multiplier_attack_speed", -0.90f);
            amaterasu.duration = 60f;
            amaterasu.path_icon = "effects/amaterasu";

            amaterasu.animated = true;
            amaterasu.texture = "fx_amaterasu";
            amaterasu.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{amaterasu.texture}", false);
            amaterasu.material = material;

            amaterasu.tier = StatusTier.Advanced;
            amaterasu.need_visual_render = true;
            amaterasu.is_animated_in_pause = false;
            amaterasu.can_be_flipped = true;
            amaterasu.removed_on_damage = false;
            amaterasu.scale = 0.4f;
            amaterasu.locale_id = "status_title_status_amaterasu";
            amaterasu.locale_description = "status_description_status_amaterasu";
            amaterasu.action = (WorldAction)Delegate.Combine(amaterasu.action, new WorldAction(amaterasu_burn));

            AssetManager.status.add(amaterasu);
            #endregion

            #region Weak Eye Effect
            StatusAsset weakEye = new StatusAsset();
            weakEye.id = "weak_eye";
            weakEye.base_stats = new BaseStats();
            weakEye.duration = 999f;
            weakEye.path_icon = "ui/icons/blind";
            weakEye.locale_id = "status_title_weak_eye";
            weakEye.locale_description = "status_description_weak_eye";
            AssetManager.status.add(weakEye);
            #endregion

            #region Suanoo Ribcage Effect
            // Susanoo Ribcage
            StatusAsset ribcage = new StatusAsset();
            ribcage.id = "susanoo_ribcage";
            ribcage.base_stats = new BaseStats();
            ribcage.base_stats.set("armor", 20f);
            ribcage.duration = 10f;
            ribcage.path_icon = "effects/HalfSusa";

            ribcage.animated = true;
            ribcage.texture = "fx_halfsusa";
            ribcage.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{ribcage.texture}", false);
            ribcage.material = material;

            ribcage.tier = StatusTier.Advanced;
            ribcage.need_visual_render = true;
            ribcage.is_animated_in_pause = false;
            ribcage.can_be_flipped = true;
            ribcage.removed_on_damage = false;
            ribcage.scale = 0.4f;
            ribcage.locale_id = "status_title_susanoo_ribcage";
            ribcage.locale_description = "status_description_susanoo_ribcage";

            AssetManager.status.add(ribcage);
            #endregion

            #region KCM Aura Effect
            StatusAsset kcmAura = new StatusAsset();
            kcmAura.id = "kcm_aura";
            kcmAura.path_icon = "ui/icons/kcm2";
            kcmAura.animated = true;
            kcmAura.duration = 2f;

            kcmAura.texture = "fx_kcmAura";
            kcmAura.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{kcmAura.texture}", false);
            kcmAura.material = material;

            kcmAura.need_visual_render = true;
            kcmAura.is_animated_in_pause = false;
            kcmAura.can_be_flipped = true;
            kcmAura.use_parent_rotation = true;
            kcmAura.scale = 1.0f;
            kcmAura.render_priority = 5;

            kcmAura.locale_id = "status_title_kcm_aura";
            kcmAura.locale_description = "status_description_kcm_aura";

            AssetManager.status.add(kcmAura);
            #endregion

            #region V1 Aura Effect
            StatusAsset v1Aura = new StatusAsset();
            v1Aura.id = "v1_aura";
            v1Aura.path_icon = "ui/icons/initial_release";
            v1Aura.animated = true;
            v1Aura.duration = 2f;

            v1Aura.texture = "fx_v1Aura";
            v1Aura.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{v1Aura.texture}", false);
            v1Aura.material = material;

            v1Aura.need_visual_render = true;
            v1Aura.is_animated_in_pause = false;
            v1Aura.can_be_flipped = true;
            v1Aura.scale = 1.0f;
            v1Aura.render_priority = 5;

            v1Aura.locale_id = "status_title_v1_aura";
            v1Aura.locale_description = "status_description_v1_aura";

            AssetManager.status.add(v1Aura);
            #endregion

            #region Baryon Life Decay
            // Baryon Life Decay
            StatusAsset baryonDecay = new StatusAsset();
            baryonDecay.id = "baryon_life_decay";
            baryonDecay.path_icon = "ui/icons/baryon";
            baryonDecay.base_stats = new BaseStats();
            baryonDecay.base_stats.set("multiplier_lifespan", -0.50f);
            baryonDecay.base_stats.set("multiplier_health", -0.60f);
            baryonDecay.duration = 10f;
            baryonDecay.locale_id = "status_title_baryon_life_decay";
            baryonDecay.locale_description = "status_description_baryon_life_decay";

            AssetManager.status.add(baryonDecay);
            #endregion

            #region Kaiten (Eight Trigrams: Palm Rotation)
            // Kaiten
            StatusAsset kaiten = new StatusAsset();
            kaiten.id = "status_kaiten";
            kaiten.base_stats = new BaseStats();
            kaiten.base_stats.set("armor", 250f);
            kaiten.base_stats.set("multiplier_speed", -1f);
            kaiten.base_stats.set("range", 3.0f);
            kaiten.base_stats.set("attack_speed", 25f);
            kaiten.base_stats.set("knockback", 2.0f);

            kaiten.duration = 4f;
            kaiten.path_icon = "effects/kaitenicon";

            kaiten.animated = true;
            kaiten.texture = "fx_kaiten";
            kaiten.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{kaiten.texture}", false);
            kaiten.material = material;

            kaiten.need_visual_render = true;
            kaiten.use_parent_rotation = false;
            kaiten.is_animated_in_pause = false;
            kaiten.can_be_flipped = true;
            kaiten.scale = 0.3f;
            kaiten.render_priority = 5;

            kaiten.locale_id = "status_title_kaiten";
            kaiten.locale_description = "status_description_kaiten";

            kaiten.action = (WorldAction)Delegate.Combine(kaiten.action, new WorldAction(KaitenShredAction));

            AssetManager.status.add(kaiten);

            // Cooldown
            StatusAsset kaitenCD = new StatusAsset();
            kaitenCD.id = "kaiten_cooldown";
            kaitenCD.path_icon = "effects/byakugan_cooldown";
            kaitenCD.duration = 15f;
            kaitenCD.animated = false;
            AssetManager.status.add(kaitenCD);
            #endregion

            #region Eight Trigrams: 64 Palms

            #region Eight Trigrams: 64 Palms (Hit)
            StatusAsset gettingHit = new StatusAsset();
            gettingHit.id = "status_getting_hit_64";
            gettingHit.duration = 5f;
            gettingHit.animated = false;
            gettingHit.path_icon = "ui/icons/byakugan";

            gettingHit.base_stats = new BaseStats();
            gettingHit.base_stats.set("speed", -999f);
            gettingHit.base_stats.set("attack_speed", -100f);

            gettingHit.locale_id = "status_title_64_palms";
            gettingHit.locale_description = "status_description_64_palms";

            gettingHit.action = (WorldAction)Delegate.Combine(gettingHit.action, new WorldAction(EightTrigrams64Palms));

            AssetManager.status.add(gettingHit);
            #endregion

            #region Eight Trigrams: 64 Palms (Attack)
            StatusAsset eightAttack = new StatusAsset();
            eightAttack.id = "eightAttack";
            eightAttack.path_icon = "effects/byakugan";
            eightAttack.duration = 5f;

            eightAttack.base_stats = new BaseStats();
            eightAttack.base_stats.set("multiplier_damage", 0.20f);

            eightAttack.animated = true;
            eightAttack.texture = "fx_eightT";
            eightAttack.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{eightAttack.texture}", false);
            eightAttack.material = material;

            eightAttack.use_parent_rotation = false;
            eightAttack.removed_on_damage = false;
            eightAttack.need_visual_render = true;
            eightAttack.scale = 0.9f;
            eightAttack.render_priority = 0;
            eightAttack.can_be_flipped = false;
            eightAttack.loop = false;

            eightAttack.locale_id = "status_title_eightAttack";
            eightAttack.locale_description = "status_description_eightAttack";

            AssetManager.status.add(eightAttack);
            #endregion

            #endregion

            #region Mokuton Tree
            StatusAsset mokuton = new StatusAsset();
            mokuton.id = "status_mokuton_tree";
            mokuton.base_stats = new BaseStats();
            mokuton.base_stats.set("speed", -999f);
            mokuton.base_stats.set("attack_speed", -999f);
            mokuton.duration = 5f;

            mokuton.path_icon = "effects/status_mokuton";

            mokuton.animated = true;
            mokuton.texture = "fx_mokuton_tree";

            mokuton.sprite_list = SpriteTextureLoader.getSpriteList("effects/fx_mokuton_tree", false);

            mokuton.material = material;
            mokuton.need_visual_render = true;
            mokuton.is_animated_in_pause = false;
            mokuton.use_parent_rotation = false;
            mokuton.can_be_flipped = true;
            mokuton.removed_on_damage = false;
            mokuton.scale = 0.2f;
            mokuton.render_priority = 5;
            mokuton.loop = false;

            mokuton.locale_id = "status_title_mokuton_tree";
            mokuton.locale_description = "status_description_mokuton_tree";

            mokuton.action_on_receive = (WorldAction)Delegate.Combine(mokuton.action_on_receive, new WorldAction(action_freeze));

            AssetManager.status.add(mokuton);
            #endregion

            #region Water Prison
            StatusAsset waterPrison = new StatusAsset();
            waterPrison.id = "water_prison";
            waterPrison.base_stats = new BaseStats();
            waterPrison.base_stats.set("speed", -999f);
            waterPrison.base_stats.set("attack_speed", -999f);
            waterPrison.duration = 5f;
            waterPrison.path_icon = "ui/icons/water_nature";

            waterPrison.animated = true;
            waterPrison.texture = "fx_water_prison";
            waterPrison.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{waterPrison.texture}", false);
            waterPrison.material = material;

            waterPrison.need_visual_render = true;
            waterPrison.use_parent_rotation = false;
            waterPrison.can_be_flipped = true;
            waterPrison.removed_on_damage = false;
            waterPrison.scale = 0.5f;
            waterPrison.locale_id = "status_title_water_prison";
            waterPrison.locale_description = "status_description_water_prison";

            waterPrison.action_on_receive = (WorldAction)Delegate.Combine(waterPrison.action_on_receive, new WorldAction(action_freeze));
            AssetManager.status.add(waterPrison);
            #endregion

            #region Base Susanoo (Mangekyo)
            StatusAsset baseSusanoo = new StatusAsset();
            baseSusanoo.id = "status_base_susanoo";
            baseSusanoo.base_stats = new BaseStats();
            baseSusanoo.base_stats.set("armor", 35f);
            baseSusanoo.base_stats.set("damage", 150f);
            baseSusanoo.base_stats.set("multiplier_damage", 0.20f);
            baseSusanoo.base_stats.set("health", 500f);
            baseSusanoo.base_stats.set("scale", 0.05f);
            baseSusanoo.base_stats.set("knockback", 1f);
            baseSusanoo.base_stats.set("area_of_effect", 0.30f);
            baseSusanoo.base_stats.set("range", 5f);
            baseSusanoo.duration = 1f;
            baseSusanoo.path_icon = "ui/icons/mangekyo";

            baseSusanoo.animated = true;
            baseSusanoo.texture = "fx_base_susanoo";
            baseSusanoo.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{baseSusanoo.texture}", false);
            baseSusanoo.material = material;

            baseSusanoo.need_visual_render = true;
            baseSusanoo.is_animated_in_pause = false;
            baseSusanoo.removed_on_damage = false;
            baseSusanoo.use_parent_rotation = false;
            baseSusanoo.can_be_flipped = true;
            baseSusanoo.scale = 0.1f;
            baseSusanoo.locale_id = "status_title_base_susanoo";
            baseSusanoo.locale_description = "status_description_base_susanoo";
            AssetManager.status.add(baseSusanoo);
            #endregion

            #region Perfect Susanoo (EMS)
            StatusAsset perfectSusanoo = new StatusAsset();
            perfectSusanoo.id = "status_perfect_susanoo";
            perfectSusanoo.base_stats = new BaseStats();
            perfectSusanoo.base_stats.set("armor", 75f);
            perfectSusanoo.base_stats.set("damage", 500f);
            perfectSusanoo.base_stats.set("multiplier_damage", 0.30f);
            perfectSusanoo.base_stats.set("health", 2000f);
            perfectSusanoo.base_stats.set("range", 9f);
            perfectSusanoo.base_stats.set("knockback", 1.5f);
            perfectSusanoo.base_stats.set("area_of_effect", 0.80f);
            perfectSusanoo.duration = 1f;
            perfectSusanoo.path_icon = "ui/icons/ems";

            perfectSusanoo.animated = true;
            perfectSusanoo.texture = "fx_perfect_susanoo";
            perfectSusanoo.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{perfectSusanoo.texture}", false);
            perfectSusanoo.material = material;

            perfectSusanoo.need_visual_render = true;
            perfectSusanoo.removed_on_damage = false;
            perfectSusanoo.use_parent_rotation = false;
            perfectSusanoo.is_animated_in_pause = false;
            perfectSusanoo.can_be_flipped = true;
            perfectSusanoo.scale = 0.4f;
            perfectSusanoo.locale_id = "status_title_perfect_susanoo";
            perfectSusanoo.locale_description = "status_description_perfect_susanoo";

            AssetManager.status.add(perfectSusanoo);
            #endregion

            #region Kurama Avatar
            /*
            StatusAsset kuramaAvatar = new StatusAsset();
            kuramaAvatar.id = "status_kurama_avatar";
            kuramaAvatar.base_stats = new BaseStats();
            kuramaAvatar.base_stats.set("armor", 80f);
            kuramaAvatar.base_stats.set("damage", 500f);
            kuramaAvatar.base_stats.set("health", 5000f);
            kuramaAvatar.base_stats.set("scale", 0.20f);
            kuramaAvatar.base_stats.set("knockback", 2f);
            kuramaAvatar.duration = 20f;
            kuramaAvatar.path_icon = "ui/icons/avatar";

            kuramaAvatar.animated = true;
            kuramaAvatar.texture = "fx_kurama_avatar";
            kuramaAvatar.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{kuramaAvatar.texture}", false);
            kuramaAvatar.material = material;

            kuramaAvatar.need_visual_render = true;
            kuramaAvatar.is_animated_in_pause = false;
            kuramaAvatar.can_be_flipped = true;
            kuramaAvatar.scale = 1.5f;
            kuramaAvatar.locale_id = "status_title_kurama_avatar";
            AssetManager.status.add(kuramaAvatar);
            */
            #endregion

            #region Kamui
            // Kamui
            StatusAsset kamui = new StatusAsset();
            kamui.id = "kamui_intangibility";
            kamui.base_stats = new BaseStats();
            kamui.base_stats.set("armor", 999f);
            kamui.base_stats.set("speed", -999f);
            kamui.base_stats.set("multiplier_speed", -1f);
            kamui.duration = 6f;
            kamui.path_icon = "effects/kamui";

            kamui.animated = true;
            kamui.texture = "fx_kamui";
            kamui.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{kamui.texture}", false);
            kamui.material = material;

            kamui.is_animated_in_pause = false;
            kamui.can_be_flipped = true;
            kamui.use_parent_rotation = false;
            kamui.removed_on_damage = false;
            kamui.cancel_actor_job = false;
            kamui.need_visual_render = true;
            kamui.scale = 0.8f;

            kamui.locale_id = "status_title_kamui_intangibility";
            kamui.locale_description = "status_description_kamui_intangibility";
            kamui.action = (WorldAction)Delegate.Combine(kamui.action, new WorldAction(action_freeze));

            AssetManager.status.add(kamui);
            #endregion

            #region Justsu Global Cooldown
            StatusAsset JutsuCD = new StatusAsset();
            JutsuCD.id = "status_ability_cooldown";
            JutsuCD.path_icon = "effects/abilitycooldown";
            JutsuCD.duration = 4f;
            JutsuCD.animated = false;

            JutsuCD.locale_id = "status_title_ability_cooldown";
            JutsuCD.locale_description = "status_description_ability_cooldown";

            AssetManager.status.add(JutsuCD);
            #endregion

            #region Eight Inner Gates Statuses
            StatusAsset gate1 = new StatusAsset();
            gate1.id = "inner_gate_1";
            gate1.base_stats = new BaseStats();
            gate1.base_stats.set("multiplier_speed", 0.05f);
            gate1.base_stats.set("multiplier_attack_speed", 0.05f);
            gate1.duration = 25f;
            gate1.path_icon = "ui/icons/eight_gates";

            gate1.animated = true;
            gate1.texture = "fx_eightGates1";
            gate1.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{gate1.texture}", false);
            gate1.material = material;

            gate1.is_animated_in_pause = false;
            gate1.can_be_flipped = true;
            gate1.use_parent_rotation = false;
            gate1.removed_on_damage = false;
            gate1.cancel_actor_job = false;
            gate1.need_visual_render = true;
            gate1.scale = 0.6f;

            gate1.locale_id = "status_title_inner_gate_1";
            gate1.locale_description = "status_description_inner_gate_1";
            gate1.action = (WorldAction)Delegate.Combine(gate1.action, new WorldAction(JutsuLibrary.InnerGatesPulseAction));
            AssetManager.status.add(gate1);

            StatusAsset gate2 = new StatusAsset();
            gate2.id = "inner_gate_2";
            gate2.base_stats = new BaseStats();
            gate2.base_stats.set("multiplier_damage", 0.05f);
            gate2.base_stats.set("multiplier_speed", 0.08f);
            gate2.base_stats.set("multiplier_attack_speed", 0.08f);
            gate2.duration = 30f;
            gate2.path_icon = "ui/icons/eight_gates";

            gate2.animated = true;
            gate2.texture = "fx_eightGates1";
            gate2.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{gate2.texture}", false);
            gate2.material = material;

            gate2.is_animated_in_pause = false;
            gate2.can_be_flipped = true;
            gate2.use_parent_rotation = false;
            gate2.removed_on_damage = false;
            gate2.cancel_actor_job = false;
            gate2.need_visual_render = true;
            gate2.scale = 0.6f;

            gate2.locale_id = "status_title_inner_gate_2";
            gate2.locale_description = "status_description_inner_gate_2";
            gate2.action = (WorldAction)Delegate.Combine(gate2.action, new WorldAction(JutsuLibrary.InnerGatesPulseAction));
            AssetManager.status.add(gate2);

            StatusAsset gate3 = new StatusAsset();
            gate3.id = "inner_gate_3";
            gate3.base_stats = new BaseStats();
            gate3.base_stats.set("multiplier_damage", 0.10f);
            gate3.base_stats.set("multiplier_speed", 0.10f);
            gate3.base_stats.set("multiplier_attack_speed", 0.12f);
            gate3.duration = 35f;
            gate3.path_icon = "ui/icons/eight_gates";

            gate3.animated = true;
            gate3.texture = "fx_eightGates3";
            gate3.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{gate3.texture}", false);
            gate3.material = material;

            gate3.is_animated_in_pause = false;
            gate3.can_be_flipped = true;
            gate3.use_parent_rotation = false;
            gate3.removed_on_damage = false;
            gate3.cancel_actor_job = false;
            gate3.need_visual_render = true;
            gate3.scale = 0.6f;

            gate3.locale_id = "status_title_inner_gate_3";
            gate3.locale_description = "status_description_inner_gate_3";
            gate3.action = (WorldAction)Delegate.Combine(gate3.action, new WorldAction(JutsuLibrary.InnerGatesPulseAction));
            AssetManager.status.add(gate3);

            StatusAsset gate4 = new StatusAsset();
            gate4.id = "inner_gate_4";
            gate4.base_stats = new BaseStats();
            gate4.base_stats.set("multiplier_damage", 0.18f);
            gate4.base_stats.set("multiplier_speed", 0.15f);
            gate4.base_stats.set("multiplier_attack_speed", 0.16f);
            gate4.duration = 40f;
            gate4.path_icon = "ui/icons/eight_gates";

            gate4.animated = true;
            gate4.texture = "fx_eightGates3";
            gate4.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{gate4.texture}", false);
            gate4.material = material;

            gate4.is_animated_in_pause = false;
            gate4.can_be_flipped = true;
            gate4.use_parent_rotation = false;
            gate4.removed_on_damage = false;
            gate4.cancel_actor_job = false;
            gate4.need_visual_render = true;
            gate4.scale = 0.6f;

            gate4.locale_id = "status_title_inner_gate_4";
            gate4.locale_description = "status_description_inner_gate_4";
            gate4.action = (WorldAction)Delegate.Combine(gate4.action, new WorldAction(JutsuLibrary.InnerGatesPulseAction));
            AssetManager.status.add(gate4);

            StatusAsset gate5 = new StatusAsset();
            gate5.id = "inner_gate_5";
            gate5.base_stats = new BaseStats();
            gate5.base_stats.set("multiplier_damage", 0.25f);
            gate5.base_stats.set("multiplier_speed", 0.20f);
            gate5.base_stats.set("multiplier_attack_speed", 0.22f);
            gate5.base_stats.set("armor", 8f);
            gate5.duration = 45f;
            gate5.path_icon = "ui/icons/eight_gates";

            gate5.animated = true;
            gate5.texture = "fx_eightGates3";
            gate5.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{gate5.texture}", false);
            gate5.material = material;

            gate5.is_animated_in_pause = false;
            gate5.can_be_flipped = true;
            gate5.use_parent_rotation = false;
            gate5.removed_on_damage = false;
            gate5.cancel_actor_job = false;
            gate5.need_visual_render = true;
            gate5.scale = 0.6f;

            gate5.locale_id = "status_title_inner_gate_5";
            gate5.locale_description = "status_description_inner_gate_5";
            gate5.action = (WorldAction)Delegate.Combine(gate5.action, new WorldAction(JutsuLibrary.InnerGatesPulseAction));
            AssetManager.status.add(gate5);

            StatusAsset gate6 = new StatusAsset();
            gate6.id = "inner_gate_6";
            gate6.base_stats = new BaseStats();
            gate6.base_stats.set("multiplier_damage", 0.35f);
            gate6.base_stats.set("multiplier_speed", 0.26f);
            gate6.base_stats.set("multiplier_attack_speed", 0.28f);
            gate6.base_stats.set("armor", 12f);
            gate6.duration = 50f;
            gate6.path_icon = "ui/icons/eight_gates";

            gate6.animated = true;
            gate6.texture = "fx_eightGates3";
            gate6.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{gate6.texture}", false);
            gate6.material = material;

            gate6.is_animated_in_pause = false;
            gate6.can_be_flipped = true;
            gate6.use_parent_rotation = false;
            gate6.removed_on_damage = false;
            gate6.cancel_actor_job = false;
            gate6.need_visual_render = true;
            gate6.scale = 0.6f;

            gate6.locale_id = "status_title_inner_gate_6";
            gate6.locale_description = "status_description_inner_gate_6";
            gate6.action = (WorldAction)Delegate.Combine(gate6.action, new WorldAction(JutsuLibrary.InnerGatesPulseAction));
            AssetManager.status.add(gate6);

            StatusAsset gate7 = new StatusAsset();
            gate7.id = "inner_gate_7";
            gate7.base_stats = new BaseStats();
            gate7.base_stats.set("multiplier_damage", 0.50f);
            gate7.base_stats.set("multiplier_speed", 0.32f);
            gate7.base_stats.set("multiplier_attack_speed", 0.34f);
            gate7.base_stats.set("armor", 18f);
            gate7.duration = 55f;
            gate7.path_icon = "ui/icons/eight_gates";

            gate7.animated = true;
            gate7.texture = "fx_eightGates3";
            gate7.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{gate7.texture}", false);
            gate7.material = material;

            gate7.is_animated_in_pause = false;
            gate7.can_be_flipped = true;
            gate7.use_parent_rotation = false;
            gate7.removed_on_damage = false;
            gate7.cancel_actor_job = false;
            gate7.need_visual_render = true;
            gate7.scale = 0.6f;

            gate7.locale_id = "status_title_inner_gate_7";
            gate7.locale_description = "status_description_inner_gate_7";
            gate7.action = (WorldAction)Delegate.Combine(gate7.action, new WorldAction(JutsuLibrary.InnerGatesPulseAction));
            AssetManager.status.add(gate7);

            StatusAsset gate8 = new StatusAsset();
            gate8.id = "inner_gate_8";
            gate8.base_stats = new BaseStats();
            gate8.base_stats.set("multiplier_damage", 0.80f);
            gate8.base_stats.set("multiplier_speed", 0.40f);
            gate8.base_stats.set("multiplier_attack_speed", 0.45f);
            gate8.base_stats.set("armor", 25f);
            gate8.duration = 60f;
            gate8.path_icon = "ui/icons/eight_gates";
            gate8.locale_id = "status_title_inner_gate_8";

            gate8.animated = true;
            gate8.texture = "fx_eightGates8";
            gate8.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{gate8.texture}", false);
            gate8.material = material;

            gate8.is_animated_in_pause = false;
            gate8.can_be_flipped = true;
            gate8.use_parent_rotation = false;
            gate8.removed_on_damage = false;
            gate8.cancel_actor_job = false;
            gate8.need_visual_render = true;
            gate8.scale = 0.6f;

            gate8.locale_description = "status_description_inner_gate_8";
            gate8.action = (WorldAction)Delegate.Combine(gate8.action, new WorldAction(JutsuLibrary.InnerGatesPulseAction));
            AssetManager.status.add(gate8);

            StatusAsset gateStrain = new StatusAsset();
            gateStrain.id = "eight_gate_strain";
            gateStrain.base_stats = new BaseStats();
            gateStrain.base_stats.set("multiplier_damage", -0.20f);
            gateStrain.base_stats.set("multiplier_speed", -0.25f);
            gateStrain.base_stats.set("multiplier_attack_speed", -0.30f);
            gateStrain.base_stats.set("armor", -12f);
            gateStrain.duration = 30f;
            gateStrain.path_icon = "ui/icons/chakra_exhaustion";
            gateStrain.locale_id = "status_title_eight_gate_strain";
            gateStrain.locale_description = "status_description_eight_gate_strain";
            AssetManager.status.add(gateStrain);
            #endregion

            #region Imperfect Frog Sage
            StatusAsset imperfectFrog = new StatusAsset();
            imperfectFrog.id = "status_imperfect_frog_sage";
            imperfectFrog.base_stats = new BaseStats();
            imperfectFrog.base_stats.set("multiplier_damage", 0.05f);
            imperfectFrog.base_stats.set("multiplier_health", 0.05f);
            imperfectFrog.base_stats.set("range", 0.5f);
            imperfectFrog.base_stats.set("experience", 5);
            imperfectFrog.duration = 999f;
            imperfectFrog.path_icon = "ui/icons/sage_mode";
            imperfectFrog.locale_id = "status_title_imperfect_frog_sage";
            imperfectFrog.locale_description = "status_description_imperfect_frog_sage";
            AssetManager.status.add(imperfectFrog);
            #endregion

            #region Imperfect Slug Sage
            StatusAsset imperfectSlug = new StatusAsset();
            imperfectSlug.id = "status_imperfect_slug_sage";
            imperfectSlug.base_stats = new BaseStats();
            imperfectSlug.base_stats.set("armor", 8f);
            imperfectSlug.base_stats.set("multiplier_health", 0.10f);
            imperfectSlug.base_stats.set("experience", 5);
            imperfectSlug.duration = 999f;
            imperfectSlug.path_icon = "ui/icons/slug_sage";
            imperfectSlug.locale_id = "status_title_imperfect_slug_sage";
            imperfectSlug.locale_description = "status_description_imperfect_slug_sage";
            AssetManager.status.add(imperfectSlug);
            #endregion

            #region Imperfect Snake Sage
            StatusAsset imperfectSnake = new StatusAsset();
            imperfectSnake.id = "status_imperfect_snake_sage";
            imperfectSnake.base_stats = new BaseStats();
            imperfectSnake.base_stats.set("multiplier_speed", 0.06f);
            imperfectSnake.base_stats.set("multiplier_damage", 0.08f);
            imperfectSnake.base_stats.set("experience", 5);
            imperfectSnake.duration = 999f;
            imperfectSnake.path_icon = "ui/icons/snake_sage";
            imperfectSnake.locale_id = "status_title_imperfect_snake_sage";
            imperfectSnake.locale_description = "status_description_imperfect_snake_sage";
            AssetManager.status.add(imperfectSnake);
            #endregion

            #region Perfect Frog Sage
            StatusAsset perfectFrog = new StatusAsset();
            perfectFrog.id = "status_perfect_frog_sage";
            perfectFrog.base_stats = new BaseStats();
            perfectFrog.base_stats.set("multiplier_damage", 0.15f);
            perfectFrog.base_stats.set("multiplier_health", 0.15f);
            perfectFrog.base_stats.set("multiplier_speed", 0.05f);
            perfectFrog.base_stats.set("critical_chance", 0.25f);
            perfectFrog.base_stats.set("critical_damage_multiplier", 0.25f);
            perfectFrog.base_stats.set("multiplier_lifespan", 0.30f);
            perfectFrog.base_stats.set("intelligence", 60f);
            perfectFrog.base_stats.set("range", 1f);
            perfectFrog.base_stats.set("accuracy", 1f);
            perfectFrog.base_stats.set("experience", 15);
            perfectFrog.duration = 999f;
            perfectFrog.path_icon = "ui/icons/sage_mode";
            perfectFrog.locale_id = "status_title_perfect_frog_sage";
            perfectFrog.locale_description = "status_description_perfect_frog_sage";
            AssetManager.status.add(perfectFrog);
            #endregion

            #region Perfect Slug Sage
            StatusAsset perfectSlug = new StatusAsset();
            perfectSlug.id = "status_perfect_slug_sage";
            perfectSlug.base_stats = new BaseStats();
            perfectSlug.base_stats.set("armor", 20f);
            perfectSlug.base_stats.set("multiplier_health", 0.20f);
            perfectSlug.base_stats.set("experience", 15);
            perfectSlug.duration = 999f;
            perfectSlug.path_icon = "ui/icons/slug_sage";
            perfectSlug.locale_id = "status_title_perfect_slug_sage";
            perfectSlug.locale_description = "status_description_perfect_slug_sage";
            AssetManager.status.add(perfectSlug);
            #endregion

            #region Perfect Snake Sage
            StatusAsset perfectSnake = new StatusAsset();
            perfectSnake.id = "status_perfect_snake_sage";
            perfectSnake.base_stats = new BaseStats();
            perfectSnake.base_stats.set("multiplier_speed", 0.15f);
            perfectSnake.base_stats.set("multiplier_damage", 0.18f);
            perfectSnake.base_stats.set("critical_chance", 0.10f);
            perfectSnake.base_stats.set("experience", 15);
            perfectSnake.duration = 999f;
            perfectSnake.path_icon = "ui/icons/snake_sage";
            perfectSnake.locale_id = "status_title_perfect_snake_sage";
            perfectSnake.locale_description = "status_description_perfect_snake_sage";
            AssetManager.status.add(perfectSnake);
            #endregion

            #region Strength Of A Hundred Status
            StatusAsset strengthHundred = new StatusAsset();
            strengthHundred.id = "status_strength_hundred";
            strengthHundred.base_stats = new BaseStats();
            strengthHundred.base_stats.set("multiplier_health", 0.40f);
            strengthHundred.duration = 15f;
            strengthHundred.path_icon = "ui/icons/slug_sage";
            strengthHundred.locale_id = "status_title_strength_hundred";
            strengthHundred.locale_description = "status_description_strength_hundred";
            AssetManager.status.add(strengthHundred);
            #endregion

            #region Creation Rebirth Cooldown
            StatusAsset creationRebirthCD = new StatusAsset();
            creationRebirthCD.id = "status_creation_rebirth_cooldown";
            creationRebirthCD.duration = 20f;
            creationRebirthCD.path_icon = "ui/icons/slug_sage";
            creationRebirthCD.locale_id = "status_title_creation_rebirth_cooldown";
            creationRebirthCD.locale_description = "status_description_creation_rebirth_cooldown";
            AssetManager.status.add(creationRebirthCD);
            #endregion

            #region Uzumaki Ability: Adamantite Sealing Chains
            StatusAsset chains = new StatusAsset();
            chains.id = "status_adamantite_chains";
            chains.base_stats = new BaseStats();
            chains.base_stats.set("multiplier_speed", -0.99f);
            chains.base_stats.set("attack_speed", -10f);
            chains.duration = 5f;
            chains.path_icon = "ui/icons/uzumaki";

            chains.animated = true;
            chains.texture = "fx_chains";
            chains.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{chains.texture}", false);
            chains.material = material;
            chains.need_visual_render = true;
            chains.scale = 0.3f;

            chains.locale_id = "status_title_adamantite_chains";
            chains.locale_description = "status_description_adamantite_chains";

            chains.action = (WorldAction)Delegate.Combine(chains.action, new WorldAction(AdamantiteDrainAction));

            AssetManager.status.add(chains);
            #endregion

            #region King Of Hell
            StatusAsset koh = new StatusAsset();
            koh.id = "koh_sprite";
            koh.path_icon = "ui/icons/rinnegan";
            koh.animated = true;
            koh.duration = 4f;

            koh.texture = "fx_king_of_hell";
            koh.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{koh.texture}", false);
            koh.material = material;

            koh.need_visual_render = true;
            koh.is_animated_in_pause = false;
            koh.use_parent_rotation = false;
            koh.removed_on_damage = false;
            koh.cancel_actor_job = true;
            koh.can_be_flipped = true;
            koh.scale = 0.2f;
            koh.render_priority = 5;
            koh.loop = false;


            koh.locale_id = "status_title_koh";
            koh.locale_description = "status_description_koh";

            AssetManager.status.add(koh);
            #endregion

            #region Akimichi Grown Status
            StatusAsset akimichiGrown = new StatusAsset();
            akimichiGrown.id = "status_akimichi_grown";
            akimichiGrown.path_icon = "effects/akimichi_grown";
            akimichiGrown.duration = 15f;

            akimichiGrown.base_stats = new BaseStats();
            akimichiGrown.base_stats.set("multiplier_damage", 0.25f);
            akimichiGrown.base_stats.set("multiplier_health", 0.20f);
            akimichiGrown.base_stats.set("armor", 15f);
            akimichiGrown.base_stats.set("multiplier_speed", -0.15f);
            akimichiGrown.base_stats.set("scale", 0.45f);

            akimichiGrown.animated = true;
            akimichiGrown.texture = "fx_akimichi_grown";
            akimichiGrown.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{akimichiGrown.texture}", false);
            akimichiGrown.material = material;
            akimichiGrown.need_visual_render = true;
            akimichiGrown.is_animated_in_pause = false;
            akimichiGrown.use_parent_rotation = true;
            akimichiGrown.removed_on_damage = false;
            akimichiGrown.cancel_actor_job = false;
            akimichiGrown.can_be_flipped = true;
            akimichiGrown.render_priority = 5;
            akimichiGrown.loop = false;

            akimichiGrown.locale_id = "status_title_akimichi_grown";
            akimichiGrown.locale_description = "status_description_akimichi_grown";
            AssetManager.status.add(akimichiGrown);
            #endregion

            #region Akimichi Shrunk Status
            StatusAsset akimichiShrunk = new StatusAsset();
            akimichiShrunk.id = "status_akimichi_shrunk";
            akimichiShrunk.path_icon = "effects/akimichi_shrunk";
            akimichiShrunk.duration = 15f;

            akimichiShrunk.base_stats = new BaseStats();
            akimichiShrunk.base_stats.set("speed", 15f);
            akimichiShrunk.base_stats.set("multiplier_speed", 0.30f);
            akimichiShrunk.base_stats.set("multiplier_damage", -0.20f);
            akimichiShrunk.base_stats.set("armor", -10f);
            akimichiShrunk.base_stats.set("multiplier_health", -0.15f);
            akimichiShrunk.base_stats.set("scale", -0.45f);

            akimichiShrunk.animated = true;
            akimichiShrunk.texture = "fx_akimichi_shrunk";
            akimichiShrunk.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{akimichiShrunk.texture}", false);
            akimichiShrunk.material = material;
            akimichiShrunk.need_visual_render = true;
            akimichiShrunk.is_animated_in_pause = false;
            akimichiShrunk.use_parent_rotation = true;
            akimichiShrunk.removed_on_damage = false;
            akimichiShrunk.cancel_actor_job = true;
            akimichiShrunk.can_be_flipped = true;
            akimichiShrunk.render_priority = 5;
            akimichiShrunk.loop = false;


            akimichiShrunk.locale_id = "status_title_akimichi_shrunk";
            akimichiShrunk.locale_description = "status_description_akimichi_shrunk";
            AssetManager.status.add(akimichiShrunk);
            #endregion

            #region Six Paths Senjutsu Status
            StatusAsset sixPathsSenjutsu = new StatusAsset();
            sixPathsSenjutsu.id = "status_six_paths_senjutsu";
            sixPathsSenjutsu.path_icon = "ui/icons/sixpathssenjutsu";
            sixPathsSenjutsu.duration = 999f;

            sixPathsSenjutsu.base_stats = new BaseStats();
            sixPathsSenjutsu.base_stats.set("multiplier_damage", 0.20f);
            sixPathsSenjutsu.base_stats.set("multiplier_health", 0.20f);
            sixPathsSenjutsu.base_stats.set("multiplier_speed", 0.20f);
            sixPathsSenjutsu.base_stats.set("intelligence", 20f);
            sixPathsSenjutsu.base_stats.set("critical_chance", 0.20f);
            sixPathsSenjutsu.base_stats.set("critical_damage_multiplier", 0.20f);
            sixPathsSenjutsu.base_stats.set("multiplier_chakra", 0.20f);

            sixPathsSenjutsu.animated = true;
            sixPathsSenjutsu.texture = "fx_TSOrbs";
            sixPathsSenjutsu.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{sixPathsSenjutsu.texture}", false);
            sixPathsSenjutsu.material = material;

            sixPathsSenjutsu.need_visual_render = true;
            sixPathsSenjutsu.removed_on_damage = false;
            sixPathsSenjutsu.use_parent_rotation = false;
            sixPathsSenjutsu.is_animated_in_pause = true;
            sixPathsSenjutsu.can_be_flipped = true;
            sixPathsSenjutsu.action_get_hit = (GetHitAction)Delegate.Combine(sixPathsSenjutsu.action_get_hit, new GetHitAction(TruthSeekingOrbShield));
            sixPathsSenjutsu.action = (WorldAction)Delegate.Combine(sixPathsSenjutsu.action, new WorldAction(JutsuLibrary.AutoTruthSeekingOrbAtTarget));
            sixPathsSenjutsu.action = (WorldAction)Delegate.Combine(sixPathsSenjutsu.action, new WorldAction(SixPathsFlightAction));
            sixPathsSenjutsu.action_interval = 5f;

            sixPathsSenjutsu.locale_id = "status_title_six_paths_senjutsu";
            sixPathsSenjutsu.locale_description = "status_description_six_paths_senjutsu";
            AssetManager.status.add(sixPathsSenjutsu);
            #endregion

            #region Truth Seeking Orb Shield Status
            StatusAsset tsoShield = new StatusAsset();
            tsoShield.id = "status_tso_shield";
            tsoShield.path_icon = "ui/icons/sixpathssenjutsu";
            tsoShield.duration = 2f;

            tsoShield.base_stats = new BaseStats();
            tsoShield.base_stats.set("armor", 100f);

            tsoShield.animated = true;
            tsoShield.texture = "fx_TSOShield";
            tsoShield.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{tsoShield.texture}", false);
            tsoShield.material = material;
            tsoShield.need_visual_render = true;
            tsoShield.removed_on_damage = false;
            tsoShield.use_parent_rotation = false;
            tsoShield.is_animated_in_pause = true;
            tsoShield.can_be_flipped = true;

            tsoShield.locale_id = "status_title_tso_shield";
            tsoShield.locale_description = "status_description_tso_shield";
            AssetManager.status.add(tsoShield);
            #endregion

            #region Infinite Tsukuyomi Status
            StatusAsset infiniteTsukuyomi = new StatusAsset();
            infiniteTsukuyomi.id = "status_infinite_tsukuyomi";
            infiniteTsukuyomi.path_icon = "ui/icons/rinne_sharingan";
            infiniteTsukuyomi.duration = 60f;

            infiniteTsukuyomi.animated = true;
            infiniteTsukuyomi.texture = "fx_infinite_tsukuyomi";
            infiniteTsukuyomi.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{infiniteTsukuyomi.texture}", false);
            infiniteTsukuyomi.material = material;

            infiniteTsukuyomi.base_stats = new BaseStats();
            infiniteTsukuyomi.base_stats.set("multiplier_speed", -1.0f);
            infiniteTsukuyomi.base_stats.set("multiplier_attack_speed", -1.0f);

            infiniteTsukuyomi.locale_id = "status_title_infinite_tsukuyomi";
            infiniteTsukuyomi.locale_description = "status_description_infinite_tsukuyomi";
            infiniteTsukuyomi.action_on_receive = (WorldAction)Delegate.Combine(infiniteTsukuyomi.action_on_receive, new WorldAction(action_freeze));
            infiniteTsukuyomi.action = (WorldAction)Delegate.Combine(infiniteTsukuyomi.action, new WorldAction(action_freeze));
            AssetManager.status.add(infiniteTsukuyomi);
            #endregion

            #region Truth Seeking Balls Status
            StatusAsset TSBalls = new StatusAsset();
            TSBalls.id = "tsballs";
            TSBalls.path_icon = "tsBalls";
            TSBalls.duration = 60f;

            TSBalls.animated = true;
            TSBalls.texture = "fx_TSBalls";
            TSBalls.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{TSBalls.texture}", false);
            TSBalls.material = material;

            TSBalls.base_stats.set("multiplier_damage", 0.01f);
            TSBalls.base_stats.set("multiplier_health", 0.01f);
            TSBalls.base_stats.set("multiplier_speed", 0.01f);
            TSBalls.base_stats.set("critical_chance", 0.01f);
            TSBalls.base_stats.set("critical_damage_multiplier", 0.01f);
            TSBalls.base_stats.set("armor", 1f);

            TSBalls.locale_id = "status_title_TSBalls";
            TSBalls.locale_description = "status_description_TSBalls";

            AssetManager.status.add(TSBalls);
            #endregion

            #region Ten Tails Jinchuriki

            StatusAsset juubi1 = new StatusAsset();
            juubi1.id = "juubi1";
            juubi1.path_icon = "ui/icons/sixpathssenjutsu";
            juubi1.duration = 60f;

            juubi1.animated = false;
            juubi1.material = material;

            juubi1.base_stats.set("health", 350f);
            juubi1.base_stats.set("damage", 100f);
            juubi1.base_stats.set("speed", 40f);
            juubi1.base_stats.set("multiplier_health", 0.10f);
            juubi1.base_stats.set("multiplier_damage", 0.10f);
            juubi1.base_stats.set("multiplier_speed", 0.10f);
            juubi1.base_stats.set("armor", 15f);
            juubi1.base_stats.set("accuracy", 0.25f);
            juubi1.base_stats.set("intelligence", 30f);
            juubi1.base_stats.set("critical_chance", 0.35f);
            juubi1.base_stats.set("critical_damage_multiplier", 0.30f);
            juubi1.base_stats.set("multiplier_attack_speed", 0.10f);
            juubi1.base_stats.set("experience", 25f);
            juubi1.base_stats.set("size", 0.05f);

            juubi1.locale_id = "status_title_juubi1";
            juubi1.locale_description = "status_description_juubi1";

            AssetManager.status.add(juubi1);

            StatusAsset juubi2 = new StatusAsset();
            juubi2.id = "juubi2";
            juubi2.path_icon = "ui/icons/sixpathssenjutsu";
            juubi2.duration = 60f;

            juubi2.animated = false;
            juubi2.material = material;

            juubi2.base_stats.set("health", 500f);
            juubi2.base_stats.set("damage", 200f);
            juubi2.base_stats.set("speed", 60f);
            juubi2.base_stats.set("multiplier_health", 0.15f);
            juubi2.base_stats.set("multiplier_damage", 0.15f);
            juubi2.base_stats.set("multiplier_speed", 0.15f);
            juubi2.base_stats.set("armor", 30f);
            juubi2.base_stats.set("accuracy", 0.50f);
            juubi2.base_stats.set("intelligence", 60f);
            juubi2.base_stats.set("critical_chance", 0.45f);
            juubi2.base_stats.set("critical_damage_multiplier", 0.45f);
            juubi2.base_stats.set("multiplier_attack_speed", 0.20f);
            juubi2.base_stats.set("experience", 50f);
            juubi2.base_stats.set("size", 0.05f);

            juubi2.locale_id = "status_title_juubi2";
            juubi2.locale_description = "status_description_juubi2";

            AssetManager.status.add(juubi2);

            #endregion

            #region Madaras Eternal Mangekyo Sharingan Jutsus
            //Madaras Perfect Susanoo
            StatusAsset madaraPS = new StatusAsset();
            madaraPS.id = "madara_ps";
            madaraPS.path_icon = "ui/icons/madara_ems";
            madaraPS.duration = 1f;

            madaraPS.animated = true;
            madaraPS.texture = "fx_madara_ps";
            madaraPS.material = material;
            madaraPS.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{madaraPS.texture}", false);

            madaraPS.need_visual_render = true;
            madaraPS.removed_on_damage = false;
            madaraPS.use_parent_rotation = false;
            madaraPS.is_animated_in_pause = false;
            madaraPS.can_be_flipped = true;
            madaraPS.scale = 0.4f;

            madaraPS.base_stats.set("armor", 75f);
            madaraPS.base_stats.set("damage", 500f);
            madaraPS.base_stats.set("multiplier_damage", 0.30f);
            madaraPS.base_stats.set("health", 2000f);
            madaraPS.base_stats.set("range", 9f);
            madaraPS.base_stats.set("knockback", 1.5f);
            madaraPS.base_stats.set("area_of_effect", 0.80f);

            madaraPS.locale_id = "status_title_madara_ps";
            madaraPS.locale_description = "status_description_madara_ps";

            AssetManager.status.add(madaraPS);

            // TENGAI SHINSEIIII
            StatusAsset tengaiShinsei = new StatusAsset();
            tengaiShinsei.id = "tengai_shinsei";
            tengaiShinsei.path_icon = "ui/icons/madara_ems";
            tengaiShinsei.duration = 1f;

            tengaiShinsei.animated = true;
            tengaiShinsei.texture = "fx_tengai_shinsei";
            tengaiShinsei.material = material;
            tengaiShinsei.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{tengaiShinsei.texture}", false);

            tengaiShinsei.need_visual_render = true;
            tengaiShinsei.removed_on_damage = false;
            tengaiShinsei.use_parent_rotation = false;
            tengaiShinsei.is_animated_in_pause = false;
            tengaiShinsei.can_be_flipped = true;
            tengaiShinsei.scale = 0.5f;

            tengaiShinsei.base_stats.set("armor", 15f);
            tengaiShinsei.base_stats.set("area_of_effect", 0.80f);

            tengaiShinsei.locale_id = "status_title_tengai_shinsei";
            tengaiShinsei.locale_description = "status_description_tengai_shinsei";

            AssetManager.status.add(tengaiShinsei);

            // Susanoo Ribcage but madaras variant
            StatusAsset madaraRibcage = new StatusAsset();
            madaraRibcage.id = "susanoo_ribcage_madara";
            madaraRibcage.base_stats = new BaseStats();
            madaraRibcage.base_stats.set("armor", 20f);
            madaraRibcage.duration = 10f;
            madaraRibcage.path_icon = "effects/BlueHalfSusa";

            madaraRibcage.animated = true;
            madaraRibcage.texture = "fx_bluehalfsusa";
            madaraRibcage.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{madaraRibcage.texture}", false);
            madaraRibcage.material = material;

            madaraRibcage.tier = StatusTier.Advanced;
            madaraRibcage.need_visual_render = true;
            madaraRibcage.is_animated_in_pause = false;
            madaraRibcage.can_be_flipped = true;
            madaraRibcage.removed_on_damage = false;
            madaraRibcage.scale = 0.4f;
            madaraRibcage.locale_id = "status_title_susanoo_ribcage_madara";
            madaraRibcage.locale_description = "status_description_susanoo_ribcage_madara";

            AssetManager.status.add(madaraRibcage);

            #endregion

            #region Justsu Global Cooldown
            StatusAsset kamuiCD = new StatusAsset();
            kamuiCD.id = "kamui_cooldown";
            kamuiCD.path_icon = "effects/abilitycooldown";
            kamuiCD.duration = 60f;
            kamuiCD.animated = false;

            kamuiCD.locale_id = "status_title_ability_cooldown";
            kamuiCD.locale_description = "status_description_ability_cooldown";

            AssetManager.status.add(kamuiCD);
            #endregion

            #region Fire Nature: Katon
            StatusAsset katon = new StatusAsset();
            katon.id = "katon_burn";
            katon.path_icon = "ui/icons/fire_nature";
            katon.duration = 2f;

            katon.animated = true;
            katon.texture = "fx_katon";
            katon.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{katon.texture}", false);
            katon.material = material;

            katon.tier = StatusTier.Advanced;
            katon.need_visual_render = true;
            katon.is_animated_in_pause = false;
            katon.can_be_flipped = true;
            katon.removed_on_damage = false;
            katon.scale = 0.2f;
            katon.locale_id = "status_title_katon_burn";
            katon.locale_description = "status_description_katon_burn";

            AssetManager.status.add(katon);
            #endregion

            #region Added weight Rock Technique
            StatusAsset weightRock = new StatusAsset();
            weightRock.id = "weight_rock";
            weightRock.path_icon = "ui/icons/earth_nature";
            weightRock.duration = 5f;

            weightRock.base_stats = new BaseStats();
            weightRock.base_stats.set("multiplier_speed", -0.90f);
            weightRock.base_stats.set("attack_speed", -0.40f);
            weightRock.base_stats.set("multiplier_damage", 0.60f);


            weightRock.animated = true;
            weightRock.texture = "fx_weighted";
            weightRock.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{weightRock.texture}", false);
            weightRock.material = material;

            weightRock.tier = StatusTier.Advanced;
            weightRock.need_visual_render = true;
            weightRock.is_animated_in_pause = false;
            weightRock.can_be_flipped = true;
            weightRock.removed_on_damage = false;
            weightRock.scale = 0.5f;
            weightRock.action = (WorldAction)Delegate.Combine(weightRock.action, new WorldAction(WeightRockTerraformAction));
            weightRock.action_interval = 0.6f;
            weightRock.locale_id = "status_title_weight_rock";
            weightRock.locale_description = "status_description_weight_rock";

            AssetManager.status.add(weightRock);
            #endregion

        }

        private static void NineTailsJinchuriki(Material material)
        {
            StatusAsset initialReleaseForm = new StatusAsset();
            initialReleaseForm.id = "status_jinchuriki_initial_release";
            initialReleaseForm.path_icon = "ui/icons/initial_release";
            initialReleaseForm.duration = -1f;
            initialReleaseForm.base_stats = new BaseStats();
            initialReleaseForm.base_stats.set("damage", 15f);
            initialReleaseForm.base_stats.set("health", 52f);
            initialReleaseForm.base_stats.set("multiplier_health", 0.05f);
            initialReleaseForm.base_stats.set("speed", 5f);
            initialReleaseForm.base_stats.set("critical_chance", 0.05f);
            initialReleaseForm.base_stats.set("critical_damage_multiplier", 0.05f);
            initialReleaseForm.base_stats.set("chakra", 16f);

            initialReleaseForm.animated = true;
            initialReleaseForm.texture = "fx_v1Aura";
            initialReleaseForm.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{initialReleaseForm.texture}", false);
            initialReleaseForm.material = material;

            initialReleaseForm.tier = StatusTier.Advanced;
            initialReleaseForm.need_visual_render = true;
            initialReleaseForm.is_animated_in_pause = false;
            initialReleaseForm.can_be_flipped = true;
            initialReleaseForm.removed_on_damage = false;

            initialReleaseForm.locale_id = "status_title_jinchuriki_initial_release";
            initialReleaseForm.locale_description = "status_description_jinchuriki_initial_release";
            initialReleaseForm.action = (WorldAction)Delegate.Combine(initialReleaseForm.action, new WorldAction(JutsuLibrary.NineTailsStatusMoves));
            initialReleaseForm.action_interval = 1.6f;
            AssetManager.status.add(initialReleaseForm);

            StatusAsset v1CloakForm = new StatusAsset();
            v1CloakForm.id = "status_jinchuriki_v1_cloak";
            v1CloakForm.path_icon = "ui/icons/v1";
            v1CloakForm.duration = -1f;
            v1CloakForm.base_stats = new BaseStats();
            v1CloakForm.base_stats.set("damage", 26f);
            v1CloakForm.base_stats.set("multiplier_damage", 0.10f);
            v1CloakForm.base_stats.set("health", 101f);
            v1CloakForm.base_stats.set("multiplier_health", 0.10f);
            v1CloakForm.base_stats.set("speed", 7f);
            v1CloakForm.base_stats.set("multiplier_speed", 0.05f);
            v1CloakForm.base_stats.set("critical_chance", 0.10f);
            v1CloakForm.base_stats.set("critical_damage_multiplier", 0.10f);
            v1CloakForm.base_stats.set("chakra", 36f);

            v1CloakForm.animated = true;
            v1CloakForm.texture = "fx_v1Aura";
            v1CloakForm.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{v1CloakForm.texture}", false);
            v1CloakForm.material = material;

            v1CloakForm.tier = StatusTier.Advanced;
            v1CloakForm.need_visual_render = true;
            v1CloakForm.is_animated_in_pause = false;
            v1CloakForm.can_be_flipped = true;
            v1CloakForm.removed_on_damage = false;

            v1CloakForm.locale_id = "status_title_jinchuriki_v1_cloak";
            v1CloakForm.locale_description = "status_description_jinchuriki_v1_cloak";
            v1CloakForm.action = (WorldAction)Delegate.Combine(v1CloakForm.action, new WorldAction(JutsuLibrary.NineTailsStatusMoves));
            v1CloakForm.action_interval = 1.5f;
            AssetManager.status.add(v1CloakForm);

            StatusAsset v2CloakForm = new StatusAsset();
            v2CloakForm.id = "status_jinchuriki_v2_cloak";
            v2CloakForm.path_icon = "ui/icons/v2";
            v2CloakForm.duration = -1f;
            v2CloakForm.base_stats = new BaseStats();
            v2CloakForm.base_stats.set("damage", 38f);
            v2CloakForm.base_stats.set("multiplier_damage", 0.15f);
            v2CloakForm.base_stats.set("health", 162f);
            v2CloakForm.base_stats.set("multiplier_health", 0.20f);
            v2CloakForm.base_stats.set("armor", 6f);
            v2CloakForm.base_stats.set("speed", 10f);
            v2CloakForm.base_stats.set("multiplier_speed", 0.10f);
            v2CloakForm.base_stats.set("critical_chance", 0.15f);
            v2CloakForm.base_stats.set("critical_damage_multiplier", 0.15f);
            v2CloakForm.base_stats.set("multiplier_lifespan", -0.10f);
            v2CloakForm.base_stats.set("chakra", 66f);

            v2CloakForm.animated = true;
            v2CloakForm.texture = "fx_v2Aura";
            v2CloakForm.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{v2CloakForm.texture}", false);
            v2CloakForm.material = material;

            v2CloakForm.tier = StatusTier.Advanced;
            v2CloakForm.need_visual_render = true;
            v2CloakForm.is_animated_in_pause = false;
            v2CloakForm.can_be_flipped = true;
            v2CloakForm.removed_on_damage = false;

            v2CloakForm.locale_id = "status_title_jinchuriki_v2_cloak";
            v2CloakForm.locale_description = "status_description_jinchuriki_v2_cloak";
            v2CloakForm.action = (WorldAction)Delegate.Combine(v2CloakForm.action, new WorldAction(JutsuLibrary.NineTailsStatusMoves));
            v2CloakForm.action_interval = 1.4f;
            AssetManager.status.add(v2CloakForm);

            StatusAsset incompleteBeastForm = new StatusAsset();
            incompleteBeastForm.id = "status_jinchuriki_incomplete_beast";
            incompleteBeastForm.path_icon = "ui/icons/incomplete";
            incompleteBeastForm.duration = -1f;
            incompleteBeastForm.base_stats = new BaseStats();
            incompleteBeastForm.base_stats.set("damage", 57f);
            incompleteBeastForm.base_stats.set("multiplier_damage", 0.20f);
            incompleteBeastForm.base_stats.set("health", 248f);
            incompleteBeastForm.base_stats.set("multiplier_health", 0.25f);
            incompleteBeastForm.base_stats.set("armor", 10f);
            incompleteBeastForm.base_stats.set("speed", 12f);
            incompleteBeastForm.base_stats.set("multiplier_speed", 0.15f);
            incompleteBeastForm.base_stats.set("critical_chance", 0.20f);
            incompleteBeastForm.base_stats.set("critical_damage_multiplier", 0.20f);
            incompleteBeastForm.base_stats.set("multiplier_lifespan", -0.13f);
            incompleteBeastForm.base_stats.set("chakra", 108f);
            incompleteBeastForm.locale_id = "status_title_jinchuriki_incomplete_beast";
            incompleteBeastForm.locale_description = "status_description_jinchuriki_incomplete_beast";
            incompleteBeastForm.action = (WorldAction)Delegate.Combine(incompleteBeastForm.action, new WorldAction(JutsuLibrary.NineTailsStatusMoves));
            incompleteBeastForm.action_interval = 1.3f;
            AssetManager.status.add(incompleteBeastForm);

            StatusAsset kcm1Form = new StatusAsset();
            kcm1Form.id = "status_jinchuriki_kcm1";
            kcm1Form.path_icon = "ui/icons/kcm1";
            kcm1Form.duration = -1f;
            kcm1Form.base_stats = new BaseStats();
            kcm1Form.base_stats.set("damage", 72f);
            kcm1Form.base_stats.set("multiplier_damage", 0.25f);
            kcm1Form.base_stats.set("health", 409f);
            kcm1Form.base_stats.set("multiplier_health", 0.30f);
            kcm1Form.base_stats.set("armor", 14f);
            kcm1Form.base_stats.set("speed", 16f);
            kcm1Form.base_stats.set("multiplier_speed", 0.20f);
            kcm1Form.base_stats.set("critical_chance", 0.25f);
            kcm1Form.base_stats.set("critical_damage_multiplier", 0.25f);
            kcm1Form.base_stats.set("intelligence", 30f);
            kcm1Form.base_stats.set("experience", 10f);
            kcm1Form.base_stats.set("chakra", 202f);

            kcm1Form.animated = true;
            kcm1Form.texture = "fx_kcmAura";
            kcm1Form.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{kcm1Form.texture}", false);
            kcm1Form.material = material;

            kcm1Form.tier = StatusTier.Advanced;
            kcm1Form.need_visual_render = true;
            kcm1Form.is_animated_in_pause = false;
            kcm1Form.can_be_flipped = true;
            kcm1Form.removed_on_damage = false;

            kcm1Form.locale_id = "status_title_jinchuriki_kcm1";
            kcm1Form.locale_description = "status_description_jinchuriki_kcm1";
            kcm1Form.action = (WorldAction)Delegate.Combine(kcm1Form.action, new WorldAction(JutsuLibrary.NineTailsStatusMoves));
            kcm1Form.action_interval = 1.25f;
            AssetManager.status.add(kcm1Form);

            StatusAsset kcm2Form = new StatusAsset();
            kcm2Form.id = "status_jinchuriki_kcm2";
            kcm2Form.path_icon = "ui/icons/kcm2";
            kcm2Form.duration = -1f;
            kcm2Form.base_stats = new BaseStats();
            kcm2Form.base_stats.set("damage", 108f);
            kcm2Form.base_stats.set("multiplier_damage", 0.30f);
            kcm2Form.base_stats.set("health", 603f);
            kcm2Form.base_stats.set("multiplier_health", 0.35f);
            kcm2Form.base_stats.set("armor", 18f);
            kcm2Form.base_stats.set("speed", 20f);
            kcm2Form.base_stats.set("multiplier_speed", 0.25f);
            kcm2Form.base_stats.set("critical_chance", 0.30f);
            kcm2Form.base_stats.set("critical_damage_multiplier", 0.30f);
            kcm2Form.base_stats.set("intelligence", 40f);
            kcm2Form.base_stats.set("experience", 14f);
            kcm2Form.base_stats.set("chakra", 306f);

            kcm2Form.animated = true;
            kcm2Form.texture = "fx_kcmAura";
            kcm2Form.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{kcm2Form.texture}", false);
            kcm2Form.material = material;

            kcm2Form.tier = StatusTier.Advanced;
            kcm2Form.need_visual_render = true;
            kcm2Form.is_animated_in_pause = false;
            kcm2Form.can_be_flipped = true;
            kcm2Form.removed_on_damage = false;

            kcm2Form.locale_id = "status_title_jinchuriki_kcm2";
            kcm2Form.locale_description = "status_description_jinchuriki_kcm2";
            kcm2Form.action = (WorldAction)Delegate.Combine(kcm2Form.action, new WorldAction(JutsuLibrary.NineTailsStatusMoves));
            kcm2Form.action_interval = 1.15f;
            AssetManager.status.add(kcm2Form);

            StatusAsset avatarForm = new StatusAsset();
            avatarForm.id = "status_jinchuriki_avatar";
            avatarForm.path_icon = "ui/icons/avatar";
            avatarForm.duration = -1f;
            avatarForm.base_stats = new BaseStats();
            avatarForm.base_stats.set("damage", 127f);
            avatarForm.base_stats.set("multiplier_damage", 0.35f);
            avatarForm.base_stats.set("health", 804f);
            avatarForm.base_stats.set("multiplier_health", 0.40f);
            avatarForm.base_stats.set("armor", 23f);
            avatarForm.base_stats.set("speed", 24f);
            avatarForm.base_stats.set("multiplier_speed", 0.30f);
            avatarForm.base_stats.set("critical_chance", 0.35f);
            avatarForm.base_stats.set("critical_damage_multiplier", 0.40f);
            avatarForm.base_stats.set("intelligence", 0.50f);
            avatarForm.base_stats.set("experience", 16f);
            avatarForm.base_stats.set("chakra", 408f);
            avatarForm.locale_id = "status_title_jinchuriki_avatar";
            avatarForm.locale_description = "status_description_jinchuriki_avatar";
            avatarForm.action = (WorldAction)Delegate.Combine(avatarForm.action, new WorldAction(JutsuLibrary.NineTailsStatusMoves));
            avatarForm.action_interval = 1.15f;
            AssetManager.status.add(avatarForm);

            StatusAsset baryonModeForm = new StatusAsset();
            baryonModeForm.id = "status_jinchuriki_baryon_mode";
            baryonModeForm.path_icon = "ui/icons/baryon";
            baryonModeForm.duration = 60f;
            baryonModeForm.base_stats = new BaseStats();
            baryonModeForm.base_stats.set("damage", 320f);
            baryonModeForm.base_stats.set("multiplier_damage", 0.50f);
            baryonModeForm.base_stats.set("health", 1203f);
            baryonModeForm.base_stats.set("multiplier_health", 0.50f);
            baryonModeForm.base_stats.set("armor", 30f);
            baryonModeForm.base_stats.set("speed", 32f);
            baryonModeForm.base_stats.set("multiplier_speed", 0.40f);
            baryonModeForm.base_stats.set("critical_chance", 0.60f);
            baryonModeForm.base_stats.set("critical_damage_multiplier", 0.70f);
            baryonModeForm.base_stats.set("attack_speed", 0.30f);
            baryonModeForm.base_stats.set("intelligence", 70f);
            baryonModeForm.base_stats.set("multiplier_lifespan", -0.25f);
            baryonModeForm.base_stats.set("experience", 40f);
            baryonModeForm.base_stats.set("chakra", 5000f);
            baryonModeForm.locale_id = "status_title_jinchuriki_baryon_mode";
            baryonModeForm.locale_description = "status_description_jinchuriki_baryon_mode";
            baryonModeForm.action = (WorldAction)Delegate.Combine(baryonModeForm.action, new WorldAction(JutsuLibrary.NineTailsStatusMoves));
            baryonModeForm.action_interval = 0.95f;
            baryonModeForm.action_finish = (WorldAction)Delegate.Combine(baryonModeForm.action_finish, new WorldAction(BaryonModeEnded));
            AssetManager.status.add(baryonModeForm);

            #region Jutsu Use Status
            StatusAsset jutsuuse = new StatusAsset();
            jutsuuse.id = "jutsuuse";
            jutsuuse.path_icon = "effects/baseseal";
            jutsuuse.duration = 1f;

            jutsuuse.animated = true;
            jutsuuse.texture = "fx_baseseal";
            jutsuuse.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{jutsuuse.texture}", false);
            jutsuuse.material = material;

            jutsuuse.need_visual_render = true;
            jutsuuse.use_parent_rotation = false;
            jutsuuse.is_animated_in_pause = false;
            jutsuuse.can_be_flipped = true;
            jutsuuse.scale = 0.3f;
            jutsuuse.render_priority = 5;

            jutsuuse.locale_id = "status_title_jutsuuse";
            jutsuuse.locale_description = "status_description_jutsuuse";

            AssetManager.status.add(jutsuuse);
            #endregion
        }

        public static bool BaryonModeEnded(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null) return false;

            if (pTarget.a.hasTrait("nine_tails_jinchuriki"))
            {
                pTarget.a.removeTrait("nine_tails_jinchuriki");
            }

            return true;
        }

        public static bool TruthSeekingOrbShield(BaseSimObject pSelf, BaseSimObject pAttackedBy, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pSelf.a.hasStatus("status_tso_shield")) return false;
            if (UnityEngine.Random.value > 0.10f) return false;

            pSelf.a.addStatusEffect("status_tso_shield", 2f);
            return true;
        }

        public static bool SixPathsFlightAction(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || pTarget.a.data == null || !pTarget.a.isAlive()) return false;

            pTarget.a._flying = true;
            return true;
        }

        public static bool RasenganAA(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (!pSelf.a.hasStatus("status_rasengan")) return false;

            pSelf.a.finishStatusEffect("status_rasengan");
            return true;
        }

        public static bool ChidoriAA(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (!pSelf.a.hasStatus("status_chidori")) return false;

            pSelf.a.finishStatusEffect("status_chidori");
            return true;
        }

        #region Shadow Clone Jutsu
        public static bool ShadowCloneGetHit(BaseSimObject pSelf, BaseSimObject pAttackedBy, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            if (Randy.randomChance(0.15f))
            {
                pSelf.finishStatusEffect("shadow_clone");
                
                return true;
            }

            return false;
        }

        public static bool ShadowCloneFinish(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            Actor actor = pTarget.a;
            WorldTile targetTile = pTarget.current_tile;

            actor.die();
            if (targetTile != null)
            {
                EffectsLibrary.spawnAtTile("fx_smoke", targetTile, 0.05f);
                JutsuLibrary.PlayWavSound("shadow_clone_spawn.wav", targetTile.posV3);
            }

            return true;
        }
        #endregion

        public static bool WeightRockTerraformAction(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            WorldTile currentTile = pTarget.current_tile ?? pTile;
            if (currentTile == null) return false;
            if (UnityEngine.Random.value > 0.35f) return false;

            TerraformOptions terraform = AssetManager.terraform.get("destroy");
            if (terraform == null) return false;

            MapAction.damageWorld(currentTile, 1, terraform, null);
            return true;
        }

        // Genjutsu Paralysis
        public static bool action_freeze(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            pTarget.a.stopMovement();
            return true;
        }

        // Amaterasu
        public static bool amaterasu_burn(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            pTarget.getHit(10f, true, AttackType.Fire, null, true, false);
            return true;
        }

        // Eight Trigrams: Palm Rotation
        public static bool KaitenShredAction(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            var nearbyUnits = Finder.getUnitsFromChunk(pTarget.current_tile, 1);

            if (nearbyUnits.Any())
            {
                foreach (var unit in nearbyUnits)
                {
                    if (unit.a == pTarget.a || (pTarget.a.kingdom != null && unit.a.kingdom == pTarget.a.kingdom)) continue;
                    if (!unit.a.isAlive()) continue;

                    unit.a.getHit(40f, true, AttackType.Fire, pTarget.a, true);

                }
            }
            return true;
        }

        // Eight Trigrams: 64 Palms
        public static bool EightTrigrams64Palms(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null) return false;

            //pTarget.a.getHit(0.5f, true, AttackType.Fire, null, false);

            return true;
        }

        public static bool AdamantiteDrainAction(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || !pTarget.a.isAlive()) return false;

            float damage = pTarget.a.getMaxHealth() * 0.05f;
            pTarget.a.getHit(damage, true, AttackType.Other, null, true);

            return true;
        }
    }
}