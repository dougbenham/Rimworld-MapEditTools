using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;

namespace MapEditTools {
	public class Designator_PlaceStone : MapEditDesignator {

		public ThingDef StoneDef { get; protected set; }

		public Designator_PlaceStone() : base() {
			this.defaultLabel = "MapEditTools.DesignatorPlaceStone".Translate().Resolve();
			this.defaultDesc = "MapEditTools.DesignatorPlaceStoneDesc".Translate().Resolve();
            this.useMouseIcon = false;
            Texture2D rockAtlas = DuplicateTexture(ContentFinder<Texture2D>.Get("Things/Building/Linked/Rock_Atlas"));
            this.icon = new Texture2D(rockAtlas.width / 4, rockAtlas.height / 4, rockAtlas.format, false);
            this.icon.SetPixels(0, 0, this.icon.width, this.icon.height, rockAtlas.GetPixels(0, 0, rockAtlas.width / 4, rockAtlas.height / 4));
            this.icon.Apply();
			this.defaultIconColor = DefDatabase<ThingDef>.GetNamed("Granite").graphicData.color;
		}

		public override void DesignateSingleCell(IntVec3 c) {
            Thing thing = ThingMaker.MakeThing(this.StoneDef);
            GenSpawn.Spawn(thing, c, this.Map);
        }

		public override AcceptanceReport CanDesignateCell(IntVec3 loc) {
			return loc.InBounds(this.Map) && !this.Map.thingGrid.ThingsAt(loc).Any(t => t.def == StoneDef);
		}
		public override void ProcessInput(Event ev) {
            if (!base.CheckCanInteract())
                return;

            List<FloatMenuOption> list = new List<FloatMenuOption>();
            foreach (ThingDef def in from td in DefDatabase<ThingDef>.AllDefs
                                    where td.building?.isNaturalRock ?? false
                                    orderby td.building.isResourceRock, td.label
                                    select td) {
                ThingDef def2 = def;
                list.Add(new FloatMenuOption(def.label.CapitalizeFirst(), delegate () {
                    this.StoneDef = def2;
                    this.defaultIconColor = def2.graphicData?.color ?? this.defaultIconColor;
                    base.ProcessInput(ev);
                }, MenuOptionPriority.High, null, null, 0f, null, null));
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }

        private static Texture2D DuplicateTexture(Texture2D source) {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
    }
}
