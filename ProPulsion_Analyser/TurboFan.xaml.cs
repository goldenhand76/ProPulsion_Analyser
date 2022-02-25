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
    public sealed partial class TurboFan : Page
    {

        public float Specific_Thrust_fan_tmp { get; private set; }
        public float Specific_Thrust_core_tmp { get; private set; }
        public float Ratio_tmp { get; private set; }
        public float Percent1_tmp { get; private set; }
        public float Percent2_tmp { get; private set; }
        public float TSFC_tmp { get; private set; }
        public float Thermal_eff_tmp { get; private set; }
        public float Propulsive_eff_tmp { get; private set; }
        public float Total_eff_tmp { get; private set; }
        public Double V0 { get; private set; }
        public Double M0 { get; private set; }
        public Double Pt2 { get; private set; }







        public TurboFan()
        {
            this.InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)Seprate.IsChecked)
                {
                    Mixer_text.Visibility = Visibility.Collapsed;
                    Mixer_grid.Visibility = Visibility.Collapsed;
                    fan_Seprated.Visibility = Visibility.Visible;
                    fan_Mixed.Visibility = Visibility.Collapsed;


                }
                else if ((bool)Mixed.IsChecked)
                {
                    Mixer_text.Visibility = Visibility.Visible;
                    Mixer_grid.Visibility = Visibility.Visible;
                    fan_Seprated.Visibility = Visibility.Collapsed;
                    fan_Mixed.Visibility = Visibility.Visible;
                }
            }
            catch (NullReferenceException) { }
        }

        private void AB_toggle_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    try
                    {
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
              || String.IsNullOrEmpty(PI_b.Text) || String.IsNullOrEmpty(Cp_t.Text) || String.IsNullOrEmpty(Q_R.Text) || String.IsNullOrEmpty(etta_b.Text)
              || String.IsNullOrEmpty(ta_landa.Text) || String.IsNullOrEmpty(gamma_t.Text) || String.IsNullOrEmpty(etta_m.Text) || String.IsNullOrEmpty(e_t.Text)
              || String.IsNullOrEmpty(PI_f.Text) || String.IsNullOrEmpty(PI_fn.Text) || String.IsNullOrEmpty(e_f.Text) || String.IsNullOrEmpty(alpha_f.Text)
              || String.IsNullOrEmpty(PI_n.Text))
            {
                Progress.IsActive = true;
                await Task.Delay(1000);
                Progress.IsActive = false;

                cry.Visibility = Visibility.Visible;
                Zero.Visibility = Visibility.Collapsed;
                Intake.Visibility = Visibility.Collapsed;
                Fan.Visibility = Visibility.Collapsed;
                Fan_n.Visibility = Visibility.Collapsed;
                Compressor.Visibility = Visibility.Collapsed;
                Burner.Visibility = Visibility.Collapsed;
                Turbine.Visibility = Visibility.Collapsed;
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
                if (String.IsNullOrEmpty(PI_f.Text)) PI_f.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(PI_fn.Text)) PI_fn.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(e_f.Text)) e_f.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(alpha_f.Text)) alpha_f.BorderBrush = new SolidColorBrush(Colors.Red);
                if (String.IsNullOrEmpty(PI_n.Text)) PI_n.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                Progress.IsActive = true;
                await Task.Delay(1000);
                Progress.IsActive = false;

                cry.Visibility = Visibility.Collapsed;
                Zero.Visibility = Visibility.Visible;
                Intake.Visibility = Visibility.Visible;
                Fan.Visibility = Visibility.Visible;
                Fan_n.Visibility = Visibility.Visible;
                Compressor.Visibility = Visibility.Visible;
                Burner.Visibility = Visibility.Visible;
                Turbine.Visibility = Visibility.Visible;
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
                PI_f.BorderBrush = new SolidColorBrush(Colors.Green);
                PI_fn.BorderBrush = new SolidColorBrush(Colors.Green);
                e_f.BorderBrush = new SolidColorBrush(Colors.Green);
                alpha_f.BorderBrush = new SolidColorBrush(Colors.Green);
                PI_n.BorderBrush = new SolidColorBrush(Colors.Green);

                float T0 = float.Parse(T_zero.Text) + 273;
                float P0 = float.Parse(P_zero.Text);
                float gam_c = float.Parse(gamma_zero.Text);
                float CPc = float.Parse(Cp_c.Text);
                Double a0 = Math.Sqrt((gam_c - 1) * CPc * T0);
                if (M_V_0.SelectedIndex == 0)
                {
                    M0 = float.Parse(M_zero.Text);
                    V0 = M0 * a0;
                }
                else if (M_V_0.SelectedIndex == 1)
                {
                    V0 = float.Parse(M_zero.Text);
                    M0 = V0 / a0;
                }
                V_zero.Text = V0.ToString("F");
                Double Tt0 = T0 * (1 + ((gam_c - 1) * Math.Pow(M0, 2)) / 2);
                Tt_0.Text = Tt0.ToString("F");
                Double Pt0 = P0 * Math.Pow(1 + ((gam_c - 1) * Math.Pow(M0, 2)) / 2, ((gam_c) / (gam_c - 1)));
                Pt_0.Text = Pt0.ToString("F");

                Double Tt2 = Tt0;
                Tt_2.Text = Tt2.ToString("F");
                if (PI_ett_d.SelectedIndex == 0)
                {
                    float PId = float.Parse(PI_d.Text);
                    Pt2 = Pt0 * PId;
                }
                else if (PI_ett_d.SelectedIndex == 1)
                {
                    float etta_d = float.Parse(PI_d.Text);
                    Pt2 = P0 * Math.Pow(1 + (etta_d * (gam_c - 1) * Math.Pow(M0, 2) / 2),gam_c/(gam_c-1));
                }
                Pt_2.Text = Pt2.ToString("F");

                float PIf = float.Parse(PI_f.Text);
                float ef = float.Parse(e_f.Text);
                Double Pt13 = Pt2 * PIf;
                Pt_13.Text = Pt13.ToString("F"); 
                Double ta_f = Math.Pow(PIf, (gam_c - 1) / (ef * gam_c));
                Double Tt13 = ta_f * Tt2;
                Tt_13.Text = Tt13.ToString("F");

                float PIfn = float.Parse(PI_fn.Text);
                Double Pt19 = PIfn * Pt13;
                Pt_19.Text = Pt19.ToString("F");
                Double Tt19 = Tt13;
                Tt_19.Text = Tt19.ToString("F");
                float M19 = 1;
                Double P19 = Pt19 / Math.Pow(1 + ((gam_c - 1) * Math.Pow(M19, 2)) / 2, ((gam_c) / (gam_c - 1)));
                Double T19 = Tt19/(1 + ((gam_c - 1) * Math.Pow(M19, 2)) / 2);
                Double a19 = Math.Sqrt((gam_c - 1) * CPc * T19);
                Double V19_act = a19 * M19;
                V_19act.Text = V19_act.ToString("F");
                Double V19_eff = V19_act + Math.Pow(a19, 2) * ((1 - P0 / P19) / (gam_c * V19_act));
                V_19eff.Text = V19_eff.ToString("F");

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
                float alpha = float.Parse(alpha_f.Text);
                Double Tt5 = ((1 + fb) * CPt * Tt4 * Etta_m - CPc * (Tt3 - Tt2) - alpha * CPc * (Tt13 - Tt2))/((1+fb)*Etta_m*CPt);
                Tt_5.Text = Tt5.ToString("F");
                Double ta_t = Tt5 / Tt4;
                Double PIt = Math.Pow(ta_t, gam_t / ((gam_t - 1) * et));
                Double Pt5 = Pt4 * PIt;
                Pt_5.Text = Pt5.ToString("F");

                float PIn = float.Parse(PI_n.Text);
                Double Tt9 = Tt5;
                Tt_9.Text = Tt9.ToString("F");
                Double Pt9 = PIn * Pt5;
                Pt_9.Text = Pt9.ToString("F");
                Double M9 = 1;
                Double T9 = Tt9 / (1 + (gam_t - 1) * Math.Pow(M9, 2) / 2);
                Double a9 = Math.Sqrt((gam_t - 1) * CPt * T9);
                Double P9 = Pt9 / Math.Pow(1 + ((gam_t - 1) * Math.Pow(M9, 2)) / 2, ((gam_t) / (gam_t - 1)));
                Double V9_act = a9 * M9;
                V_9act.Text = V9_act.ToString("F");
                Double V9_eff = V9_act + (Math.Pow(a9, 2) * (1 - P0 / P9) / (gam_t * V9_act));
                V_9eff.Text = V9_eff.ToString("F");

                Double specific_t_fan = (alpha * (V19_eff - V0)) / ((1 + alpha) * a0);
                Specific_Thrust_fan.Text = specific_t_fan.ToString("F3");

                Double specific_t_core = ((1 + fb) * V9_eff - V0) / ((1 + alpha) * a0);
                Specific_Thrust_core.Text = specific_t_core.ToString("F3");

                Double R = specific_t_fan/specific_t_core;
                Ratio.Text = R.ToString("F3");

                Double fan_total = 100 * specific_t_fan / (specific_t_core + specific_t_fan);
                Percent1.Text = fan_total.ToString("F1");

                Double core_total = 100 * specific_t_core / (specific_t_core + specific_t_fan);
                Percent2.Text = core_total.ToString("F1");

                Double Fn = specific_t_fan + specific_t_core;
                Double tsfc = fb * Math.Pow(10,6) / ((1 + alpha) * a0 * Fn);
                TSFC.Text = tsfc.ToString("F1");

                Double etta_th = 100 * (alpha * Math.Pow(V19_eff, 2) + (1 + fb) * Math.Pow(V9_eff, 2) - (1 + alpha) * Math.Pow(V0, 2)) / (2 * fb * QR);
                Thermal_eff.Text = etta_th.ToString("F1");

                Double etta_p = 100 * (2 * Fn * (1 + alpha) * a0 * V0) / (alpha * Math.Pow(V19_eff, 2) + (1 + fb) * Math.Pow(V9_eff, 2) - (1 + alpha) * Math.Pow(V0, 2));
                Propulsive_eff.Text = etta_p.ToString("F1");


                Double etta_o = etta_p * etta_th / 100;
                overal_eff.Text = etta_o.ToString("F2");

                // TempOrary Specific_Thrust_fan
                if (float.Parse(Specific_Thrust_fan.Text) != Specific_Thrust_fan_tmp)
                {
                    double diff = float.Parse(Specific_Thrust_fan.Text) - Specific_Thrust_fan_tmp;
                    error0.Text = diff.ToString("F3");
                    if (diff > 0) error0.Foreground = new SolidColorBrush(Colors.Green);
                    else error0.Foreground = new SolidColorBrush(Colors.Red);
                    Specific_Thrust_fan_tmp = float.Parse(Specific_Thrust_fan.Text);
                }
                else
                {
                    error0.Text = string.Empty;
                }

                // TempOrary Specific_Thrust_core
                if (float.Parse(Specific_Thrust_core.Text) != Specific_Thrust_core_tmp)
                {
                    double diff = float.Parse(Specific_Thrust_core.Text) - Specific_Thrust_core_tmp;
                    error1.Text = diff.ToString("F3");
                    if (diff > 0) error1.Foreground = new SolidColorBrush(Colors.Green);
                    else error1.Foreground = new SolidColorBrush(Colors.Red);
                    Specific_Thrust_core_tmp = float.Parse(Specific_Thrust_core.Text);
                }
                else
                {
                    error1.Text = string.Empty;
                }

                // TempOrary Ratio
                if (float.Parse(Ratio.Text) != Ratio_tmp)
                {
                    double diff = float.Parse(Ratio.Text) - Ratio_tmp;
                    error11.Text = diff.ToString("F3");
                    if (diff > 0) error11.Foreground = new SolidColorBrush(Colors.Green);
                    else error11.Foreground = new SolidColorBrush(Colors.Red);
                    Ratio_tmp = float.Parse(Ratio.Text);
                }
                else
                {
                    error11.Text = string.Empty;
                }

                // TempOrary Percent1
                if (float.Parse(Percent1.Text) != Percent1_tmp)
                {
                    double diff = float.Parse(Percent1.Text) - Percent1_tmp;
                    error12.Text = diff.ToString("F1");
                    if (diff > 0) error12.Foreground = new SolidColorBrush(Colors.Green);
                    else error12.Foreground = new SolidColorBrush(Colors.Red);
                    Percent1_tmp = float.Parse(Percent1.Text);
                }
                else
                {
                    error12.Text = string.Empty;
                }

                // TempOrary Percent2
                if (float.Parse(Percent2.Text) != Percent2_tmp)
                {
                    double diff = float.Parse(Percent2.Text) - Percent2_tmp;
                    error13.Text = diff.ToString("F1");
                    if (diff > 0) error13.Foreground = new SolidColorBrush(Colors.Green);
                    else error13.Foreground = new SolidColorBrush(Colors.Red);
                    Percent2_tmp = float.Parse(Percent2.Text);
                }
                else
                {
                    error13.Text = string.Empty;
                }

                // TempOrary tsfc
                if (float.Parse(TSFC.Text) != TSFC_tmp)
                {
                    double diff = float.Parse(TSFC.Text) - TSFC_tmp;
                    error2.Text = diff.ToString("F1");
                    if (diff > 0) error2.Foreground = new SolidColorBrush(Colors.Green);
                    else error2.Foreground = new SolidColorBrush(Colors.Red);
                    TSFC_tmp = float.Parse(TSFC.Text);
                }
                else
                {
                    error2.Text = string.Empty;
                }

                // TempOrary Thermal_eff
                if (float.Parse(Thermal_eff.Text) != Thermal_eff_tmp)
                {
                    double diff = float.Parse(Thermal_eff.Text) - Thermal_eff_tmp;
                    error3.Text = diff.ToString("F1");
                    if (diff > 0) error3.Foreground = new SolidColorBrush(Colors.Green);
                    else error3.Foreground = new SolidColorBrush(Colors.Red);
                    Thermal_eff_tmp = float.Parse(Thermal_eff.Text);
                }
                else
                {
                    error3.Text = string.Empty;
                }

                // TempOrary Propulsive_eff
                if (float.Parse(Propulsive_eff.Text) != Propulsive_eff_tmp)
                {
                    double diff = float.Parse(Propulsive_eff.Text) - Propulsive_eff_tmp;
                    error4.Text = diff.ToString("F1");
                    if (diff > 0) error4.Foreground = new SolidColorBrush(Colors.Green);
                    else error4.Foreground = new SolidColorBrush(Colors.Red);
                    Propulsive_eff_tmp = float.Parse(Propulsive_eff.Text);
                }
                else
                {
                    error4.Text = string.Empty;
                }

                // TempOrary Propulsive_eff
                if (float.Parse(overal_eff.Text) != Total_eff_tmp)
                {
                    double diff = float.Parse(overal_eff.Text) - Total_eff_tmp;
                    error5.Text = diff.ToString("F2");
                    if (diff > 0) error5.Foreground = new SolidColorBrush(Colors.Green);
                    else error5.Foreground = new SolidColorBrush(Colors.Red);
                    Total_eff_tmp = float.Parse(overal_eff.Text);
                }
                else
                {
                    error5.Text = string.Empty;
                }
            }

        }

        private void M_V_0_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            if (par1 == null) return;
            if (combo.SelectedIndex == 0) par1.Text = "Mach";
            else if (combo.SelectedIndex == 1) par1.Text = "m/s";
        }

    }
}
