// CODE FROM NERF FIRE DAMAGE MOD BY RayTracerX

using HarmonyLib;
using UnityEngine;

namespace ShinobiBox; 
[HarmonyPatch(typeof(StatusLibrary), nameof(StatusLibrary.burningEffect))]
public static class BurningEffectPatch
{
    private const float BurnPercentPerTick = 0.01f; // Vanilla is 0.10f, change this to however you feel fits.

    private const float DamageMultiplier = 0.05f; // Vanilla is 0.25f, change this to however you feel fits.


    static bool Prefix(BaseSimObject pTarget, WorldTile pTile, ref bool __result)
    {
        if (pTarget == null)
        {
            __result = false;
            return false;
        }

        if (pTarget.isActor() && pTarget.a.asset.has_skin && Randy.randomBool())
        {
            pTarget.a.addInjuryTrait("skin_burns");
        }

        int damage = Mathf.Max(1, pTarget.getMaxHealthPercent(BurnPercentPerTick));
        if (pTarget.isBuilding() && pTarget.b.isRuin())
        {
            damage = Mathf.Max(1, (int)(damage * DamageMultiplier + 1f));
        }

        pTarget.getHit(damage, pFlash: true, AttackType.Fire);

        if (MapBox.isRenderGameplay() && Randy.randomChance(0.1f))
        {
            World.world.particles_fire.spawn(pTarget.current_position.x, pTarget.current_position.y, true);
        }

        __result = true;
        return false;
    }
}
