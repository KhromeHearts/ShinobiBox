using System;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using NeoModLoader.api;

namespace ShinobiBox
{
    public class Main : BasicMod<Main>
    {
        protected override void OnModLoad()
        {
            var harmony = new Harmony("com.shinobibox.mod");
            try
            {
                harmony.PatchAll(typeof(ChakraIconPatch));
                harmony.PatchAll(typeof(ChakraValuePatch));
                harmony.PatchAll(typeof(HagoromoBirthPatch));
                harmony.PatchAll(typeof(GuaranteedInheritancePatch));
                harmony.PatchAll(typeof(WillOfFireDeathPatch));
                harmony.PatchAll(typeof(CurseOfHatredPatch));
                harmony.PatchAll(typeof(LastStandPatch));
                harmony.PatchAll(typeof(EMSKillPatch));
                harmony.PatchAll(typeof(KamuiPhasePatch));
                harmony.PatchAll(typeof(JinchurikiStressPatch));
                harmony.PatchAll(typeof(BlindnessIconPatch));
                harmony.PatchAll(typeof(RealTimeChakraUpdate));
                harmony.PatchAll(typeof(NatureExpIconPatch));

                Debug.Log("[ShinobiBox] All systems loaded.");
            }
            catch (Exception e)
            {
                Debug.LogError("[ShinobiBox] Patch Error: " + e.Message);
            }

            ShinobiTraits.Init();
            ShinobiProjectiles.Init();
            ShinobiItems.Init();
            ShinobiActors.Init();
            ShinobiTab.Init();
            ShinobiStatus.Init();
            ShinobiEffects.Init();
            ShinobiSounds.Init();
            ShinobiKingdoms.Init();


            if (GameObject.Find("ShinobiBox_Logic") == null)
            {
                var go = new GameObject("ShinobiBox_Logic");
                UnityEngine.Object.DontDestroyOnLoad(go);
                go.AddComponent<ShinobiLoop>();
            }
        }
    }

    public class ShinobiLoop : MonoBehaviour
    {
        private float timer = 0f;
        private float cleanupTimer = 0f;
        private float regenTimer = 0f;
        private float progressionTimer = 0f;

        void Update()
        {
            if (World.world.isPaused()) return;

            JutsuLibrary.TickWorldAgeRevert(Time.deltaTime);

            timer += Time.deltaTime;
            if (timer >= 0.1f)
            {
                foreach (Actor actor in MapBox.instance.units)
                {
                    if (actor == null || !actor.isAlive()) continue;
                    if (!ChakraSystem.IsChakraUser(actor)) continue;

                    ChakraSystem.UpdateMax(actor);
                    ChakraSystem.ProcessDrain(actor, 0.1f);

                    // Chakra Regen
                    if (!ShinobiProgression.IsModeActive(actor) && !actor.hasStatus("chakra_exhaustion") && (Time.time - ChakraSystem.GetLastMoveTime(actor)) > 7f)
                    {
                        ChakraSystem.AddChakra(actor, ChakraSystem.GetRegenRate(actor) * 0.1f);
                    }


                }
                timer = 0f;
            }

            cleanupTimer += Time.deltaTime;
            if (cleanupTimer >= 10f)
            {
                ChakraSystem.CleanDeadActors();
                cleanupTimer = 0f;
            }

            regenTimer += Time.deltaTime;
            if (regenTimer >= 1.0f)
            {
                foreach (Actor a in MapBox.instance.units)
                {
                    if (a.isAlive())
                    {
                        if (a.hasTrait("uzumaki_clan") && a.data.health < a.getMaxHealth()) a.restoreHealth(4);
                        if (a.hasTrait("senju_clan") && a.data.health < a.getMaxHealth()) a.restoreHealth(1);
                        if (a.hasTrait("hashi_cells") && a.data.health < a.getMaxHealth()) a.restoreHealth(5);
                        if (a.hasTrait("rank_kage") && a.data.health < a.getMaxHealth()) a.restoreHealth(1);
                    }
                }
                regenTimer = 0f;
            }

            progressionTimer += Time.deltaTime;
            if (progressionTimer >= 3.0f)
            {
                foreach (Actor a in MapBox.instance.units)
                {
                    if (a.isAlive())
                    {
                        ShinobiProgression.CheckProgression(a);
                    }
                }
                progressionTimer = 0f;
            }


            // a way for me to test spawn rates basically
            if (Input.GetKeyDown(KeyCode.K)) RunCensus();
        }



        #region Census Helper
        public void RunCensus()
        {
            int total = 0;
            int sage = 0;
            int uchiha = 0;
            int jinchuriki = 0;
            int mangekyo = 0;

            foreach (Actor a in MapBox.instance.units)
            {
                if (!a.isAlive()) continue;
                total++;

                if (a.hasTrait("sage_mode")) sage++;
                if (a.hasTrait("uchiha_clan")) uchiha++;
                if (a.hasTrait("nine_tails_jinchuriki")) jinchuriki++;
                if (a.hasTrait("mangekyo_sharingan")) mangekyo++;
            }

            if (total > 0)
            {
                UnityEngine.Debug.Log("Shinobi Traits");
                UnityEngine.Debug.Log($"Total Pop: {total}");
                UnityEngine.Debug.Log($"Uchiha: {uchiha} ({(float)uchiha / total * 100:F1}%)");
                UnityEngine.Debug.Log($"Sages: {sage} ({(float)sage / total * 100:F1}%)");
                UnityEngine.Debug.Log($"Jinchuriki: {jinchuriki} ({(float)jinchuriki / total * 100:F1}%)");
                UnityEngine.Debug.Log($"Mangekyo Unlocked: {mangekyo}");

                WorldTip.instance.show($"Census: {jinchuriki} Jinchuriki, {sage} Sages, {uchiha} Uchiha, {mangekyo} MS Users", false, "top", 2f);
            }
        }
        #endregion


    }
}