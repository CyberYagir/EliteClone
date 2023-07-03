namespace Core.Location
{
    public static class OrbitalStationStaticBuilder
    {
        public static void InitNames()
        {
            NamesHolder.Init();
        }
        
        public static int CalcSeed(string stationName, string systemName)
        {
            int uniqSeed = NamesHolder.StringToSeed(stationName + systemName);
            uniqSeed *= uniqSeed;
            return uniqSeed;
        }
    }
}