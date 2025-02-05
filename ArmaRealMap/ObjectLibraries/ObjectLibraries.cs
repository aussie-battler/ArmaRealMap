﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ArmaRealMap.Core;
using ArmaRealMap.Core.ObjectLibraries;

namespace ArmaRealMap.Libraries
{
    public class ObjectLibraries
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters ={
                new JsonStringEnumConverter()
            },
            WriteIndented = true
        };

        private readonly List<ObjectLibrary> libraries = new List<ObjectLibrary>();
        private readonly TerrainRegion region;

        public ObjectLibraries(TerrainRegion region)
        {
            this.region = region;
        }

        public bool HasLibrary(ObjectCategory objectCategory)
        {
            return libraries.Any(l => l.Category == objectCategory);
        }

        public ObjectLibrary GetSingleLibrary(ObjectCategory objectCategory)
        {
            var libs = libraries.Where(l => l.Category == objectCategory);
            return libs.FirstOrDefault(l => l.Terrain == region) ??
                libs.FirstOrDefault(l => l.Terrain == null || l.Terrain == TerrainRegion.Unknown);
        }

        public List<ObjectLibrary> GetLibraries(ObjectCategory objectCategory)
        {
            var libs = libraries.Where(l => l.Category == objectCategory);
            var result = libs.Where(l => l.Terrain == region).ToList();
            if (result.Count > 0)
            {
                return result;
            }
            return libs.Where(l => l.Terrain == null || l.Terrain == TerrainRegion.Unknown).ToList();
        }

        public void Load(string filename)
        {
            var libs = JsonSerializer.Deserialize<JsonObjectLibrary[]>(File.ReadAllText(filename), options);

            foreach(var lib in libs)
            {
                if (lib.Terrain == null || lib.Terrain == TerrainRegion.Unknown || lib.Terrain == region)
                {
                    libraries.Add(new ObjectLibrary()
                    {
                        Category = lib.Category,
                        Density = lib.Density,
                        Terrain = lib.Terrain,
                        Objects = lib.Objects.Select(o => new SingleObjetInfos()
                        {
                            CX = o.CX,
                            CY = o.CY,
                            CZ = o.CZ,
                            Depth = o.Depth,
                            Height = o.Height,
                            MaxZ = o.MaxZ,
                            MinZ = o.MinZ,
                            Name = o.Name,
                            PlacementProbability = o.PlacementProbability,
                            PlacementRadius = o.PlacementRadius,
                            ReservedRadius = o.ReservedRadius,
                            Width = o.Width
                        }).ToList(),
                        Compositions = lib.Compositions?.Select(c => new CompositionInfos()
                        {
                            Height = c.Height,
                            Depth = c.Depth,
                            Width = c.Width,
                            Objects = c.Objects.Select(o => new CompositionObjetInfos()
                            {
                                Angle = o.Angle,
                                Width = o.Width,
                                Depth = o.Depth,
                                Height = o.Height,
                                Name = o.Name,
                                X = o.X,
                                Y = o.Y,
                                Z = o .Z
                            }).ToList()
                        })?.ToList()
                    }); 
                }
            }
        }

        internal SingleObjetInfos GetObject(string name)
        {
            return libraries.SelectMany(l => l.Objects).First(o => o.Name == name);
        }
    }
}
