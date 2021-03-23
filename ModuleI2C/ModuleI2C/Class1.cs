
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

namespace ModuleI2C
{

    /// <summary>
    /// Clase para el manejo bajo comandos de I2C
    /// menymp 2019
    /// </summary>
    public class Wrapperi2c
    {
        /// <summary>
        /// Interfaz accesible de I2C
        /// </summary>
        public I2C_Module objInterface;
        /// <summary>
        /// Direcciones de dispositivos encontrados en el bus I2C
        /// </summary>
        private IEnumerable<byte> Devices;
        /// <summary>
        /// Manifiesto de capacidades
        /// </summary>
        public readonly string Manifest = "I2C,FUNCT:Read,Write,ReadWrite";
        /// <summary>
        /// Inicializa la interfaz de I2C
        /// </summary>
        /// <param name="Interface">Nombre de interface en el sistema</param>
        /// <param name="Args">Argumentos de inicializacion de busqueda Address Min Address Max</param>
        /// <returns></returns>
        public async Task Init(string Interface,string Args)
        {
            var Tokens = Args.Split(',');

            var ID_DOWN = Convert.ToByte(Convert.ToInt16(Tokens[0]));
            var ID_UP = Convert.ToByte(Convert.ToInt16(Tokens[1]));
            objInterface = new I2C_Module();
            Devices = await I2C_Module.FindDevicesAsync(ID_DOWN, ID_UP);
            //await objInterface.initcomunica(ID, Interface, I2C_Speed_enum.I2C_STANDARD);

        }
        /// <summary>
        /// Ejecuta un comando de la interfaz
        /// </summary>
        /// <param name="CMD">Comando a ejecutar</param>
        /// <param name="Args">Argumentos</param>
        /// <returns>Resultado</returns>
        public async Task<string> ExecuteCMD(string CMD, string Args)
        {
            string Result = "ERR";
            switch (CMD)
            {
                case "Read":
                    //'var argB = Encoding.Unicode.GetBytes(Args)
                    var argLen = Convert.ToInt16(Args);
                    byte[] Buff_read = new byte[argLen];
                    Read(Buff_read);
                    //Result = Encoding.Unicode.GetString(Buff_read);
                    //Console.Write
                    Result = "";
                    foreach(byte item in Buff_read)Result += Convert.ToChar(item);
                    break;

                case "Write":
                    var Buff_writeS = Encoding.Unicode.GetBytes(Args);
                    var Buff_write = Buff_writeS.Where((x, i) => i % 2 == 0).ToArray();
                    Write(Buff_write);
                    Result = "OK";
                    break;

                case "ReadWrite":
                    var Buff_sendS = Encoding.Unicode.GetBytes(Args);
                    var Buff_send = Buff_sendS.Where((x, i) => i % 2 == 0).ToArray();
                    byte[] Buff_receive = new byte[Buff_send.Length];
                    ReadWrite(Buff_send, Buff_receive);
                    //Result = Encoding.Unicode.GetString(Buff_receive);
                    Debug.Write(Convert.ToString(Buff_receive[0]));
                    Result = "";
                    foreach (byte item in Buff_receive) Result += Convert.ToChar(item);
                    break;

                case "FindDevicesAsync":
                    //pendiente
                    break;

                case "Set":
                    Result = "ERR";
                    bool IsFound = false;
                    var Tokens = Args.Split(',');
                    var ByteDeviceID = Convert.ToByte(Convert.ToInt16(Tokens[0]));
                    foreach (byte Item in Devices)
                    {
                        if (Item == ByteDeviceID)
                        {
                            IsFound = true;
                            await objInterface.initcomunica(Item, Tokens[1], I2C_Speed_enum.I2C_STANDARD); ;
                        }
                    }

                    if (IsFound == true) Result = "OK";
                    break;

                default:
                    Result = "ERR";
                    break;

            }
            return Result;
        }
        /// <summary>
        /// Lee un buffer de valores
        /// </summary>
        /// <param name="Buff_in">Buffer de entrada de valores obtenido</param>
        public void Read(byte[] Buff_in)
        {   
            objInterface.read_data(Buff_in);
        }
        /// <summary>
        /// Escribe un buffer de valores
        /// </summary>
        /// <param name="CMD">Buffer de salida de valores a escribir</param>
        public void Write(byte[] CMD)
        {
            objInterface.write_data(CMD);
        }
        /// <summary>
        /// Lee y escribe en una sola operacion de transferencia los buffers de entrada y salida deben coincidir
        /// </summary>
        /// <param name="Buff_in">buffer de entrada</param>
        /// <param name="Buff_out">buffer de salida</param>
        public void ReadWrite(byte[] Buff_in, byte[] Buff_out)
        {
            objInterface.writeRead_data(Buff_in, Buff_out);
        }
        /// <summary>
        /// Busca los dispositivos disponibles en el bus de I2C
        /// </summary>
        /// <param name="MinIdAddress">Direccion minima de busqueda</param>
        /// <param name="MaxIdAddress">Direccion maxima de busqueda</param>
        /// <returns></returns>
        public static async Task<IEnumerable<byte>> FindDevicesAsync(byte MinIdAddress, byte MaxIdAddress)
        {
            return await I2C_Module.FindDevicesAsync(MinIdAddress, MaxIdAddress);
        }
    }
    /// <summary>
    /// Clase para el manejo de la interfaz de hardware de I2C
    /// menymp 2019
    /// </summary>
    public class I2C_Module
    {
        /// <summary>
        /// Dispositivo de I2C
        /// </summary>
        private I2cDevice Device;
        /// <summary>
        /// Referencia en formato de cadena de la interfaz del SO I2C
        /// </summary>
        private string AQS_str;

