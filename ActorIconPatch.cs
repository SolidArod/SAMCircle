using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace SAMcircle
{
    // Token: 0x02000002 RID: 2
    [HarmonyPatch(typeof(MFDPTacticalSituationDisplay), "CreateActorIcon")]
    internal class ActorIconPatch
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static void Postfix(MFDPTacticalSituationDisplay __instance, ref TSDContactIcon __result, TacticalSituationController.TSActorTargetInfo info)
        {
            bool flag = ActorIconPatch.tsdTraverse == null || ActorIconPatch.ogTsd == null || ActorIconPatch.ogTsd != __instance;
            if (flag)
            {
                FlightLogger.Log("TSD Traverse is null, creating new one");
                ActorIconPatch.tsdTraverse = Traverse.Create(__instance);
                ActorIconPatch.ogTsd = __instance;
                FlightLogger.Log("Created new tsd traverse");
            }
            bool flag2 = (int)VTOLAPI.VTAPI.GetPlayersVehicleEnum() == 3 && info.actor.team == Teams.Enemy && info.actor.hasRadar && info.actor.role != Actor.Roles.Air;
            if (flag2)
            {
                Transform radarLockIconTf = __instance.radarLockIconTf;
                GameObject gameObject = __result.radarIcon.gameObject;
                GameObject gameObject2 = new GameObject("Radar Circle");
                gameObject2.transform.SetParent(gameObject.transform);
                gameObject2.transform.localScale = Vector3.one;
                gameObject2.transform.localPosition = Vector3.zero;
                gameObject2.transform.localEulerAngles = Vector3.zero;
                bool active = gameObject.active;
                gameObject.SetActive(false);
                UICircle uicircle = gameObject2.AddComponent<UICircle>();
                uicircle.color = new Color32(225, 225, 20, byte.MaxValue);
                uicircle.raycastTarget = true;
                uicircle.fill = false;
                uicircle.FixedToSegments = false;
                uicircle.thickness = 0.4f;
                uicircle.segments = 32;
                RadarCircle radarCircle = gameObject2.AddComponent<RadarCircle>();
                radarCircle.tsdTraverse = ActorIconPatch.tsdTraverse;
                radarCircle.tsd = __instance;
                radarCircle.unitRadar = info.actor.gameObject.GetComponentInChildren<Radar>();
                RectTransform component = gameObject2.GetComponent<RectTransform>();
                gameObject.SetActive(active);
            }
        }

        // Token: 0x04000001 RID: 1
        private static MFDPTacticalSituationDisplay ogTsd;

        // Token: 0x04000002 RID: 2
        private static Traverse tsdTraverse;
    }
}

