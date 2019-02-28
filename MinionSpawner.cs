using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    class MinionSpawner
    {

        List<Minion> minionList = new List<Minion>();//Liste contenant tous les minions
        int ellapsedFrame;//Temps en image 
        int spawnTime;//temps avant l'apparition de minions
        int time;//Temps en seconde
        int iteration;//Permet de gérer la réduction du spawnTime
        Texture2D texture;
        SoundEffect soundEffectExplosion;

        public int SpawnTime { get => spawnTime; set => spawnTime = value; }
        public Texture2D Texture { get => texture; set => texture = value; }
        public SoundEffect SoundEffectExplosion { get => soundEffectExplosion; set => soundEffectExplosion = value; }

        /// <summary>
        /// Constructeur
        /// </summary>

        public MinionSpawner()
        {
            spawnTime = 15;
            ellapsedFrame = 0;
            iteration = 0;
        }

        /// <summary>
        /// Creation des minions en fonction du temps
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        /// <param name="gEnemy"></param>
        /// <param name="yEnemy"></param>
        /// <param name="rEnemy"></param>
        /// <param name="bEnemy"></param>
        /// <returns></returns>

        public List<Minion> Update(GameTime gameTime, PlayerShip player, GreenEnemy gEnemy, YellowEnemy yEnemy, RedEnemy rEnemy, BlueEnemy bEnemy)
        {

            if (ellapsedFrame == 60)
            {
                time++;
                ellapsedFrame = 0;
            }
            if (time == spawnTime)
            {
                AddMinion(player, gEnemy, yEnemy, rEnemy, bEnemy);
                time = 0;
                iteration++;
            }
            if (iteration == 60)
            {
                iteration = 0;
                spawnTime = spawnTime / 2;
            }
            foreach (Minion m in minionList)
            {
                if (m.IsAlive)
                {
                    m.Update(player, gameTime);
                }
            }
            for (int i = 0; i < minionList.Count; i++)
            {
                if (!minionList[i].IsAlive)
                {
                    Shmup.Score += minionList[i].Points * Shmup.Multiplicateur;
                    Shmup.Multiplicateur++;
                    Shmup.TimeResetMultiplicateur = 0;
                    SoundEffectInstance explosionInstance = minionList[i].SoundEffectExplosion.CreateInstance();
                    explosionInstance.Volume = 0.05f;
                    explosionInstance.Play();
                    minionList.RemoveAt(i);
                }
            }
            ellapsedFrame++;
            return minionList;
        }

        /// <summary>
        /// Affichage des minions
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="particle"></param>

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Minion m in minionList)
            {
                m.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Reset les parametres lors de la création d'une nouvelle partie
        /// </summary>

        public void Reset()
        {
            spawnTime = 15;
            ellapsedFrame = 0;
            iteration = 0;
            minionList.Clear();
        }

        /// <summary>
        /// Ajout de minions
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gEnemy"></param>
        /// <param name="yEnemy"></param>
        /// <param name="rEnemy"></param>
        /// <param name="bEnemy"></param>

        public void AddMinion(PlayerShip player, GreenEnemy gEnemy, YellowEnemy yEnemy, RedEnemy rEnemy, BlueEnemy bEnemy)
        {
            Random r = new Random();
            int x;
            int y;
            Enemies e = gEnemy;
            double distance = Math.Sqrt(Math.Pow(gEnemy.Position.X - player.Position.X, 2) + Math.Pow(gEnemy.Position.Y - player.Position.Y, 2));
            double newDistance = Math.Sqrt(Math.Pow(yEnemy.Position.X - player.Position.X, 2) + Math.Pow(yEnemy.Position.Y - player.Position.Y, 2));
            if (distance < newDistance)
            {
                distance = newDistance;
                e = yEnemy;
            }
            newDistance = Math.Sqrt(Math.Pow(rEnemy.Position.X - player.Position.X, 2) + Math.Pow(rEnemy.Position.Y - player.Position.Y, 2));
            if (distance < newDistance)
            {
                distance = newDistance;
                e = rEnemy;
            }
            newDistance = Math.Sqrt(Math.Pow(bEnemy.Position.X - player.Position.X, 2) + Math.Pow(bEnemy.Position.Y - player.Position.Y, 2));
            if (distance < newDistance)
            {
                distance = newDistance;
                e = bEnemy;
            }
            if (distance < 4000)
            {
                for (int i = 0; i < 5; i++)
                {
                    x = r.Next(-46 + (int)e.Position.X, 46 + (int)e.Position.X);
                    y = r.Next(-46 + (int)e.Position.Y, 46 + (int)e.Position.Y);
                    minionList.Add(new Minion(texture, soundEffectExplosion, x, y));
                }
            }
            else
            {
                int i = r.Next(0, 4);
                switch (i)
                {
                    case 0:
                        x = r.Next((int)player.Position.X - Shmup.Width / 2 - 50, (int)player.Position.X - Shmup.Width / 2);
                        y = r.Next((int)player.Position.Y - Shmup.Height / 2 - 50, (int)player.Position.Y - Shmup.Height / 2);
                        minionList.Add(new Minion(texture, soundEffectExplosion, x, y));
                        break;
                    case 1:
                        x = r.Next((int)player.Position.X + Shmup.Width / 2, (int)player.Position.X + Shmup.Width / 2 + 50);
                        y = r.Next((int)player.Position.Y - Shmup.Height / 2 - 50, (int)player.Position.Y - Shmup.Height / 2);
                        minionList.Add(new Minion(texture, soundEffectExplosion, x, y));
                        break;
                    case 2:
                        x = r.Next((int)player.Position.X + Shmup.Width / 2, (int)player.Position.X + Shmup.Width / 2 + 50);
                        y = r.Next((int)player.Position.Y + Shmup.Height / 2, (int)player.Position.Y + Shmup.Height / 2 + 50);
                        minionList.Add(new Minion(texture, soundEffectExplosion, x, y));
                        break;
                    case 3:
                        x = r.Next((int)player.Position.X - Shmup.Width / 2 - 50, (int)player.Position.X - Shmup.Width / 2);
                        y = r.Next((int)player.Position.Y + Shmup.Height / 2, (int)player.Position.Y + Shmup.Height / 2 + 50);
                        minionList.Add(new Minion(texture, soundEffectExplosion, x, y));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
