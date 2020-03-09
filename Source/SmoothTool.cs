using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MapEditTools
{
    public class Designator_SmoothSurface : RimWorld.Designator_SmoothSurface
    {
        public Designator_SmoothSurface() : base() { }

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            AcceptanceReport report = base.CanDesignateCell(c);
            if (report.Accepted || report.Reason == "SurfaceBeingSmoothed".Translate().Resolve() || report.Reason == "TooCloseToMapEdge".Translate().Resolve())
                return true;
            return report;
        }

        public override void DesignateSingleCell(IntVec3 c)
        {
            Building edifice = c.GetEdifice(base.Map);
            if (edifice != null && edifice.def.IsSmoothable)
            {
                Faction faction = edifice.Faction;
                edifice.Destroy(DestroyMode.WillReplace);
                Thing thing = ThingMaker.MakeThing(edifice.def.building.smoothedThing, edifice.Stuff);
                thing.SetFaction(faction, null);
                GenSpawn.Spawn(thing, edifice.Position, Map, edifice.Rotation, WipeMode.Vanish, false);
                Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.SmoothWall);
            } else
            {
                this.Map.terrainGrid.SetTerrain(c, this.Map.terrainGrid.TerrainAt(c).smoothedTerrain);
                Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.SmoothFloor);
            }
        }
    }
}
