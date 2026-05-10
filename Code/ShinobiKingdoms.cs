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
        public const string RougeKingdomId = "rouge";
        public const string TailedBeastsKingdomId = "tailed_beasts";
        public const string KonohaKingdomId = "konoha";

        public static void Init()
        {
            KingdomAsset shinobiMobs = AssetManager.kingdoms.get(RougeKingdomId);
            bool createdRougeKingdom = false;

            if (shinobiMobs == null)
            {
                shinobiMobs = new KingdomAsset();
                shinobiMobs.id = RougeKingdomId;
                AssetManager.kingdoms.add(shinobiMobs);
                createdRougeKingdom = true;
            }

            shinobiMobs.mobs = true;
            shinobiMobs.addTag(RougeKingdomId);
            shinobiMobs.addFriendlyTag("animal");
            shinobiMobs.addFriendlyTag("neutral_animals");
            shinobiMobs.addEnemyTag(TailedBeastsKingdomId);
            shinobiMobs.addEnemyTag("civ");
            shinobiMobs.default_kingdom_color = new ColorAsset("#c45b16");

            KingdomAsset tailedBeasts = AssetManager.kingdoms.get(TailedBeastsKingdomId);
            bool createdTailedBeastKingdom = false;
            if (tailedBeasts == null)
            {
                tailedBeasts = new KingdomAsset();
                tailedBeasts.id = TailedBeastsKingdomId;
                AssetManager.kingdoms.add(tailedBeasts);
                createdTailedBeastKingdom = true;
            }

            tailedBeasts.mobs = true;
            tailedBeasts.addTag(TailedBeastsKingdomId);
            tailedBeasts.addFriendlyTag(TailedBeastsKingdomId);
            tailedBeasts.addEnemyTag("civ");
            tailedBeasts.addEnemyTag("human");
            tailedBeasts.default_kingdom_color = new ColorAsset("#5a1f1f");

            KingdomAsset konoha = AssetManager.kingdoms.get(KonohaKingdomId);
            bool createdKonohaKingdom = false;
            if (konoha == null)
            {
                konoha = new KingdomAsset();
                konoha.id = KonohaKingdomId;
                AssetManager.kingdoms.add(konoha);
                createdKonohaKingdom = true;
            }

            konoha.mobs = false;
            konoha.addTag(KonohaKingdomId);
            konoha.addFriendlyTag(KonohaKingdomId);
            konoha.addFriendlyTag("civ");
            konoha.addFriendlyTag("human");
            konoha.addEnemyTag(RougeKingdomId);
            konoha.addEnemyTag(TailedBeastsKingdomId);
            konoha.default_kingdom_color = new ColorAsset("#ff0000");

            if (World.world != null && World.world.kingdoms_wild != null)
            {
                if (createdRougeKingdom)
                {
                    World.world.kingdoms_wild.newWildKingdom(shinobiMobs);
                }
                if (createdTailedBeastKingdom)
                {
                    World.world.kingdoms_wild.newWildKingdom(tailedBeasts);
                }
                if (createdKonohaKingdom)
                {
                    World.world.kingdoms_wild.newWildKingdom(konoha);
                }
            }

        }
    }
}