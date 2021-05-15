﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OsmSharp;
using OsmSharp.Complete;
using OsmSharp.Db;
using OsmSharp.Geo;
using OsmSharp.Streams;
using OsmSharp.Tags;

namespace ArmaRealMap.Osm
{
    internal static class OsmCategorizer
    {

        internal static List<OsmShape> GetShapes(SnapshotDb db, OsmStreamSource filtered)
        {
            Console.WriteLine("Filter OSM data...");
            var toRender = new List<OsmShape>();
            var interpret = new DefaultFeatureInterpreter2();
            var list = filtered.Where(osmGeo =>
            (osmGeo.Type == OsmSharp.OsmGeoType.Way || osmGeo.Type == OsmSharp.OsmGeoType.Relation)
            && osmGeo.Tags != null).ToList();
            var report = new ProgressReport("GetShapes", list.Count);
            foreach (OsmGeo osmGeo in list)
            {
                var category = GetCategory(osmGeo.Tags, interpret);
                if (category != null)
                {
                    var complete = osmGeo.CreateComplete(db);
                    var count = 0;
                    foreach (var feature in interpret.Interpret(complete))
                    {
                        toRender.Add(new OsmShape(category, osmGeo, feature.Geometry));
                        count++;
                    }
                    if (count == 0)
                    {
                        Trace.TraceWarning($"NO GEOMETRY FOR {osmGeo.Tags}");
                    }
                }
                report.ReportOneDone();
            }
            report.TaskDone();
            return toRender;
        }

        private static OsmShapeCategory GetCategory(TagsCollectionBase tags, FeatureInterpreter interpreter)
        {
            if (tags.ContainsKey("water") || (tags.ContainsKey("waterway") && !tags.IsFalse("waterway")))
            {
                if (Get(tags, "water") == "lake")
                {
                    return OsmShapeCategory.Lake;
                }
                return OsmShapeCategory.Water;
            }
            if (tags.ContainsKey("building") && !tags.IsFalse("building"))
            {
                switch (Get(tags, "building"))
                {
                    case "church":
                        return OsmShapeCategory.BuildingChurch;
                }
                if (Get(tags, "historic") == "fort")
                {
                    return OsmShapeCategory.BuildingHistoricalFort;
                }
                if (tags.ContainsKey("brand"))
                {
                    return OsmShapeCategory.BuildingRetail;
                }
                return OsmShapeCategory.Building;
            }

            if (Get(tags, "type") == "boundary")
            {
                return null;
            }

            switch (Get(tags, "surface"))
            {
                case "grass": return OsmShapeCategory.Grass;
                case "sand": return OsmShapeCategory.Sand;
                case "concrete": return OsmShapeCategory.Concrete;
            }



            switch (Get(tags, "landuse"))
            {
                case "forest": return OsmShapeCategory.Forest;
                case "grass": return OsmShapeCategory.Grass;
                case "village_green": return OsmShapeCategory.Grass;
                case "farmland": return OsmShapeCategory.FarmLand;
                case "farmyard": return OsmShapeCategory.FarmLand;
                case "vineyard": return OsmShapeCategory.FarmLand;
                case "orchard": return OsmShapeCategory.FarmLand;
                case "meadow": return OsmShapeCategory.FarmLand;
                case "industrial": return OsmShapeCategory.Industrial;
                case "residential": return OsmShapeCategory.Residential;
                case "cemetery": return OsmShapeCategory.Grass;
                case "railway": return OsmShapeCategory.Dirt;
                case "retail": return OsmShapeCategory.Retail;
                case "basin": return OsmShapeCategory.Water;
                case "reservoir": return OsmShapeCategory.Lake;
                case "allotments": return OsmShapeCategory.Grass;
                case "military": return OsmShapeCategory.Military;
            }

            switch (Get(tags, "natural"))
            {
                case "wood": return OsmShapeCategory.Forest;
                case "water": return OsmShapeCategory.Lake;
                case "grass": return OsmShapeCategory.Grass;
                case "heath": return OsmShapeCategory.Grass;
                case "meadow": return OsmShapeCategory.Grass;
                case "grassland": return OsmShapeCategory.Grass;
                case "scrub": return OsmShapeCategory.Grass;
                case "wetland": return OsmShapeCategory.WetLand;
                case "tree_row": return OsmShapeCategory.Forest;
                case "scree": return OsmShapeCategory.Sand;
                case "sand": return OsmShapeCategory.Sand;
                case "beach": return OsmShapeCategory.Sand;
            }

            if (Get(tags, "leisure") == "garden")
            {
                return OsmShapeCategory.Grass;
            }

            var road = ToRoadType(Get(tags, "highway"));
            if (road != null && road.Value < RoadType.SingleLaneDirtRoad)
            {
                return OsmShapeCategory.Road;
            }

            if (interpreter.IsPotentiallyArea(tags))
            {
                tags.RemoveKey("source");
                tags.RemoveKey("name");
                tags.RemoveKey("alt_name");
                Trace.WriteLine(tags);
                //Console.WriteLine(tags);
            }
            return null;
        }

        internal static RoadType? ToRoadType(string highway)
        {
            switch (highway)
            {
                case "motorway":
                    return RoadType.TwoLanesMotorway;
                case "trunk":
                case "primary":
                case "primary_link":
                case "trunk_link":
                case "motorway_link":
                    return RoadType.TwoLanesPrimaryRoad;
                case "secondary":
                case "tertiary":
                case "seconday_link":
                case "tertiary_link":
                case "unclassified":
                case "road":
                    return RoadType.TwoLanesSecondaryRoad;
                case "living_street":
                case "residential":
                case "pedestrian":
                    return RoadType.TwoLanesConcreteRoad;
                case "footway":
                    return RoadType.SingleLaneDirtRoad;
                case "path":
                    return RoadType.Trail;
                case "track":
                    return RoadType.SingleLaneDirtPath;
            }
            Trace.WriteLine($"Unknown highway='{highway}'");
            return null;
        }

        internal static string Get(TagsCollectionBase tags, string key)
        {
            string value;
            if (tags != null && tags.TryGetValue(key, out value))
            {
                return value;
            }
            return null;
        }
    }
}
