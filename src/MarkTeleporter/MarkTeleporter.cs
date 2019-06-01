using System;
using System.Linq;
using BepInEx;
using BepInEx.Bootstrap;
using Frogtown;
using On.RoR2;
using RoR2.UI;
using UnityEngine;
using PositionIndicator = RoR2.PositionIndicator;
using TeleporterInteraction = RoR2.TeleporterInteraction;

namespace MarkTeleporter
{
    [BepInDependency("com.frogtown.shared", BepInDependency.DependencyFlags.SoftDependency)] //Doesn't seem to actually work
    [BepInPlugin("com.orangutan.markteleporter", "MarkTeleporter", "1.0.3")]
    public class MarkTeleporter : BaseUnityPlugin
    {
        public FrogtownModDetails modDetails;
        GameObject _teleporterPositionIndicator;
        public bool frogtownInstalled;

        public void Awake()
        {
            frogtownInstalled =
                Chainloader.Plugins.Any(x => MetadataHelper.GetMetadata(x).GUID == "com.frogtown.shared");

            if (frogtownInstalled)
            {
                modDetails = new FrogtownModDetails("com.orangutan.markteleporter")
                {
                    description = "Marks the teleporter.",
                    githubAuthor = "OrangutanGaming",
                    githubRepo = "RoR2-MarkTeleporter",
                    thunderstoreFullName = "OrangutanGaming-MarkTeleporter"
                };
                try
                {
                    FrogtownShared.RegisterMod(modDetails);
                }
                catch (ArgumentException)
                {
                    // Spams Awake event when disabled for some reason causing this to error due to it already being registered
                }
            }

            SceneDirector.PlaceTeleporter += (orig, self) =>
            {
                orig(self);
                var prefab =
                    Resources.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator");
                _teleporterPositionIndicator = Instantiate(prefab, TeleporterInteraction.instance.transform.position,
                    Quaternion.identity);
                _teleporterPositionIndicator.GetComponent<PositionIndicator>().targetTransform =
                    TeleporterInteraction.instance.transform;
                _teleporterPositionIndicator.GetComponent<ChargeIndicatorController>().isCharged = true;
            };
        }

        public void Update()
        {
            var teleporter = TeleporterInteraction.instance;
            if (teleporter == null)
            {
                return;
            }

            if (!frogtownInstalled || !modDetails.enabled)
            {
                if (_teleporterPositionIndicator)
                {
                    Destroy(_teleporterPositionIndicator);
                }
            }
            else
            {
                if (!_teleporterPositionIndicator)
                {
                    var prefab =
                        Resources.Load<GameObject>(
                            "Prefabs/PositionIndicators/TeleporterChargingPositionIndicator");
                    _teleporterPositionIndicator = Instantiate(prefab,
                        TeleporterInteraction.instance.transform.position,
                        Quaternion.identity);
                    _teleporterPositionIndicator.GetComponent<PositionIndicator>().targetTransform =
                        TeleporterInteraction.instance.transform;
                    _teleporterPositionIndicator.GetComponent<ChargeIndicatorController>().isCharged = true;
                }
            }

            var poi = teleporter.gameObject.GetComponent<MarkTeleporter>();

            if (!frogtownInstalled || !modDetails.enabled)
            {
                if (poi)
                {
                    Destroy(poi);
                    return;
                }
            }

            if (poi == null)
            {
                teleporter.gameObject.AddComponent<MarkTeleporter>();
            }
        }
    }
}