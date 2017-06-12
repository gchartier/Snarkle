using System;

namespace Snarkle
{
    class Player
    {
        private int totalScore;
        public String Name { get; set; }
        public int TotalScore
        {
            get { return totalScore; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Total score cannot be less than 0");
                totalScore = value;
            }
        }
    }
}
