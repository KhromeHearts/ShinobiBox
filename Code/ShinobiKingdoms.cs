using System;
using UnityEngine;

namespace ShinobiBox
{
    public static class ShinobiKingdoms
    {
        public const string RogueKingdomId = "rogue";
        public const string TailedBeastsKingdomId = "tailed_beasts";
        public const string KonohaKingdomId = "konoha";

        public static void Init()
        {
            KingdomAsset shinobiMobs = AssetManager.kingdoms.get(RogueKingdomId);
            bool createdRogueKingdom = false;

            if (shinobiMobs == null)
            {
                shinobiMobs = new KingdomAsset();
                shinobiMobs.id = RogueKingdomId;
                AssetManager.kingdoms.add(shinobiMobs);
                createdRogueKingdom = true;
            }

            shinobiMobs.mobs = true;
            shinobiMobs.count_as_danger = true;
            shinobiMobs.units_always_looking_for_enemies = true;
            shinobiMobs.addTag(RogueKingdomId);
            shinobiMobs.addFriendlyTag("animal");
            shinobiMobs.addFriendlyTag("neutral_animals");
            shinobiMobs.addFriendlyTag("nature_creatures");
            shinobiMobs.addEnemyTag(TailedBeastsKingdomId);
            shinobiMobs.addEnemyTag("civ");
            shinobiMobs.addEnemyTag(KonohaKingdomId);
            shinobiMobs.default_kingdom_color = new ColorAsset("#851b08");

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
            tailedBeasts.addFriendlyTag("animal");
            tailedBeasts.addFriendlyTag("neutral_animals");
            tailedBeasts.addFriendlyTag("nature_creatures");
            tailedBeasts.addEnemyTag("civ");
            tailedBeasts.addEnemyTag("human");
            tailedBeasts.addEnemyTag(RogueKingdomId);
            tailedBeasts.addEnemyTag(KonohaKingdomId);

            tailedBeasts.default_kingdom_color = new ColorAsset("#cf7712");

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
            konoha.neutral = true;
            konoha.addTag(KonohaKingdomId);
            konoha.addTag("human");
            konoha.addFriendlyTag(KonohaKingdomId);
            konoha.addFriendlyTag("civ");
            konoha.addFriendlyTag("human");
            konoha.addFriendlyTag("animal");
            konoha.addFriendlyTag("neutral_animals");
            konoha.addFriendlyTag("nature_creatures");
            konoha.addEnemyTag(RogueKingdomId);
            konoha.addEnemyTag(TailedBeastsKingdomId);
            konoha.default_kingdom_color = new ColorAsset("#257b0b");

            if (World.world != null && World.world.kingdoms_wild != null)
            {
                if (createdRogueKingdom)
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