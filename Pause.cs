using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    static class Pause
    {       
        static bool hold = false; //pour vérifier si le joueur reste appuyé ou non sur le bouton
        static bool hasPaused = false; //pour vérifier si le jeu est deja en pause ou non
        

        public static bool HasPaused { get => hasPaused; set => hasPaused = value; }

        /// <summary>
        /// Quand le joueur souhaite mettre ou retirer une pause
        /// </summary>

        public static void WantToPause()
        {
            KeyboardState key = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);

            //Si le joueur appuie sur le bouton Start de la manette ou la touche P du clavier, on met le jeu en pause ou on l'enlève
            if (key.IsKeyDown(Keys.P) || gamePad.Buttons.Start == ButtonState.Pressed && !hold)
            {
                hold = true;

                //si le jeu n'est pas déja en pause, il le devient
                if (!hasPaused)
                    hasPaused = true;

                //si le jeu est en pause, elle est retirée
                else
                    hasPaused = false;            
            }

            //pour empêcher d'enchainer les pauses/remises en jeu en restant appuyé sur le bouton
            if (key.IsKeyUp(Keys.P) && gamePad.Buttons.Start == ButtonState.Released)
                hold = false;

        }     
    }
}
