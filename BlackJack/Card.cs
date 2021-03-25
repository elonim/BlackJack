using System;
using System.Collections.Generic;

namespace BlackJack_Console
{
    public class Card
    {
        public string Suit { get; set; }
        public int Value { get; set; }
        public string ValName { get; set; }
        public string SuitIcon { get; set; }
    }
    public static class CardMethods
    {
        public static List<Card> CreateCards()
        {
            //arrays to create cards and values
            string[] suit = { "Spades", "Hearts", "Diamonds", "Clubs" };
            int[] value = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 };
            string[] valName = { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };
            string[] suitIcon = { "♠", "♥", "♦", "♣" };

            int i = 0;
            int j = 0;

            List<Card> Cards = new();
            foreach (string s in suit)
            {
                foreach (int v in value)
                {
                    Card newCard = new()
                    {
                        Suit = s,
                        Value = v,
                        ValName = valName[i],
                        SuitIcon = suitIcon[j]
                    };
                    Cards.Add(newCard);
                    i++;
                }
                i = 0;
                j++;
            }
            return Cards;
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rnd = new();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
