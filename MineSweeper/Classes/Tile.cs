using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Classes
{
    public class Tile
    {
        private bool _isMine;
        private bool _isFlagged;
        private bool _isCleared;
        private int _column;
        private int _row;


        public Tile()
        {

        }

        public int Row
        {
            get => _row;
            set => _row = value;
        }

        public int Column
        {
            get => _column;
            set => _column = value;
        }

        public bool IsMine
        {
            get => _isMine;
            set => _isMine = value;
        }

        public bool IsFlagged
        {
            get => _isFlagged;
            set => _isFlagged = value;
        }

        public bool IsCleared
        {
            get => _isCleared;
            set => _isCleared = value;
        }

        public override string ToString()
        {
            return $"This tile is on Row: {Row}, Column: {Column}, Cleared: {IsCleared}, IsMine: {IsMine}, IsFlagged: {IsFlagged}";
        }
    }
}
