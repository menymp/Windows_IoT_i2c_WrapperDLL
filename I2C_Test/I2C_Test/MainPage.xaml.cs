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

using System.Diagnostics;
//using Windows.Devices.I2c;
using ModuleI2C; //usar el namespace y declarar las clases como publicas
// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace I2C_Test
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private I2cDevice Device;

        private I2C_Module Dispositivo;
        private I2C_Module Dispositivo2;
        private IEnumerable<byte> coleccion;

        private DispatcherTimer periodicTimer;

        public MainPage()
        {
            this.InitializeComponent();
            initcomunica();
        }

        private async void initcomunica()
        {

            /*
            var settings = new I2cConnectionSettings(0x40); // Arduino address
            settings.BusSpeed = I2cBusSpeed.StandardMode;
            string aqs = I2cDevice.GetDeviceSelector("I2C1");
            var dis = await DeviceInformation.FindAllAsync(aqs);
            Device = await I2cDevice.FromIdAsync(dis[0].Id, settings);
            */
            //busqueda de dispositivos, realizarlo antes de hacer instancias, de lo contrario los dispositivos
            //conectados apareceran como null



            //Dispositivo = new I2C_Module(2, "I2C1", I2C_Speed_enum.I2C_STANDARD);//0X40
            //await Dispositivo.initcomunica();

            //Dispositivo2 = new I2C_Module(0x42, "I2C1", I2C_Speed_enum.I2C_STANDARD);
            //await Dispositivo2.initcomunica();

            //coleccion = await I2C_Module.FindDevicesAsync(0x40 - 1, 0x43);
            coleccion = await I2C_Module.FindDevicesAsync(0, 4);
            foreach (byte item in coleccion)
            {
                Debug.WriteLine(item);
                DevicesListTXT.Items.Add(item);
            }
            //while( I2C_Mode_state.I2C_SYSTEM_OK!= await Dispositivo.initcomunica(0x40));
            Debug.WriteLine("objeto iniciado");
            //while (Dispositivo.Init_ok != I2C_Mode_state.I2C_SYSTEM_OK) ;
            Debug.WriteLine("objeto terminado");

            //periodicTimer = new Timer(this.TimerCallback, null, 0, 1000); // Create a timmer

            /*
            periodicTimer = new DispatcherTimer();
            periodicTimer.Tick += TimerCallback;
            periodicTimer.Interval = new TimeSpan(0, 0, 1);
            periodicTimer.Start();
            */

        }

        private void TimerCallback(object state, Object E)
        {
            if (System.Diagnostics.Debugger.IsAttached == true)
            {
                periodicTimer.Stop();
            }

            byte[] RegAddrBuf = new byte[] { 0x40 };
            byte[] ReadBuf = new byte[1];
            byte[] WriteBuf = new byte[] { 0xE3 };

            try
            {
                //Device.Read(ReadBuf); // read the data
                if (Dispositivo.writeRead_data(WriteBuf, ReadBuf) == I2C_Mode_state.I2C_SYSTEM_OK)
                {
                    char[] cArray = System.Text.Encoding.UTF8.GetString(ReadBuf, 0, ReadBuf.Length).ToCharArray();  // Converte  Byte to Char
                    String c = new String(cArray);
                    Debug.WriteLine(c);
                }
            }
            catch (Exception f)
            {
                Debug.WriteLine(f.Message);
            }

            /*
            if (Dispositivo.read_data(ReadBuf) == I2C_Mode_state.I2C_SYSTEM_OK)
            {
            
                char[] cArray = System.Text.Encoding.UTF8.GetString(ReadBuf, 0, 5).ToCharArray();  // Converte  Byte to Char
                String c = new String(cArray);
                Debug.WriteLine(c);
            }*/

            // refresh the screen, note Im using a textbock @ UI
            /*
            var task = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>

            { temperatura.Text = c; });
            */
            if (System.Diagnostics.Debugger.IsAttached == true)
            {
                periodicTimer.Start();
            }
        }

        private void ArduinoWrite_Click(object sender, RoutedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached == true)
            {
                //periodicTimer.Stop();
            }

            byte[] RegAddrBuf = new byte[] { 2 };
            byte[] ReadBuf = new byte[1];
            byte[] WriteBuf = new byte[] { 1 };

            try
            {
                //Device.Read(ReadBuf); // read the data
                /*if (Dispositivo.writeRead_data(WriteBuf, ReadBuf) == I2C_Mode_state.I2C_SYSTEM_OK)
                {
                    char[] cArray = System.Text.Encoding.UTF8.GetString(ReadBuf, 0, ReadBuf.Length).ToCharArray();  // Converte  Byte to Char
                    String c = new String(cArray);
                    ArduinoText.Text = c;
                }*/

                Dispositivo.write_data(WriteBuf);
                Dispositivo.read_data(ReadBuf);
            }
            catch (Exception f)
            {
                Debug.WriteLine(f.Message);
            }
        }

        private void ARMwrite_Click(object sender, RoutedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached == true)
            {
                //periodicTimer.Stop();
            }

            byte[] RegAddrBuf = new byte[] { 0x42 };
            byte[] ReadBuf = new byte[1];
            byte[] WriteBuf = new byte[] { 0xE3 };

            try
            {
                //Device.Read(ReadBuf); // read the data
                if (Dispositivo2.writeRead_data(WriteBuf, ReadBuf) == I2C_Mode_state.I2C_SYSTEM_OK)
                {
                    char[] cArray = System.Text.Encoding.UTF8.GetString(ReadBuf, 0, ReadBuf.Length).ToCharArray();  // Converte  Byte to Char
                    String c = new String(cArray);
                    ARMText.Text = c;
                }
            }
            catch (Exception f)
            {
                Debug.WriteLine(f.Message);
            }
        }
    }
}