        /// <summary>
        /// Estatus de la interfaz I2C
        /// </summary>
        public I2C_Mode_state Dev_state { get; set; }
        /// <summary>
        /// Velocidad de la interfaz I2C
        /// </summary>
        public I2cBusSpeed DeviceSpeed { get; set; }
        /// <summary>
        /// Registro de dispositivos
        /// </summary>
        public DeviceInformationCollection Devices_reg { get; set; }

        //public IList<byte> Device_Id_list { get; set; }
        /// <summary>
        /// Direccion de esclavo I2C
        /// </summary>
        public int SlaveAdd { get; set; }
        /// <summary>
        /// Crea una nueva interfaz de I2C
        /// </summary>
        public I2C_Module()
        {
            /*AQS_str = AQS_type;
            DeviceSpeed = (I2cBusSpeed)Speed;
            SlaveAdd = SlaveAddress;
            Dev_state = I2C_Mode_state.I2C_SYSTEM_BUSY;*/
        }
        /// <summary>
        /// Inicializa la interfaz de I2C como maestro para la comunicacion
        /// </summary>
        /// <param name="SlaveAddress">Direccion del dispositivo esclavo de destino</param>
        /// <param name="AQS_type">Cadena de inicializacion</param>
        /// <param name="Speed">Velocidad de reloj</param>
        /// <returns></returns>
        public async Task initcomunica(int SlaveAddress, string AQS_type, I2C_Speed_enum Speed)
        {
            AQS_str = AQS_type;
            DeviceSpeed = (I2cBusSpeed)Speed;
            SlaveAdd = SlaveAddress;
            Dev_state = I2C_Mode_state.I2C_SYSTEM_BUSY;

            var settings = new I2cConnectionSettings(SlaveAdd); //add del dispositivo Sa
            settings.BusSpeed = DeviceSpeed; //velocidad del dispositivo
            settings.SharingMode = I2cSharingMode.Shared; // ###### Nota: revisar efectos colaterales ########
            string aqs = I2cDevice.GetDeviceSelector(AQS_str); //buscamos un selector para el aqs I2CN
            var dis = await DeviceInformation.FindAllAsync(aqs);
            Device = await I2cDevice.FromIdAsync(dis[0].Id, settings); //conectamos con el dispositivo que coincida
            Dev_state = I2C_Mode_state.I2C_SYSTEM_OK;
        }
        /// <summary>
        /// Busca los dispositivos disponibles en el bus de I2C
        /// </summary>
        /// <param name="MinIdAddress">Direccion minima de busqueda</param>
        /// <param name="MaxIdAddress">Direccion maxima de busqueda</param>
        /// <returns>Coleccion con las direcciones de los dispositivos encontrados</returns>
        public static async Task<IEnumerable<byte>> FindDevicesAsync(byte MinIdAddress, byte MaxIdAddress)
        {
            IList<byte> returnValue = new List<byte>();
            //selector de controladores en el sistema
            string aqs = I2cDevice.GetDeviceSelector();
            //buscamos el controlador del bus
            var dis = await DeviceInformation.FindAllAsync(aqs).AsTask();
            if (dis.Count > 0) //si se encuentran dispositivos
            {
                //const int minimumAddress = 0x08; //addres maximo y minimo para la busqueda
                //const int maximumAddress = 0x77;
                for (byte address = MinIdAddress; address <= MaxIdAddress; address++)
                {
                    var settings = new I2cConnectionSettings(address);
                    settings.BusSpeed = I2cBusSpeed.FastMode;
                    settings.SharingMode = I2cSharingMode.Shared;
                    //creamos un nuevo dispositivo con las caracteristicas y el Id 
                    using (I2cDevice device = await I2cDevice.FromIdAsync(dis[0].Id, settings))
                    {
                        if (device != null) //Si hay algun dispositio privado
                        {
                            try
                            {
                                byte[] writeBuffer = new byte[1] { 0 };
                                device.Write(writeBuffer);
                                // si no acontece exepciones un dispositivo respondio a esa direccion
                                returnValue.Add(address); //incorporamos el ID a la coleccion
                            }
                            catch
                            {
                                //significa que no hubo respuesta
                            }

                        }
                    }
                }
            }

            return returnValue;
        }
        /// <summary>
        /// Lee datos en un buffer de entrada
        /// </summary>
        /// <param name="buff_in">buffer de almacenamiento de datos</param>
        /// <returns>Estado del sistema</returns>
        public I2C_Mode_state read_data(byte[] buff_in)
        {
            if (Dev_state == I2C_Mode_state.I2C_SYSTEM_OK)
            {
                try
                {
                    Device.Read(buff_in);
                    return I2C_Mode_state.I2C_SYSTEM_OK;
                }
                catch
                {
                    return I2C_Mode_state.I2C_SYSTEM_FAULT;
                }
            }
            else
                return Dev_state;
        }
        /// <summary>
        /// Escribe un buffer de datos
        /// </summary>
        /// <param name="buff_out">Buffer de datos de salida</param>
        /// <returns>Estado del sistema</returns>
        public I2C_Mode_state write_data(byte[] buff_out)
        {
            if (Dev_state == I2C_Mode_state.I2C_SYSTEM_OK)
            {
                try
                {
                    Device.Write(buff_out);
                    return I2C_Mode_state.I2C_SYSTEM_OK;
                }
                catch
                {
                    return I2C_Mode_state.I2C_SYSTEM_FAULT;
                }
            }
            else
                return Dev_state;
        }
        /// <summary>
        /// Lee y escribe un buffer en una sola operacion
        /// </summary>
        /// <param name="buff_out">Buffer de entrada</param>
        /// <param name="buff_in">Buffer de salida</param>
        /// <returns>Estatus</returns>
        public I2C_Mode_state writeRead_data(byte[] buff_out, byte[] buff_in)
        {
            if (Dev_state == I2C_Mode_state.I2C_SYSTEM_OK)
            {
                try
                {
                    Device.WriteRead(buff_out, buff_in);
                    return I2C_Mode_state.I2C_SYSTEM_OK;
                }
                catch
                {
                    return I2C_Mode_state.I2C_SYSTEM_FAULT;
                }
            }
            else
                return Dev_state;
        }
    }
    /// <summary>
    /// Enumerador de estado de la interfaz I2C
    /// </summary>
    public enum I2C_Mode_state
    {
        I2C_MASTER,
        I2C_SLAVE,
        I2C_SYSTEM_OK,
        I2C_SYSTEM_FAULT,
        I2C_SYSTEM_NOINIT,
        I2C_SYSTEM_BUSY
    }
    /// <summary>
    /// Enumerador de velocidad de la interfaz I2C
    /// </summary>
    public enum I2C_Speed_enum
    {
        I2C_STANDARD = 0,
        I2C_FAST
    }
    //modos de funcionamiento del modulo de i2c

}
