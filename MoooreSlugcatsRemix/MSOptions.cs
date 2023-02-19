using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Menu.Remix.MixedUI;
using UnityEngine;

namespace MoooreSlugcatsRemix
{
    internal class MSOptions : OptionInterface
    {
        public static Configurable<KeyCode> SPAWN_KEY;
        public static Configurable<KeyCode> MULTI_SPAWN_MODIFIER;
        public static Configurable<KeyCode> MULTI_SPAWN_KEY;

        public override void Initialize()
        {
            base.Initialize();

            Tabs = new OpTab[] {new OpTab(this)};

            Tabs[0].AddItems(new OpLabel(new Vector2(32f, 536f), new Vector2(400f + 128f - 32f, 32f), "Mooore Slugcats Remix", FLabelAlignment.Center, true));

            float startPos = 480f;
            float distanceBetween = 48f;
            float xOffset = 400f;

            Tabs[0].AddItems(
                new OpKeyBinder(SPAWN_KEY, new Vector2(32f + xOffset, startPos), new Vector2(128f, 32f)) { description = "The key used to spawn a slugcat" },
                new OpLabel(new Vector2(32f, startPos + 8f), new Vector2(352f, 16f), "Spawn Key", FLabelAlignment.Left),
                new OpLabel(new Vector2(32f, startPos - 8f), new Vector2(352f, 16f), "Pressing will spawn one slugcat", FLabelAlignment.Left),
                
                new OpKeyBinder(MULTI_SPAWN_MODIFIER, new Vector2(32f + xOffset, startPos - distanceBetween), new Vector2(128f, 32f)) { description = "The key used to spawn a slugcat" },
                new OpLabel(new Vector2(32f, startPos - distanceBetween + 8f), new Vector2(352f, 16f), "Multi-Spawn Modifier", FLabelAlignment.Left),
                new OpLabel(new Vector2(32f, startPos - distanceBetween - 8f), new Vector2(352f, 16f), "Holding with the Spawn Key will spawn many slugcats", FLabelAlignment.Left),

                new OpKeyBinder(MULTI_SPAWN_KEY, new Vector2(32f + xOffset, startPos - distanceBetween * 2), new Vector2(128f, 32f)) { description = "The key used to spawn a slugcat" },
                new OpLabel(new Vector2(32f, startPos - distanceBetween * 2 + 8f), new Vector2(352f, 32f), "Multi-Spawn Key", FLabelAlignment.Left),
                new OpLabel(new Vector2(32f, startPos - distanceBetween * 2 - 8f), new Vector2(352f, 32f), "Holding will spawn many slugcats", FLabelAlignment.Left)
            );
        }

        public MSOptions()
        {
            SPAWN_KEY = config.Bind("spawn_key", KeyCode.L);
            MULTI_SPAWN_MODIFIER = config.Bind("multi_spawn_modifier", KeyCode.RightShift);
            MULTI_SPAWN_KEY = config.Bind("multi_spawn_key", KeyCode.None);
        }
    }
}
