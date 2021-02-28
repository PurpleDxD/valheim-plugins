using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Purps.Valheim.Locator.Patches;
using UnityEngine;

namespace Purps.Valheim.Locator.Utils {
    public static class WorldUtils {
        private static readonly List<Vector3> MapPoints = generateMapPoints();

        private static List<Minimap.PinData> MapPins =>
            (List<Minimap.PinData>) Traverse.Create(Minimap.instance).Field("m_pins").GetValue();

        private static List<ZoneSystem.LocationInstance> MapLocations => ZoneSystem.instance.GetLocationList().ToList();

        private static List<Vector3> generateMapPoints() {
            const int radius = 10000;
            const int cycle = 360;
            const int pointCount = 6;

            var mapPoints = new List<Vector3>();
            for (var r = radius / pointCount; r <= radius; r += radius / pointCount * 2)
            for (var d = 0.0; d <= cycle; d += cycle / pointCount) {
                var x = (float) (r * Math.Cos(d * (Math.PI / 180)));
                var y = (float) (r * Math.Sin(d * (Math.PI / 180)));
                mapPoints.Add(new Vector3(x, 0, y));
            }

            return mapPoints;
        }

        public static void Locate(Minimap.PinType pinType, List<Tuple<string, string>> names, bool multiple) {
            if (!StatusUtils.isPlayerLoaded()) return;
            foreach (var name in names)
                if (multiple)
                    MapPoints.ForEach(p => Locate(name, p, pinType));
                else
                    Locate(name, Player.m_localPlayer.transform.position, pinType);
        }

        private static void Locate(Tuple<string, string> name, Vector3 position, Minimap.PinType pinType) {
            Game.instance.DiscoverClosestLocation(name.Item1, position, name.Item2, (int) pinType);
        }

        public static void ListLocations(string[] parameters) {
            if (!StatusUtils.isPlayerLoaded() || !StatusUtils.isPlayerOffline()) return;
            var locations = MapLocations;
            if (parameters != null && parameters.Length > 0)
                locations = locations.FindAll(location =>
                    parameters.Contains(location.m_location.m_prefabName.Replace(' ', '-')));

            locations.ForEach(location =>
                ConsoleUtils.WriteToConsole(location.m_location.m_prefabName, location.m_position.ToString()));
        }

        public static void ListPins(string[] parameters) {
            if (!StatusUtils.isPlayerLoaded()) return;
            var pins = MapPins;
            if (parameters != null && parameters.Length > 0)
                pins = pins.FindAll(pin => parameters.Contains(pin.m_name.Replace(' ', '_')));

            pins.ForEach(pin =>
                ConsoleUtils.WriteToConsole(pin.m_name, pin.m_pos.ToString(), pin.m_icon.name));
        }
    }
}