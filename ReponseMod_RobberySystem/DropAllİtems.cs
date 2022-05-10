using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ReponseMod_RobberySystem
{
    public class DropAllİtems
    {
        private static byte invPages;

        public static void DropAllItems(UnturnedPlayer pl, UnturnedPlayer unturnedplayer, string Yazı1, string Yazı2, string Logo)
        {
            for (byte page = 0; page < invPages; page++)
            {
                byte itemCount = pl.Player.inventory.getItemCount(page);
                ChatManager.serverSendMessage(Yazı1, Color.white, null, unturnedplayer.SteamPlayer(), (EChatMode)4, Logo, true);
                ChatManager.serverSendMessage(Yazı2, Color.white, null, pl.SteamPlayer(), (EChatMode)4, Logo, true);

                for (byte index = 0; index < itemCount; index++)
                {
                    byte posX = pl.Player.inventory.getItem(page, index).x;
                    byte posY = pl.Player.inventory.getItem(page, index).y;
                    try
                    {
                        pl.Player.inventory.askDropItem(pl.CSteamID, page, posX, posY);
                        index--;
                        itemCount--;

                    }
                    catch (Exception e)
                    {
                        ChatManager.serverSendMessage($"<color=red>ERROR |</color> Operation Failed!", Color.white, null, pl.SteamPlayer(), (EChatMode)4, Logo, true);

                    }
                }
            }
        }

    }
}
