using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Utils;
using Purps.Valheim.Locator.Components.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Purps.Valheim.Locator.Components.Utils {
    public static class MinimapUtils {
        private static readonly HashSet<Component> TrackedComponents = new HashSet<Component>();

        private static Dictionary<Type, Tuple<bool, List<TrackedObject>>> TrackedTypes =>
            new Dictionary<Type, Tuple<bool, List<TrackedObject>>> {
                {
                    typeof(Destructible),
                    Tuple.Create(BasePlugin.GetConfigData<bool>("pinDestructibles").value,
                        LocatorPlugin.Config.DestructibleInclusions)
                }, {
                    typeof(MineRock),
                    Tuple.Create(BasePlugin.GetConfigData<bool>("pinMineRocks").value,
                        LocatorPlugin.Config.MineRockInclusions)
                }, {
                    typeof(Location),
                    Tuple.Create(BasePlugin.GetConfigData<bool>("pinLocations").value,
                        LocatorPlugin.Config.LocationInclusions)
                }, {
                    typeof(Pickable),
                    Tuple.Create(BasePlugin.GetConfigData<bool>("pinPickables").value,
                        LocatorPlugin.Config.PickableInclusions)
                }, {
                    typeof(SpawnArea),
                    Tuple.Create(BasePlugin.GetConfigData<bool>("pinSpawners").value,
                        LocatorPlugin.Config.SpawnerInclusions)
                }, {
                    typeof(Vegvisir),
                    Tuple.Create(BasePlugin.GetConfigData<bool>("pinVegvisirs").value,
                        LocatorPlugin.Config.VegvisirInclusions)
                }, {
                    typeof(Leviathan),
                    Tuple.Create(BasePlugin.GetConfigData<bool>("pinLeviathans").value,
                        LocatorPlugin.Config.LeviathanInclusions)
                }
            };

        public static void SetPinFilters(string[] parameters) =>
            Minimap.instance.GetComponent<CustomMinimapData>().PinFilters = parameters;

        public static string[] GetPinFilters() => Minimap.instance.GetComponent<CustomMinimapData>().PinFilters;


        public static List<Minimap.PinData> MapPins =>
            Traverse.Create(Minimap.instance)?.Field("m_pins").GetValue<List<Minimap.PinData>>() ??
            new List<Minimap.PinData>();

        public static bool IsMinimapAvailable() => Minimap.instance != null;

        public static void Update() {
            if (!IsMinimapAvailable()) return;
            if (!BasePlugin.GetConfigData<bool>("pinEnabled").value) return;
            if (!Physics.Raycast(GameCamera.instance.transform.position, GameCamera.instance.transform.forward,
                out var hitInfo, BasePlugin.GetConfigData<float>("pinRayDistance").value,
                LocatorPlugin.CastMask)) return;

            foreach (var type in TrackedTypes.Keys) {
                var obj = hitInfo.collider.GetComponentInParent(type);
                if (obj != null) {
                    if (BasePlugin.GetConfigData<bool>("debug").value)
                        LocatorPlugin.DebugText = $"name={obj.name}, type={obj.GetType()}";
                    var configData = TrackedTypes.GetValueSafe(type);
                    if (configData != null && configData.Item1) AddTrackedPin(obj, configData.Item2);
                }
            }
        }

        public static void ListPins(string[] parameters) {
            if (!StatusUtils.IsPlayerLoaded()) return;
            if (!IsMinimapAvailable()) return;
            var pins = MapPins;
            if (parameters != null && parameters.Length > 0)
                pins = pins.FindAll(pin => parameters.Contains(pin.m_name.ToLower().Replace(' ', '_')));
            
            pins.ForEach(pin =>
                ConsoleUtils.WriteToConsole(pin.m_name, pin.m_pos.ToString(),
                    $"({pin.m_type.ToString()}) {pin.m_icon.name}"));
        }

        public static void AddPin(string name, Vector3 position, Minimap.PinType type) =>
            Minimap.instance.AddPin(position, type, name, true, false);

        public static void RemovePin(Minimap.PinData pin) {
            if (!IsMinimapAvailable()) return;
            Minimap.instance.RemovePin(pin);
        }

        public static void RemovePin(Vector3 position) {
            if (TrackedComponents.RemoveWhere(
                component => component != null && component.transform != null &&
                             Vector2DDistance(component.transform.position, position) <
                             BasePlugin.GetConfigData<float>("pinDistance").value) == 0)
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

        private static TrackedObject GetTrackedObject(Component component, List<TrackedObject> typeTrackedObjects) {
            var trackedObject = typeTrackedObjects
                .FindAll(typeTrackedObject => component.name.Contains(typeTrackedObject.Name))
                .OrderBy(typeTrackedObject => TrackedObject.QueryOrder(typeTrackedObject.Name, component.name))
                .FirstOrDefault();
            if (trackedObject == null || !trackedObject.ShouldTrack) return null;
            return trackedObject;
        }

        private static TrackedObject Track(Component component, List<TrackedObject> trackedObjects) {
            if (!IsMinimapAvailable()) return null;
            var trackedObject = GetTrackedObject(component, trackedObjects);
            if (trackedObject == null) return trackedObject;

            var pinExists = MapPins.FindAll(pin =>
                    pin.m_name == trackedObject.PinName &&
                    Vector2DDistance(pin.m_pos, component.transform.position) <
                    BasePlugin.GetConfigData<float>("pinDistance").value)
                .Count == 0;

            if (TrackedComponents.Count == 0) {
                TrackedComponents.Add(component);
                return pinExists ? trackedObject : null;
            }

            var components = TrackedComponents.Where(t =>
                t != null && component != null &&
                Vector3.Distance(t.transform.position, component.transform.position) <
                BasePlugin.GetConfigData<float>("pinDistance").value);

            if (components.Count() != 0) return pinExists ? trackedObject : null;

            TrackedComponents.Add(component);
            return trackedObject;
        }

        private static float Vector2DDistance(Vector3 v1, Vector3 v2) =>
            Mathf.Sqrt(Mathf.Pow(Mathf.Abs(v1.x - v2.x), 2f) + Mathf.Pow(Mathf.Abs(v1.z - v2.z), 2f));

        public static void OnDestroy() {
            Object.Destroy(Minimap.instance?.gameObject.GetComponent<CustomMinimapData>());
            TrackedComponents?.Clear();
        }

        public static void OnAwake() {
            Minimap.instance.gameObject.AddComponent<CustomMinimapData>();
        }

        private static bool ShouldPinRender(Minimap.PinData pin) {
            var pinFilters = GetPinFilters();
            if (pinFilters == null || pinFilters.Length == 0) return true;
            return pinFilters.Count(filter => pin.m_name.ToLower().Replace(' ', '_').Contains(filter)) != 0;
        }

        public static void FilterPins() =>
            MapPins.ForEach(pin => pin.m_uiElement?.gameObject.SetActive(ShouldPinRender(pin)));
    }
}