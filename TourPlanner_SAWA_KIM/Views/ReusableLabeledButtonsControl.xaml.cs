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

namespace TourPlanner_SAWA_KIM.Views
{
    /// <summary>
    /// Interaction logic for ReusableLabeledButtonsControl.xaml
    /// </summary>
    public partial class ReusableLabeledButtonsControl : UserControl
    {
        public ReusableLabeledButtonsControl()
        {
            InitializeComponent();
        }

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(string), typeof(ReusableLabeledButtonsControl), new PropertyMetadata("Default Label"));

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(ReusableLabeledButtonsControl));

        public ICommand RemoveCommand
        {
            get { return (ICommand)GetValue(RemoveCommandProperty); }
            set { SetValue(RemoveCommandProperty, value); }
        }

        public static readonly DependencyProperty RemoveCommandProperty =
            DependencyProperty.Register("RemoveCommand", typeof(ICommand), typeof(ReusableLabeledButtonsControl));

        public ICommand MoreCommand
        {
            get { return (ICommand)GetValue(MoreCommandProperty); }
            set { SetValue(MoreCommandProperty, value); }
        }

        public static readonly DependencyProperty MoreCommandProperty =
            DependencyProperty.Register("MoreCommand", typeof(ICommand), typeof(ReusableLabeledButtonsControl));
    }

}
