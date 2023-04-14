using System;
namespace Mummies.Models.ViewModels
{
	public class Mummy
	{
        public long? Id { get; set; }

        // Burial Info
        public string? Squarenorthsouth { get; set; }
        public string? Headdirection { get; set; }
        public string? Northsouth { get; set; }
        public string? Depth { get; set; }
        public string? Eastwest { get; set; }
        public string? Squareeastwest { get; set; }
        public string? Area { get; set; }
        public string? Burialnumber { get; set; }

        // Mummy Info
        public string? Wrapping { get; set; }
        public string? Haircolor { get; set; }
        public string? Ageatdeath { get; set; }
        public string? Sex { get; set; }

        // Body Analysis Info
        public double? Estimatestature { get; set; }

        // Textile Info
        public long? Textileid { get; set; }
        public string? TextileDescription { get; set; }
        public string? TextileFunction { get; set; }
        public string? TextileColor { get; set; }
        public string? TextileStructure { get; set; }
    }
}

