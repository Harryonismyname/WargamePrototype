using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolearmStudios.Utils
{
    public static class DiceBox
    {

        public static int Roll(int faces, int number)
        {
            if (number < 1) number = 1;
            if (faces < 1) faces = 1;
            int total = 0;
            for (int i = 0; i < number; i++)
            {
                total += Random.Range(1, faces);
            }
            return total;
        }
        public static int RollD4(int number = 1) => Roll(4, number);
        public static int RollD6(int number = 1) => Roll(6, number);
        public static int RollD8(int number = 1) => Roll(8, number);
        public static int RollD10(int number = 1) => Roll(10, number);
        public static int RollD12(int number = 1) => Roll(12, number);
        public static int RollD20(int number = 1) => Roll(20, number);
        public static int RollD100(int number = 1) => Roll(100, number);
    }
}
