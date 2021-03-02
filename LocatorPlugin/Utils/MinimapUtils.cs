using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using HarmonyLib;
using Purps.Valheim.Framework.Utils;
using UnityEngine;

namespace Purps.Valheim.Locator.Utils {
    public static class MinimapUtils {
        public static Dictionary<Type, Tuple<bool, List<TrackedObject>>> TrackedObjects =>
            new Dictionary<Type, Tuple<bool, List<TrackedObject>>> {
                {
                    typeof(Destructible),
                    Tuple.Create(LocatorPlugin.Config.AutoPinDestructibles, LocatorPlugin.Config.DestructibleInclusions)
                }, {
                    typeof(MineRock),
                    Tuple.Create(LocatorPlugin.Config.AutoPinMineRocks, LocatorPlugin.Config.MineRockInclusions)
                }, {
                    typeof(Location),
                    Tuple.Create(LocatorPlugin.Config.AutoPinLocations, LocatorPlugin.Config.LocationInclusions)
                }, {
                    typeof(Pickable),
                    Tuple.Create(LocatorPlugin.Config.AutoPinPickables, LocatorPlugin.Config.PickableInclusions)
                }, {
                    typeof(SpawnArea),
                    Tuple.Create(LocatorPlugin.Config.AutoPinSpawners, LocatorPlugin.Config.SpawnerInclusions)
                }, {
                    typeof(Vegvisir),
                    Tuple.Create(LocatorPlugin.Config.AutoPinVegvisirs, LocatorPlugin.Config.VegvisirInclusions)
                }, {
                    typeof(Leviathan),
                    Tuple.Create(LocatorPlugin.Config.AutoPinLeviathans, LocatorPlugin.Config.LeviathanInclusions)
                }
            };

        private static readonly HashSet<Component> TrackedComponents = new HashSet<Component>();

        public static List<Minimap.PinData> MapPins =>
            Traverse.Create(Minimap.instance)?.Field("m_pins").GetValue<List<Minimap.PinData>>() ??
            new List<Minimap.PinData>();

        public static bool IsMinimapAvailable() {
            return Minimap.instance != null;
        }

        public static void ListPins(string[] parameters) {
            if (!StatusUtils.IsPlayerLoaded()) return;
            if (!IsMinimapAvailable()) return;
            var pins = MapPins;
            if (parameters != null && parameters.Length > 0)
                pins = pins.FindAll(pin => parameters.Contains(pin.m_name.Replace(' ', '_')));

            pins.ForEach(pin =>
                ConsoleUtils.WriteToConsole(pin.m_name, pin.m_pos.ToString(),
                    $"({pin.m_type.ToString()}) {pin.m_icon.name}"));
        }

        public static void AddPin(string name, Vector3 position, Minimap.PinType type) {
            Minimap.instance.AddPin(position, type, name, true, false);
        }

        public static void RemovePin(Minimap.PinData pin) {
            if (!IsMinimapAvailable()) return;
            Minimap.instance.RemovePin(pin);
        }

        public static void RemovePin(Vector3 position) {
            if (TrackedComponents.RemoveWhere(
                component => component != null && component.transform != null &&
                             Vector2DDistance(component.transform.position, position) <
                             LocatorPlugin.Config.AutoPinDistance) == 0)
                Minimap.instance.RemovePin(position, 1f);
        }

        public static void ClearPins(string[] parameters) {
            if (!IsMinimapAvailable()) return;
            new List<Minimap.PinData>(MapPins)
                .FindAll(pin => pin.m_icon.name != "mapicon_start" && pin.m_icon.name != "mapicon_trader")
                .ForEach(RemovePin);
            TrackedComponents.Clear();
        }

        public static void AddTrackedPin(Component component, List<TrackedObject> trackedObjects) {
            if (!IsMinimapAvailable()) return;
            var trackedObject = Track(component, trackedObjects);
            if (trackedObject == null) return;
            AddPin(trackedObject.PinName, component.transform.position, GetPinType(component));
        }

        private static Minimap.PinType GetPinType(Component component) {
            switch (component) {
                default:
                    return Minimap.PinType.Icon3;
            }
        }

        private static TrackedObject GetTrackedObject(Component component, List<TrackedObject> trackedObjects) {
            return trackedObjects.FirstOrDefault(trackedObject => component.name.StartsWith(trackedObject.Name));
        }

        private static TrackedObject Track(Component component, List<TrackedObject> trackedObjects) {
            if (!IsMinimapAvailable()) return null;
            var trackedObject = GetTrackedObject(component, trackedObjects);
            if (trackedObject == null) return trackedObject;

            var pinExists = MapPins.FindAll(pin =>
                    pin.m_name == trackedObject.PinName &&
                    Vector2DDistance(pin.m_pos, component.transform.position) < LocatorPlugin.Config.AutoPinDistance)
                .Count == 0;

            if (TrackedComponents.Count == 0) {
                TrackedComponents.Add(component);
                return pinExists ? trackedObject : null;
            }

            var components = TrackedComponents.Where(t =>
                t != null && component != null &&
                Vector3.Distance(t.transform.position, component.transform.position) <
                LocatorPlugin.Config.AutoPinDistance);

            if (components.Count() != 0) return pinExists ? trackedObject : null;

            TrackedComponents.Add(component);
            return trackedObject;
        }

        private static float Vector2DDistance(Vector3 v1, Vector3 v2) {
            return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(v1.x - v2.x), 2f) + Mathf.Pow(Mathf.Abs(v1.z - v2.z), 2f));
        }

        public static void Update() {
            if (!IsMinimapAvailable()) return;
            if (!LocatorPlugin.Config.AutoPin) return;
            if (!Physics.Raycast(GameCamera.instance.transform.position, GameCamera.instance.transform.forward,
                out var hitInfo, LocatorPlugin.Config.AutoPinRayDistance, LocatorPlugin.CastMask)) return;

            foreach (var type in TrackedObjects.Keys) {
                var obj = hitInfo.collider.GetComponentInParent(type);
                if (obj != null) {
                    if (LocatorPlugin.Config.Debug)
                        Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft,
                            $"name={obj.name}, type={obj.GetType()}");
                    var configData = TrackedObjects.GetValueSafe(type);
                    if (configData != null && configData.Item1) {
                        AddTrackedPin(obj, configData.Item2);
                    }
                }
            }
        }

        public static void ClearTrackedComponents() {
            TrackedComponents.Clear();
        }
    }
}