using UMM;
using UnityEngine;
using HarmonyLib;
using System.IO;
using System;

namespace PrimePresidents
{
    [UKPlugin("gov.PrimePresidents","Prime Presidents", "1.0.0", "Jowari da", true, true)]
    public class Presidents : UKMod
    {
        private static Harmony harmony;

        private static readonly string BaseDirectory = Directory.GetParent(Application.dataPath).FullName;
        internal static readonly AssetBundle PresidentsAssetBundle = AssetBundle.LoadFromFile(Path.Combine(BaseDirectory, "BepInEx/UMM Mods/PrimePresidents/Assets/primepresidents"));

        public override void OnModLoaded()
        {
            Debug.Log("Prime presidents starting");

            //start harmonylib to swap assets
            harmony = new Harmony("gov.PrimePresidents");
            harmony.PatchAll();
        }

        public override void OnModUnload()
        {
            harmony.UnpatchSelf();
            base.OnModUnload();
        }

        //replace minos prime data
        [HarmonyPatch(typeof(MinosPrime), "Start")]
        internal class Patch01
        {
            static void Postfix(MinosPrime __instance){
                Debug.Log("Replacing minos voice lines");

                //set judgement to biden blast
                AudioClip[] dropkickLines = new AudioClip[1];
                dropkickLines[0] = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_blast.mp3");
                __instance.dropkickVoice = dropkickLines;

                //set ppt to choco chip
                AudioClip[] comboLines = new AudioClip[1];
                comboLines[0] = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_chocolate_chocolate.mp3");
                __instance.comboVoice = comboLines;

                //set thy end is now to come here bucko
                AudioClip[] boxingLines = new AudioClip[1];
                boxingLines[0] = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_come_here.mp3");
                __instance.boxingVoice = boxingLines;

                //set die to jowarida
                AudioClip[] riderkickLines = new AudioClip[1];
                riderkickLines[0] = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_joewareeda.mp3");
                __instance.riderKickVoice = riderkickLines;

                //set weak to thats it
                __instance.phaseChangeVoice = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_thats_it.mp3");

                Debug.Log("Replacing minos mesh texture");
                               
                //set texture to be biden prime
                var body = __instance.transform.Find("Model").Find("MinosPrime_Body.001");
                var renderer = body.GetComponent<Renderer>();
                var newMat = new Material(renderer.material);
                newMat.mainTexture = PresidentsAssetBundle.LoadAsset<Texture2D>("JoePrime_1.png");
                renderer.sharedMaterial = newMat;
            }
        }

        //replace captions for minos attacks
        [HarmonyPatch(typeof(SubtitleController), nameof(SubtitleController.DisplaySubtitle), new Type[]{typeof(string), typeof(AudioSource)})]
        internal class Patch02
        {
            static void Prefix(ref string caption, AudioSource audioSource){
                //change caption
                if(caption == "Thy end is now!")
                {
                    caption = "Come here, bucko!";
                }
                else if(caption == "Die!"){
                    caption = "Jowarida!";
                }
                else if(caption == "WEAK"){
                    caption = "THAT'S IT BUD";
                }
                else if(caption == "Judgement!"){
                    caption = "Biden blast!";
                }
                else if(caption == "Crush!"){
                    caption = "[REPLACE ME]";
                }
                else if(caption == "Prepare thyself!"){
                    caption = "Eat some chocolate chocolate chip!";
                }
            }
        }


        //use map info to inject data
        [HarmonyPatch(typeof(StockMapInfo), "Awake")]
        internal class Patch03
        {
            static void Postfix(StockMapInfo __instance){
                //try to find dialog in scene and replace it
                foreach(var source in Resources.FindObjectsOfTypeAll<AudioSource>())
                {
                    if(source.clip){
                        if(source.clip.GetName() == "mp_intro2"){
                            source.clip = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_intro.mp3");
                        }
                        if(source.clip.GetName() == "mp_outro"){
                            source.clip = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_outro.mp3");
                        }
                        if(source.clip.GetName() == "mp_deathscream"){
                            source.clip = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_soda.mp3");
                        }
                        if(source.clip.GetName() == "mp_useless"){
                            source.clip = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_nicetry.mp3");
                        }
                    }
                }

                //replace minos meshes
                foreach(var renderer in Resources.FindObjectsOfTypeAll<SkinnedMeshRenderer>())
                {
                    if(renderer.gameObject.name == "MinosPrime_Body.001")
                    {
                        var newMat = new Material(renderer.material);
                        newMat.mainTexture = PresidentsAssetBundle.LoadAsset<Texture2D>("JoePrime_1.png");
                        renderer.sharedMaterial = newMat;
                    }
                }
            }
        }

        //replace boss names
        [HarmonyPatch(typeof(BossHealthBar), "Awake")]
        internal class Patch04
        {
            static void Prefix(BossHealthBar __instance)
            {
                if(__instance.bossName == "MINOS PRIME"){
                    __instance.bossName = "BIDEN PRIME";
                }
                if(__instance.bossName == "SISYPHUS PRIME"){
                    __instance.bossName = "TRUMP PRIME";
                }
            }
        }

        //replace intro texts
        [HarmonyPatch(typeof(LevelNamePopup), "Start")]
        internal class Patch05
        {
            //replace name AFTER to not interfere with saves
            static void Postfix(LevelNamePopup __instance)
            {
                Traverse field = Traverse.Create(__instance).Field("nameString");
                if(field.GetValue() as string == "SOUL SURVIVOR"){
                    field.SetValue("CHIEF OF STATE");
                }
                if(field.GetValue() as string == "WAIT OF THE WORLD"){
                    field.SetValue("SIN OF THE APPRENTICE");
                }
            }
        }
    }
}
