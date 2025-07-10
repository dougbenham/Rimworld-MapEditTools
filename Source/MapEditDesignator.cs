using System.Collections.Generic;
using Verse;
using RimWorld;

namespace MapEditTools
{
    public abstract class MapEditDesignator : Designator
    {
        public MapEditDesignator()
        {
            this.soundDragSustain = SoundDefOf.Designate_DragStandard;
            this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
            this.soundSucceeded = SoundDefOf.Designate_PlaceBuilding;
            this.useMouseIcon = true;
        }

        public override DrawStyleCategoryDef DrawStyleCategory => DrawStyleCategoryDefOf.Orders;

        public override bool DragDrawMeasurements => true;

        public override void SelectedUpdate()
        {
            GenUI.RenderMouseoverBracket();
        }

        public override void RenderHighlight(List<IntVec3> dragCells)
        {
            DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
        }
    }
}
