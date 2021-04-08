using GlobalThinkersHelper.Model.Entities;
using Syncfusion.UI.Xaml.Schedule;
using Syncfusion.Windows.Shared;
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
    /// Interaction logic for MonthSchedule.xaml
    /// </summary>
    public partial class MonthSchedule : UserControl
    {
        List<term> terms = new List<term>();
        public MonthSchedule()
        {
            InitializeComponent();
            terms = term.SelectAll();
            terms.ForEach(t =>
            {
                schedule.Appointments.Add(
                    new ScheduleAppointment() { StartTime = new DateTime(t.rental_date.Year, t.rental_date.Month, t.rental_date.Day, t.rent_time_start.Hours, t.rent_time_start.Minutes, t.rent_time_start.Seconds),
                        EndTime = new DateTime(t.rental_date.Year, t.rental_date.Month, t.rental_date.Day, t.rent_time_end.Hours, t.rent_time_end.Minutes, t.rent_time_end.Seconds),
                        Subject = t.reservation.client.ToString(), Location = t.hall.name, AllDay = false,
                        AppointmentBackground = t.hall_id % 5 == 0 ? (SolidColorBrush)new BrushConverter().ConvertFrom("#4a148c") : t.hall_id % 3 == 0 ? (SolidColorBrush)new BrushConverter().ConvertFrom("#1e88e5") : t.hall_id % 2 == 0 ? Brushes.Green :
                         (SolidColorBrush)new BrushConverter().ConvertFrom("#dd2c00")
                    });
            });

        }

        private void schedule_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (schedule.ScheduleType.Equals(ScheduleType.Month))
            {
                schedule.ScheduleType = ScheduleType.Day;
                btnBack.Visibility = Visibility.Visible;
                tbTitle.Text = "Dnevni pregled rezervacija";
                DependencyObject dependencyobject = ((DependencyObject)e.OriginalSource);
                if (dependencyobject != null)
                {
                    dependencyobject = VisualTreeHelper.GetParent(dependencyobject);
                }
                if (dependencyobject is ScheduleMonthDateContentControl)
                {
                    DateTime selectedDate = ((ScheduleMonthDateContentControl)dependencyobject).Date;
                    if (schedule.SelectedDate.CompareTo(selectedDate) > 0)
                    {
                        int days = (schedule.SelectedDate - selectedDate).Days;
                        while (days-- != 0)
                        {
                            schedule.MoveToPreviousView();
                        }
                    }
                    else if (schedule.SelectedDate.CompareTo(selectedDate) < 0)
                    {
                        int days = (selectedDate - schedule.SelectedDate).Days;
                        while (days-- > 0)
                        {
                            schedule.MoveToNextView();
                        }
                    }
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            schedule.ScheduleType = ScheduleType.Month;
            btnBack.Visibility = Visibility.Hidden;
            tbTitle.Text = "Mjesečni pregled rezervacija";
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selected = schedule.SelectedAppointment;
            var index = schedule.Appointments.IndexOf(schedule.Appointments.First(a => a.Subject.Equals(selected.Subject) && a.Location.Equals(selected.Location) && a.StartTime.Equals(selected.StartTime) && a.EndTime.Equals(selected.EndTime)));
            CreateReservation createReservationControl = new CreateReservation();
            EntityFactory.Term = terms[index];
            EntityFactory.Reservation = EntityFactory.Term.reservation;
            createReservationControl.Update_Reservation();
            Content = createReservationControl;
        }
    }
    
}
