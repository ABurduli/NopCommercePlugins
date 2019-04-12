using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceLib
{


    public class ProdItem
    {
        public Guid uid { get; set; }
        public string Name { get; set; }
        public Guid Group { get; set; }
        public bool IsGroup { get; set; }
        public string InternalArticle {get;set;}
        public string Unit { get; set; }
        public string ItemType { get; set; }
        public string Weight { get; set; }
        public string VATRate { get; set; }
        public string InventoriesAccount { get; set; }
        public string ExpensesAccount { get; set; }
        public string CostsAccount { get; set; }
        public string VATPayableAccount { get; set; }
        public string ImportTaxRate { get; set; }
        public string Barcode { get; set; }
        public string FullName { get; set; }
        public string VATArticle { get; set; }
        public string OwnProduction { get; set; }
        public string ProductionAccount { get; set; }
        public string UnitCost { get; set; }
        public string LoadingPerVehicle { get; set; }
        public string Price { get; set; }
        public string ExtCode { get; set; }
        public List<PackageItem> Packages { get; set; }
        //public string Packages { get; set; }
        public string AdditionalRequisites1 { get; set; }
        public string AdditionalRequisites2 { get; set; }
        public string AdditionalRequisites3 { get; set; }

    }

    public class PackageItem
    {
        public string Package { get; set; }
        public decimal Coefficient { get; set; }
    }
}
