using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
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
