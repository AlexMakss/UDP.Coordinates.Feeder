using System;
using System.Runtime.CompilerServices;
using System.Timers;
using System.IO;
using System.Linq;

/*
 * There are 2 + 1 questions
 *     Questions #1 and #2 are mutually exclusive, if one of them could be solved I do not care about the other one
 *     
 *     Q#1. Is it possible to call the OnTimedEvent() subroutine with parameters from the Main() module?
 *     Q#2. Is it possible to move the arrCoord array inside the Main() module and make it dynamic ?
 *     Q#3. Is it possible to pass value from UDPSocket.sc:Line57 to UDPSocket.sc:Line59 ?
 *          i.e. pass remote server return code back to the Main() module.
 */



namespace CS.UDP.Sample01
{
	class Program
	{
		// Timer declaration
		private static Timer timer1;
		public CoordinatesData CoordData;
		// Accessible from anywhere integer index value
		public static int ArrayIndexGlobal = 0;
		// Static array (should be turned into a dynamic array)
		public static double[,] arrCoord = { 
				{ 43.773090d, -79.4680160d },
				{ 43.773191d, -79.4681161d },
				{ 43.773292d, -79.4682162d },
				{ 43.773393d, -79.4683163d },
				{ 43.773494d, -79.4684164d },
				{ 43.773595d, -79.4685165d },
				{ 43.773696d, -79.4686166d }
			};
		/************************************************
		 * 
		 *   MAIN
		 * 
		 ************************************************/
		static void Main(string[] args)
		{
			/* Time initiation */
			timer1 = new System.Timers.Timer();
			timer1.Interval = 1000;
			/* Assigning listener to the timer event (never saw the "+=" as a binding operand) */
			timer1.Elapsed +=  OnTimedEvent;
			timer1.AutoReset = true;
			timer1.Enabled =   true;

			/* Wait for the program natural termination */
			Console.WriteLine("Waiting for the end of main loop... ");
			Console.ReadKey();
		}

		/**
		 * 
		 * Wrapper for the UDP module
		 * 
		 */
		static public void SendCoordinates(double fLat, double fLng)
		{
			UDPSocket UDPconnection = new UDPSocket();
			UDPconnection.Client("99.252.230.148", 1340);

			string sDate = String.Format("{0}", DateTime.Now);

			string sMessage = "{\"id\":2, \"time\":\"" + sDate + "\", \"latitude\":" + fLat.ToString() + ", \"longitude\":" + fLng.ToString() + "}";
			//Console.WriteLine(sMessage);

			UDPconnection.Send(sMessage);

			UDPconnection = null;
		}

		/*
		 * 
		 *   Time event handler
		 * 
		 */
		private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
		{
			/* Office Lat  43.773690 */
			/* Office Lng -79.468160 */

			if (arrCoord.GetUpperBound(0) <= ArrayIndexGlobal)
			{
				System.Environment.Exit(0);
			}
  		SendCoordinates(arrCoord[ArrayIndexGlobal, 0], arrCoord[ArrayIndexGlobal, 1]);
  		Console.WriteLine("Raised at: {0}:::{1}", e.SignalTime, ArrayIndexGlobal);
  		ArrayIndexGlobal++; 
		}

	}
}
