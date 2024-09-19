using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTOLAPI;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;

namespace SAMcircle
{
    // Token: 0x02000004 RID: 4
    public class RadarCircle : MonoBehaviour
    {
        // Token: 0x06000008 RID: 8 RVA: 0x0000228F File Offset: 0x0000048F
        private void Awake()
        {
            ScaleTextPatch.onScaleText.AddListener(new UnityAction(this.updateScale));
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000022A9 File Offset: 0x000004A9
        private void Start()
        {
            this.rectTf = base.gameObject.GetComponent<RectTransform>();
            this.measurements = VTAPI.GetPlayersVehicleGameObject().GetComponentInChildren<MeasurementManager>(true);
            this.rcs = VTAPI.GetPlayersVehicleGameObject().GetComponent<RadarCrossSection>();
            this.updateScale();
        }

        // Token: 0x0600000A RID: 10 RVA: 0x000022E5 File Offset: 0x000004E5
        private void Update()
        {
            this.updateSize();
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000022F0 File Offset: 0x000004F0
        public void updateScale()
        {
            this.scaleIdx = (int)this.tsdTraverse.Field("viewScaleIdx").GetValue();
            this.viewScale = this.getViewScale();
            this.scaleFactor = this.viewScale / 100f;
        }

        // Token: 0x0600000C RID: 12 RVA: 0x0000233C File Offset: 0x0000053C
        public void updateSize()
        {
            Vector3 vector = this.tsd.referenceTf.position - this.unitRadar.transform.position;
            float num = this.rangeToDelta(MeasurementManager.ConvertDistance(Radar.EstimateDetectionDistance(this.rcs.GetCrossSection(Vector3.right), this.unitRadar), this.measurements.distanceMode));
            this.rectTf.sizeDelta = new Vector2(num, num);
            this.rectTf.localPosition = Vector3.zero;
        }

        // Token: 0x0600000D RID: 13 RVA: 0x000023C8 File Offset: 0x000005C8
        private float rangeToDelta(float range)
        {
            return range / this.scaleFactor;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000023E4 File Offset: 0x000005E4
        private float getViewScale()
        {
            float num;
            switch (this.measurements.distanceMode)
            {
                case MeasurementManager.DistanceModes.Meters:
                    num = this.tsd.meterViewScales[this.scaleIdx];
                    break;
                case MeasurementManager.DistanceModes.NautMiles:
                    num = this.tsd.nautMileViewScales[this.scaleIdx];
                    break;
                case MeasurementManager.DistanceModes.Feet:
                    num = this.tsd.feetViewScales[this.scaleIdx];
                    break;
                case MeasurementManager.DistanceModes.Miles:
                    num = this.tsd.mileViewScales[this.scaleIdx];
                    break;
                default:
                    num = -1f;
                    break;
            }
            return num;
        }

        // Token: 0x04000003 RID: 3
        public MFDPTacticalSituationDisplay tsd;

        // Token: 0x04000004 RID: 4
        public RadarCrossSection rcs;

        // Token: 0x04000005 RID: 5
        public Radar unitRadar;

        // Token: 0x04000006 RID: 6
        public MeasurementManager measurements;

        // Token: 0x04000007 RID: 7
        public Traverse tsdTraverse;

        // Token: 0x04000008 RID: 8
        private float testRange = 10f;

        // Token: 0x04000009 RID: 9
        private float viewScale = 0f;

        // Token: 0x0400000A RID: 10
        private float scaleFactor = 0f;

        // Token: 0x0400000B RID: 11
        private int scaleIdx = 0;

        // Token: 0x0400000C RID: 12
        private RectTransform rectTf;

        // Token: 0x0400000D RID: 13
        private bool useAverage = false;
    }
}

