using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace CS.UDP.Sample01
{
	public class CoordinatesData : InterfaceList.ICoordinatesData
	{
		private int _CurrIndex;

		public int CurrIndex
		{
			get { return _CurrIndex ; }
			set { _CurrIndex = value ; }
		}
	}
}
