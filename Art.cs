using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
   static  class Art
    {
        public static Texture2D MainMenu { get; private set; }

        public static Texture2D SPlayer { get; private set; }
        public static Texture2D Seek { get; private set; }
        public static Texture2D Seekto { get; private set; }
        public static Texture2D FBullet { get; private set; }
        public static Texture2D Pointer { get; private set; }
        public static Texture2D BGalaxy { get; private set; }
        public static Texture2D BGalaxy1 { get; private set; }
        public static Texture2D BGalaxy11 { get; private set; }
        public static Texture2D BGalaxy12 { get; private set; }
        public static Texture2D BGalaxy21 { get; private set; }
        public static Texture2D Wanderer { get; private set; }
        public static SpriteFont Font1 { get; private set; }
        public static SpriteFont Font21 { get; private set; }
        public static Texture2D BGalaxy2 { get; private set; }
        public static Texture2D EndGame12 { get; private set; }
        public static void Load(ContentManager Content)
        {
            MainMenu = Content.Load<Texture2D>("Art/MainMenu");
            BGalaxy = Content.Load<Texture2D>("Art/BGalaxy");
            BGalaxy1 = Content.Load<Texture2D>("Art/BGalaxy1");
            BGalaxy2 = Content.Load<Texture2D>("Art/BGalaxy2");
            BGalaxy11 = Content.Load<Texture2D>("Art/BGalaxy11");
            BGalaxy12 = Content.Load<Texture2D>("Art/BGalaxy12");
            BGalaxy21 = Content.Load<Texture2D>("Art/BGalaxy21");
            SPlayer = Content.Load<Texture2D>("Art/SPlayer");
            Seek = Content.Load<Texture2D>("Art/Seek");
            Seekto = Content.Load<Texture2D>("Art/Seek2");
            FBullet = Content.Load<Texture2D>("Art/FBullet");
            Pointer = Content.Load<Texture2D>("Art/Pointer");
            Wanderer = Content.Load<Texture2D>("Art/Wanderer");
            Font1 = Content.Load<SpriteFont>("Fonts/Font1");
            Font21 = Content.Load<SpriteFont>("Fonts/Font21");
            EndGame12 = Content.Load<Texture2D>("Art/EndGame12");
        }


    }
}
