using System;
using System.Collections.Generic;
using System.Linq;
using Purps.Valheim.Framework.Utils;
using UnityEngine;

namespace Purps.Valheim.Locator.Components.Utils {
    public static class WorldUtils {
        private static readonly List<Vector3> locationPoints = GenerateLocationPoints();

        private static List<ZoneSystem.LocationInstance> MapLocations => ZoneSystem.instance.GetLocationList().ToList();

        private static List<Vector3> GenerateLocationPoints() {
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
            if (!StatusUtils.IsPlayerLoaded()) return;
            foreach (var name in names)
                if (multiple)
                    locationPoints.ForEach(point => Locate(name, point, pinType));
                else
                    Locate(name, Player.m_localPlayer.transform.position, pinType);
        }

        private static void Locate(Tuple<string, string> name, Vector3 position, Minimap.PinType pinType) {
            Game.instance.DiscoverClosestLocation(name.Item1, position, name.Item2, (int) pinType);
        }

        public static void ListLocations(string[] parameters) {
            if (!StatusUtils.IsPlayerLoaded() || !StatusUtils.IsPlayerOffline()) return;
            var locations = MapLocations;

            if (parameters != null && parameters.Length > 0)
                locations = locations.FindAll(location =>
                    parameters.Any(location.m_location.m_prefabName.ToLower().Replace(' ', '-').Contains));

            locations.ForEach(location =>
                ConsoleUtils.WriteToConsole(location.m_location.m_prefabName, location.m_position.ToString()));
        }
    }
}