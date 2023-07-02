namespace Core.Location
{
    public static class OrbitalStationStaticBuilder
    {
        public static Event OnInit = new Event();
        
        public static void InitNames()
        {
            NamesHolder.Init();
        }

        public static void ClearEvent()
        {
            OnInit = new Event();
            WorldOrbitalStation.Instance?.Clear();
        }

        public static int CalcSeed(string stationName, string systemName)
        {
            int uniqSeed = NamesHolder.StringToSeed(stationName + systemName);
            uniqSeed *= uniqSeed;
            return uniqSeed;
        }
    }
}