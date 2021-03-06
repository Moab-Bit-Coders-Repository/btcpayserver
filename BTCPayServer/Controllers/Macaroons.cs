﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BTCPayServer.Controllers
{
    public class Macaroons
    {
        public class Macaroon
        {
            public Macaroon(byte[] bytes)
            {
                Bytes = bytes;
                Hex = NBitcoin.DataEncoders.Encoders.Hex.EncodeData(bytes);
            }

            public string Hex { get; set; }
            public byte[] Bytes { get; set; }
        }
        public static async Task<Macaroons> GetFromDirectoryAsync(string directoryPath)
        {
            if (directoryPath == null)
                throw new ArgumentNullException(nameof(directoryPath));
            Macaroons macaroons = new Macaroons();
            if (!Directory.Exists(directoryPath))
                return macaroons;
            foreach(var file in Directory.GetFiles(directoryPath, "*.macaroon"))
            {
                try
                {
                    switch (Path.GetFileName(file))
                    {
                        case "admin.macaroon":
                            macaroons.AdminMacaroon = new Macaroon(await File.ReadAllBytesAsync(file));
                            break;
                        case "readonly.macaroon":
                            macaroons.ReadonlyMacaroon = new Macaroon(await File.ReadAllBytesAsync(file));
                            break;
                        case "invoice.macaroon":
                            macaroons.InvoiceMacaroon = new Macaroon(await File.ReadAllBytesAsync(file));
                            break;
                        default:
                            break;
                    }
                }
                catch { }
            }
            return macaroons;
        }
        public Macaroon ReadonlyMacaroon { get; set; }

        public Macaroon InvoiceMacaroon { get; set; }
        public Macaroon AdminMacaroon { get; set; }
    }
}
