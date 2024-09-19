using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;

namespace SAMcircle
{
    // Token: 0x02000005 RID: 5
    [HarmonyPatch(typeof(MFDPTacticalSituationDisplay), "UpdateScaleText")]
    internal class ScaleTextPatch
    {
        // Token: 0x06000010 RID: 16 RVA: 0x000024B0 File Offset: 0x000006B0
        public static void Postfix()
        {
            bool flag = ScaleTextPatch.onScaleText != null;
            if (flag)
            {
                ScaleTextPatch.onScaleText.Invoke();
                Debug.Log("Invoked onScaleText!");
            }
            else
            {
                Debug.Log("Did not invoke onScaleText :(");
            }
        }

        // Token: 0x0400000E RID: 14
        public static UnityEvent onScaleText = new UnityEvent();
    }
}

