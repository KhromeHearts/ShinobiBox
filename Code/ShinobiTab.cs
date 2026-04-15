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

            Sprite kuramaIcon = SpriteTextureLoader.getSprite("ui/icons/9Tails");
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

            Sprite tenTailsIcon = SpriteTextureLoader.getSprite("ui/icons/10Tails");
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

            Sprite hashiramaIcon = SpriteTextureLoader.getSprite("ui/icons/hashirama_cells");
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

            Sprite madaraIcon = SpriteTextureLoader.getSprite("ui/icons/madara_mangekyo");
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