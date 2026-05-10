using System;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;
using NeoModLoader.General.UI.Tab;

namespace ShinobiBox
{
    public static class ShinobiTab
    {
        private static PowersTab tab;

        public static void Init()
        {
            Sprite tabIcon = SpriteTextureLoader.getSprite("ui/icons/chakra_drain");
            if (tabIcon == null)
            {
                Debug.LogWarning("[ShinobiBox] Tab icon not found, skipping tab creation");
                return;
            }


            tab = TabManager.CreateTab(
                "Shinobi",
                "Button_Tab_Shinobi",
                "Button_Tab_Shinobi Description",
                tabIcon,
                "ShinobiBox"
            );
            if (tab == null)
            {
                Debug.LogError("[ShinobiBox] Failed to create Shinobi tab");
                return;
            }

            LoadButtons();
        }

        private static void LoadButtons()
        {
            if (tab == null) return;
            try
            {
                #region Kurama Power
                // Kurama Button
                GodPower kuramaPower = new GodPower();
                kuramaPower.id = "summon_kurama";
                kuramaPower.name = "summon_kurama";
                kuramaPower.actor_asset_id = ShinobiActors.KuramaActorId;
                kuramaPower.show_spawn_effect = true;
                kuramaPower.actor_spawn_height = 3f;
                kuramaPower.sound_event = "spawn";
                kuramaPower.multiple_spawn_tip = true;
                kuramaPower.click_action = new PowerActionWithID(CallSpawnUnit);
                AssetManager.powers.add(kuramaPower);

                Sprite kuramaIcon = SpriteTextureLoader.getSprite("ui/icons/units/9Tails");
                if (kuramaIcon != null)
                {
                    PowerButtons.CreateButton(
                        "summon_kurama",
                        kuramaIcon,
                        "Summon Kurama",
                        "Spawn the Nine-Tailed Beast",
                        new Vector2(254f, 18f),
                        ButtonType.GodPower,
                        tab.transform,
                        null
                    );
                }
                #endregion

                #region Ten Tails Power
                // Ten-Tails Button
                GodPower tenTailsPower = new GodPower();
                tenTailsPower.id = "summon_ten_tails";
                tenTailsPower.name = "summon_ten_tails";
                tenTailsPower.actor_asset_id = ShinobiActors.JuubiActorId;
                tenTailsPower.show_spawn_effect = true;
                tenTailsPower.actor_spawn_height = 3f;
                tenTailsPower.sound_event = "spawn";
                tenTailsPower.multiple_spawn_tip = true;
                tenTailsPower.click_action = new PowerActionWithID(CallSpawnUnit);
                AssetManager.powers.add(tenTailsPower);

                Sprite tenTailsIcon = SpriteTextureLoader.getSprite("ui/icons/units/10Tails");
                if (tenTailsIcon != null)
                {
                    PowerButtons.CreateButton(
                        "summon_ten_tails",
                        tenTailsIcon,
                        "Summon Ten-Tails",
                        "Spawn the Ten-Tailed Beast",
                        new Vector2(290f, 18f),
                        ButtonType.GodPower,
                        tab.transform,
                        null
                    );
                }
                #endregion

                #region Hashirama Power
                GodPower hashiramaPower = new GodPower();
                hashiramaPower.id = "summon_hashirama";
                hashiramaPower.name = "summon_hashirama";
                hashiramaPower.actor_asset_id = ShinobiActors.HashiramaActorId;
                hashiramaPower.show_spawn_effect = true;
                hashiramaPower.multiple_spawn_tip = true;
                hashiramaPower.actor_spawn_height = 3f;
                hashiramaPower.sound_event = "spawn";
                hashiramaPower.click_action = new PowerActionWithID(CallSpawnUnit);
                AssetManager.powers.add(hashiramaPower);

                Sprite hashiramaIcon = SpriteTextureLoader.getSprite("ui/icons/units/IconHashirama");
                if (hashiramaIcon != null)
                {
                    PowerButtons.CreateButton(
                        "summon_hashirama",
                        hashiramaIcon,
                        "Summon Hashirama",
                        "Spawn legendary Hashirama",
                        new Vector2(326f, 18f),
                        ButtonType.GodPower,
                        tab.transform,
                        null
                    );
                }
                #endregion

                #region Madara Power
                GodPower madaraPower = new GodPower();
                madaraPower.id = "summon_madara";
                madaraPower.name = "summon_madara";
                madaraPower.actor_asset_id = ShinobiActors.MadaraActorId;
                madaraPower.show_spawn_effect = true;
                madaraPower.actor_spawn_height = 3f;
                madaraPower.sound_event = "spawn";
                madaraPower.click_action = new PowerActionWithID(CallSpawnUnit);
                AssetManager.powers.add(madaraPower);

                Sprite madaraIcon = SpriteTextureLoader.getSprite("ui/icons/units/IconMadara");
                if (madaraIcon != null)
                {
                    PowerButtons.CreateButton(
                        "summon_madara",
                        madaraIcon,
                        "Summon Madara",
                        "Spawn legendary Madara",
                        new Vector2(362f, 18f),
                        ButtonType.GodPower,
                        tab.transform,
                        null
                    );
                }
                #endregion

                #region Sasuke Power
                GodPower sasukePower = new GodPower();
                sasukePower.id = "summon_sasuke";
                sasukePower.name = "summon_sasuke";
                sasukePower.actor_asset_id = ShinobiActors.SasukeActorId;
                sasukePower.show_spawn_effect = true;
                sasukePower.actor_spawn_height = 3f;
                sasukePower.sound_event = "spawn";
                sasukePower.click_action = new PowerActionWithID(CallSpawnUnit);
                AssetManager.powers.add(sasukePower);

                Sprite sasukeIcon = SpriteTextureLoader.getSprite("ui/icons/units/IconSasuke");
                if (sasukeIcon != null)
                {
                    PowerButtons.CreateButton(
                        "summon_sasuke",
                        sasukeIcon,
                        "Summon Sasuke",
                        "Spawn Sasuke Uchiha",
                        new Vector2(254f, 54f),
                        ButtonType.GodPower,
                        tab.transform,
                        null
                    );
                }
                #endregion

                #region Itachi Power
                GodPower itachiPower = new GodPower();
                itachiPower.id = "summon_itachi";
                itachiPower.name = "summon_itachi";
                itachiPower.actor_asset_id = ShinobiActors.ItachiActorId;
                itachiPower.show_spawn_effect = true;
                itachiPower.actor_spawn_height = 3f;
                itachiPower.sound_event = "spawn";
                itachiPower.click_action = new PowerActionWithID(CallSpawnUnit);
                AssetManager.powers.add(itachiPower);

                Sprite itachiIcon = SpriteTextureLoader.getSprite("ui/icons/units/IconItachi");
                if (itachiIcon != null)
                {
                    PowerButtons.CreateButton(
                        "summon_itachi",
                        itachiIcon,
                        "Summon Itachi",
                        "Spawn Itachi Uchiha",
                        new Vector2(290f, 54f),
                        ButtonType.GodPower,
                        tab.transform,
                        null
                    );
                }
                #endregion

                #region Naruto Power
                GodPower narutoPower = new GodPower();
                narutoPower.id = "summon_naruto";
                narutoPower.name = "summon_naruto";
                narutoPower.actor_asset_id = ShinobiActors.NarutoActorId;
                narutoPower.show_spawn_effect = true;
                narutoPower.actor_spawn_height = 3f;
                narutoPower.sound_event = "spawn";
                narutoPower.click_action = new PowerActionWithID(CallSpawnUnit);
                AssetManager.powers.add(narutoPower);

                Sprite narutoIcon = SpriteTextureLoader.getSprite("ui/icons/units/IconNaruto");
                if (narutoIcon != null)
                {
                    PowerButtons.CreateButton(
                        "summon_naruto",
                        narutoIcon,
                        "Summon Naruto",
                        "Spawn Naruto Uzumaki",
                        new Vector2(326f, 54f),
                        ButtonType.GodPower,
                        tab.transform,
                        null
                    );
                }
                #endregion

                #region Sakura Power
                GodPower sakuraPower = new GodPower();
                sakuraPower.id = "summon_sakura";
                sakuraPower.name = "summon_sakura";
                sakuraPower.actor_asset_id = ShinobiActors.SakuraActorId;
                sakuraPower.show_spawn_effect = true;
                sakuraPower.actor_spawn_height = 3f;
                sakuraPower.sound_event = "spawn";
                sakuraPower.click_action = new PowerActionWithID(CallSpawnUnit);
                AssetManager.powers.add(sakuraPower);

                Sprite sakuraIcon = SpriteTextureLoader.getSprite("ui/icons/units/IconSakura");
                if (sakuraIcon != null)
                {
                    PowerButtons.CreateButton(
                        "summon_sakura",
                        sakuraIcon,
                        "Summon Sakura",
                        "Spawn Sakura Haruno",
                        new Vector2(362f, 54f),
                        ButtonType.GodPower,
                        tab.transform,
                        null
                    );
                }
                #endregion
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        public static bool CallSpawnUnit(WorldTile pTile, string pPowerID)
        {
            if (pTile == null || string.IsNullOrEmpty(pPowerID)) return false;
            try
            {
                var power = AssetManager.powers.get(pPowerID);
                if (power == null) return false;
                if (string.IsNullOrEmpty(power.actor_asset_id)) return false;
                if (AssetManager.actor_library.get(power.actor_asset_id) == null)
                {
                    Debug.LogWarning($"[ShinobiBox] Spawn failed: actor asset '{power.actor_asset_id}' is not registered for power '{pPowerID}'.");
                    return false;
                }

                float spawnHeight = power.actor_spawn_height > 0f ? power.actor_spawn_height : 3f;

                Actor spawned = World.world.units.spawnNewUnit(
                    power.actor_asset_id,
                    pTile,
                    pSpawnSound: true,
                    pMiracleSpawn: true,
                    pSpawnHeight: spawnHeight,
                    pSubspecies: null,
                    pGiveOwnerlessItems: false,
                    pAdultAge: true
                );

                if (spawned == null)
                {
                    spawned = World.world.units.createNewUnit(power.actor_asset_id, pTile, true, spawnHeight);
                }
                if (spawned == null) return false;

                spawned.data.level = 15;


                if (spawned.kingdom == null)
                {
                    Kingdom fallbackKingdom = null;
                    if (pTile.zone != null && pTile.zone.city != null)
                    {
                        fallbackKingdom = pTile.zone.city.kingdom;
                    }
                    if (fallbackKingdom == null)
                    {
                        fallbackKingdom = World.world?.kingdoms?.getRandom();
                    }
                    if (fallbackKingdom != null)
                    {
                        spawned.setKingdom(fallbackKingdom);
                    }
                }

                if (pPowerID == "summon_hashirama")
                {
                    spawned.setName("Hashirama", true);
                    spawned.data.name = "Hashirama";
                }
                else if (pPowerID == "summon_kurama")
                {
                    spawned.setName("Kurama", true);
                    spawned.data.name = "Kurama";
                    spawned.data.set("chakra_nature_exp", 1000f);
                    spawned.data.set("chakra_nature_roll_steps", 10);
                    spawned.data.set("nature_progress_initialized", 1);
                    spawned.data.set("nature_last_level", spawned.data.level);
                    spawned.data.set("nature_last_kills", spawned.data.kills);
                }
                else if (pPowerID == "summon_madara")
                {
                    spawned.setName("Madara", true);
                    spawned.data.name = "Madara";
                }
                else if (pPowerID == "summon_sasuke")
                {
                    spawned.setName("Sasuke", true);
                    spawned.data.name = "Sasuke";
                }
                else if (pPowerID == "summon_itachi")
                {
                    spawned.setName("Itachi", true);
                    spawned.data.name = "Itachi";
                }
                else if (pPowerID == "summon_naruto")
                {
                    spawned.setName("Naruto", true);
                    spawned.data.name = "Naruto";
                }
                else if (pPowerID == "summon_sakura")
                {
                    spawned.setName("Sakura", true);
                    spawned.data.name = "Sakura";
                }
                else if (pPowerID == "summon_ten_tails")
                {
                    spawned.setName("Ten-Tails", true);
                    spawned.data.name = "Ten-Tails";
                    spawned.data.set("chakra_nature_exp", 1000f);
                    spawned.data.set("chakra_nature_roll_steps", 10);
                    spawned.data.set("nature_progress_initialized", 1);
                    spawned.data.set("nature_last_level", spawned.data.level);
                    spawned.data.set("nature_last_kills", spawned.data.kills);
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }
}