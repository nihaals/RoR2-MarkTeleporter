using BepInEx;
using Frogtown;
using RoR2;
using UnityEngine;

namespace MarkTeleporter
{
    [BepInDependency("com.frogtown.shared")]
    [BepInPlugin("com.orangutan.markteleporter", "MarkTeleporter", "1.0.1")]
    public class MarkTeleporter : BaseUnityPlugin
    {
        public FrogtownModDetails modDetails;
        GameObject _teleporterPositionIndicator;

        public void Awake()
        {
            modDetails = new FrogtownModDetails("com.orangutan.markteleporter")
            {
                description = "Marks the teleporter.",
                githubAuthor = "OrangutanGaming",
                githubRepo = "RoR2-MarkTeleporter",
                thunderstoreFullName = "OrangutanGaming-MarkTeleporter",
            };
            FrogtownShared.RegisterMod(modDetails);

            On.RoR2.SceneDirector.PlaceTeleporter += (orig, self) =>
            {
                orig(self);
                var prefab =
                    Resources.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator");
                _teleporterPositionIndicator = Instantiate(prefab, TeleporterInteraction.instance.transform.position,
                    Quaternion.identity);
                _teleporterPositionIndicator.GetComponent<PositionIndicator>().targetTransform =
                    TeleporterInteraction.instance.transform;
                _teleporterPositionIndicator.GetComponent<RoR2.UI.ChargeIndicatorController>().isCharged = true;
            };
        }

        public void Update()
        {
            if (!modDetails.enabled)
            {
                if (_teleporterPositionIndicator)
                {
                    GameObject.Destroy(_teleporterPositionIndicator);
                }
            }
            else
            {
                if (!_teleporterPositionIndicator)
                {
                    var prefab =
                        Resources.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator");
                    _teleporterPositionIndicator = Instantiate(prefab,
                        TeleporterInteraction.instance.transform.position,
                        Quaternion.identity);
                    _teleporterPositionIndicator.GetComponent<PositionIndicator>().targetTransform =
                        TeleporterInteraction.instance.transform;
                    _teleporterPositionIndicator.GetComponent<RoR2.UI.ChargeIndicatorController>().isCharged = true;
                }
            }

            var teleporter = TeleporterInteraction.instance;
            if (teleporter == null)
                return;
            var poi = teleporter.gameObject.GetComponent<MarkTeleporter>();
            if (!modDetails.enabled)
            {
                if (poi)
                {
                    Destroy(poi);
                    return;
                }
            }

            if (poi == null)
                teleporter.gameObject.AddComponent<MarkTeleporter>();
        }
    }
}