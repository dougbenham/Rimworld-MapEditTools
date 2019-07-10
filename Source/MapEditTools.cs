using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace MapEditTools
{
    [StaticConstructorOnStartup]
    public static class MapEditTools
    {
        static MapEditTools()
        {
            Thing.allowDestroyNonDestroyable = true;
        }
    }
}
