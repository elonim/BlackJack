using System;
using System.Collections.Generic;


namespace BlackJack_Console
{
    static class Program
    {
        static void Main(string[] args)
        {
            SillyBlackJackText(); //silly logotype welcome message with colours
            var cards = CardMethods.CreateCards();//create a list of card objects
            CardMethods.Shuffle(cards); //shuffle the cards

            bool keepPlaying = true;
            while (keepPlaying)
            {
                CardMethods.Shuffle(cards); //double shuffle ;-) and shuffle on new game
                int cardCount = 0;//Cardcounter
                List<Card> dealer = new();//dealers cards
                List<Card> player = new();//players cards
                int pScore = 0;
                int dScore = 0;


                //innitially draw cards for dealer and player
                Draw(cards, ref cardCount, player);
                Draw(cards, ref cardCount, dealer);
                Draw(cards, ref cardCount, player);
                Draw(cards, ref cardCount, dealer);
                //show starting hands of dealer and player
                bool hiddencard = true;
                ShowDealerHand(dealer, hiddencard);
                ShowPlayerHand(player);

                Console.WriteLine("\nYour score is " + GetScore(player));

                Player_Playing(cards, ref cardCount, dealer, player, ref pScore, ref hiddencard);

                Dealer_Playing(cards, ref cardCount, dealer, ref dScore);

                SillyBlackJackText();
                ShowDealerHand(dealer, false);
                Console.WriteLine("\nDealer score is " + GetScore(dealer));
                ShowPlayerHand(player);
                Console.WriteLine("\nYour score is " + GetScore(player));
                Console.WriteLine();


                WinLooseCondition(player, pScore, dScore);//show the winner

                keepPlaying = NewGame(keepPlaying);
            }
        }

        private static void WinLooseCondition(List<Card> player, int pScore, int dScore)
        {
            if (pScore == 21 && player.Count == 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tBlackJack!");
                Console.ForegroundColor = ConsoleColor.Gray;
                WinLooseScoreText(pScore, dScore);
            }
            else if (pScore > 21)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tBust!");
                Console.ForegroundColor = ConsoleColor.Gray;
                WinLooseScoreText(pScore, dScore);
            }
            else if (pScore <= 21 && dScore > 21)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tYou Win!");
                Console.ForegroundColor = ConsoleColor.Gray;
                WinLooseScoreText(pScore, dScore);
            }
            else if (pScore == dScore)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\tPush!");
                Console.ForegroundColor = ConsoleColor.Gray;
                WinLooseScoreText(pScore, dScore);
            }
            else if (pScore < 21 && dScore <= 21 && dScore > pScore)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tYou Loose!");
                Console.ForegroundColor = ConsoleColor.Gray;
                WinLooseScoreText(pScore, dScore);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tYou Win!");
                Console.ForegroundColor = ConsoleColor.Gray;
                WinLooseScoreText(pScore, dScore);
            }
        }
        private static void WinLooseScoreText(int pScore, int dScore)
        {
            Console.Write($"\nYour score is ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(pScore);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" and the dealers score is ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"{dScore}\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        private static void Player_Playing(List<Card> cards, ref int cardCount, List<Card> dealer, List<Card> player, ref int pScore, ref bool hiddencard)
        {
            bool keepDrawing = true;
            while (keepDrawing)
            {
                pScore = GetScore(player);
                if (pScore < 21 && keepDrawing == true)
                {
                    Console.Write("\nDraw another card? Y/n : ");
                    string playerDrawAgain = Console.ReadLine().ToLower();

                    if (playerDrawAgain == "y")
                    {
                        Draw(cards, ref cardCount, player);
                        SillyBlackJackText();
                        ShowDealerHand(dealer, hiddencard);
                        ShowPlayerHand(player);
                        Console.WriteLine("\nYour score is " + GetScore(player));
                    }
                    else
                    {
                        keepDrawing = false;
                    }
                }
                else
                {
                    hiddencard = false;
                    break;
                }
            }
        }
        private static void Dealer_Playing(List<Card> cards, ref int cardCount, List<Card> dealer, ref int dScore)
        {
            bool dealerPlaying = true;
            while (dealerPlaying)
            {
                dScore = GetScore(dealer);
                if (dScore <= 17 && dealerPlaying == true)
                {
                    Draw(cards, ref cardCount, dealer);
                }
                else
                {
                    dealerPlaying = false;
                }
            }
        }
        private static int GetScore(List<Card> someOne)
        {
            int score = 0;
            for (int i = 0; i < someOne.Count; i++)
            {
                if (someOne[i].Value == 1)//if ace is drawn then +10
                {
                    score += someOne[i].Value + 10;
                }
                else
                {
                    score += someOne[i].Value;
                }
            }
            if (score > 20 && someOne.Exists(x => x.Value == 1))//if ace is drawn and score is over 20 -10
            {
                score -= 10;
            }
            return score;
        }
        private static void ShowPlayerHand(List<Card> player)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nYour Hand");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (Card card in player) //show all cards
            {
                Console.WriteLine($"{card.ValName} of {card.SuitIcon}");
            }
        }
        private static void ShowDealerHand(List<Card> dealer, bool hiddencard)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nDealers Hand");
            Console.ForegroundColor = ConsoleColor.Gray;
            if (hiddencard)
            {
                var card1Val = dealer[1].Value;
                if (dealer[1].Value == 1)//if the shown card is an ace then set its value to 11
                {
                    card1Val = 11;
                }


                Console.WriteLine($"'Hidden card' \n{dealer[1].ValName} of {dealer[1].SuitIcon}");
                Console.WriteLine($"\nThe dealers score is {card1Val}");
            }
            else
            {
                foreach (Card card in dealer) //show all cards
                {
                    Console.WriteLine($"{card.ValName} of {card.SuitIcon}");
                }
            }
        }
        private static void Draw(List<Card> cards, ref int cardCount, List<Card> someOne)
        {
            someOne.Add(cards[cardCount]);
            cardCount++;
        }
        private static bool NewGame(bool kp)
        {
            Console.Write("\nNew Game? Y/n : ");
            var again = Console.ReadLine().ToLower();
            if (again != "y")
            {
                kp = false;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n\tThank you for playing\n\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                return kp;
            }

            SillyBlackJackText();
            return kp;

        }
        private static void SillyBlackJackText()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\t|♠|♥|♦|♣| ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("BlackJack");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" |♣|♦|♥|♠|\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}