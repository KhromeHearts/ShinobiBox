using System.Net.NetworkInformation;
using UnityEngine;

namespace ShinobiBox
{
    public static class Shikotsumyaku
    {
        public static void Init()
        {
            loadShikoStatus();
            loadShikoProjectile();
        }

        private static void loadShikoStatus()
        {
            Material material = LibraryMaterials.instance.dict["mat_world_object_lit"];

            #region Shikotsumyaku Defense
            StatusAsset shikodef = new StatusAsset();
            shikodef.id = "shiko_defense";
            shikodef.path_icon = "effects/shikotsumyaku";
            shikodef.duration = 60f;

            shikodef.animated = true;
            shikodef.texture = "fx_shikodef";
            shikodef.sprite_list = SpriteTextureLoader.getSpriteList($"effects/{shikodef.texture}", false);
            shikodef.material = material;

            shikodef.need_visual_render = true;
            shikodef.use_parent_rotation = false;
            shikodef.is_animated_in_pause = false;
            shikodef.can_be_flipped = true;
            //shikodef.scale = 0.3f;
            shikodef.render_priority = 5;

            shikodef.base_stats.set("armor", 15f);

            shikodef.locale_id = "status_title_shikodef";
            shikodef.locale_description = "status_description_shikodef";

            shikodef.action_get_hit = new GetHitAction(thornsEffect);       
            
            AssetManager.status.add(shikodef);
            #endregion
        }

        private static void loadShikoProjectile()
        {
            //monkey;
        }
        public static bool ShikoDefense(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            if (pSelf == null || !pSelf.isAlive()) return false;

            if (Randy.randomChance(0.35f))
            {
                if (!JutsuLibrary.CheckAndConsumeChakra(pSelf.a, 45f)) return false;
                Actor a = pSelf.a;

                a.addStatusEffect("shiko_defense", 6f);
                return true;
            }
            return false;
        }

        public static bool BoneShot(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            if (pSelf.current_tile == null || pTarget.current_tile == null) return false;

            if (Randy.randomChance(0.35f))
            {
                if (!JutsuLibrary.CheckAndConsumeChakra(pSelf.a, 25f)) return false;

                World.world.projectiles.spawn(
                    pSelf,
                    pTarget,
                    "arrow",
                    pSelf.current_tile.posV3,
                    pTarget.current_tile.posV3
                );

                return true;
            }

            return false;
        }

        public static bool thornsEffect(BaseSimObject pSelf, BaseSimObject pAttackedBy, WorldTile pTile = null)
        {
            if (pSelf.a == null || !pSelf.isAlive()) return false;
            if (pAttackedBy == null || !pAttackedBy.isAlive()) return false;
            Actor a = pSelf.a;
            Actor b = pAttackedBy.a;

            if (Randy.randomChance(0.35f))
            {
                float tDamage = b.stats["damage"] * 0.2f;
                b.getHit(tDamage, true, AttackType.Weapon, pSelf, true, false, true);
                return true;
            }
            return false;
        }
    }
}