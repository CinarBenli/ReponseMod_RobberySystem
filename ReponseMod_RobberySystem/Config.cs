using Rocket.API;
using System.Collections.Generic;

namespace ReponseMod_RobberySystem
{
    public class Config : IRocketPluginConfiguration
    {
        public string ServerLogo;
        public int ForcibTime;
        public List<ushort> GunID = new List<ushort>();

        public void LoadDefaults()
        {
            ServerLogo = "HTTP";
            ForcibTime = 10;
            GunID = new List<ushort>()
            {
                116,
                363
            };
        }
    }
}