using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Purps.Valheim.Locator.Patches;
using UnityEngine;

namespace Purps.Valheim.Locator.Utils {
    public static class MinimapUtils {
        private static readonly List<Tuple<string, string>> TrackedObjects = new List<Tuple<string, string>> {
            // Destructibles
            Tuple.Create("BlueberryBush", "Blueberry"),
            Tuple.Create("CloudberryBush", "Cloudberry"),
            Tuple.Create("RaspberryBush", "Raspberry"),
            Tuple.Create("MineRock_Tin", "Tin"),
            Tuple.Create("rock4_copper", "Copper"),
            Tuple.Create("MineRock_Obsidian", "Obsidian"),

            // Pickables
            Tuple.Create("Pickable_Thistle", "Thistle"),
            Tuple.Create("Pickable_Mushroom", "Mushroom"),
            Tuple.Create("Pickable_SeedCarrot", "Carrot"),
            Tuple.Create("Pickable_Dandelion", "Dandelion"),
            Tuple.Create("Pickable_SeedTurnip", "Turnip"),

            // Leviathans
            Tuple.Create("Leviathan", "Leviathan"),

            // Locations
            Tuple.Create("TrollCave", "Troll"),
            Tuple.Create("Crypt", "Crypt"),
            Tuple.Create("SunkenCrypt", "Crypt"),
            Tuple.Create("Grave", "Grave"),
            Tuple.Create("DrakeNest", "Egg"),
            Tuple.Create("Runestone", "Runestone"),
            Tuple.Create("Eikthyrnir", "Eikthyr"),
            Tuple.Create("GDKing", "The Elder"),
            Tuple.Create("Bonemass", "Bonemass"),
            Tuple.Create("Dragonqueen", "Moder"),
            Tuple.Create("GoblinKing", "Yagluth"),

            // Spawners
            Tuple.Create("Spawner", "Spawner")
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
                             Config.AutoPinDistance) == 0)
                Minimap.instance.RemovePin(position, 1f);
        }

        public static void ClearPins(string[] parameters) {
            if (!IsMinimapAvailable()) return;
            new List<Minimap.PinData>(MapPins)
                .FindAll(pin => pin.m_icon.name != "mapicon_start" && pin.m_icon.name != "mapicon_trader")
                .ForEach(RemovePin);
            TrackedComponents.Clear();
        }

        public static void AddTrackedPin(Component component) {
            if (!IsMinimapAvailable()) return;
            var trackedObject = Track(component);
            if (trackedObject == null) return;
            AddPin(trackedObject.Item2, component.transform.position, GetPinType(component));
        }

        private static Minimap.PinType GetPinType(Component component) {
            switch (component) {
                case Destructible destructible:
                    return Minimap.PinType.Icon3;
                case Location location:
                    return Minimap.PinType.Icon3;
                default:
                    return Minimap.PinType.Icon3;
            }
        }

        private static Tuple<string, string> GetTrackedObject(Component component) {
            return TrackedObjects.FirstOrDefault(name => component.name.StartsWith(name.Item1));
        }

        private static Tuple<string, string> Track(Component component) {
            if (!IsMinimapAvailable()) return null;

            var trackedObject = GetTrackedObject(component);
            if (trackedObject == null) return trackedObject;

            var pinExists = MapPins.FindAll(pin =>
                    pin.m_name == trackedObject.Item2 &&
                    Vector2DDistance(pin.m_pos, component.transform.position) < Config.AutoPinDistance)
                .Count == 0;

            if (TrackedComponents.Count == 0) {
                TrackedComponents.Add(component);
                return pinExists ? trackedObject : null;
            }

            var components = TrackedComponents.Where(t =>
                t != null && component != null &&
                Vector3.Distance(t.transform.position, component.transform.position) < Config.AutoPinDistance);

            if (components.Count() != 0) return pinExists ? trackedObject : null;

            TrackedComponents.Add(component);
            return trackedObject;
        }

        private static float Vector2DDistance(Vector3 v1, Vector3 v2) {
            return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(v1.x - v2.x), 2f) + Mathf.Pow(Mathf.Abs(v1.z - v2.z), 2f));
        }

        public static void Update() {
            if (!IsMinimapAvailable()) return;
            if (!Config.AutoPin) return;

            if (!Physics.Raycast(GameCamera.instance.transform.position, GameCamera.instance.transform.forward,
                out var hitInfo, 25f, Plugin.CastMask)) return;

            if (Config.AutoPinSpawners) {
                var spawnArea = hitInfo.collider.GetComponentInParent<SpawnArea>();
                if (spawnArea != null) {
                    Debug.Log(spawnArea);
                    AddTrackedPin(spawnArea);
                }
            }

            if (Config.AutoPinLocations) {
                var location = hitInfo.collider.GetComponentInParent<Location>();
                if (location != null) AddTrackedPin(location);
            }

            if (Config.AutoPinDestructibles) {
                var destructible = hitInfo.collider.GetComponentInParent<Destructible>();
                if (destructible != null) AddTrackedPin(destructible);
            }

            if (Config.AutoPinPickables) {
                var pickable = hitInfo.collider.GetComponentInParent<Pickable>();
                if (pickable != null) AddTrackedPin(pickable);
            }

            if (!Config.AutoPinLeviathans) return;
            var leviathan = hitInfo.collider.GetComponentInParent<Leviathan>();
            if (leviathan != null) AddTrackedPin(leviathan);
        }

        public static void ClearTrackedComponents() {
            TrackedComponents.Clear();
        }
    }
}