using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Shmup
{
    class Level
    {
        static int width = 4000;
        static int height = 4000;
        Random rnd = new Random();
        int randomTime;
        double time;

        Texture2D background;
        int backgroundWidth = 1920;
        int backgroundHeight = 1080;

        Texture2D bordure;        
        Vector2 backgroundPosition = new Vector2(0, 0);
        Color colorBordure = Color.Red;
        static int bordureWidth = 10;
        static int bordureHeight = height;
        Rectangle hitbox1 = new Rectangle(0, -bordureWidth, bordureHeight, bordureWidth);
        Rectangle hitbox2 = new Rectangle(4000, 0, bordureWidth, bordureHeight);
        Rectangle hitbox3 = new Rectangle(-bordureWidth, 0, bordureWidth, bordureHeight);
        Rectangle hitbox4 = new Rectangle(0, 4000, bordureHeight, bordureWidth);
        float backgroundVelocity = 0.5f;

        Vector2 spawnTop;
        Vector2 spawnRight;
        Vector2 spawnDown;
        Vector2 spawnLeft;

        public Texture2D Background { get => background; set => background = value; }
        public static int Width { get => width; }
        public static int Height { get => height; }
        public Texture2D Bordure { get => bordure; set => bordure = value; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="player"></param>
        /// <param name="r"></param>
        /// <param name="b"></param>
        /// <param name="g"></param>
        /// <param name="y"></param>

        public Level(PlayerShip player ,RedEnemy r, BlueEnemy b, GreenEnemy g, YellowEnemy y)
        {
            player.Position = new Vector2(width / 2, height / 2);        
            spawnTop = new Vector2(rnd.Next(300, 3800), 200);
            spawnRight = new Vector2(3800, rnd.Next(300, 3800));
            spawnDown = new Vector2(rnd.Next(200, 3700), 3800);
            spawnLeft = new Vector2(200, rnd.Next(200, 3700));
            PlaceEnnemies(b, r, g, y);               
        }

        /// <summary>
        /// Update du level
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="playerShip"></param>

        public void Update(GameTime gameTime, PlayerShip playerShip)
        {
            backgroundPosition.Y += backgroundVelocity;
            if(backgroundPosition.Y > backgroundHeight)
            {
                backgroundPosition.Y = 0;
            }

            time += gameTime.ElapsedGameTime.TotalSeconds;


            if (randomTime == 0)
                randomTime = rnd.Next(5, 21);

            if (time >= randomTime)
            {
                Color colorRandom = Color.White;
                do
                {
                    int random = rnd.Next(1, 5);                    
                    if (random == 1)
                        colorRandom = Color.Red;
                    else if (random == 2)
                        colorRandom = Color.Green;
                    else if (random == 3)
                        colorRandom = Color.Yellow;
                    else if (random == 4)
                        colorRandom = Color.Blue;
                } while (colorRandom == colorBordure);
                colorBordure = colorRandom;
                randomTime = 0;
                time = 0;
            }

            if ((playerShip.Hitbox.Intersects(hitbox1) || playerShip.Hitbox.Intersects(hitbox2) || playerShip.Hitbox.Intersects(hitbox3) || playerShip.Hitbox.Intersects(hitbox4)) && playerShip.Color != colorBordure)
                playerShip.IsAlive = false;
        }

        /// <summary>
        /// Affichage des sprites
        /// </summary>
        /// <param name="spriteBatch"></param>

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = -backgroundWidth; i < height + backgroundWidth; i += backgroundWidth)
            {
                for (int y = -backgroundHeight; y < height + backgroundHeight; y += backgroundHeight)
                {
                    spriteBatch.Draw(background, backgroundPosition + new Vector2(i, y), Color.White);
                }
            }
            spriteBatch.Draw(bordure, hitbox1, null, colorBordure);
            spriteBatch.Draw(bordure, hitbox2, null, colorBordure);
            spriteBatch.Draw(bordure, hitbox3, null, colorBordure);
            spriteBatch.Draw(bordure, hitbox4, null, colorBordure);

        }

        /// <summary>
        /// Place les ennemis aléatoirement dans le niveau
        /// </summary>
        /// <param name="b"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="y"></param>

        public void PlaceEnnemies(BlueEnemy b, RedEnemy r, GreenEnemy g, YellowEnemy y)
        {
            List<Enemies> l = new List<Enemies>
            {
                b,
                r,
                g,
                y
            };

            Shuffle<Enemies>(l);

            for(int i = 0; i <= l.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        l[0].Position = spawnTop;
                        break;
                    case 1:
                        l[1].Position = spawnRight;
                        break;
                    case 2:
                        l[2].Position = spawnDown;
                        break;
                    case 3:
                        l[3].Position = spawnLeft;
                        break;
                    default:
                        break;
                }              
                
            }
            
        }

        /// <summary>
        /// Reset le niveau une fois que le vaisseau ou que tous les ennemis soient morts
        /// </summary>
        /// <param name="player"></param>
        /// <param name="r"></param>
        /// <param name="b"></param>
        /// <param name="g"></param>
        /// <param name="y"></param>
        /// <param name="ms"></param>
        /// <param name="particle"></param>

        public void ResetLevel(PlayerShip player, RedEnemy r, BlueEnemy b, GreenEnemy g, YellowEnemy y, MinionSpawner ms, ParticleManager particle)
        {
            Shmup.Multiplicateur = 1;
            Shmup.TimeResetMultiplicateur = 0;
            player.Reset();
            r.Reset();
            g.Reset();
            y.Reset();
            b.Reset();
            player.Position = new Vector2(width / 2, height / 2);
            spawnTop = new Vector2(rnd.Next(300, 3800), 200);
            spawnRight = new Vector2(3800, rnd.Next(300, 3800));
            spawnDown = new Vector2(rnd.Next(200, 3700), 3800);
            spawnLeft = new Vector2(200, rnd.Next(200, 3700));
            PlaceEnnemies(b, r, g, y);
            particle.Particles.Clear();
            particle.Start = 0;
            ms.Reset();
        }

        /// <summary>
        /// Melange les ennemis dans la liste
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>

        private void Shuffle<T>(List<Enemies> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                Enemies value = list[k];
                list[k] = list[n];
                list[n] = value;   
            }

            list[0].Spawn = 1;
            list[0].Angle = 0f;

            list[1].Spawn = 2;
            list[1].Angle = (float)Math.PI / 2; 

            list[2].Spawn = 3;
            list[2].Angle = (float)Math.PI;

            list[3].Spawn = 4;
            list[3].Angle = -(float)Math.PI / 2;           
        }
    }
}
