using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BarRoomBrawl
{
    public class Camera
    {
        private Vector2 m_position;
        private Vector2 m_origin;

        private const int height = 5;
        private const int ytranslate = 10;
        private const int xtranslate = 10;

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-m_position,0))
                    * Matrix.CreateTranslation(new Vector3 (m_origin,0));
            }
        }

        public Camera(Vector2 center, Vector2 origin)
        {
            m_position = center;
            m_origin = origin;
        }

        public void Update(Vector2 center)
        {
            m_position = center;
        }
    }
}
