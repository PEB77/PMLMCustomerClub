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
using System.Globalization;
using DevExpress.XtraEditors;
using Org.BouncyCastle.Bcpg;
using System.Drawing;
using DevExpress.Xpf.Editors.Helpers;

namespace PMLMCustomerClub.CustomControls
{
    /// <summary>
    /// Interaction logic for CalenderViewer.xaml
    /// </summary>
    public partial class CalenderViewer : UserControl
    {
        public CalenderViewer()
        {
            InitializeComponent();
            SetMonthComboBox();
            PersianCalenderChange += CalenderViewer_PersianCalenderChange;
            GregorianCalenderChange += CalenderViewer_GregorianCalenderChange;
        }

        public object TitleContent
        {
            get => TitleLabel.Content;
            set => TitleLabel.Content = value;
        }

        private bool SettingComponent = false;
        private int calenderType;
        private DateTime OutputDate;

        private delegate void PersianChangeCalenderEventHandler(int year, int month, int day);
        private delegate void GregorianChangeCalenderEventHandler(int year, int month, int day);
        public delegate void CalenderChangeEventHandler(DateTime date);

        private event PersianChangeCalenderEventHandler PersianCalenderChange;
        private event GregorianChangeCalenderEventHandler GregorianCalenderChange;
        public event CalenderChangeEventHandler CalenderChange;

        private enum PersianMonths
        {
            Farvardin,
            Ordibehesht,
            Khordad,
            Tir,
            Mordad,
            Shahrivar,
            Mehr,
            Aban,
            Azar,
            Dey,
            Bahman,
            Esfand
        }

        private enum GregorianMonths
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

        private void SetMonthComboBox()
        {
            List<string> persianMonthsList = typeof(PersianMonths).GetEnumNames().ToList();
            List<string> gregorianMonthsList = typeof(GregorianMonths).GetEnumNames().ToList();
            PersianMonthEditor.ItemsSource = persianMonthsList;
            GregorianMonthEditor.ItemsSource = gregorianMonthsList;
        }

        public void SetDate(DateTime dateTime)
        {
            OutputDate = dateTime;
            SetComponent();
        }

        private void SetComponent()
        {
            SettingComponent = true;
            GregorianYearEditor.Value = OutputDate.Year;
            GregorianMonthEditor.SelectedIndex = OutputDate.Month - 1;
            GregorianDayEditor.Value = OutputDate.Day;
            PersianCalendar pCalendar = new PersianCalendar();
            PersianYearEditor.Value = pCalendar.GetYear(OutputDate);
            PersianMonthEditor.SelectedIndex = pCalendar.GetMonth(OutputDate) - 1;
            PersianDayEditor.Value = pCalendar.GetDayOfMonth(OutputDate);
            SettingComponent = false;
        }

        private void DXTabControl_SelectionChanged(object sender, DevExpress.Xpf.Core.TabControlSelectionChangedEventArgs e)
        {
            calenderType = CalenderTab.SelectedIndex;
        }

        private void PersianYearEditor_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SettingComponent) return;
            int year = (int)PersianYearEditor.Value;
            if (year <= 0) return;
            PersianCalenderChange?.Invoke(year, PersianMonthEditor.SelectedIndex + 1, (int)PersianDayEditor.Value);
        }

        private void PersianMonthEditor_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SettingComponent) return;
            int month = PersianMonthEditor.SelectedIndex + 1;
            if (month <= 0) return;
            PersianCalenderChange?.Invoke((int)PersianYearEditor.Value, month, (int)PersianDayEditor.Value);
        }

        private void PersianDayEditor_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SettingComponent) return;
            int day = (int)PersianDayEditor.Value;
            if (day <= 0) return;
            PersianCalenderChange?.Invoke((int)PersianYearEditor.Value, PersianMonthEditor.SelectedIndex + 1, day);
        }

        private void GregorianYearEditor_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SettingComponent) return;
            int year = (int)GregorianYearEditor.Value;
            if (year <= 0) return;
            GregorianCalenderChange?.Invoke(year, OutputDate.Month, OutputDate.Day);
        }

        private void GregorianMonthEditor_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SettingComponent) return;
            int month = GregorianMonthEditor.SelectedIndex + 1;
            if (month <= 0) return;
            GregorianCalenderChange?.Invoke(OutputDate.Year, month, OutputDate.Day);
        }

        private void GregorianDayEditor_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SettingComponent) return;
            int day = (int)GregorianDayEditor.Value;
            if (day <= 0) return;
            GregorianCalenderChange?.Invoke(OutputDate.Year, OutputDate.Month, day);
        }

        private void CalenderViewer_GregorianCalenderChange(int year, int month, int day)
        {
            DateTime outDate;
            if (CheckGregorianDate(year, month, day, out outDate))
            {
                OutputDate = outDate;
                CalenderChange?.Invoke(OutputDate);
            }
            else
            {
                XtraMessageBox.Show("Careful, the date you input is not exist in calender");
            }
            SetComponent();
        }

        private void CalenderViewer_PersianCalenderChange(int year, int month, int day)
        {
            DateTime outDate;
            if (CheckPersianDate(year, month, day, out outDate))
            {
                OutputDate = outDate;
                CalenderChange?.Invoke(OutputDate);
            }
            else
            {
                XtraMessageBox.Show("Careful, the date you input is not exist in calender");
            }
            SetComponent();
        }

        private bool CheckGregorianDate(int year, int month, int day, out DateTime outDate)
        {
            string sDate = ToStringMaker(year, month, day);
            if (DateTime.TryParse(sDate,out outDate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckPersianDate(int year, int month, int day, out DateTime outDate)
        {
            string sDate = ToStringMaker(year, month, day);
            PersianCalendar pCalender = new PersianCalendar();
            int monthDays = pCalender.GetDaysInMonth(year, month);
            if (monthDays < day)
            {
                outDate = DateTime.Now;
                return false;
            }
            else
            {
                outDate = new DateTime(year, month, day, pCalender);
                return true;
            }
        }
        private string ToStringMaker(int year, int month, int day)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(year.ToString());
            builder.Append('-');
            builder.Append(month.ToString("00"));
            builder.Append('-');
            builder.Append(day.ToString("00"));
            return builder.ToString();
        }
    }
}
