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
    public static class ShinobiEffects
    {
        public static void Init()
        {
            // KCM/BARYON EXPLOSION
            EffectAsset antimatter = new EffectAsset();
            antimatter.id = "fx_shinobi_antimatter";
            antimatter.use_basic_prefab = true;
            antimatter.prefab_id = "effects/prefabs/PrefabAntimatterEffect";
            antimatter.sprite_path = "effects/antimatterEffect";
            antimatter.sorting_layer_id = "EffectsTop";
            antimatter.draw_light_area = false;
            antimatter.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionAntimatterBomb";

            AssetManager.effects_library.add(antimatter);

            /* no sprite yet
            // Small Bijuu Explosion
            EffectAsset sbBomb = new EffectAsset();
            sbBomb.id = "fx_sbBomb";
            sbBomb.use_basic_prefab = true;
            sbBomb.prefab_id = "effects/prefabs/PrefabSimpleEffect";
            sbBomb.sprite_path = "effects/fx_sbBomb";
            sbBomb.sorting_layer_id = "EffectsTop";
            sbBomb.draw_light_area = true;
            sbBomb.sound_launch = "event:/SFX/POWERS/Grenade";

            AssetManager.effects_library.add(sbBomb);
            */

            //need to fix so it doesnt even do anything

            #region Bijuu Bite Effect
            // no sprite yet
            EffectAsset bite = new EffectAsset();
            bite.id = "fx_bijuu_bite";
            bite.use_basic_prefab = true;
            bite.prefab_id = "effects/prefabs/PrefabSimpleEffect";
            bite.sprite_path = "effects/fx_bijuu_bite";
            bite.sorting_layer_id = "EffectsTop";
            bite.draw_light_area = true;
            bite.time_between_frames = 0.1f;

            AssetManager.effects_library.add(bite);
            #endregion

            #region Chakra Arm Effect
            EffectAsset arm = new EffectAsset();
            arm.id = "fx_chakra_arm";
            arm.use_basic_prefab = true;
            arm.sprite_path = "effects/fx_chakra_arm";
            arm.sorting_layer_id = "EffectsTop";
            arm.draw_light_size = 1.0f;

            AssetManager.effects_library.add(arm);
            #endregion

            #region King of Hell Effect
            EffectAsset koh = new EffectAsset();
            koh.id = "fx_king_of_hell";
            koh.use_basic_prefab = true;
            koh.sprite_path = "effects/fx_king_of_hell";
            koh.sorting_layer_id = "EffectsTop";
            koh.draw_light_size = 1.0f;

            AssetManager.effects_library.add(koh);
            #endregion

            #region Yellow Flash Effect
            EffectAsset customTeleportEffect = new EffectAsset();
            customTeleportEffect.id = "fx_teleport_yellow";
            customTeleportEffect.use_basic_prefab = true;
            customTeleportEffect.draw_light_size = 1.0f;
            customTeleportEffect.sorting_layer_id = "EffectsTop";
            customTeleportEffect.sprite_path = "effects/fx_teleport_yellow";
            AssetManager.effects_library.add(customTeleportEffect);
            #endregion

            #region Katon
            EffectAsset katonJutsu = new EffectAsset();
            katonJutsu.id = "fx_katon";
            katonJutsu.use_basic_prefab = true;
            katonJutsu.draw_light_size = 1.0f;
            katonJutsu.sorting_layer_id = "EffectsTop";
            katonJutsu.sprite_path = "effects/fx_katon";
            AssetManager.effects_library.add(katonJutsu);
            #endregion

            #region Beast Charge
            EffectAsset beastCharge = new EffectAsset();
            beastCharge.id = "fx_beastcharge";
            beastCharge.use_basic_prefab = true;
            beastCharge.draw_light_size = 1.0f;
            beastCharge.sorting_layer_id = "EffectsTop";
            beastCharge.sprite_path = "effects/fx_god_body_effect";
            AssetManager.effects_library.add(beastCharge);
            #endregion

        }
    }
}