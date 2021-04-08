using GlobalThinkersHelper.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CreateReceipt.xaml
    /// </summary>
    public partial class CreateReceipt : UserControl
    {
        Dictionary<long, string> clients = new Dictionary<long, string>();
        ObservableCollection<receipt> receipts;


        public CreateReceipt()
        {
            InitializeComponent();
            client.SelectAll().ForEach(c => clients.Add(c.id, c.first_name + " " + c.last_name)); 
            atbClientSearch.AutoCompleteSource = clients.Values;
            payButton.Visibility = Visibility.Hidden;
        }

        private void atbClientSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                atbClientSearch.Focusable = false;
                tbClientSearch.Focus();
            }
        }


        private void atbClientSearch_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = atbClientSearch.SelectedItem as string;
            if (obj != null) 
            {
                ObservableCollection<reservation> allReservations = null;
                client newClient = client.SelectById(clients.FirstOrDefault(c => c.Value.Equals(obj)).Key);
                if (newClient != null)
                {
                    datagridReservation.SelectedItem = null;
                    allReservations = new ObservableCollection<reservation>();
                    newClient.reservations.ToList().ForEach(r => allReservations.Add(r));
                    newClient.attends_events.ToList().ForEach(r => allReservations.Add(r));
                    datagridReservation.ItemsSource = allReservations;
                    EntityFactory.Client = newClient;
                }
            }
        }

        private void tbClientSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbClientSearch.SelectAll();
            if (tbClientSearch.Text.Length > 0)
            {
                atbClientSearch.Select(tbClientSearch.Text.Length, 0);
                atbClientSearch.Focusable = true;
                atbClientSearch.Focus();
            }
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            reservation reservation = datagridReservation.SelectedItem as reservation;
            if (reservation != null)
            {
                tbInstallmentsButton.IsChecked = false;
                tbInstallmentsCount.IsEnabled = false;
                tbInstallmentsCount.Text = "";
                tbDiscountAmount.Text = "";
                datagridReceipts.ItemsSource = receipts = null;
                if (reservation.receipts.Count == 0)
                {
                    EntityFactory.Receipt = new receipt(EntityFactory.User.id, reservation.client_id, reservation.id, null, calculatePrice(reservation), 0, false, 0, null);
                    spPaymentPlan.Visibility = Visibility.Collapsed;
                    payButton.Visibility = Visibility.Hidden;
                    tbInstallmentsButton.IsEnabled = tbDiscountAmount.IsEnabled = btnPayingPlan.IsEnabled = cbDiscount.IsEnabled = true;
                } else
                {
                    SetReceiptFields(reservation);
                }
                EntityFactory.Reservation = reservation;
                tbReceiptNumber.Text = tbReceiptNumber2.Text = tbReceiptNumber4.Text = EntityFactory.Receipt.serial_number;
                tbClientName.Text = tbClientName2.Text = tbClientName4.Text = reservation.client.ToString();
                tbEventName.Text = tbEventName2.Text = tbEventName4.Text = reservation._event != null ? reservation._event.name : "Nije odabran događaj";
                tbPrice.Text = tbPrice2.Text = tbPrice4.Text = EntityFactory.Receipt.amount + " €";
                SetTreeview(reservation);
                receiptExpander.IsExpanded = true;
            }
        }

        public void SetReceiptFields(reservation reservation)
        {
            payButton.Visibility = Visibility.Visible;
            EntityFactory.Receipt = reservation.receipts.ToList().Where(r => r.receipt_id == null).ToList()[0];
            if (EntityFactory.Receipt.receipts.ToList().Count > 0)
            {

                tbInstallmentsCount.Text = EntityFactory.Receipt.receipts.ToList().Count.ToString();
                decimal? discount = 0;
                if (EntityFactory.Receipt.discount < 1)
                {
                    discount = EntityFactory.Receipt.discount * 100;
                    cbDiscount.SelectedIndex = 0;
                }
                else
                {
                    discount = EntityFactory.Receipt.discount;
                    cbDiscount.SelectedIndex = 1;
                }
                tbDiscountAmount.Text = discount.ToString();
                tbInstallmentsButton.IsEnabled = tbDiscountAmount.IsEnabled = btnPayingPlan.IsEnabled = cbDiscount.IsEnabled = false;
                receipts = new ObservableCollection<receipt>(EntityFactory.Receipt.receipts.ToList());
                datagridReceipts.ItemsSource = receipts;
                spPaymentPlan.Visibility = Visibility.Visible;
            }
            else
            {
                spPaymentPlan.Visibility = Visibility.Collapsed;
            }
        }

        public void SetTreeview(reservation reservation)
        {
            if (EntityFactory.Client.attends_events.Contains(reservation))
            {
                spTerms.Visibility = Visibility.Collapsed;
                return;
            }
            if (EntityFactory.Client.reservations.Contains(reservation))
            {
                tvTerms.Items.Clear();
                reservation.terms.ToList().ForEach(t => 
                {
                    TreeViewItem term = new TreeViewItem();
                    term.Header = "Termin " + (reservation.terms.ToList().IndexOf(t) + 1) + " - " + t.rental_date.ToString("dd.MM.yyyy");
                    term.Items.Add("Sala: " + t.hall.name);
                    term.Items.Add("Vrijeme početka: " + t.rent_time_start);
                    term.Items.Add("Vrijeme kraja: " + t.rent_time_end);
                    TreeViewItem items = new TreeViewItem();
                    items.Header = t.items.ToList().Count > 0 ? "Stavke koje su potrebe" : "Nema stavki";
                    t.items.ToList().ForEach(i => items.Items.Add(i.quantity + " " + i.item.name));
                    term.Items.Add(items);
                    tvTerms.Items.Add(term);
                });
                spTerms.Visibility = Visibility.Visible;
            }
        }

        public decimal calculatePrice(reservation reservation)
        {
            decimal total = 0;
            if(EntityFactory.Client.attends_events.Contains(reservation))
            {
                total = reservation._event != null ? (decimal)reservation.event_price : 0;
                return total;
            }
            reservation.terms.ToList().ForEach(t =>
            {
                var dayInWeek = (DayInWeek)((int)t.rental_date.DayOfWeek);
                var pricelists = t.hall.price_lists.ToList().Where(p => p.day.Equals(dayInWeek.ToString())).ToList();
                if (pricelists.Count > 0)
                {
                    total += (decimal)((t.rent_time_end - t.rent_time_start).Hours * pricelists[0].price_hour);
                }
                t.items.ToList().ForEach(i => { if (i.item.price > 0) { total += (decimal)i.item.price; } });
            });
            return total;
        } 

        private void txtCalendar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (calendar.Visibility == Visibility.Collapsed)
            {
                calendar.Visibility = Visibility.Visible;
            }
            else
            {
                calendar.Visibility = Visibility.Collapsed;
            }
        }

        private void calendar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                calendar.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAcceptInstallment_Click(object sender, RoutedEventArgs e)
        {
            var selectedReceipt = datagridReceipts.SelectedItem as receipt;
            selectedReceipt.payment_date = dpReceiptDate.SelectedDate;
            selectedReceipt.paid = true;
            receipt.Update(selectedReceipt, selectedReceipt.id);
        }

        private void btnAcceptReceipt_Click(object sender, RoutedEventArgs e)
        {
            EntityFactory.Receipt.payment_date = dpReceiptDate.SelectedDate;
            EntityFactory.Receipt.paid = true;
            receipt.Update(EntityFactory.Receipt, EntityFactory.Receipt.id);
        }

        private void TbInstallmentsButton_Click(object sender, RoutedEventArgs e)
        {
            tbInstallmentsCount.IsEnabled = !tbInstallmentsCount.IsEnabled;
        }

        private void btnUpdateInstallment_Click(object sender, RoutedEventArgs e)
        {
            var selectedReceipt = datagridReceipts.SelectedItem as receipt;
            if(EntityFactory.Reservation.receipts.ToList().Count == 0)
            {
                errorPopup.IsOpen = true;
                return;
            }
            errorPopup.IsOpen = false;
            if (selectedReceipt != null)
            {
                tbReceiptNumber3.Text = selectedReceipt.serial_number;
                tbInstallmentNumber.Text = (datagridReceipts.SelectedIndex + 1).ToString();
                tbPaymentNoDiscount.Text = selectedReceipt.amount + " €";
                tbDiscount.Text = selectedReceipt.discount + " €";
                tbPaymnetAmount.Text = (selectedReceipt.amount - selectedReceipt.discount) + " €";
            }
            billPopup.IsOpen = true;
        }

        private void btnSaveReciept_Click(object sender, RoutedEventArgs e)
        {
            var selectedReceipt = datagridReceipts.SelectedItem as receipt;
            if (EntityFactory.Reservation.receipts.ToList().Count == 0)
            {
                errorPopup.IsOpen = true;
                return;
            }
            errorPopup.IsOpen = false;
        }


        private void payButton_Click(object sender, RoutedEventArgs e)
        {
            billPopup2.IsOpen = true;
        }

        private void btnPayingPlan_Click(object sender, RoutedEventArgs e)
        {
            if (tbInstallmentsCount.IsEnabled)
            {
                BindingExpression installmentNumberValidation = tbInstallmentsCount.GetBindingExpression(TextBox.TextProperty);
                installmentNumberValidation.UpdateSource();
                BindingExpression discountValidation = tbDiscountAmount.GetBindingExpression(TextBox.TextProperty);
                discountValidation.UpdateSource();
                if(!installmentNumberValidation.HasValidationError && !discountValidation.HasValidationError)
                {
                    EntityFactory.Receipt.installments = true;
                    int installments = int.Parse(tbInstallmentsCount.Text);
                    var amount = Math.Round(EntityFactory.Receipt.amount / installments, 2);
                    decimal discount = 0;
                    decimal.TryParse(tbDiscountAmount.Text, out discount);
                    EntityFactory.Receipt.discount = cbDiscount.SelectedIndex == 0 ? EntityFactory.Receipt.amount * discount / 100 : discount;
                    discount = cbDiscount.SelectedIndex == 0 ? amount * discount / 100 : discount;
                    receipts = new ObservableCollection<receipt>();
                    for (int i = 1; i <= installments; i++)
                    {
                        DateTime? payment_date = i == 1 ? DateTime.Today : (DateTime?)null;
                        receipt newRecepit = new receipt(EntityFactory.User.id, EntityFactory.Reservation.client_id, EntityFactory.Reservation.id, payment_date, amount, discount, true, i, 0);
                        receipts.Add(newRecepit);
                    }
                    datagridReceipts.ItemsSource = receipts;
                    datagridReceipts.Items.Refresh();
                    spPaymentPlan.Visibility = Visibility.Visible;
                }
            }
            else
            {
                EntityFactory.Receipt.installments = false;
                receipts = null;
                spPaymentPlan.Visibility = Visibility.Collapsed;
            }
            
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            receipt.Insert(EntityFactory.Receipt);
            if(receipts != null)
            {
                receipts.ToList().ForEach(r => 
                {
                    r.receipt_id = EntityFactory.Receipt.id;
                    receipt.Insert(r);
                });
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            atbClientSearch.SelectedItem = null;
            datagridReservation.ItemsSource = null;
            datagridReservation.Items.Refresh();
            tbInstallmentsButton.IsChecked = false;
            tbInstallmentsCount.IsEnabled = false;
            tbInstallmentsCount.Text = "";
            tbDiscountAmount.Text = "";
            datagridReceipts.ItemsSource = receipts = null;
            tbReceiptNumber.Text = tbReceiptNumber2.Text = tbReceiptNumber4.Text = "";
            tbClientName.Text = tbClientName2.Text = tbClientName4.Text = "";
            tbEventName.Text = tbEventName2.Text = tbEventName4.Text = "";
            tbPrice.Text = tbPrice2.Text = tbPrice4.Text = "";
            tvTerms.Items.Clear();
            spTerms.Visibility = Visibility.Collapsed;
            spPaymentPlan.Visibility = Visibility.Collapsed;
            payButton.Visibility = Visibility.Hidden;
            clientExpander.IsExpanded = true;
            receiptExpander.IsExpanded = false;
        }

        public void SetAllFields()
        {
            tbClientSearch.Text = EntityFactory.Client.ToString();
            datagridReservation.SelectedItem = null;
            ObservableCollection<reservation> allReservations = new ObservableCollection<reservation>();
            EntityFactory.Client.reservations.ToList().ForEach(r => allReservations.Add(r));
            EntityFactory.Client.attends_events.ToList().ForEach(r => allReservations.Add(r));
            datagridReservation.SelectedItem = EntityFactory.Reservation;
            datagridReservation.ItemsSource = allReservations;
            datagridReservation.Items.Refresh();

            tbReceiptNumber.Text = tbReceiptNumber2.Text = tbReceiptNumber4.Text = tbReceiptNumber.Text = tbReceiptNumber2.Text = tbReceiptNumber4.Text = EntityFactory.Receipt.serial_number;
            tbClientName.Text = tbClientName2.Text = tbClientName4.Text = EntityFactory.Client.ToString();
            tbEventName.Text = tbEventName2.Text = tbEventName4.Text = EntityFactory.Reservation._event != null ? EntityFactory.Reservation._event.name : "Nije odabran događaj";
            tbPrice.Text = tbPrice2.Text = tbPrice4.Text = EntityFactory.Receipt.amount + " €";
            SetReceiptFields(EntityFactory.Reservation);
            SetTreeview(EntityFactory.Reservation);

            payButton.Visibility = Visibility.Hidden;
            clientExpander.IsExpanded = true;
            receiptExpander.IsExpanded = false;
        }

    }
}
