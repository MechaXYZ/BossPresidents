using UMM;
using UnityEngine;

namespace PrimePresidents
{
    [UKPlugin("org.PrimePresidents","Prime Presidents", "1.0.0", "Jowari da", true, true)]
    public class Presidents : UKMod
    {
        public override void OnModLoaded()
        {
            Debug.Log("Prime presidents starting");
        }
    }
}
