using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TutorialMod.Common.Systems
{
    public class BossDownedSystem : ModSystem
    {
        public static bool downedTutorialBoss = false;

        public override void ClearWorld()
        {
            downedTutorialBoss = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["downedTutorialBoss"] = downedTutorialBoss;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedTutorialBoss = tag.GetBool("downedTutorialBoss");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = downedTutorialBoss;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedTutorialBoss = flags[0];
        }
    }
}
