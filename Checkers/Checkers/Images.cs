using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CheckersLogic;

namespace Checkers
{
    public static class Images
    {
        public static ImageSource GetImageSource(CheckersLogic.Color color, bool isKing)
        {
            switch (color)
            {
                case CheckersLogic.Color.White:
                    if (isKing) { return LoadImage("Assets/KingW.png"); }
                    else { return LoadImage("Assets/ManW.png"); }
                
                case CheckersLogic.Color.Black:
                    if (isKing) { return LoadImage("Assets/KingB.png"); }
                    else { return LoadImage("Assets/ManB.png"); }

                default: return null;
            }
        }
        // internal protoze jinak to breci
        internal static ImageSource GetImageSource(Piece piece)
        {
            if (piece == null) { return null; }
            return GetImageSource(piece.Color, piece.IsKing);
        }

        private static ImageSource LoadImage(string filepath)
        {
            return new BitmapImage(new Uri(filepath, UriKind.Relative));
        }
    }
}
