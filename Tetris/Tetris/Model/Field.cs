using System.Windows.Media;

namespace Tetris.Model
{
    public class Field
    {
        public bool Active;
        public Brush Color;

        public Field(Color color)
        {
            Color = new SolidColorBrush(color);
        }
    }
}