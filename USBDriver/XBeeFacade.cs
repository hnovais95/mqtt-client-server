using System;

namespace USBDriver
{
	public class XBeeFacade
    {
		private XBee _xbee;

        public event Action<byte, byte, byte[]>? OnReceiveData;

        public XBeeFacade(XBee xBee)
		{
			_xbee = xBee;
            _xbee.RecebeDados += XBee_RebeceDados;
        }

        public void Connect()
		{
			_xbee.Connect();
		}

        public void Disconnect()
		{
			_xbee.Disconnect();
		}

        public void SendData(byte[] buffer)
		{
			_xbee.enviaXBeeTransparent(buffer);
		}

        public void SendData(byte address, byte[] buffer)
		{
			_xbee.enviaXBeeAPI(address, buffer);
		}

		private void XBee_RebeceDados(byte length, byte address, byte[] bufferBytes)
        {
			OnReceiveData?.Invoke(length, address, bufferBytes);
		}
    }
}

