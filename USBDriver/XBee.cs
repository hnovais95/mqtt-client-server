using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace USBDriver
{
    public class XBee
    {
        private int baudRate = 9600;
        private int connectTimeout = 1000;
        private SerialPort spL;
        private Parity parity = Parity.None;
        private StopBits stopBits = StopBits.One;

        public event Action<byte, byte, byte[]>? RecebeDados;

        public XBee(SerialPort serialPort)
        {
            this.spL = serialPort;
            spL.BaudRate = baudRate;
            spL.Parity = parity;
            spL.StopBits = stopBits;
            spL.WriteTimeout = 10000;
            spL.ReadTimeout = connectTimeout;
            spL.DtrEnable = true; // resetar Arduino toda vez que abir a porta

            spL.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        public bool IsOpen()
        {
            if (spL.IsOpen) return true;
            else return false;
        }

        public void Connect()
        {
            if (spL.IsOpen)
            {
                spL.DiscardInBuffer();
                spL.Close();
            }
            try
            {
                if (spL != null)
                {
                    if (!spL.IsOpen)
                    {
                        spL.BaudRate = baudRate;
                        spL.Parity = parity;
                        spL.StopBits = stopBits;
                        spL.WriteTimeout = 10000;
                        spL.ReadTimeout = connectTimeout;
                        spL.Open();
                    }
                }
            }
            //// give a message, if the port is not available:
            catch
            {
                Console.WriteLine("A porta serial " + spL.PortName + " não pode ser aberta");
                //comboBox1.SelectedText = "";
            }
        }

        public void Disconnect()
        {
            try
            {
                if (spL.IsOpen)
                {
                    spL.DiscardInBuffer();
                    spL.Close();
                    tentativa = 0;
                    debug = 0;
                    carregaBuffer = 0;
                    offset = 0;
                    endBuffer = 0;
                    contagemBuffer = 4;
                    Array.Clear(bff, 0, bff.Length);
                    Array.Clear(bffExtra, 0, bff.Length);
                }
            }
            //// give a message, if the port is not available:
            catch
            {
                Console.WriteLine("A porta serial " + spL.PortName + " não pode ser aberta");
                //comboBox1.SelectedText = "";
            }
        }

        // recepção máquina de estados
        int debug = 0;
        int carregaBuffer = 0;
        int endBuffer = 0;
        int contagemBuffer = 4; // garante que pelo menos o início do pacote chegue: 7E 00 __ 81 00
        Boolean logCom = true, logExc = false;
        byte[] bff = new byte[1024]; //tamanho do maior pacote 109
        byte[] bffExtra = new byte[1024];
        int m = 0, offset = 0;
        int inicio = 0;
        int tentativa = 0;

        private void DataReceivedHandler(object sender,
                        SerialDataReceivedEventArgs e)
        {
            tentativa = 0;
        Recebe:
            try
            {
                if (logCom) Console.WriteLine($"====== MÉTODO DATA RECEIVED =====");

                SerialPort spL = (SerialPort)sender;

                while (spL.BytesToRead > contagemBuffer)
                {
                    if (logCom) Console.WriteLine($"debug = {debug}");
                    debug++;

                    m = spL.Read(bff, offset, spL.BytesToRead);
                    if (logCom) Console.WriteLine($"m = {m}");
                    if (logCom) Console.WriteLine($"offset = {offset}");

                    // busca índice do first byte
                    if (carregaBuffer == 0)
                    {
                        for (int i = 0; i < bff.Length; i++)
                        {
                            if (bff[i] == 126)
                            {
                                carregaBuffer = i;
                                inicio = i;
                                break;
                            }
                        }
                    }
                    if (logCom) Console.WriteLine($"carregaBuffer = {carregaBuffer}");
                    // garantir que os bytes iniciais do pacote sejam: 7E 00 __ 81 00 ou 7E 00 __ 01 00 ou ou 7E 00 __ 81 01
                    if (bff[carregaBuffer] == 126)//126 = 0x7E - Start Delimiter
                    {
                        if (logCom) Console.WriteLine($"1st byte ok");
                        carregaBuffer++;
                        if (bff[carregaBuffer] == 0)
                        {
                            if (logCom) Console.WriteLine($"2nd byte ok");
                            carregaBuffer++;
                            endBuffer = bff[carregaBuffer] + 4 + inicio;
                            if (logCom) Console.WriteLine($"qtidade dados XBee = {endBuffer}");
                            // aguarda receber até o fim do pacote
                            while (m < endBuffer) // recebe ,aos dados se a quantidade recebida até aqui for menor que o tamanho do pacote
                            {
                                m += spL.Read(bff, offset + m, spL.BytesToRead);
                                System.Threading.Thread.Sleep(10);

                            }
                            carregaBuffer++;
                            if (bff[carregaBuffer] == 129 || bff[carregaBuffer] == 01)//0X81 (RX (Receive) Packet 16-bit Address) - Frame Type
                            {                                                         //0X01 (TX (Trasmit) Packet 16-bit Address) - Frame Type
                                if (logCom) Console.WriteLine($"4th byte ok");
                                carregaBuffer++;
                                if (bff[carregaBuffer] == 0)
                                {
                                    byte checkSum = 0;
                                    string pkg = "";
                                    if (logCom) Console.WriteLine($"5th byte ok");
                                    if (logCom) Console.WriteLine($"conferindo dados:");
                                    for (int c = offset; c < offset + m; c++)
                                    {
                                        pkg += (bff[c].ToString("X2") + " ");

                                    }
                                    if (logCom) Console.WriteLine(pkg);
                                    if (logCom) Console.WriteLine($"buffer:");
                                    pkg = "";
                                    for (int c = 0; c < bff.Length; c++)
                                    {
                                        pkg += (bff[c].ToString("X2") + " ");

                                    }
                                    if (logCom) Console.WriteLine(pkg);
                                    if ((m + offset) == endBuffer) // Tamanho recebido + offset (no caso de recuperação de dados) é igual ao tamanho do pacote
                                    {
                                        Console.WriteLine($"pacote XBee exato:");
                                        pkg = "";
                                        for (int c = inicio; c < endBuffer; c++) // monta vetor para impressão
                                        {
                                            pkg += (bff[c].ToString("X2") + " ");

                                        }
                                        for (int c = inicio; c < bff[inicio + 2]; c++) // calcula checksum
                                        {

                                            checkSum += bff[c + 3];
                                        }
                                        checkSum = (byte)(0xFF - checkSum);
                                        if (checkSum == bff[endBuffer - 1]) // check sum ok
                                        {
                                            if (logCom) Console.WriteLine($"checksum ok");
                                            byte[] XBeepack = new byte[endBuffer];
                                            Array.Copy(bff, inicio, XBeepack, 0, XBeepack.Length);

                                            if (RecebeDados != null)
                                            {
                                                // tamanho do pacote, endreço de origem, pacote API 
                                                RecebeDados?.Invoke(XBeepack[2], XBeepack[5], XBeepack);
                                            }
                                            // reiniciando condições TESTE
                                            tentativa = 0;
                                            debug = 0;
                                            carregaBuffer = 0;
                                            offset = 0;
                                            endBuffer = 0;
                                            contagemBuffer = 4;
                                            Array.Clear(bff, 0, bff.Length);
                                            Array.Clear(bffExtra, 0, bff.Length);
                                            return;
                                        }
                                        else
                                        {
                                            if (logCom) Console.WriteLine($"checksum error");
                                        }
                                        Console.WriteLine(pkg);
                                        // reiniciando condições
                                        tentativa = 0;
                                        debug = 0;
                                        carregaBuffer = 0;
                                        offset = 0;
                                        endBuffer = 0;
                                        contagemBuffer = 4;
                                        Array.Clear(bff, 0, bff.Length);
                                        Array.Clear(bffExtra, 0, bff.Length);
                                        return;
                                    }
                                    else if ((m + offset) > endBuffer) // quantidade de dados recebidos maior que o pacote
                                    {
                                        Console.WriteLine($"pacote XBee:");
                                        pkg = "";
                                        for (int c = inicio; c < endBuffer; c++) // monta vetor em formato hexadecimal com dois dígitos
                                        {
                                            pkg += (bff[c].ToString("X2") + " ");
                                        }
                                        for (int c = inicio; c < bff[inicio + 2]; c++) // calcula checksum
                                        {

                                            checkSum += bff[c + 3];
                                        }
                                        checkSum = (byte)(0xFF - checkSum);
                                        if (checkSum == bff[endBuffer - 1]) // se checksum ok
                                        {
                                            if (logCom) Console.WriteLine($"checksum ok");
                                            byte[] XBeepack = new byte[endBuffer];
                                            Array.Copy(bff, inicio, XBeepack, 0, XBeepack.Length);

                                            if (RecebeDados != null)
                                            {
                                                // tamanho do pacote, endreço de origem, pacote API 
                                                RecebeDados?.Invoke(XBeepack[2], XBeepack[5], XBeepack);
                                            }
                                        }
                                        else
                                        {
                                            if (logCom) Console.WriteLine($"checksum error");
                                        }

                                        Console.WriteLine(pkg);

                                        // recuperar dados após o pacote
                                        pkg = "";
                                        for (int i = 0; i < (m + offset - endBuffer); i++)
                                        {
                                            bffExtra[i] = bff[endBuffer + i];
                                            pkg += (bffExtra[i].ToString("X2") + " ");
                                        }


                                        if (logCom) Console.WriteLine($"salvando dados além do pacote:");
                                        if (logCom) Console.WriteLine(pkg);

                                        Array.Clear(bff, 0, bff.Length);
                                        Array.Copy(bffExtra, bff, bffExtra.Length); // copia final dos dados que não foi reconhecido como pacote para início do buffer
                                        debug = 0;
                                        carregaBuffer = 0;
                                        offset = m + offset - endBuffer; // atualiza offset para receber da porta serial a partir dos dados salvos, para completar o pacote

                                        contagemBuffer = 4;
                                        Array.Clear(bffExtra, 0, bff.Length);
                                        if (tentativa == 0)
                                        {
                                            tentativa++;
                                            if (logCom) Console.WriteLine($"recuperação imediata:");
                                            goto Recebe;
                                        }
                                        else
                                        {
                                            tentativa = 0;
                                            // reiniciando condições
                                            debug = 0;
                                            carregaBuffer = 0;
                                            offset = 0;
                                            endBuffer = 0;
                                            contagemBuffer = 4;
                                            Array.Clear(bff, 0, bff.Length);
                                            Array.Clear(bffExtra, 0, bff.Length);
                                            return;
                                        }

                                    }
                                    else // quantidade de dados recebidos é menor que um pacote
                                    {
                                        if (logCom) Console.WriteLine($"aguardando receber mais dados para completar o pacote...");
                                        contagemBuffer = endBuffer - m; // atualiza condição do while
                                        if (logCom) Console.WriteLine($"bytes a receber = {contagemBuffer}");

                                        offset += m;
                                        carregaBuffer = 0;

                                        if (tentativa == 0)
                                        {
                                            tentativa++;
                                            goto Recebe;
                                        }
                                        else
                                        {
                                            tentativa = 0;
                                            // reiniciando condições
                                            debug = 0;
                                            carregaBuffer = 0;
                                            offset = 0;
                                            endBuffer = 0;
                                            contagemBuffer = 4;
                                            Array.Clear(bff, 0, bff.Length);
                                            Array.Clear(bffExtra, 0, bff.Length);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"5th byte error");

                                    carregaBuffer = 0;
                                    return;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"4th byte error");
                                carregaBuffer = 0;
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"2nd byte error");
                            carregaBuffer = 0;
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"1st byte error");
                        carregaBuffer = 0;
                        return;
                    }

                }
                if (debug > 3)
                {
                    tentativa = 0;
                    debug = 0;
                    carregaBuffer = 0;
                    offset = 0;
                    endBuffer = 0;
                    contagemBuffer = 4;
                    Array.Clear(bff, 0, bff.Length);
                    Array.Clear(bffExtra, 0, bff.Length);
                }
            }
            catch (Exception ex)
            {

                if (logExc) Console.WriteLine($"Erro na recepção de dados" + ex.Message);
                // reiniciando condições
                tentativa = 0;
                debug = 0;
                carregaBuffer = 0;
                offset = 0;
                endBuffer = 0;
                contagemBuffer = 4;
                Array.Clear(bff, 0, bff.Length);
                Array.Clear(bffExtra, 0, bff.Length);
            }
        }

        public void enviaXBeeTransparent(byte[] buffer)
        {
            try
            {
                if (spL.IsOpen)
                {
                    spL.Write(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                Console.WriteLine("A porta serial " + spL.PortName + " não pode ser aberta");
                //comboBox1.SelectedText = "";
            }
        }

        public void enviaXBeeAPI(byte adress, byte[] data)
        {
            try
            {
                if (spL.IsOpen)
                {
                    byte sd = 0x7E, msb = 0, lsb = 0, ft = 1, fid = 0, ad_msb = 0, ad_lsb = 0, op = 0, datasum = 0, cs; //fid igual a zero não recebe status de volta

                    int i = 0;
                    string pkg = "";

                    byte[] buffer = new byte[data.Length + 9];
                    ad_lsb = adress; // endereço do XBee
                    lsb = (byte)(buffer.Length - 4);


                    buffer[0] = sd;
                    buffer[1] = msb;
                    buffer[2] = lsb;
                    buffer[3] = ft;
                    buffer[4] = fid;
                    buffer[5] = ad_msb;
                    buffer[6] = ad_lsb;
                    buffer[7] = op;

                    for (i = 8; i < (data.Length + 8); i++)
                    {
                        buffer[i] = data[i - 8];
                    }

                    for (int j = 8; j < (buffer.Length - 1); j++)
                    {
                        datasum += Convert.ToByte(buffer[j]);
                    }
                    cs = (byte)(0xFF - (byte)(ft + fid + ad_msb + ad_lsb + op + datasum));
                    buffer[i] = cs;

                    if (spL.IsOpen)
                    {
                        spL.Write(buffer, 0, buffer.Length);
                    }

                    for (int k = 0; k < buffer.Length; k++)
                    {
                        pkg += (buffer[k].ToString("X2") + " ");
                    }
                    if (logCom) Console.WriteLine($"Envio de pacote API");
                    Console.WriteLine(pkg);
                }
            }
            catch
            {
                Console.WriteLine("A porta serial " + spL.PortName + " não pode ser aberta");
                //comboBox1.SelectedText = "";
            }

        }
    }
}