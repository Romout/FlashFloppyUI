using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashFloppyUI.Models
{
	public class ADFFileReference
	{
		public ADFFileReference() { }
		public ADFFileReference(string filePath)
		{
			FilePath = filePath;
			Name = Path.GetFileNameWithoutExtension(filePath);
		}
		public string? Name { get; set; } = "";
		public string? FilePath { get; set; } = "";
	}
}
