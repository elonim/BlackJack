using System;
using System.Collections.Generic;

namespace BlackJack_Console
{
    static class Program
    {
        static void Main()
        {
            SillyBlackJackText(); //silly logotype welcome message with colours
            var cards = CardMethods.CreateCards();//create a list of card objects
            CardMethods.Shuffle(cards); //shuffle the cards
            int bank = 500;
            
            bool keepPlaying = true;
            while (keepPlaying)
            {
                CardMethods.Shuffle(cards); //double shuffle ;-) and shuffle on new game
                int cardCount = 0;//Cardcounter
                List<Card> dealer = new();//dealers cards
                List<Card> player = new();//players cards
                int pScore = 0;
                int dScore = 0;
                int bet;

                //betting
                bet = Betting(bank);


                //innitially draw cards for dealer and player
                Draw(cards, ref cardCount, player);
                Draw(cards, ref cardCount, dealer);
                Draw(cards, ref cardCount, player);
                Draw(cards, ref cardCount, dealer);
                //show starting hands of dealer and player
                bool hiddencard = true;
                ShowDealerHand(dealer, hiddencard);
                ShowPlayerHand(player);


                //the player and dealer take their turns
                hiddencard = Player_Playing(cards, ref cardCount, dealer, player, ref pScore, ref hiddencard);
                Dealer_Playing(cards, ref cardCount, dealer, ref dScore);


                //show the result of both the dealer and the players and and who won
                SillyBlackJackText();
                ShowDealerHand(dealer, hiddencard);
                ShowPlayerHand(player);
                WinLooseCondition(player, pScore, dScore, ref bank, ref bet);//show the winner
                WinLooseScoreText(pScore, dScore, ref bank);

                keepPlaying = NewGame(keepPlaying, ref bank);
            }
        }


        #region Drawing Functions
        private static void Draw(List<Card> cards, ref int cardCount, List<Card> someOne)
        {
            someOne.Add(cards[cardCount]);
            cardCount++;
        }

        private static bool Player_Playing(List<Card> cards, ref int cardCount, List<Card> dealer, List<Card> player, ref int pScore, ref bool hiddencard)
        {
            bool keepDrawing = true;
            while (keepDrawing)
            {
                pScore = GetScore(player);
                if (pScore < 21 && keepDrawing == true)
                {
                    Console.Write("\n Draw another card? Y/n : ");
                    string playerDrawAgain = Console.ReadLine().ToLower();

                    if (playerDrawAgain == "y")
                    {
                        Draw(cards, ref cardCount, player);
                        SillyBlackJackText();
                        ShowDealerHand(dealer, hiddencard);
                        ShowPlayerHand(player);
                    }
                    else
                    {
                        keepDrawing = false;
                    }
                }
                else
                {
                    return false;
                    
                }
            }
            return false;
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
        #endregion


        #region Show Hands Functions
        private static void ShowPlayerHand(List<Card> player)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n Your Hand");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (Card card in player) //show all cards
            {
                Console.WriteLine($"  {card.SuitIcon}  {card.ValName} of {card.Suit}");
            }
            Console.WriteLine("\n Your score is " + GetScore(player));
        }

        private static void ShowDealerHand(List<Card> dealer, bool hiddencard)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n Dealers Hand");
            Console.ForegroundColor = ConsoleColor.Gray;
            if (hiddencard)
            {
                var card1Val = dealer[1].Value;
                if (dealer[1].Value == 1)//if the shown card is an ace then set its value to 11
                {
                    card1Val = 11;
                }


                Console.WriteLine($"    'Hidden card' \n  {dealer[1].SuitIcon}  {dealer[1].ValName} of {dealer[1].Suit}");
                Console.WriteLine($"\n The dealers score is {card1Val}");
            }
            else
            {
                foreach (Card card in dealer) //show all cards
                {
                    Console.WriteLine($"  {card.SuitIcon}  {card.ValName} of {card.Suit}");
                }
                Console.WriteLine("\n The dealers score is " + GetScore(dealer));
            }
        }
        #endregion 


        #region Score Functions
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
        private static void WinLooseCondition(List<Card> player, int pScore, int dScore, ref int bank,ref int bet)
        {
            Console.WriteLine();
            if (pScore == 21 && player.Count == 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tBlackJack!");
                Console.ForegroundColor = ConsoleColor.Gray;
                bank += (bet);
            }
            else if ((pScore == dScore) && (pScore <= 21))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\tPush!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else if (pScore > 21)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tBust!");
                Console.ForegroundColor = ConsoleColor.Gray;
                bank -= (bet);
            }
            else if (pScore <= 21 && dScore > 21)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tYou Win!");
                Console.ForegroundColor = ConsoleColor.Gray;
                bank += (bet);
            }
            else if (pScore < 21 && dScore <= 21 && dScore > pScore)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tYou Loose!");
                Console.ForegroundColor = ConsoleColor.Gray;
                bank -= (bet);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tYou Win!");
                Console.ForegroundColor = ConsoleColor.Gray;
                bank += (bet);
            }
        }

        private static void WinLooseScoreText(int pScore, int dScore, ref int bank)
        {
            Console.Write($"\n Your score was ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(pScore);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" And the dealers score was ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"{dScore}\n");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write("\n You now have ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"{bank}$\n");
            Console.ForegroundColor = ConsoleColor.Gray;


        }
        #endregion


        #region New game and betting functions
        private static int Betting(int bank)
        {
            int bet;
            try
            {
                Console.Write("\n You have ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($"{bank}$\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" Place bet (Default bet is 10): ");
                bet = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                bet = 10;
            }
            if (bet>bank)
            {
                Console.Write("\n Sorry you only have ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(bank + "$");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" Try again\n");
                Betting(bank);
            }

            return bet;
        }
        private static bool NewGame(bool kp, ref int bank)
        {
            Console.Write("\n New Game? Y/n : ");
            var again = Console.ReadLine().ToLower();
            if (again == "y" && bank <= 0)
            {
                Console.WriteLine("\n Sorry you have gone bankrupt!");
                Console.Write("\n Want to start over with ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("500$");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("? Y/n : ");
                var moreMoney = Console.ReadLine().ToLower();
                if (moreMoney == "y")
                {
                    bank = 500;
                    SillyBlackJackText();
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\n\tThank you for playing\n\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return false;
                }
            }

            if (again != "y")
            {
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n\tThank you for playing\n\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                return false;
            }
            SillyBlackJackText();
            return kp;
        }

        private static void SillyBlackJackText()
        {
            Console.Clear();//clear the console to minimize clutter
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\t|♠|♥|♦|♣| ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("BlackJack");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" |♣|♦|♥|♠|\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        #endregion
    }
}