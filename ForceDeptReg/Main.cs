using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.API;
using FivePD.API.Utils;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json.Linq;

#pragma warning disable 1998
namespace ForceDeptReg
{
    public class Main : Plugin
    {
        internal Main()
        {
            _ = Initialize();
            
        }

        internal async Task Initialize()
        {
            TriggerServerEvent("FivePD::Allowlist::IsPlayerAllowed", new Action<bool>(allowed =>
            {
                if (allowed)
                {
                    _ = DeptReg();
                }
            }));
        }

        internal async Task DeptReg()
        {
            try
            {
                JToken id = JObject.Parse(LoadResourceFile(GetCurrentResourceName(), "config/callouts.json"))["deptid"];
                bool check = int.TryParse((string)id, out int result);

                if (id != null)
                {
                    if (check == true)
                    {
                        int deparmentID = Utilities.GetPlayerData().DepartmentID;
                        PlayerData playerData = new PlayerData()
                        {
                            DepartmentID = result

                        };
                        if (deparmentID < 1)
                        {
                            Task task = Utilities.SetPlayerData(playerData, Utilities.SetPlayerDataFlags.DepartmentID);
                        };
                    }
                    else
                    {
                        Debug.WriteLine("[ ^5forcedept ^7]^1 Invalid deparment id^7 - Please use numbers only!");
                    }
                }
                else
                {
                    Debug.WriteLine("[ ^5forcedept ^7]^1 Plugin diabled^7 - No forcedept config found, please check the readme to configure!");
                    
                }
                }
            catch (Exception e)
            {
                Debug.WriteLine("[ ^5forcedept ^7] Could not read the config file");
                Debug.WriteLine(e.ToString());
            }

        }
    }
}
