using System;
using System.Runtime.CompilerServices;
using System.Timers;
using System.IO;
using System.Linq;

namespace CS.UDP.Sample01
{
	class Program
	{
		private static Timer timer1;
		public CoordinatesData CoordData;
		public static int ArrayIndexGlobal = 0;
		public static double[,] arrCoord = { 
				{ 43.773090d, -79.4680160d },
				{ 43.773191d, -79.4681161d },
				{ 43.773292d, -79.4682162d },
				{ 43.773393d, -79.4683163d },
				{ 43.773494d, -79.4684164d },
				{ 43.773595d, -79.4685165d },
				{ 43.773696d, -79.4686166d }
			};

		public double[,] arrCoordPair;

		static void Main(string[] args)
		{
			timer1 = new System.Timers.Timer();
			timer1.Interval = 1000;

			timer1.Elapsed +=  OnTimedEvent;
			timer1.AutoReset = true;
			timer1.Enabled =   true;

			Console.WriteLine("Waiting for the end of main loop... ");
			Console.ReadKey();
		}

		static public void SendCoordinates(double fLat, double fLng)
		{
			UDPSocket UDPconnection = new UDPSocket();
			UDPconnection.Client("99.252.230.148", 1340);

			string sDate = String.Format("{0}", DateTime.Now);

			string sMessage = "{\"id\":2, \"time\":\"" + sDate + "\", \"latitude\":" + fLat.ToString() + ", \"longitude\":" + fLng.ToString() + "}";
			Console.WriteLine(sMessage);

			UDPconnection.Send(sMessage);

			UDPconnection = null;
		}

		private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
		{
			/* Office Lat  43.773690 */
			/* Office Lng -79.468160 */

			if (arrCoord.GetUpperBound(0) > ArrayIndexGlobal)
			{
				SendCoordinates(arrCoord[ArrayIndexGlobal, 0], arrCoord[ArrayIndexGlobal, 1]);
				Console.WriteLine("Raised at: {0}:::{1}", e.SignalTime, ArrayIndexGlobal);
				ArrayIndexGlobal++;
			}
			else
			{
				System.Environment.Exit(0);
			}
		}

	}
}
