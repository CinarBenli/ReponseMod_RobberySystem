using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ReponseMod_RobberySystem
{
    public class Class1 : RocketPlugin<Config>
    {
        public List<CSteamID> Kelepçeliler = new List<CSteamID>();
        public void Chat(string Yazı, SteamPlayer Kullanıcı)
        {
            ChatManager.serverSendMessage(Yazı, Color.white, null, Kullanıcı, (EChatMode)4, Configuration.Instance.ServerLogo, true);

        }
        protected override void Load()
        {

            base.Load();
        }



        [RocketCommand("forcib", "You Clamp the User for the Seconds You Specify", " /forcib", AllowedCaller.Both)]
        [RocketCommandPermission("r.forcib")]
        public void forcib(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = caller as UnturnedPlayer;



            if (Physics.Raycast(unturnedPlayer.Player.look.aim.position, unturnedPlayer.Player.look.aim.forward, out RaycastHit hit, 10, RayMasks.PLAYER) && hit.transform.TryGetComponent(out Player pls))
            {
                UnturnedPlayer pl = UnturnedPlayer.FromPlayer(pls);
                if (pl != null)
                {
                    Chat(DefaultTranslations.Translate("WarningNullUser"), unturnedPlayer.SteamPlayer());
                }
                else
                {
                    foreach (var Guns in Configuration.Instance.GunID)
                    {
                        if (unturnedPlayer.Player.equipment.asset.id != Guns)
                        {
                            Chat(DefaultTranslations.Translate("WarningGunError"), unturnedPlayer.SteamPlayer());

                        }
                        else
                        {
                            Kelepçeliler.Add(unturnedPlayer.CSteamID);
                            unturnedPlayer.Player.movement.sendPluginSpeedMultiplier(0.1f);
                            unturnedPlayer.Player.movement.sendPluginJumpMultiplier(0.1f);
                            unturnedPlayer.Player.equipment.dequip();
                            unturnedPlayer.Player.stance.stance = EPlayerStance.PRONE;
                            unturnedPlayer.Player.stance.checkStance(EPlayerStance.PRONE);
                            if (unturnedPlayer.IsInVehicle)
                            {
                                VehicleManager.forceRemovePlayer(unturnedPlayer.CSteamID);
                            }
                            StartCoroutine(Starts(pl));
                            StartCoroutine(Ends(pl));

                        }
                    }

                }
            }

        }
        private IEnumerator<WaitForSeconds> Starts(UnturnedPlayer Suclu)
        {
            while (Kelepçeliler.Contains(Suclu.CSteamID))
            {
                Suclu.Player.equipment.dequip();
                Suclu.Player.animator.sendGesture(EPlayerGesture.ARREST_START, true);
                if (Suclu.IsInVehicle)
                {
                    VehicleManager.forceRemovePlayer(Suclu.CSteamID);
                }
                yield return new WaitForSeconds(0.4f);
            }
        }
        private IEnumerator<WaitForSeconds> Ends(UnturnedPlayer Suclu)
        {
            yield return new WaitForSeconds(Configuration.Instance.ForcibTime);
            Kelepçeliler.Remove(Suclu.CSteamID);
            Suclu.Player.movement.sendPluginSpeedMultiplier(1f);
            Suclu.Player.movement.sendPluginJumpMultiplier(1f);
            Suclu.Player.animator.sendGesture(EPlayerGesture.ARREST_STOP, true);
        }

        [RocketCommand("search", "Searching for the Opposite User", " /search", AllowedCaller.Both)]
        [RocketCommandPermission("r.search")]
        public void search(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = caller as UnturnedPlayer;


            if (Physics.Raycast(unturnedPlayer.Player.look.aim.position, unturnedPlayer.Player.look.aim.forward, out RaycastHit hit, 10, RayMasks.PLAYER) && hit.transform.TryGetComponent(out Player pls))
            {
                UnturnedPlayer pl = UnturnedPlayer.FromPlayer(pls);
                if (pl.Player.animator.gesture == EPlayerGesture.ARREST_START)
                {

                    DropAllİtems.DropAllItems(pl, unturnedPlayer, DefaultTranslations.Translate("SuccessSearch"), DefaultTranslations.Translate("Wanted"), Configuration.Instance.ServerLogo);
                }
                else
                {
                    Chat(DefaultTranslations.Translate("WarningClamp"), unturnedPlayer.SteamPlayer());
                }
            }
            return;
        }

        [RocketCommand("Rob", "You Rob the Player Opposite", " /rob", AllowedCaller.Both)]
        [RocketCommandPermission("r.rob")]
        public void Rob(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = caller as UnturnedPlayer;


            if (Physics.Raycast(unturnedPlayer.Player.look.aim.position, unturnedPlayer.Player.look.aim.forward, out RaycastHit hit, 10, RayMasks.PLAYER) && hit.transform.TryGetComponent(out Player pls))
            {
                UnturnedPlayer pl = UnturnedPlayer.FromPlayer(pls);
                if (pl.Player.animator.gesture == EPlayerGesture.ARREST_START)
                {

                    DropAllİtems.DropAllItems(pl, unturnedPlayer, DefaultTranslations.Translate("SuccessRob"), DefaultTranslations.Translate("Robbed"), Configuration.Instance.ServerLogo);
                }
                else
                {
                    Chat(DefaultTranslations.Translate("WarningClamp"), unturnedPlayer.Player.channel.owner);
                }
            }
            return;

        }




        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {"WarningNullUser","<color=red>WARNİNG |</color> There Is No User Against You"},
            {"WarningGunError","<color=red>WARNİNG |</color> Must Have Weapon Designated for Coercion"},
            {"SuccessSearch"  ,"<color=green>SUCCES |</color> Successfully Searched"},
            {"Wanted"         ,"<color=green>CAUTİON |</color> Superior Wanted"},
            {"SuccessRob"  ,"<color=green>SUCCES |</color> You Successfully Robbed User!"},
            {"Robbed"         ,"<color=green>CAUTİON |</color> You've been robbed!"},
            {"WarningClamp"   ,"<color=green>WARNİNG |</color> Opposite Player Must Be Handcuffed"}


        };

        protected override void Unload()
        {
            base.Unload();
        }
    }
}
