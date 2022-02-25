using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ProPulsion_Analyser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Hamburger_Button_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }


        private void TurboJet_panel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;
        }

        private void TurboFan_panel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;
        }

        private void TurboProp_panel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;

        }

        private void MyBox_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
        }

        private void MyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TurboJetItem.IsSelected) 
            {
                MainPagePic.Opacity = 0;
                NewFrame.Navigate(typeof(TurboJet)); 
            }
            if (TurboFanItem.IsSelected)
            {
                MainPagePic.Opacity = 0;
                NewFrame.Navigate(typeof(TurboFan));
            }
            if (TurboPropItem.IsSelected)
            {
                MainPagePic.Opacity = 0;
                NewFrame.Navigate(typeof(TurboProp));
            }

        }
    }
}
