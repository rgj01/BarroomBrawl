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

        private const int height = 5;
        private const int ytranslate = 10;
        private const int xtranslate = 10;

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(m_position.X - xtranslate, m_position.Y - ytranslate, height);
            }
        }

        public Camera(Vector2 center)
        {
            m_position = center;
        }

        public void Update(Vector2 center)
        {
            m_position = center;
        }
    }
}
