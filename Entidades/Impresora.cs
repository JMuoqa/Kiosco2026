using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    // Clase auxiliar para mandar bytes crudos al spooler (spooler = COLA DE IMPRESION)
    public class ComportamientoImpresora
    {
        /*
         * DOCINFOA es una estructura ANSI utilizada en la API de Windows (wingdi.h) y el spooler de impresión (winspool.drv) que contiene información sobre un documento de impresión.
         * ANSI hace referencia a los estándares establecidos por el Instituto Nacional Estadounidense de Estándares, Se utiliza 
         * principalmente para garantizar la compatibilidad, portabilidad y coherencia del código en diferentes plataformas
         */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        /*
         * Aca importamos funciones nativas de windows de la libreria "winspool.Drv" que seria "las funciones de la cola de impresion" 
         * como "OpenPrinterA" y la guardamos en metodos como "OpenPrinter" para poder declararlas dentro de c#
         */
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true)]
        public static extern bool OpenPrinter(string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter")]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true)]
        public static extern bool WritePrinter(IntPtr hPrinter, byte[] pBytes, int dwCount, out int dwWritten);

        public static void SendBytesToPrinter(string szPrinterName, byte[] pBytes)
        {
            IntPtr hPrinter;
            DOCINFOA di = new DOCINFOA(); // DOCINFOA es una estructura ANSI utilizada en la API de Windows y el spooler de impresión que contiene información sobre un documento de impresión
            di.pDocName = "Raw ESC/POS Command";
            di.pDataType = "RAW";

            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    if (StartPagePrinter(hPrinter))
                    {
                        WritePrinter(hPrinter, pBytes, pBytes.Length, out int dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
        }
    }
}
