using System;

namespace Common.Models
{
	public class XbeeDTO
    {
        public byte Length { get; set; }
        public byte Address { get; set; }
        public byte[] BufferBytes { get; set; }
    }
}

