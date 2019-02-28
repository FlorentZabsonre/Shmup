using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    static class Camera
    {
        static Matrix world;      

        public static Matrix World { get => world; }

        /// <summary>
        /// Centre la caméra sur le vaisseau, avec possibilité de regarder aux alentours
        /// </summary>
        /// <param name="ship"></param>

        public static void Update(PlayerShip ship)
        {
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            KeyboardState key = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            //Si on appuie sur la gachette gauche et qu'on bouge le stick droit, on peut regarder les alentours du vaisseau, normalement non visibles
            if (gamePad.Triggers.Left == 1 && gamePad.ThumbSticks.Right != new Vector2(0, 0))
                world = Matrix.CreateTranslation(-ship.Position.X - ship.Origin.X, -ship.Position.Y - ship.Origin.Y, 0)
                         * Matrix.CreateTranslation(Shmup.Width / 2 - 0.5f * Shmup.Width * gamePad.ThumbSticks.Right.X, Shmup.Height / 2 - 0.5f * -Shmup.Height * gamePad.ThumbSticks.Right.Y, 0);
            else
            {
                //Si on appuie sur la touche espace et qu'on bouge la souris, on peut aussi regarder les alentours du vaisseau, normalement non visibles
                if ((key.IsKeyDown(Keys.Space)) && Shmup.IsInside)
                    world = Matrix.CreateTranslation(-ship.Position.X - ship.Origin.X, -ship.Position.Y - ship.Origin.Y, 0)
                            * Matrix.CreateTranslation(Shmup.Width / 2 - 0.5f * (mouse.Position.X - Shmup.Width / 2), Shmup.Height / 2 - 0.5f * (mouse.Position.Y - Shmup.Height / 2), 0);
                //Centre la caméra sur le vaisseau
                else
                {
                    world = Matrix.CreateTranslation(-ship.Position.X - ship.Origin.X, -ship.Position.Y - ship.Origin.Y, 0)
                                * Matrix.CreateTranslation(Shmup.Width / 2, Shmup.Height / 2, 0);
                }
            }
        }
    }
}
