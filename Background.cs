using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    class Background
    {
        
        Texture2D background; //la texture du background
        Vector2 screenPosition; //la position du background dans la fenêtre de jeu
        Vector2 origin; //le point d'origine du background pour le déplacer
        Vector2 size; //la taille du background
        int width; //la largeur
        int height; //la hauteur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="Background"></param>

        public Background(GraphicsDeviceManager graphics, Texture2D Background)
        {
            background = Background;
            width = 1920;
            height = 1080;
            origin = new Vector2(background.Width / 2, 0);
            screenPosition = new Vector2(width/2, height / 2);
            size = new Vector2(0, background.Height);
        }

        /// <summary>
        /// Pour actualiser le scrolling du background
        /// 
        /// </summary>
        /// <param name="scrolling"></param>
        /// c'est le nombre de frames de déplacement du background à chaque appel de la fonction

        public void Update(double scrolling)
        {
            screenPosition.Y += (float)scrolling;
            screenPosition.Y = screenPosition.Y % background.Height;
        }

        /// <summary>
        /// Pour afficher le background. Deux backgrounds sont affichés, l'un au dessus de l'autre afin de donner un ressenti de background de taille infini
        /// </summary>
        /// <param name="spriteBatch"></param>

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, screenPosition, null, Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(background, screenPosition - size, null, Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}
