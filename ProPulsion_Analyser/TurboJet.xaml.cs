using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProPulsion_Analyser
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TurboJet : Page
    {
        private object wait;

        public float Thermal_eff_tmp { get; private set; }
        public float TSFC_tmp { get; private set; }
        public float Specific_Thrust_tmp { get; private set; }
        public float Propulsive_eff_tmp { get; private set; }
        public float Total_eff_tmp { get; private set; }
        public Double Tt7 { get; private set; }
        public Double Pt7 { get; private set; }
        public Double fab { get; private set; }
        public Double gam_ab { get; private set; }
        public Double QR_ab { get; private set; }
        public Double CPab { get; private set; }




        public TurboJet()
        {
            this.InitializeComponent();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    try { 
                        AfterBurner.Foreground = new SolidColorBrush(Colors.White);
                        AB_view.Visibility = Visibility.Visible;
                        AB_view_off.Visibility = Visibility.Collapsed;
                    }
                    catch (NullReferenceException) { };
                }
                else
                {
                    AfterBurner.Foreground = new SolidColorBrush(Colors.DarkGray);
                    AB_view.Visibility = Visibility.Collapsed;
                    AB_view_off.Visibility = Visibility.Visible;
                }
            }
        }
        
        private async void Calculate_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(T_zero.Text) || String.IsNullOrEmpty(P_zero.Text) || String.IsNullOrEmpty(M_zero.Text) || String.IsNullOrEmpty(gamma_zero.Text)
               || String.IsNullOrEmpty(Cp_c.Text) || String.IsNullOrEmpty(PI_d.Text) || String.IsNullOrEmpty(PI_c.Text) || String.IsNullOrEmpty(e_c.Text)
               || String.IsNullOrEmpty(PI_b.Text)|| String.IsNullOrEmpty(Cp_t.Text) || String.IsNullOrEmpty(Q_R.Text)|| String.IsNullOrEmpty(etta_b.Text)
               || String.IsNullOrEmpty(ta_landa.Text)|| String.IsNullOrEmpty(gamma_t.Text)|| String.IsNullOrEmpty(etta_m.Text) || String.IsNullOrEmpty(e_t.Text)
               || String.IsNullOrEmpty(PI_AB.Text) || String.IsNullOrEmpty(ta_landa_AB.Text) || String.IsNullOrEmpty(Cp_AB.Text) || String.IsNullOrEmpty(gamma_AB.Text)
               || String.IsNullOrEmpty(PI_n.Text) || String.IsNullOrEmpty(P_nine.Text) || String.IsNullOrEmpty(QR_AB.Text) || String.IsNullOrEmpty(etta_AB.Text)
               || String.IsNullOrEmpty(PI_AB_off.Text))
            {
                Progress.IsActive = true;
                await Task.Delay(1000);
                Progress.IsActive = false;

                cry.Visibility = Visibility.Visible;
                Zero.Visibility = Visibility.Collapsed;
                Intake.Visibility = Visibility.Collapsed;
                Compressor.Visibility = Visibility.Collapsed;
                Burner.Visibility = Visibility.Collapsed;
                Turbine.Visibility = Visibility.Collapsed;
                AB.Visibility = Visibility.Collapsed;
                Nozzle.Visibility = Visibility.Collapsed;
                Performance.Visibility = Visibility.Collapsed;
                sp_thrust.Visibility = Visibility.Collapsed;
                sfc.Visibility = Visibility.Collapsed;
                ther_eff.Visibility = Visibility.Collapsed;
                pro_eff.Visibility = Visibility.Collapsed;
                pro_eff2.Visibility = Visibility.Collapsed;


                if (String.IsNullOrEmpty(T_zero.Text)) T_zero.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(P_zero.Text)) P_zero.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(M_zero.Text)) M_zero.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(gamma_zero.Text)) gamma_zero.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(Cp_c.Text)) Cp_c.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(PI_d.Text)) PI_d.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(PI_c.Text)) PI_c.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(e_c.Text)) e_c.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(PI_b.Text)) PI_b.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(Cp_t.Text)) Cp_t.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(Q_R.Text)) Q_R.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(etta_b.Text)) etta_b.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(ta_landa.Text)) ta_landa.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(gamma_t.Text)) gamma_t.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(etta_m.Text)) etta_m.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(e_t.Text)) e_t.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(PI_AB.Text)) PI_AB.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(PI_AB_off.Text)) PI_AB.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(ta_landa_AB.Text)) ta_landa_AB.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(Cp_AB.Text)) Cp_AB.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(gamma_AB.Text)) gamma_AB.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(PI_n.Text)) PI_n.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(P_nine.Text)) P_nine.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(QR_AB.Text)) QR_AB.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(etta_AB.Text)) etta_AB.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                Progress.IsActive = true;
                await Task.Delay(1000);
                Progress.IsActive = false;

                cry.Visibility = Visibility.Collapsed;
                Zero.Visibility = Visibility.Visible;
                Intake.Visibility = Visibility.Visible;
                Compressor.Visibility = Visibility.Visible;
                Burner.Visibility = Visibility.Visible;
                Turbine.Visibility = Visibility.Visible;
                AB.Visibility = Visibility.Visible;
                Nozzle.Visibility = Visibility.Visible;
                Performance.Visibility = Visibility.Visible;
                sp_thrust.Visibility = Visibility.Visible;
                sfc.Visibility = Visibility.Visible;
                ther_eff.Visibility = Visibility.Visible;
                pro_eff.Visibility = Visibility.Visible;
                pro_eff2.Visibility = Visibility.Visible;

                T_zero.BorderBrush = new SolidColorBrush(Colors.Green);
                 P_zero.BorderBrush = new SolidColorBrush(Colors.Green);
                 M_zero.BorderBrush = new SolidColorBrush(Colors.Green);
                 gamma_zero.BorderBrush = new SolidColorBrush(Colors.Green);
                 Cp_c.BorderBrush = new SolidColorBrush(Colors.Green);
                 PI_d.BorderBrush = new SolidColorBrush(Colors.Green);
                 PI_c.BorderBrush = new SolidColorBrush(Colors.Green);
                 e_c.BorderBrush = new SolidColorBrush(Colors.Green);
                 PI_b.BorderBrush = new SolidColorBrush(Colors.Green);
                 Cp_t.BorderBrush = new SolidColorBrush(Colors.Green);
                 Q_R.BorderBrush = new SolidColorBrush(Colors.Green);
                 etta_b.BorderBrush = new SolidColorBrush(Colors.Green);
                 ta_landa.BorderBrush = new SolidColorBrush(Colors.Green);
                 gamma_t.BorderBrush = new SolidColorBrush(Colors.Green);
                 etta_m.BorderBrush = new SolidColorBrush(Colors.Green);
                 e_t.BorderBrush = new SolidColorBrush(Colors.Green);
                 PI_AB.BorderBrush = new SolidColorBrush(Colors.Green);
                 PI_AB_off.BorderBrush = new SolidColorBrush(Colors.Green);
                 ta_landa_AB.BorderBrush = new SolidColorBrush(Colors.Green);
                 Cp_AB.BorderBrush = new SolidColorBrush(Colors.Green);
                 gamma_AB.BorderBrush = new SolidColorBrush(Colors.Green);
                 PI_n.BorderBrush = new SolidColorBrush(Colors.Green);
                 P_nine.BorderBrush = new SolidColorBrush(Colors.Green);
                 QR_AB.BorderBrush = new SolidColorBrush(Colors.Green);
                 etta_AB.BorderBrush = new SolidColorBrush(Colors.Green);

                float T0 = float.Parse(T_zero.Text) + 273;
                float P0 = float.Parse(P_zero.Text);
                float M0 = float.Parse(M_zero.Text);
                float gam_c = float.Parse(gamma_zero.Text);
                float CPc = float.Parse(Cp_c.Text);
                float PId = float.Parse(PI_d.Text);
                Double a0 = Math.Sqrt((gam_c - 1) * CPc * T0);
                Double V0 = M0 * a0;
                V_zero.Text = V0.ToString("F");
                Double Tt0 = T0 * (1 + ((gam_c - 1) * Math.Pow(M0, 2)) / 2);
                Tt_0.Text = Tt0.ToString("F");
                Double Pt0 = P0 * Math.Pow(1 + ((gam_c - 1) * Math.Pow(M0, 2)) / 2 , ((gam_c) / (gam_c - 1)));
                Pt_0.Text = Pt0.ToString("F");


                Double Tt2 = Tt0;
                Tt_2.Text = Tt2.ToString("F");
                Double Pt2 = Pt0 * PId;
                Pt_2.Text = Pt2.ToString("F");


                float PIc = float.Parse(PI_c.Text);
                float ec = float.Parse(e_c.Text);
                Double Ta_c = Math.Pow(PIc, (gam_c - 1) / (ec * gam_c));        
                Double Tt3 = Tt2 * Ta_c;
                Tt_3.Text = Tt3.ToString("F");
                Double Pt3 = Pt2 * PIc;
                Pt_3.Text = Pt3.ToString("F");


                float PIb = float.Parse(PI_b.Text);
                float CPt = float.Parse(Cp_t.Text);
                float QR = float.Parse(Q_R.Text) * 1000;
                float Etta_b = float.Parse(etta_b.Text);
                float Ta_landa = float.Parse(ta_landa.Text);
                Double Tt4 = CPc * T0 * Ta_landa / CPt;
                Tt_4.Text = Tt4.ToString("F");
                Double Pt4 = Pt3 * PIb;
                Pt_4.Text = Pt4.ToString("F");
                Double fb = (CPt * Tt4 - CPc * Tt3) / (QR * Etta_b - CPt * Tt4);
                f.Text = fb.ToString("F5");


                float gam_t = float.Parse(gamma_t.Text);
                float Etta_m = float.Parse(etta_m.Text);
                float et = float.Parse(e_t.Text);
                Double Tt5 = (Tt4 * Etta_m * CPt * (1 + fb) - CPc * (Tt3 - Tt2)) / (CPt * Etta_m * (1 + fb));
                Tt_5.Text = Tt5.ToString("F");
                Double ta_t = Tt5 / Tt4;
                Double PIt = Math.Pow(ta_t, gam_t / ((gam_t - 1) * et));
                Double Pt5 = Pt4 * PIt;
                Pt_5.Text = Pt5.ToString("F");


                if (AB_toggle.IsOn) { 
                    float PIab = float.Parse(PI_AB.Text);
                    float Ta_landa_AB = float.Parse(ta_landa_AB.Text);
                    CPab = float.Parse(Cp_AB.Text);
                    gam_ab = float.Parse(gamma_AB.Text);
                    QR_ab = float.Parse(QR_AB.Text) * 1000;
                    float Etta_ab = float.Parse(etta_AB.Text);
                    Tt7 = CPc * T0 * Ta_landa_AB / CPab;
                    Tt_7.Text = Tt7.ToString("F");
                    Pt7 = PIab * Pt5;
                    Pt_7.Text = Pt7.ToString("F");
                    fab = ((1 + fb) * (Tt7 * CPab - Tt5 * CPt)) / (QR_ab * Etta_ab - Tt7 * CPab);
                    f_AB.Text = fab.ToString("F5");
                    AB.Visibility = Visibility.Visible;
                }
                else
                {
                    float PIab = float.Parse(PI_AB_off.Text);
                    CPab = CPt;
                    gam_ab = gam_t;
                    fab = 0;
                    QR_ab = 0;
                    Tt7 = Tt5;
                    Tt_7.Text = Tt7.ToString("F");
                    Pt7 = PIab * Pt5;
                    Pt_7.Text = Pt7.ToString("F");
                    AB.Visibility = Visibility.Collapsed;
                }



                float PIn = float.Parse(PI_n.Text);
                float P9 = float.Parse(P_nine.Text);
                Double Tt9 = Tt7;
                Tt_9.Text = Tt9.ToString("F");
                Double Pt9 = PIn * Pt7;
                Pt_9.Text = Pt9.ToString("F");
                Double M9 = Math.Sqrt((2 / (gam_ab - 1)) * (Math.Pow(Pt9 / P9, ((gam_ab - 1) / gam_ab)) - 1));
                Double T9 = Tt9 / (1 + (gam_ab - 1) * Math.Pow(M9, 2) / 2);
                Double a9 = Math.Sqrt((gam_ab - 1) * CPab * T9);
                Double V9 = a9 * M9;
                V_nine.Text = V9.ToString("F");

                Double specific_t = (1 + fb + fab) * (V9 / a0) - M0;
                Specific_Thrust.Text = specific_t.ToString("F3");

                Double tsfc = Math.Pow(10,6) * (fb + fab)/(specific_t * a0);
                TSFC.Text = tsfc.ToString("F1");

                Double etta_th = 100 * ((1 + fb + fab) * Math.Pow(V9, 2) - Math.Pow(V0, 2)) / (2 * fb * QR + 2 * fab * QR_ab);
                Thermal_eff.Text = etta_th.ToString("F1");

                Double etta_p = 100 * (specific_t * a0 * V0) / ((1 + fb + fab) * Math.Pow(V9, 2) / 2 - Math.Pow(V0, 2) / 2);
                Propulsive_eff.Text = etta_p.ToString("F1");

                Double etta_p2 = 100 * 2 / (1 + V9 / V0);
                Total_eff.Text = etta_p2.ToString("F1");

                // TempOrary specific_t
                if (float.Parse(Specific_Thrust.Text) != Specific_Thrust_tmp)
                {
                    double diff = float.Parse(Specific_Thrust.Text) - Specific_Thrust_tmp;
                    error0.Text = diff.ToString("F3");
                    if (diff > 0) error0.Foreground = new SolidColorBrush(Colors.Green);
                    else error0.Foreground = new SolidColorBrush(Colors.Red);
                    Specific_Thrust_tmp = float.Parse(Specific_Thrust.Text);
                }
                else
                {
                    error0.Text = string.Empty;
                }

                // TempOrary tsfc
                if (float.Parse(TSFC.Text) != TSFC_tmp)
                {
                    double diff = float.Parse(TSFC.Text) - TSFC_tmp;
                    error1.Text = diff.ToString("F1");
                    if (diff > 0) error1.Foreground = new SolidColorBrush(Colors.Green);
                    else error1.Foreground = new SolidColorBrush(Colors.Red);
                    TSFC_tmp = float.Parse(TSFC.Text);
                }
                else
                {
                    error1.Text = string.Empty;
                }

                // TempOrary Thermal_eff
                if (float.Parse(Thermal_eff.Text) != Thermal_eff_tmp)
                {
                    double diff = float.Parse(Thermal_eff.Text) - Thermal_eff_tmp;
                    error2.Text = diff.ToString("F1");
                    if (diff > 0) error2.Foreground = new SolidColorBrush(Colors.Green);
                    else error2.Foreground = new SolidColorBrush(Colors.Red);
                    Thermal_eff_tmp = float.Parse(Thermal_eff.Text);
                }
                else
                {
                    error2.Text = string.Empty;
                }

                // TempOrary Propulsive_eff
                if (float.Parse(Propulsive_eff.Text) != Propulsive_eff_tmp)
                {
                    double diff = float.Parse(Propulsive_eff.Text) - Propulsive_eff_tmp;
                    error3.Text = diff.ToString("F1");
                    if (diff > 0) error3.Foreground = new SolidColorBrush(Colors.Green);
                    else error3.Foreground = new SolidColorBrush(Colors.Red);
                    Propulsive_eff_tmp = float.Parse(Propulsive_eff.Text);
                }
                else
                {
                    error3.Text = string.Empty;
                }

                // TempOrary Propulsive_eff
                if (float.Parse(Total_eff.Text) != Total_eff_tmp)
                {
                    double diff = float.Parse(Total_eff.Text) - Total_eff_tmp;
                    error4.Text = diff.ToString("F1");
                    if (diff > 0) error4.Foreground = new SolidColorBrush(Colors.Green);
                    else error4.Foreground = new SolidColorBrush(Colors.Red);
                    Total_eff_tmp = float.Parse(Total_eff.Text);
                }
                else
                {
                    error4.Text = string.Empty;
                }
            }


        }
    }
}
