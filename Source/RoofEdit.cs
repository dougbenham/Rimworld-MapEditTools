using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MapEditTools
{
    public abstract class Designator_SetRoof : MapEditDesignator
    {
        public RoofDef RoofDef { get; protected set; }

        public Designator_SetRoof() : base() { }
        /*
        public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
        {
            base.DesignateMultiCell(cells);
            RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(cells.ToList(), this.Map);
        }
        */

        public override void DesignateSingleCell(IntVec3 c)
        {
            Map.roofGrid.SetRoof(c, RoofDef);
        }
    }
    public class Designator_BuildRoof : Designator_SetRoof
    {
        public Designator_BuildRoof() : base()
        {
            this.defaultLabel = "MapEditTools.DesignatorBuildRoof".Translate().Resolve();
            this.defaultDesc = "MapEditTools.DesignatorBuildRoofDesc".Translate().Resolve();
            this.icon = ContentFinder<Texture2D>.Get("UI/Designators/BuildRoofArea");
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 loc)
        {
            return loc.InBounds(this.Map) && this.Map.roofGrid.RoofAt(loc) != this.RoofDef;
        }

        public override void ProcessInput(Event ev)
        {
            if (!base.CheckCanInteract())
                return;
            List<FloatMenuOption> list = new List<FloatMenuOption>();
            foreach (RoofDef def in from rd in DefDatabase<RoofDef>.AllDefs
                                    orderby rd.label
                                    select rd)
            {
                RoofDef def2 = def;
                list.Add(new FloatMenuOption(def.label.CapitalizeFirst(), delegate () {
                    this.RoofDef = def2;
                    base.ProcessInput(ev);
                }, MenuOptionPriority.High, null, null, 0f, null, null));
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }
    }

    public class Designator_RemoveRoof : Designator_SetRoof
    {
        public Designator_RemoveRoof() : base()
        {
            this.defaultLabel = "MapEditTools.DesignatorRemoveRoof".Translate().Resolve();
            this.defaultDesc = "MapEditTools.DesignatorRemoveRoofDesc".Translate().Resolve();
            this.icon = ContentFinder<Texture2D>.Get("UI/Designators/NoRoofArea");
            this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 loc)
        {
            return loc.InBounds(this.Map) && this.Map.roofGrid.Roofed(loc);
        }
    }
}
