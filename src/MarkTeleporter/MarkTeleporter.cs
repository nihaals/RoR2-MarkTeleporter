using BepInEx;
using RoR2;
using UnityEngine;

namespace MarkTeleporter
{
    [BepInPlugin("com.orangutan.markteleporter", "MarkTeleporter", "0.0.1")]

    public class MarkTeleporter : BaseUnityPlugin
    {
        public void Update()
        {
            var teleporter = TeleporterInteraction.instance;
                if (teleporter == null)
                    return;
                var poi = teleporter.gameObject.GetComponent<TeleporterIndicator>();
                if (poi == null)
                    teleporter.gameObject.AddComponent<TeleporterIndicator>();
        }

        class TeleporterIndicator : BaseUnityPlugin
        {
            GameObject _teleporterPositionIndicator;
            private void Awake()
            {
                var prefab = Resources.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator");
                _teleporterPositionIndicator = Instantiate(prefab, TeleporterInteraction.instance.transform.position, Quaternion.identity);
                _teleporterPositionIndicator.GetComponent<PositionIndicator>().targetTransform = TeleporterInteraction.instance.transform;
                _teleporterPositionIndicator.GetComponent<RoR2.UI.ChargeIndicatorController>().isCharged = true;
            }
        }
    }
}