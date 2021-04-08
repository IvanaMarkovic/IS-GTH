using GlobalThinkersHelper.Model.Entities;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GlobalThinkersHelper.View
{
    /// <summary>
    /// Interaction logic for DailyNotes.xaml
    /// </summary>
    public partial class DailyNotes : UserControl
    {
        List<term> terms = new List<term>();
        List<Card> cards = new List<Card>();
        List<ContentControl> cardsContent = new List<ContentControl>();

        public DailyNotes()
        {
            InitializeComponent();
            cards.Add(card1);
            cardsContent.Add(cardContentControl1);
            cards.Add(card2);
            cardsContent.Add(cardContentControl2);
            cards.Add(card3);
            cardsContent.Add(cardContentControl3);
            cards.Add(card4);
            cardsContent.Add(cardContentControl4);
            cards.Add(card5);
            cardsContent.Add(cardContentControl5);
            cards.Add(card6);
            cardsContent.Add(cardContentControl6);
            cards.Add(card7);
            cardsContent.Add(cardContentControl7);
            cards.Add(card8);
            cardsContent.Add(cardContentControl8);
            cards.Add(card9);
            cardsContent.Add(cardContentControl9);
            DateTime currentDate = new DateTime(2020, 2, 20, 8,0,0);
            terms = term.SelectAll().Where(t => t.rental_date.Date.CompareTo(currentDate.Date) == 0 && t.rent_time_start.CompareTo(new TimeSpan(currentDate.Hour, currentDate.Minute, currentDate.Second)) > 0).ToList();
            AddCards();

        }

        public void AddCards()
        {
            int i = 0;
            for (; i < terms.Count && i < cards.Count; i++)
            {
                cards[i].Visibility = Visibility.Visible;
                ViewTerm newViewTerm = new ViewTerm();
                newViewTerm.gridCardContent.DataContext = terms[i];
                cardsContent[i].Content = newViewTerm;
            }
            if(i < cards.Count)
            {
                for (; i < cards.Count; i++)
                {
                    cards[i].Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
