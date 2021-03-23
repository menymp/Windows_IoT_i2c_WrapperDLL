using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using System.Diagnostics;
using System.Threading;

namespace I2C_Test
{
    //class I2C_Module
    //{



    //    private I2cDevice Device;
    //    private string AQS_str;


    //    public I2C_Mode_state Dev_state { get; set; }

    //    public I2cBusSpeed DeviceSpeed { get; set; }

    //    public DeviceInformationCollection Devices_reg { get; set; }



    //    public int SlaveAdd { get; set; }

    //    public I2C_Module(int SlaveAddress, string AQS_type, I2C_Speed_enum Speed)
    //    {
    //        AQS_str = AQS_type;
    //        DeviceSpeed = (I2cBusSpeed)Speed;
    //        SlaveAdd = SlaveAddress;
    //        Dev_state = I2C_Mode_state.I2C_SYSTEM_BUSY;
    //    }

    //    public async Task initcomunica()
    //    {
    //        var settings = new I2cConnectionSettings(SlaveAdd); //add del dispositivo Sa
    //        settings.BusSpeed = DeviceSpeed; //velocidad del dispositivo
    //        settings.SharingMode = I2cSharingMode.Shared; // ###### Nota: revisar efectos colaterales ########
    //        string aqs = I2cDevice.GetDeviceSelector(AQS_str); //buscamos un selector para el aqs I2CN
    //        var dis = await DeviceInformation.FindAllAsync(aqs);
    //        Device = await I2cDevice.FromIdAsync(dis[0].Id, settings); //conectamos con el dispositivo que coincida
    //        Dev_state = I2C_Mode_state.I2C_SYSTEM_OK;
    //    }

    //    public static async Task<IEnumerable<byte>> FindDevicesAsync(byte MinIdAddress, byte MaxIdAddress)
    //    {
    //        IList<byte> returnValue = new List<byte>();
    //        //selector de controladores en el sistema
    //        string aqs = I2cDevice.GetDeviceSelector();
    //        //buscamos el controlador del bus
    //        var dis = await DeviceInformation.FindAllAsync(aqs).AsTask();
    //        if (dis.Count > 0) //si se encuentran dispositivos
    //        {
    //            //const int minimumAddress = 0x08; //addres maximo y minimo para la busqueda
    //            //const int maximumAddress = 0x77;
    //            for (byte address = MinIdAddress; address <= MaxIdAddress; address++)
    //            {
    //                var settings = new I2cConnectionSettings(address);
    //                settings.BusSpeed = I2cBusSpeed.FastMode;
    //                settings.SharingMode = I2cSharingMode.Shared;
    //                //creamos un nuevo dispositivo con las caracteristicas y el Id 
    //                using (I2cDevice device = await I2cDevice.FromIdAsync(dis[0].Id, settings))
    //                {
    //                    if (device != null) //Si hay algun dispositio privado
    //                    {
    //                        try
    //                        {
    //                            byte[] writeBuffer = new byte[1] { 0 };
    //                            device.Write(writeBuffer);
    //                            // si no acontece exepciones un dispositivo respondio a esa direccion
    //                            returnValue.Add(address); //incorporamos el ID a la coleccion
    //                        }
    //                        catch
    //                        {
    //                            //significa que no hubo respuesta
    //                        }

    //                    }
    //                }
    //            }
    //        }

    //        return returnValue;
    //    }

    //    public I2C_Mode_state read_data(byte[] buff_in)
    //    {
    //        if (Dev_state == I2C_Mode_state.I2C_SYSTEM_OK)
    //        {
    //            try
    //            {
    //                Device.Read(buff_in);
    //                return I2C_Mode_state.I2C_SYSTEM_OK;
    //            }
    //            catch
    //            {
    //                return I2C_Mode_state.I2C_SYSTEM_FAULT;
    //            }
    //        }
    //        else
    //            return Dev_state;
    //    }

    //    public I2C_Mode_state write_data(byte[] buff_out)
    //    {
    //        if (Dev_state == I2C_Mode_state.I2C_SYSTEM_OK)
    //        {
    //            try
    //            {
    //                Device.Write(buff_out);
    //                return I2C_Mode_state.I2C_SYSTEM_OK;
    //            }
    //            catch
    //            {
    //                return I2C_Mode_state.I2C_SYSTEM_FAULT;
    //            }
    //        }
    //        else
    //            return Dev_state;
    //    }

    //    public I2C_Mode_state writeRead_data(byte[] buff_out, byte[] buff_in)
    //    {
    //        if (Dev_state == I2C_Mode_state.I2C_SYSTEM_OK)
    //        {
    //            try
    //            {
    //                Device.WriteRead(buff_out, buff_in);
    //                return I2C_Mode_state.I2C_SYSTEM_OK;
    //            }
    //            catch
    //            {
    //                return I2C_Mode_state.I2C_SYSTEM_FAULT;
    //            }
    //        }
    //        else
    //            return Dev_state;
    //    }
    //}

    //enum I2C_Mode_state
    //{
    //    I2C_MASTER,
    //    I2C_SLAVE,
    //    I2C_SYSTEM_OK,
    //    I2C_SYSTEM_FAULT,
    //    I2C_SYSTEM_NOINIT,
    //    I2C_SYSTEM_BUSY
    //}
    //enum I2C_Speed_enum
    //{
    //    I2C_STANDARD = 0,
    //    I2C_FAST
    //}
    //modos de funcionamiento del modulo de i2c

}
