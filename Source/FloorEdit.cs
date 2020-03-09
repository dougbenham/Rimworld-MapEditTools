using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MapEditTools
{
    public class Designator_SetTerrain : MapEditDesignator
    {
        public TerrainDef TerrainDef { get; protected set; }

        public Designator_SetTerrain() : base()
        {
            this.defaultLabel = "MapEditTools.DesignatorSetTerrain".Translate().Resolve();
            this.defaultDesc = "MapEditTools.DesignatorSetTerrainDesc".Translate().Resolve();
        }

        public override void DesignateSingleCell(IntVec3 c)
        {
            this.Map.terrainGrid.SetTerrain(c, TerrainDef);
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 loc)
        {
            return Map.terrainGrid.TerrainAt(loc) != TerrainDef;
        }

        public override void ProcessInput(Event ev)
        {
            if (!base.CheckCanInteract())
                return;
            List<FloatMenuOption> list = new List<FloatMenuOption>();
            foreach (TerrainDef def in from rd in DefDatabase<TerrainDef>.AllDefs
                                    orderby rd.label
                                    select rd)
            {
                TerrainDef def2 = def;
                list.Add(new FloatMenuOption(def.label.CapitalizeFirst(), delegate () {
                    this.TerrainDef = def2;
                    base.ProcessInput(ev);
                }, MenuOptionPriority.High, null, null, 0f, null, null));
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }

    }
}