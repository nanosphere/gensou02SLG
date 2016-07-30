using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using game.db;

namespace game.ai
{
    public class Computer
    {

        public void noon_ack(Player myp)
        {
            var dat = net.CreateStoryCode.NoonNo(myp.id);
            new story.game2.Noon().run(dat);

        }
        public void noon_item(Player myp,Player opp)
        {
            var dat = net.CreateStoryCode.NoonItem(myp.id, 0);
            new story.game2.Noon().run(dat);

        }
        public void noon_end(Player myp)
        {
            var dat = net.CreateStoryCode.NoonEnd(myp.id);
            new story.game2.Noon().run(dat);
        }

        public void night_ack(Player myp)
        {
            int rand = MyRandom.rand(0, 1);
            net.NetworkData dat;
            if (rand == 1)
            {
                dat = net.CreateStoryCode.NightYes(myp.id);
            }
            else
            {
                dat = net.CreateStoryCode.NightNo(myp.id);
            }
            new story.game2.Night().run(dat);
        }
        public void night_vote(Player myp)
        {
            List<int> koho = new List<int>();
            foreach(var p in GameFactory.getGame().shareData.players.players)
            {
                if (p.fdead) continue;
                if (p.id == myp.id) continue;

                koho.Add(p.id);
            }

            int rand = MyRandom.rand(0, koho.Count - 1);
            var dat = net.CreateStoryCode.NightVote(myp.id, koho[rand]);
            new story.game2.Night().run(dat);
        }

        public void midnight_select(Player myp)
        {
            var myplayer = GameFactory.getGame().shareData.players.getPlayer(myp.id);
            int itemindex = -1;
            int chara = 0;
            {
                List<int> koho = new List<int>();
                foreach (var p in GameFactory.getGame().shareData.players.players)
                {
                    if (p.fdead) continue;
                    if (p.id == myp.id) continue;

                    koho.Add(p.id);
                }

                int rand = MyRandom.rand(0, koho.Count - 1);
                chara = koho[rand];
            }
            {
                List<int> koho = new List<int>();
                for (int i = 0; i < myplayer.items.Length; i++)
                {
                    var item = myplayer.items[i];
                    if (item == ITEM.NONE) continue;

                    koho.Add(i);
                }
                if (koho.Count != 0)
                {
                    int rand = MyRandom.rand(0, koho.Count - 1);
                    itemindex = koho[rand];
                }
            }
            var dat = net.CreateStoryCode.MidnightSelect(myp.id, chara, itemindex);
            new story.game2.MidNight().run(dat);
            
        }
        public void midnight_item_select(Player myp,Player opp)
        {
            var myplayer = GameFactory.getGame().shareData.players.getPlayer(myp.id);

            var dat = net.CreateStoryCode.MidnightSelectItem(myp.id, opp.id, myplayer.items,opp.items);
            new story.game2.MidNight().run(dat);
        }
    }
}
