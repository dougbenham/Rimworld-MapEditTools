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
