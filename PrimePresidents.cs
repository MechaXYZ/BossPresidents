using UMM;
using UnityEngine;
using HarmonyLib;
using System.IO;

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

        //repalace minos prime data
        [HarmonyPatch(typeof(MinosPrime), "Start")]
        internal class Patch01
        {
            static void Postfix(MinosPrime __instance){
                Debug.Log("Replacing prime voice lines");

                //set judgement to biden blast
                AudioClip[] dropkickLines = new AudioClip[1];
                dropkickLines[0] = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_blast.mp3");
                __instance.dropkickVoice = dropkickLines;

                Debug.Log("Replacing mesh texture");
                               
                //set texture to be biden prime
                var body = __instance.transform.Find("Model").Find("MinosPrime_Body.001");
                var newMat = new Material(body.GetComponent<Renderer>().material);
                newMat.mainTexture = PresidentsAssetBundle.LoadAsset<Texture2D>("JoePrime_1.png");
                body.GetComponent<Renderer>().sharedMaterial = newMat;
            }
        }
    }
}
