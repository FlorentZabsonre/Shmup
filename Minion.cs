using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Shmup
{
    class Minion : Enemies
    {
        Vector2 maxVelocity;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>

        public Minion(Texture2D t, SoundEffect s, int x, int y)
        {
            Random r = new Random();
            spriteIndex = (Sprites)r.Next(0, 3);
            isAlive = true;
            width = 24;
            height = 24;
            rectangleSource = new Rectangle(width * (int)SpriteIndex, 0, Width, Height);
            pv = 1;
            pvMax = pv;
            origin = new Vector2(width / 2, height / 2);
            angle = 0f;
            position.X = x;
            position.Y = y;
            velocity = new Vector2(0, 0);
            maxVelocity = new Vector2(5, 5);
            hasCollided = false;
            points = 10;
            switch ((int)spriteIndex)
            {
                case 0:
                    color = Color.Red;
                    break;
                case 1:
                    color = Color.Green;
                    break;
                case 2:
                    color = Color.Yellow;
                    break;
                case 3:
                    color = Color.Blue;
                    break;
            }
            texture = t;
            soundEffectExplosion = s;
        }

        /// <summary>
        /// Fonction update principale
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>

        public void Update(PlayerShip player, GameTime gameTime)
        {
            Move(player);
            CollisionBullet(player);
            CollisionMainShip(player);
        }

        /// <summary>
        /// Fonction Draw principale
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="particle"></param>

        public void Draw(SpriteBatch spriteBatch)
        {           
                spriteBatch.Draw(texture, position, rectangleSource, Color.White, angle + ((float) Math.PI / 2), origin, 1.0f, SpriteEffects.None, 1);
        }


        /// <summary>
        /// Deplacement des minions
        /// </summary>
        /// <param name="player"></param>

        public void Move(PlayerShip player)
        {

            Vector2 distance = new Vector2(player.Position.X - position.X, player.Position.Y- position.Y);
            angle = (float)Math.Atan2(distance.Y, distance.X);//pour trouver l'angle entre le minion et le vaisseau du joueur
            velocity += new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 0.5f;
            if (velocity.X < -maxVelocity.X)
            {
                velocity.X = -maxVelocity.X;
            }else if (velocity.X > maxVelocity.X)
            {
                velocity.X = maxVelocity.X;
            }

            if (velocity.Y < -maxVelocity.Y)
            {
                velocity.Y = -maxVelocity.Y;
            }
            else if (velocity.Y > maxVelocity.Y)
            {
                velocity.Y = maxVelocity.Y;
            }

            position += velocity;
            InitialisationHitbox();
        }
    }
}
