using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiveMaterial.Model
{
	public class ReceiveModel
	{
        public string PALLET { get; set; }
        public string DN { get; set; }
        public int DNITEM { get; set; }
        public string PART { get; set; }
        public int CARTON { get; set; }

		public int BOXCOUNT { get; set; }

		public int QTY { get; set; }
        public string WO { get; set; }
        public string SN { get; set; } = "";
	}
}
