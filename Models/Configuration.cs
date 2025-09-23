using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlashFloppyUI.Models
{
	public class Configuration
	{
		public BindingList<ADFFileReference> ADFFileReferences { get; set; } = new BindingList<ADFFileReference>();
	}
}
