using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    //afin de pouvoir créer des boutons pour le menu par exemples
    class Button
    {
        Vector2 position;
        bool clique;
        bool disponible;
        String image;
        SpriteFont font;
        Color couleur;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="disp"></param>
        
        public Button(Vector2 Position, bool disp)
        {
            position = Position;
            disponible = disp;
            clique = false;
        }

        //getters et setters
        public bool Clique
        {

            get { return clique; }

            set { clique = value; }

        }
        public bool Diponible
        {
            get { return disponible; }
            set { disponible = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Pour créer le texte
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Font"></param>
        
            //telecharger 
        public void Load(string name, SpriteFont Font)
        {
            image = name;
            font = Font;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouse"></param>
        /// <returns></returns>

        public bool Update(Vector2 mouse)
        {
            if (mouse.X >= position.X && mouse.X <= position.X + font.MeasureString(image).X && mouse.Y >= position.Y && mouse.Y <= position.Y + font.MeasureString(image).Y)
            {
                clique = true;
                disponible = false;
            }               

            else
                clique = false;            

            return clique;
        }

        /// <summary>
        /// Affiche le texte dont la couleur change en fonction de la position de la souris
        /// </summary>
        /// <param name="spriteBatch"></param>
        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {            

            if (!disponible)
                couleur = new Color(140, 140, 140);


            if (clique)
                couleur = Color.White;

            spriteBatch.DrawString(font, image, position, couleur);
        }
    }
}
