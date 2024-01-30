using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.API;
using FivePD.API.Utils;
using Newtonsoft.Json.Linq;
using static CitizenFX.Core.Native.API;

#pragma warning disable 1998
namespace ForceDeptReg
{
    public class Main : Plugin
    {
        internal Main()
        {
            _ = Init();
        }

        internal async Task Init()
        {
            try
            {
                JObject _config = JObject.Parse(LoadResourceFile(GetCurrentResourceName(), "config/callouts.json"));
                var configSelect = _config["deptid"];
                bool goodConfig = int.TryParse((string)configSelect, out int configDep);
                int deparmentID = Utilities.GetPlayerData().DepartmentID;
                if (goodConfig)
                {
                    TriggerServerEvent("FivePD::Allowlist::IsPlayerAllowed", new Action<bool>(allowed =>
                    {
                        if (allowed)
                        {
                            PlayerData playerData = new PlayerData()
                            {
                                DepartmentID = configDep
                            };
                            if (deparmentID < 1)
                            {
                                Utilities.SetPlayerData(playerData, Utilities.SetPlayerDataFlags.DepartmentID);
                            };
                        }
                    }));
                }
                else
                {
                    Debug.WriteLine("[ ^5forcedept ^7]^1 Plugin diabled^7 - No forcedept config found or ^1 Invalid deparment id^7 - Please use numbers only!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\n[ ^5forcedept ^7] Could not read the config file");
                Debug.WriteLine($"\n{ex}");
            }
        }
    }
}
