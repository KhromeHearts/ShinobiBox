using System;
using System.Linq;
using System.Collections.Generic;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NeoModLoader.services;
using NeoModLoader.api;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using HarmonyLib;

namespace ShinobiBox
{
    public static class ShinobiKingdoms
    {
        public static void Init()
        {
            KingdomAsset shinobiMobs = AssetManager.kingdoms.get("rouge");
            bool createdKingdom = false;

            if (shinobiMobs == null)
            {
                shinobiMobs = new KingdomAsset();
                shinobiMobs.id = "rouge";
                shinobiMobs.mobs = true;
                shinobiMobs.addTag("rouge");
                shinobiMobs.addFriendlyTag("animal");
                shinobiMobs.addFriendlyTag("neutral_animals");
                shinobiMobs.addEnemyTag("civ");
                shinobiMobs.addEnemyTag("good");
                shinobiMobs.addEnemyTag("neutral");
                shinobiMobs.addEnemyTag("nature_creature");
                shinobiMobs.addEnemyTag("nature");
                shinobiMobs.default_kingdom_color = new ColorAsset("#c45b16");
                AssetManager.kingdoms.add(shinobiMobs);
                createdKingdom = true;
            }

            if (createdKingdom && World.world != null && World.world.kingdoms_wild != null)
            {
                World.world.kingdoms_wild.newWildKingdom(shinobiMobs);
            }

        }
    }
}